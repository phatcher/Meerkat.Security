using Meerkat.Configuration;

namespace Meerkat.Security.Activities.Configuration
{
    public class ClaimElementCollection : NamedConfigElementCollection<ClaimElement>
    {
        protected override string ElementName
        {
            get { return "claim"; }
        }
    }
}