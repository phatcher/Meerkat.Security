using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meerkat.Security.Activities
{
    /// <summary>
    /// Aggregates multiple <see cref="IActivityProvider"/> together, last provider wins in terms of permissions
    /// </summary>
    public class AggregateActivityProvider : IActivityProvider
    {
        private readonly IList<IActivityProvider> providers;

        /// <summary>
        /// Creates a new instance of the <see cref="AggregateActivityProvider"/> class.
        /// </summary>
        /// <param name="providers"></param>
        public AggregateActivityProvider(IActivityProvider[] providers)
        {
            this.providers = new List<IActivityProvider>(providers);
        }

        /// <copydoc cref="IActivityProvider.Activities" />
        /// <remarks>Can potentially return duplicate <see cref="Activity"> if multiple providers supply the same activity</see></remarks>
        public IList<Activity> Activities()
        {
            var activities = new List<Activity>();
            foreach (var provider in providers)
            {
                activities.AddRange(provider.Activities());
            }

            return activities;
        }

        /// <copydoc cref="IActivityProvider.ActivitiesAsync" />
        /// <remarks>Can potentially return duplicate <see cref="Activity"> if multiple providers supply the same activity</see></remarks>
        public async Task<IList<Activity>> ActivitiesAsync()
        {
            // Kick them all off
            var tasks = providers.Select(provider => provider.ActivitiesAsync()).ToList();

            // Wait to finish
            var results = await Task.WhenAll(tasks).ConfigureAwait(false);

            // Now aggregate them
            var activities = new List<Activity>();
            foreach (var result in results)
            {
                activities.AddRange(result);
            }

            return activities;
        }

        /// <copydoc cref="IActivityProvider.DefaultActivity" />
        /// <remarks>Last non-null provider wins</remarks>
        public string DefaultActivity()
        {
            string defaultActivity = null;
            foreach (var provider in providers)
            {
                var value = provider.DefaultActivity();
                if (!string.IsNullOrEmpty(value))
                {
                    defaultActivity = value;
                }
            }

            return defaultActivity;
        }

        /// <copydoc cref="IActivityProvider.DefaultAllowUnauthenticated" />
        /// <remarks>Last non-null provider wins</remarks>
        public bool? DefaultAllowUnauthenticated()
        {
            bool? defaultAllowUnauthenticated = null;
            foreach (var provider in providers)
            {
                var value = provider.DefaultAllowUnauthenticated();
                if (value.HasValue)
                {
                    defaultAllowUnauthenticated = value;
                }
            }

            return defaultAllowUnauthenticated;
        }

        /// <copydoc cref="IActivityProvider.DefaultActivity" />
        /// <remarks>Last non-null provider wins</remarks>
        public bool? DefaultAuthorization()
        {
            bool? defaultAuthorization = null;
            foreach (var provider in providers)
            {
                var value = provider.DefaultAuthorization();
                if (value.HasValue)
                {
                    defaultAuthorization = value;
                }
            }

            return defaultAuthorization;
        }
    }
}