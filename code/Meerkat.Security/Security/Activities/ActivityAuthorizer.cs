using System.Collections.Generic;
using System.Security.Principal;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Meerkat.Logging;

namespace Meerkat.Security.Activities
{
    /// <copydoc cref="IActivityAuthorizer.IsAuthorized" />
    public class ActivityAuthorizer : IActivityAuthorizer
    {
        private static readonly ILog Logger = LogProvider.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IAuthorizationScopeProvider scopeProvider;

        /// <summary>
        /// Creates a new instance of the <see cref="ActivityAuthorizer"/> class.
        /// </summary>
        /// <param name="scopeProvider">Scope provider to use</param>
        /// <param name="authorized">Default authorization value</param>
        /// <param name="defaultActivity">Default activity</param>
        /// <param name="allowUnauthenticated">Whether to allow unauthenticated users</param>
        public ActivityAuthorizer(IAuthorizationScopeProvider scopeProvider, bool authorized, string defaultActivity = null, bool allowUnauthenticated = false)
        {
            this.scopeProvider = scopeProvider;

            DefaultAuthorization = authorized;
            DefaultAllowUnauthenticated = allowUnauthenticated;
            DefaultActivity = defaultActivity;
        }

        /// <summary>
        /// Get or set the default authorization value if the authorizer has no opinion.
        /// </summary>
        public bool DefaultAuthorization { get; }

        /// <summary>
        /// Get or set the default allow unauthenticated value if the authorizer has no opinion.
        /// </summary>
        public bool DefaultAllowUnauthenticated { get; }

        /// <summary>
        /// Gets or sets the default activity to perform authorization against.
        /// </summary>
        public string DefaultActivity { get; }

        /// <copydoc cref="IActivityAuthorizer.IsAuthorized" />
        public AuthorizationReason IsAuthorized(string resource, string action, IPrincipal principal, IDictionary<string, object> values = null)
        {
            return IsAuthorizedAsync(resource, action, principal, values).Result;
        }

        /// <copydoc cref="IActivityAuthorizer.IsAuthorizedAsync" />
        public async Task<AuthorizationReason> IsAuthorizedAsync(string resource, string action, IPrincipal principal, IDictionary<string, object> values = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var scope = await scopeProvider.AuthorizationScopeAsync(cancellationToken).ConfigureAwait(false);
            if (scope == null)
            {
                Logger.Warn("No data returned from scope provider");
                scope = new AuthorizationScope { Name = "Default" };
            }

            var activities = scope.Activities.ToDictionary();

            // Get the state for this request
            var defaultAuthorization = scope.DefaultAuthorization ?? DefaultAuthorization;
            var defaultActivity = scope.DefaultActivity ?? DefaultActivity;
            var defaultAllowUnauthenticated = scope.AllowUnauthenticated ?? DefaultAllowUnauthenticated;

            // Set up the reason
            var reason = new AuthorizationReason
            {
                Resource = resource,
                Action = action,
                Principal = principal,
                // We will always make a decision at this level
                NoDecision = false,
                IsAuthorized = defaultAuthorization
            };

            // Do a short-circuit check for the unauthenticated state, will be overridden if there is a matching activity
            if (!defaultAllowUnauthenticated && !principal.Identity.IsAuthenticated)
            {
                reason.IsAuthorized = false;
                reason.Reason = "IsAuthenticated: false";
            }

            // Check the activities
            foreach (var activity in activities.FindActivities(resource, action, defaultActivity))
            {
                var rs = principal.IsAuthorized(activity, defaultAuthorization);
                if (rs.NoDecision)
                {
                    // Try the next one
                    continue;
                }

                // Ok, we have a decision
                reason.IsAuthorized = rs.IsAuthorized;
                reason.Identity = rs.Identity;

                if (reason.Resource != rs.Resource || reason.Action != rs.Action)
                {
                    // Decided based on some other activity, so say what that was
                    reason.PrincipalReason = rs;
                }
                else
                {
                    // Preserve the reason information
                    reason.Reason = rs.Reason;
                }

                // We are done as we have a decision
                break;
            }

            if (!reason.IsAuthorized)
            {
                Logger.InfoFormat("Failed authorization: User '{0}', Resource: '{1}', Action: '{2}'", principal == null ? "<Unknown>" : principal.Identity.Name, resource, action);
            }

            return reason;
        }
    }
}