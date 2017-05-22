using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

using Meerkat.Logging;
using Meerkat.Security.Activities;
using Meerkat.Security.Web;

namespace Meerkat.Web.Mvc
{
    /// <summary>
    /// Uses <see cref="IActivityAuthorizer"/> to authorize a controller action.
    /// <para>
    /// If the <see cref="P:Resource"/> and <see cref="P:Action" /> are empty values, the controller and action names are passed to the <see cref="IControllerActivityMapper"/>.
    /// </para> 
    /// </summary>
    public class ActivityAuthorizeAttribute : AuthorizeAttribute
    {
        private static readonly ILog Logger = LogProvider.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private IActivityAuthorizer authorizer;
        private IControllerActivityMapper inferrer;

        /// <summary>
        /// Gets or sets the resource to authorize, 
        /// </summary>
        /// <remarks>
        /// If not set will default to the current Controller and then interpreted by the <see cref="IControllerActivityMapper"/>
        /// </remarks>
        public string Resource { get; set; }

        /// <summary>
        /// Gets or sets the resource to authorize, 
        /// </summary>
        /// <remarks>
        /// If not set will default to the current controller Action and then interpreted by the <see cref="IControllerActivityMapper"/>
        /// </remarks>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the URL to use on failure of authorization.
        /// </summary>
        public string RedirectUrl { get; set; }

        public IActivityAuthorizer Authorizer
        {
            // NB Not great, but avoids coupling this class to a specific IoC container.
            get { return authorizer ?? (authorizer = DependencyResolver.Current.GetService<IActivityAuthorizer>()); }
            set { authorizer = value; }
        }

        public IControllerActivityMapper Inferrer
        {
            // NB Not great, but avoids coupling this class to a specific IoC container.
            get { return inferrer ?? (inferrer = DependencyResolver.Current.GetService<IControllerActivityMapper>()); }
            set { inferrer = value; }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            try
            {
                var principal = httpContext.User;
                if (principal == null)
                {
                    Logger.Warn("No principal");
                    return false;
                }

                // Check for basic user and role authentication only if we are authenticated
                // Core immedidately fails for unauthenticated and we have our own checks in the IsAuthorized method
                if (principal.Identity.IsAuthenticated && !base.AuthorizeCore(httpContext))
                {
                    return false;
                }

                var routeData = httpContext.Request.RequestContext.RouteData;

                // NB These can fail if there is no controller/action value, but we have no access to the ActionDescriptor!
                var controller = routeData.GetRequiredString("controller");
                var controllerAction = routeData.GetRequiredString("action");

                // Take a copy, we might be a global filter so can't override the property values
                var resource = Resource;
                var action = Action;

                // If nothing is specified, we need to infer the resource/action
                if (string.IsNullOrEmpty(resource) || string.IsNullOrEmpty(action))
                {
                    var activity = Inferrer.Map(controller, controllerAction);
                    if (string.IsNullOrEmpty(resource))
                    {
                        resource = activity.Item1;
                    }
                    if (string.IsNullOrEmpty(action))
                    {
                        action = activity.Item2;
                    }
                }

                // Now check if we are authorised
                var reason = Authorizer.IsAuthorized(resource, action, principal);

                Logger.DebugFormat("Authorizing {0}/{1} ({4}.{5}) for '{2}': {3}", controller, controllerAction, principal.Identity.Name, reason.IsAuthorized, resource, action);

                return reason.IsAuthorized;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);

                // Fail-safe by denying access.
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated && !string.IsNullOrEmpty(RedirectUrl))
            {
                // User is logged in and we have another url
                filterContext.Result = new RedirectResult(RedirectUrl);
            }
            else
            {
                // Do the base 
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}