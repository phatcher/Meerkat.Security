using System.Configuration;

namespace Meerkat.Configuration
{
    /// <summary>
    /// Provides a generic collection of <see cref="NamedConfigElement" />, implementing all necessary behaviour for client types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NamedConfigElementCollection<T> : ConfigElementCollection<T>
        where T : NamedConfigElement, new()
    {
        protected override bool IsElementName(string elementName)
        {
            return !string.IsNullOrEmpty(elementName) && elementName == ElementName;
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((T)element).Name;
        }
    }
}