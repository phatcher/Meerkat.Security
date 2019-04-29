#if NETSTANDARD
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

using Meerkat.Caching;

using Microsoft.Extensions.Caching.Memory;

namespace Meerkat.Security.Activities
{
    /// <summary>
    /// A caching <see cref="IActivityAuthorizer"/> using a <see cref="IMemoryCache"/>.
    /// </summary>
    public class ActivityAuthorizerMemoryCache : IActivityAuthorizer
    {
        private const string CacheRegion = "activities";

        private readonly IActivityAuthorizer service;
        private readonly IMemoryCache cache;
        private readonly TimeSpan duration;

        /// <summary>
        /// Creates a new instance of the <see cref="ActivityAuthorizerMemoryCache"/> class.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="cache"></param>
        /// <param name="duration"></param>
        public ActivityAuthorizerMemoryCache( IActivityAuthorizer service, IMemoryCache cache, TimeSpan? duration = null)
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

            var result = await cache.GetOrCreateAsync(key, entry =>
            {
                entry.AbsoluteExpiration = DateTimeOffset.UtcNow.Add(duration);
                return service.IsAuthorizedAsync(resource, action, principal, values, cancellationToken);
            });

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