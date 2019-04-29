namespace Meerkat.Security.Web
{
    public static class ControllerActivityMapperExtensions
    {
        /// <summary>
        /// Determine the resource name to use.
        /// </summary>
        /// <param name="mapper">Mapper to use</param>
        /// <param name="controller">Controller name to use</param>
        /// <param name="action">Action name to use</param>
        /// <returns>The resource name as mapped</returns>
        public static string Resource(this IControllerActivityMapper mapper, string controller, string action)
        {
            var result = mapper.Map(controller, action);
            return result?.Item1;
        }

        /// <summary>
        /// Determine the action name to use.
        /// </summary>
        /// <param name="mapper">Mapper to use</param>
        /// <param name="controller">Controller name to use</param>
        /// <param name="action">Action name to use</param>
        /// <returns>The action name as mapped</returns>
        public static string Action(this IControllerActivityMapper mapper, string controller, string action)
        {
            var result = mapper.Map(controller, action);
            return result?.Item2;
        }
    }
}