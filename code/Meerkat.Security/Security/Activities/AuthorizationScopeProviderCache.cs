#if NETSTANDARD
using System.Threading;
using System.Threading.Tasks;

using Meerkat.Caching;

using Polly.Caching;

namespace Meerkat.Security.Activities
{
    /// <summary>
    /// A caching <see cref="IAuthorizationScopeProvider"/> using a <see cref="CachePolicy"/>.
    /// </summary>
    public class AuthorizationScopeProviderCache : IAuthorizationScopeProvider
    {
        private readonly IAuthorizationScopeProvider service;
        private readonly CachePolicy policy;

        public AuthorizationScopeProviderCache(IAuthorizationScopeProvider service, CachePolicy policy)
        {
            this.service = service;
            this.policy = policy;
        }

        /// <copydoc cref="IAuthorizationScopeProvider.AuthorizationScopeAsync" />
        public async Task<AuthorizationScope> AuthorizationScopeAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // TODO: With this design we can only cache one provider which might be issue with multiple micro-services.
            // Might need the provider to have a name property so we can record it against that or we always cache the AggregateScopeProvider
            var key = this.CacheKey();

            var result = await policy.Cache(key, () => service.AuthorizationScopeAsync(cancellationToken));

            return result;
        }
    }
}
#endif