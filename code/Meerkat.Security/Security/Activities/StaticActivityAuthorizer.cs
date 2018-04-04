using System.Collections.Generic;
using System.Threading.Tasks;

namespace Meerkat.Security.Activities
{
    /// <summary>
    /// Implementation of <see cref="IActivityProvider"/> that is initialized statically.
    /// </summary>
    public class StaticActivityProvider : IActivityProvider
    {
        private readonly IList<Activity> activities;
        private readonly string defaultActivity;
        private readonly bool? defaultAuthorization;
        private readonly bool? defaultAllowUnauthenticated;

        public StaticActivityProvider(IList<Activity> activities, bool? defaultAuthorization = null, string defaultActivity = null, bool? defaultAllowUnauthenticated = null)
        {
            this.activities = activities;
            this.defaultAuthorization = defaultAuthorization;
            this.defaultActivity = defaultActivity;
            this.defaultAllowUnauthenticated = defaultAllowUnauthenticated;
        }

        /// <copydoc cref="IActivityProvider.Activities" />
        public IList<Activity> Activities()
        {
            return activities;
        }

        /// <copydoc cref="IActivityProvider.ActivitiesAsync" />
        public Task<IList<Activity>> ActivitiesAsync()
        {
            return Task.FromResult(Activities());
        }

        /// <copydoc cref="IActivityProvider.DefaultActivity" />
        public string DefaultActivity()
        {
            return defaultActivity;
        }

        /// <copydoc cref="IActivityProvider.DefaultAllowUnauthenticated" />
        public bool? DefaultAllowUnauthenticated()
        {
            return defaultAllowUnauthenticated;
        }

        /// <copydoc cref="IActivityProvider.DefaultAuthorization" />
        public bool? DefaultAuthorization()
        {
            return defaultAuthorization;
        }
    }
}