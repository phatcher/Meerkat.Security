using System;
using System.Collections.Generic;
using System.Configuration;

using Meerkat.Security.Activities.Configuration;

namespace Meerkat.Security.Activities
{
    /// <summary>
    /// Implements <see cref="IActivityProvider"/> by reading a named configuration section
    /// </summary>
    public class ConfigurationActivityProvider : IActivityProvider
    {
        private readonly ActivityAuthorizationSection section;
        private IList<Activity> activities;

        /// <summary>
        /// Creates a new instance of the <see cref="ConfigurationActivityProvider"/> class.
        /// </summary>
        /// <param name="sectionName"></param>
        public ConfigurationActivityProvider(string sectionName = "activityAuthorization")
        {
            section = ActivitySection(sectionName);
            activities = new List<Activity>();
        }

        /// <copydoc cref="IActivityProvider.Activities" />
        public IList<Activity> Activities()
        {
            if (activities.Count == 0 && section != null)
            {
                activities = section.ToActivitites();
            }

            return activities;
        }

        /// <copydoc cref="IActivityProvider.DefaultActivity" />
        /// <remarks>Returns the <see cref="ActivityAuthorizationSection.DefaultActivity"/> value.</remarks>
        public string DefaultActivity()
        {
            return section != null ? section.DefaultActivity : null;
        }

        /// <copydoc cref="IActivityProvider.DefaultAllowUnauthenticated" />
        /// <remarks>Returns the <see cref="ActivityAuthorizationSection.DefaultAllowUnauthenticated"/> value.</remarks>
        public bool? DefaultAllowUnauthenticated()
        {
            return section != null ? section.DefaultAllowUnauthenticated : null;
        }

        /// <copydoc cref="IActivityProvider.DefaultAuthorization" />
        /// <remarks>Returns the <see cref="ActivityAuthorizationSection.Default"/> value.</remarks>
        public bool? DefaultAuthorization()
        {
            return section != null ? (bool?)section.Default : null;
        }

        private ActivityAuthorizationSection ActivitySection(string name)
        {
            try
            {
                return (ActivityAuthorizationSection)ConfigurationManager.GetSection(name);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
