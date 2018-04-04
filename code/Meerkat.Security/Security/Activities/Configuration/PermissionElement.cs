using System.Configuration;

namespace Meerkat.Security.Activities.Configuration
{
    public class PermissionElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the roles
        /// </summary>
        [ConfigurationProperty("roles", IsRequired = false, IsKey = false)]
        public string Roles
        {
            get => this["roles"] as string;
            set => this["roles"] = value;
        }

        /// <summary>
        /// Gets or sets the users
        /// </summary>
        [ConfigurationProperty("users", IsRequired = false, IsKey = false)]
        public string Users
        {
            get => this["users"] as string;
            set => this["users"] = value;
        }

        /// <summary>
        /// Gets or sets the claims
        /// </summary>
        [ConfigurationProperty("", IsDefaultCollection = true, IsKey = false, IsRequired = true)]
        public ClaimElementCollection Claims
        {
            get => (ClaimElementCollection)this[""];
            set => this[""] = value;
        }
    }
}