namespace Meerkat.Security.Web
{
    public static class ControllerActivityMapperExtensions
    {
        public static string Resource(this IControllerActivityMapper mapper, string controller, string action)
        {
            var result = mapper.Map(controller, action);
            return result?.Item1;
        }

        public static string Action(this IControllerActivityMapper mapper, string controller, string action)
        {
            var result = mapper.Map(controller, action);
            return result?.Item2;
        }
    }
}