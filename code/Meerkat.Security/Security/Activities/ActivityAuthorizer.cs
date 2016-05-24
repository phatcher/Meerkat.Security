using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Reflection;

using Common.Logging;

namespace Meerkat.Security.Activities
{
    /// <copydoc cref="IActivityAuthorizer.IsAuthorized" />
    public class ActivityAuthorizer : IActivityAuthorizer
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDictionary<string, Activity> activities;

        /// <summary>
        /// Creates a new instance of the <see cref="ActivityAuthorizer"/> class.
        /// </summary>
        /// <param name="activities"></param>
        /// <param name="authorized"></param>
        /// <param name="defaultActivity"></param>
        public ActivityAuthorizer(IList<Activity> activities, bool authorized, string defaultActivity = null)
        {
            this.activities = new Dictionary<string, Activity>();
            foreach (var activity in activities)
            {
                this.activities[activity.Name] = activity;
            }

            DefaultAuthorization = authorized;
            DefaultActivity = defaultActivity;
        }

        /// <summary>
        /// Get or set the default authorization value if the authorizer has no opinion.
        /// </summary>
        public bool DefaultAuthorization { get; set; }

        /// <summary>
        /// Gets or sets the default activity to perform authorization against.
        /// </summary>
        public string DefaultActivity { get; set; }

        /// <summary>
        /// Get all the activities supported
        /// </summary>
        public IList<Activity> Activities
        {
            get { return activities.Values.ToList(); }
        }

        /// <copydoc cref="IActivityAuthorizer.IsAuthorized" />
        public AuthorizationReason IsAuthorized(string resource, string action, IPrincipal principal)
        {
            var reason = new AuthorizationReason
            {
                Resource = resource,
                Action = action,
                Principal = principal,
                // We will always make a decision at this level
                NoDecision = false,
                IsAuthorized = DefaultAuthorization
            };

            // Check the activities
            foreach (var activity in FindActivities(resource, action))
            {
                var rs = principal.IsAuthorized(activity, DefaultAuthorization);
                if (rs.NoDecision == false)
                {
                    // Ok, we have a decision
                    reason.IsAuthorized = rs.IsAuthorized;
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

                    break;
                }
            }

            if (!reason.IsAuthorized)
            {
                Logger.InfoFormat("Failed authorization: User '{0}', Resource: '{1}', Action: '{2}'", principal == null ? "<Unknown>" : principal.Identity.Name, resource, action);
            }

            return reason;
        }

        /// <summary>
        /// Find all activities that have been modelled.
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private IEnumerable<Activity> FindActivities(string resource, string action)
        {
            Activity value = null;

            // Find the closest activity match - resource centric
            foreach (var activity in ActivityExtensions.Activities(resource, action))
            {
                if (activities.TryGetValue(activity, out value))
                {
                    yield return value;
                }
            }

            if (!string.IsNullOrEmpty(DefaultActivity))
            {
                // Attempt to get the default activity.
                if (activities.TryGetValue(DefaultActivity, out value))
                {
                    yield return value;
                }
            }
        }
    }
}