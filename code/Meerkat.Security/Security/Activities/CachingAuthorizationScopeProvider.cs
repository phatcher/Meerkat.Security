using System;
using System.Threading;
using System.Threading.Tasks;

using Meerkat.Caching;

namespace Meerkat.Security.Activities
{
    /// <summary>
    /// Caches the output from another <see cref="IAuthorizationScopeProvider"/>.
    /// </summary>
    public class CachingAuthorizationScopeProvider : IAuthorizationScopeProvider
    {
        private const string CacheRegion = "activityProvider";

        private readonly IAuthorizationScopeProvider provider;
        private readonly ICache cache;
        private readonly TimeSpan duration;

        /// <summary>
        /// Creates a new instance of the <see cref="CachingAuthorizationScopeProvider"/> class.
        /// </summary>
        /// <param name="provider">Activity provider to use</param>
        /// <param name="cache">Cache to use</param>
        /// <param name="duration">Cache duration to use (default: 5 mins)</param>
        public CachingAuthorizationScopeProvider(IAuthorizationScopeProvider provider, ICache cache, TimeSpan? duration)
        {
            this.provider = provider;
            this.cache = cache;
            this.duration = duration ?? TimeSpan.FromMinutes(5);
        }

        public async Task<AuthorizationScope> AuthorizationScopeAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // TODO: With this design we can only cache one provider which might be issue with multiple microservices.
            // Might need the provider to have a name property so we can record it against that
            return await AddOrGetExistingAsync("activities", async () => await provider.AuthorizationScopeAsync(cancellationToken).ConfigureAwait(false)).ConfigureAwait(false);
        }

        private Task<T> AddOrGetExistingAsync<T>(string key, Func<Task<T>> func)
        {
            return cache.AddOrGetExistingAsync(key, func, DateTimeOffset.UtcNow.Add(duration), CacheRegion);
        }
    }
}
