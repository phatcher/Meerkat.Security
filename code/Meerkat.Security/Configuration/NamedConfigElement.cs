using System.Configuration;

namespace Meerkat.Configuration
{
    /// <summary>
    /// Extends <see cref="Configuration" /> with a standard name property which is required an is the key of the element.
    /// </summary>
    public class NamedConfigElement : ConfigurationElement
    {
        public NamedConfigElement()
        {            
        }

        public NamedConfigElement(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Get or set the Name property.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public virtual string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }
    }
}