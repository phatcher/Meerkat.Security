namespace Meerkat.Security.Web
{
    public static class ControllerActivityMapperExtensions
    {
        public static string Resource(this IControllerActivityMapper mapper, string controller, string action)
        {
            var result = mapper.Map(controller, action);
            return result != null ? result.Item1 : null;
        }

        public static string Action(this IControllerActivityMapper mapper, string controller, string action)
        {
            var result = mapper.Map(controller, action);
            return result != null ? result.Item2 : null;
        }
    }
}
