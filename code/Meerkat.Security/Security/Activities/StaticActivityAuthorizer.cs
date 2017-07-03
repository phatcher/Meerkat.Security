using System.Collections.Generic;
using System.Threading.Tasks;

namespace Meerkat.Security.Activities
{
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

        public IList<Activity> Activities()
        {
            return activities;
        }

        public Task<IList<Activity>> ActivitiesAsync()
        {
            return Task.FromResult(Activities());
        }

        public string DefaultActivity()
        {
            return defaultActivity;
        }

        public bool? DefaultAllowUnauthenticated()
        {
            return defaultAllowUnauthenticated;
        }

        public bool? DefaultAuthorization()
        {
            return defaultAuthorization;
        }
    }
}