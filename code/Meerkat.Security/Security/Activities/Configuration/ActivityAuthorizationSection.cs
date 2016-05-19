using System.Configuration;

namespace Meerkat.Security.Activities.Configuration
{
    /// <summary>
    /// Configuration section for activity authorization.
    /// </summary>
    public class ActivityAuthorizationSection : ConfigurationSection
    {
        /// <summary>
        /// Default authorization behaviour for the section (default: false).
        /// </summary>
        [ConfigurationProperty("authorized", DefaultValue = false, IsRequired = false, IsKey = false)]
        public bool Default
        {
            get { return (bool)base["authorized"]; }
            set { base["authorized"] = value; }
        }

        /// <summary>
        /// Default activity to authorize against if specified activity not found (default: null).
        /// </summary>
        [ConfigurationProperty("defaultActivity", DefaultValue = null, IsRequired = false, IsKey = false)]
        public string DefaultActivity
        {
            get { return (string)base["defaultActivity"]; }
            set { base["defaultActivity"] = value; }
        }

        /// <summary>
        /// Gets or sets the activities
        /// </summary>
        [ConfigurationProperty("", IsDefaultCollection = true, IsKey = false, IsRequired = true)]
        public ActivityElementCollection Activities
        {
            get { return (ActivityElementCollection)this[""]; }
            set { this[""] = value; }
        }
    }
}