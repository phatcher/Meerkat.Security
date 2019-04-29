#if NETSTANDARD
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

using Meerkat.Caching;

using Polly.Caching;

namespace Meerkat.Security.Activities
{
    /// <summary>
    /// A caching <see cref="IActivityAuthorizer"/> using a <see cref="CachePolicy"/>
    /// </summary>
    public class ActivityAuthorizerCache : IActivityAuthorizer
    {
        private readonly IActivityAuthorizer service;
        private readonly CachePolicy policy;

        /// <summary>
        /// Creates a new instance of the <see cref="ActivityAuthorizerCache"/> class.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="policy"></param>
        public ActivityAuthorizerCache(IActivityAuthorizer service, CachePolicy policy)
        {
            this.service = service;
            this.policy = policy;
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

            var result = await policy.Cache(key, () => service.IsAuthorizedAsync(resource, action, principal, values, cancellationToken));

            return result;
        }

        private string Key(string resource, string action, ClaimsPrincipal principal, IDictionary<string, object> values)
        {
            // TODO: Work out how/if we include values in the key
            return this.CacheKey(resource, action, principal.ToCacheKey());
        }
    }
}
#endif