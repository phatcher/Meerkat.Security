using System;

namespace Meerkat.Security.Web
{
    /// <summary>
    /// Infers the activity resource and action name from the web request
    /// </summary>
    public interface IControllerActivityMapper
    {
        /// <summary>
        /// Maps the controller/action onto a resource/action
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        Tuple<string, string> Map(string controller, string action);
    }
}