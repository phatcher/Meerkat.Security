namespace Meerkat.Security.Activities.Configuration
{
    using System.Configuration;

    public class PermissionElement : ConfigurationElement
    {
        [ConfigurationProperty("roles", IsRequired = false, IsKey = false)]
        public string Roles
        {
            get { return this["roles"] as string; }
            set { this["roles"] = value; }
        }

        [ConfigurationProperty("users", IsRequired = false, IsKey = false)]
        public string Users
        {
            get { return this["users"] as string; }
            set { this["users"] = value; }
        }
    }
}