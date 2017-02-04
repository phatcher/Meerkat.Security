using System.Configuration;

namespace Meerkat.Security.Activities.Configuration
{
    public class ActivityElement : NamedConfigElement
    {
        [ConfigurationProperty("resource", IsRequired = false, IsKey = false)]
        public string Resource
        {
            get { return (string) base["resource"]; }
            set { base["resource"] = value; }
        }

        [ConfigurationProperty("action", IsRequired = false, IsKey = false)]
        public string Action
        {
            get { return (string)base["action"]; }
            set { base["action"] = value; }
        }

        [ConfigurationProperty("allowUnauthenticated", IsRequired = false, IsKey = false)]
        public bool? AllowUnauthenticated
        {
            get { return (bool?)base["allowUnauthenticated"]; }
            set { base["allowUnauthenticated"] = value; }
        }

        [ConfigurationProperty("authorized", IsRequired = false, IsKey = false)]
        public bool? Default
        {
            get { return (bool?)base["authorized"]; }
            set { base["authorized"] = value; }
        }

        [ConfigurationProperty("allow", IsRequired = false, IsKey = false)]
        public PermissionElement Allow
        {
            get { return base["allow"] as PermissionElement; }
            set { base["allow"] = value; }
        }

        [ConfigurationProperty("deny", IsRequired = false, IsKey = false)]
        public PermissionElement Deny
        {
            get { return base["deny"] as PermissionElement; }
            set { base["deny"] = value; }
        }
    }
}