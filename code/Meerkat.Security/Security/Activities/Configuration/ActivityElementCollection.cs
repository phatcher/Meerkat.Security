namespace Meerkat.Security.Activities.Configuration
{
    public class ActivityElementCollection : NamedConfigElementCollection<ActivityElement>
    {
        protected override string ElementName
        {
            get { return "activity"; }
        }
    }
}