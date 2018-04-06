using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Meerkat.Security.Activities
{
    /// <summary>
    /// Aggregates multiple <see cref="IAuthorizationScopeProvider"/> together, last provider wins in terms of permissions
    /// </summary>
    public class AggregateAuthorizationScopeProvider : IAuthorizationScopeProvider
    {
        private readonly string name;
        private readonly IList<IAuthorizationScopeProvider> providers;

        /// <summary>
        /// Creates a new instance of the <see cref="AggregateAuthorizationScopeProvider"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="providers"></param>
        public AggregateAuthorizationScopeProvider(string name, IAuthorizationScopeProvider[] providers)
        {
            this.name = name;
            this.providers = new List<IAuthorizationScopeProvider>(providers);
        }

        /// <copydoc cref="IAuthorizationScopeProvider.AuthorizationScopeAsync" />
        /// <remarks>Can potentially return duplicate <see cref="Activity"> if multiple providers supply the same activity</see></remarks>
        public async Task<AuthorizationScope> AuthorizationScopeAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var scope = new AuthorizationScope
            {
                Name = name
            };

            // Kick them all off
            var tasks = providers.Select(provider => provider.AuthorizationScopeAsync(cancellationToken)).ToList();

            // Wait to finish
            var results = await Task.WhenAll(tasks).ConfigureAwait(false);

            // Now aggregate them
            var activities = new List<Activity>();
            foreach (var result in results)
            {
                if (result == null)
                {
                    // Ignore if we don't get anything
                    continue;                    
                }

                // Last one wins on the global properties
                if (!string.IsNullOrEmpty(result.DefaultActivity))
                {
                    scope.DefaultActivity = result.DefaultActivity;
                }

                if (result.DefaultAuthorization.HasValue)
                {
                    scope.DefaultAuthorization = result.DefaultAuthorization;
                }

                if (result.AllowUnauthenticated.HasValue)
                {
                    scope.AllowUnauthenticated = result.AllowUnauthenticated;
                }

                activities.AddRange(result.Activities);
            }

            scope.Activities = activities;

            return scope;
        }
    }
}