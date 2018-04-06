using System.Configuration;

namespace Meerkat.Security.Activities.Configuration
{
    /// <summary>
    /// Configuration section for activity authorization.
    /// </summary>
    public class ActivityAuthorizationSection : ConfigurationSection
    {
        [ConfigurationProperty("name", DefaultValue = null, IsRequired = false, IsKey = false)]
        public string Name
        {
            get => (string)base["name"];
            set => base["name"] = value;
        }

        /// <summary>
        /// Default authorization behaviour for the section (default: false).
        /// </summary>
        [ConfigurationProperty("authorized", DefaultValue = false, IsRequired = false, IsKey = false)]
        public bool Default
        {
            get => (bool)base["authorized"];
            set => base["authorized"] = value;
        }

        /// <summary>
        /// Default allow unauthenticated behaviour for the section (default: null).
        /// </summary>
        [ConfigurationProperty("allowUnauthenticated", IsRequired = false, IsKey = false)]
        public bool? DefaultAllowUnauthenticated
        {
            get => (bool?)base["allowUnauthenticated"];
            set => base["allowUnauthenticated"] = value;
        }

        /// <summary>
        /// Default activity to authorize against if specified activity not found (default: null).
        /// </summary>
        [ConfigurationProperty("defaultActivity", DefaultValue = null, IsRequired = false, IsKey = false)]
        public string DefaultActivity
        {
            get => (string)base["defaultActivity"];
            set => base["defaultActivity"] = value;
        }

        /// <summary>
        /// Gets or sets the activities
        /// </summary>
        [ConfigurationProperty("", IsDefaultCollection = true, IsKey = false, IsRequired = true)]
        public ActivityElementCollection Activities
        {
            get => (ActivityElementCollection)this[""];
            set => this[""] = value;
        }
    }
}