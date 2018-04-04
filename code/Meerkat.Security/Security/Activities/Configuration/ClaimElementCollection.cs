namespace Meerkat.Security.Activities.Configuration
{
    public class ClaimElementCollection : NamedConfigElementCollection<ClaimElement>
    {
        protected override string ElementName => "claim";
    }
}