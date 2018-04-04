using System.Configuration;

namespace Meerkat.Security.Activities.Configuration
{
    public class ClaimElement : NamedConfigElement
    {
        /// <summary>
        /// Gets or sets the issuer
        /// </summary>
        [ConfigurationProperty("issuer", IsRequired = false, IsKey = false)]
        public string Issuer
        {
            get => this["issuer"] as string;
            set => this["issuer"] = value;
        }

        /// <summary>
        /// Gets or sets the claims
        /// </summary>
        [ConfigurationProperty("values", IsRequired = true, IsKey = false)]
        public string Claims
        {
            get => this["values"] as string;
            set => this["values"] = value;
        }
    }
}