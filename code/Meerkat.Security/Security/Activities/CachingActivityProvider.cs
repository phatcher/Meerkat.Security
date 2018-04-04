using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Meerkat.Caching;

namespace Meerkat.Security.Activities
{
    /// <summary>
    /// Caching version of a <see cref="IActivityProvider"/>.
    /// </summary>
    public class CachingActivityProvider : IActivityProvider
    {
        private const string CacheRegion = "activityProvider";

        private readonly IActivityProvider provider;
        private readonly ICache cache;
        private readonly TimeSpan duration;

        /// <summary>
        /// Creates a new instance of the <see cref="CachingActivityProvider"/> class.
        /// </summary>
        /// <param name="provider">Activity provider to use</param>
        /// <param name="cache">Cache to use</param>
        /// <param name="duration">Cache duration to use (default: 5 mins)</param>
        public CachingActivityProvider(IActivityProvider provider, ICache cache, TimeSpan? duration)
        {
            this.provider = provider;
            this.cache = cache;
            this.duration = duration ?? TimeSpan.FromMinutes(5);
        }

        /// <copydoc cref="IActivityProvider.Activities" />
        public IList<Activity> Activities()
        {
            return AddOrGetExisting("activities", () => provider.Activities());
        }


        /// <copydoc cref="IActivityProvider.Activities" />
        public async Task<IList<Activity>> ActivitiesAsync()
        {
            return await AddOrGetExistingAsync("activities", async () => await provider.ActivitiesAsync().ConfigureAwait(false)).ConfigureAwait(false);
        }

        /// <copydoc cref="IActivityProvider.DefaultActivity" />

        public string DefaultActivity()
        {
            return AddOrGetExisting("defaultActivity", () => provider.DefaultActivity());
        }

        /// <copydoc cref="IActivityProvider.DefaultAllowUnauthenticated" />

        public bool? DefaultAllowUnauthenticated()
        {
            return AddOrGetExisting("defaultAllowUnauthenticated", () => provider.DefaultAllowUnauthenticated());
        }

        /// <copydoc cref="IActivityProvider.DefaultAuthorization" />

        public bool? DefaultAuthorization()
        {
            return AddOrGetExisting("defaultAuthorization", () => provider.DefaultAuthorization());
        }

        private T AddOrGetExisting<T>(string key, Func<T> func)
        {
            return cache.AddOrGetExisting<T>(key, func, DateTimeOffset.UtcNow.Add(duration), CacheRegion);
        }

        private Task<T> AddOrGetExistingAsync<T>(string key, Func<Task<T>> func)
        {
            return cache.AddOrGetExistingAsync(key, func, DateTimeOffset.UtcNow.Add(duration), CacheRegion);
        }
    }
}