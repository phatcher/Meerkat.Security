using System;
using System.Collections.Generic;

using Meerkat.Caching;

namespace Meerkat.Security.Activities
{
    public class CachingActivityProvider : IActivityProvider
    {
        private const string CacheRegion = "activityProvider";

        private readonly IActivityProvider provider;
        private readonly ICache cache;        

        public CachingActivityProvider(IActivityProvider provider, ICache cache)
        {
            this.provider = provider;
            this.cache = cache;
        }

        public IList<Activity> Activities()
        {
            return AddOrGetExisting("activities", () => provider.Activities());
        }

        public string DefaultActivity()
        {
            return AddOrGetExisting("defaultActivity", () => provider.DefaultActivity());
        }

        public bool? DefaultAuthorization()
        {
            return AddOrGetExisting("defaultAuthorization", () => provider.DefaultAuthorization());
        }

        private T AddOrGetExisting<T>(string key, Func<T> func)
        {
            return cache.AddOrGetExisting<T>(key, func, DateTimeOffset.UtcNow.AddMinutes(5), CacheRegion);
        }
    }
}