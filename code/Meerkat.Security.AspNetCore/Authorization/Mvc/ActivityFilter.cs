using System.Reflection;

using Meerkat.Logging;
using Meerkat.Security.Activities;
using Meerkat.Security.Web;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Meerkat.Security.Authorization.Mvc
{
    /// <summary>
    /// An <see cref="IAuthorizationFilter"/> that uses <see cref="IActivityAuthorizer"/> to confirm request authorization.
    /// </summary>
    public class ActivityFilter : IAuthorizationFilter
    {
        private static readonly ILog Logger = LogProvider.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Creates a new instance of the <see cref="ActivityFilter"/>
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="action"></param>
        public ActivityFilter(string resource, string action)
        {
            Resource = resource;
            Action = action;
        }

        public string Resource { get; }

        public string Action { get; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authorizer = context.HttpContext.RequestServices.GetService<IActivityAuthorizer>();

            var controllerActionDescription = context.ActionDescriptor as ControllerActionDescriptor;
            var controller = controllerActionDescription?.ControllerName;
            var controllerAction =  controllerActionDescription?.ActionName;
            var user = context.HttpContext.User;

            var resource = Resource;
            var action = Action;

            // If not specified, we need to infer the resource/action
            if (string.IsNullOrEmpty(resource) || string.IsNullOrEmpty(action))
            {
                var activityMapper = context.HttpContext.RequestServices.GetService<IControllerActivityMapper>();
                var activity = activityMapper.Map(controller, controllerAction);
                if (string.IsNullOrEmpty(resource))
                {
                    resource = activity.Item1;
                }
                if (string.IsNullOrEmpty(action))
                {
                    action = activity.Item2;
                }
            }

            var reason = authorizer.IsAuthorized(resource, action, user);

            Logger.DebugFormat("Authorizing {0}/{1} ({4}.{5}) for '{2}': {3}", controller, controllerAction, user.Identity.Name, reason.IsAuthorized, resource, action);

            if (reason.IsAuthorized)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}