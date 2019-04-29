using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

using Meerkat.Caching;

namespace Meerkat.Security.Activities
{
    /// <summary>
    /// Caching version of a <see cref="IActivityAuthorizer"/>.
    /// </summary>
#if NETSTANDARD
    [Obsolete("Use ActivityAuthorizerCache/ActivityAuthorizerMemoryCache/ActivityAuthorizerDistributedCache")]
#endif
    public class CachingActivityAuthorizer : IActivityAuthorizer
    {
        private readonly IActivityAuthorizer authorizer;
        private readonly ICache cache;
        private readonly TimeSpan duration;

        /// <summary>
        /// Creates a new instance of the <see cref="CachingActivityAuthorizer"/> class
        /// </summary>
        /// <param name="authorizer">Authorizer to use</param>
        /// <param name="cache">Cache to use</param>
        /// <param name="duration">Cache duration (default: 5 mins)</param>
        public CachingActivityAuthorizer(IActivityAuthorizer authorizer, ICache cache, TimeSpan? duration = null)
        {
            this.authorizer = authorizer;
            this.cache = cache;
            this.duration = duration ?? TimeSpan.FromMinutes(5);
        }

        /// <copydoc cref="IActivityAuthorizer.IsAuthorized" />
        public AuthorizationReason IsAuthorized(string resource, string action, IPrincipal principal, IDictionary<string, object> values = null)
        {
            return IsAuthorizedAsync(resource, action, principal, values).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public Task<AuthorizationReason> IsAuthorizedAsync(string resource, string action, IPrincipal principal, IDictionary<string, object> values = null, CancellationToken token = default(CancellationToken))
        {
            // TODO: Use AuthorizationReason.Expiry to influence cache duration
            return cache.AddOrGetExistingAsync(Key(resource, action, principal, values), async () => await authorizer.IsAuthorizedAsync(resource, action, principal, values, token).ConfigureAwait(false), DateTimeOffset.UtcNow.Add(duration), "authorizer");
        }

        private string Key(string resource, string action, IPrincipal principal, IDictionary<string, object> values)
        {
            // TODO: Work out how/if we include values in the key
            return $"{resource}:{action}:{principal}";
        }
    }
}