#if NETSTANDARD
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

using Meerkat.Caching;

using Microsoft.Extensions.Caching.Distributed;

namespace Meerkat.Security.Activities
{
    /// <summary>
    /// A caching <see cref="IActivityAuthorizer"/> using a <see cref="IDistributedCache"/>.
    /// </summary>
    public class ActivityAuthorizerDistributedCache : IActivityAuthorizer
    {
        private const string CacheRegion = "activities";

        private readonly IActivityAuthorizer service;
        private readonly IDistributedCache cache;
        private readonly TimeSpan duration;

        public ActivityAuthorizerDistributedCache(IActivityAuthorizer service, IDistributedCache cache, TimeSpan? duration = null)
        {
            this.service = service;
            this.cache = cache;
            this.duration = duration ?? TimeSpan.FromMinutes(5);
        }

        /// <copydoc cref="IActivityAuthorizer.IsAuthorized" />
        public AuthorizationReason IsAuthorized(string resource, string action, IPrincipal principal, IDictionary<string, object> values = null)
        {
            return IsAuthorizedAsync(resource, action, principal, values).GetAwaiter().GetResult();
        }

        /// <copydoc cref="IActivityAuthorizer.IsAuthorizedAsync" />
        public async Task<AuthorizationReason> IsAuthorizedAsync(string resource, string action, IPrincipal principal, IDictionary<string, object> values = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var key = Key(resource, action, principal as ClaimsPrincipal, values);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = duration
            };

            var result = await cache.GetOrCreateAsync(key, () => service.IsAuthorizedAsync(resource, action, principal, values, cancellationToken), options, cancellationToken);

            return result;
        }

        private string Key(string resource, string action, ClaimsPrincipal principal, IDictionary<string, object> values)
        {
            // TODO: Work out how/if we include values in the key
            return CacheRegion.CacheKey(resource, action, principal.ToCacheKey());
        }
    }
}
#endif