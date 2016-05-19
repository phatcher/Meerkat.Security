using System.Web;
using System.Web.Mvc;

using Meerkat.Security.Activities;

namespace Meerkat.Web.Mvc
{
    /// <summary>
    /// Uses <see cref="IActivityAuthorizer"/> to authorize a controller action
    /// </summary>
    public class ActivityAuthorizeAttribute : AuthorizeAttribute
    {
        private IActivityAuthorizer authorizer;

        /// <summary>
        /// Gets or sets the resource to authorize, 
        /// </summary>
        /// <remarks>
        /// If not set will default to the curruent Controller
        /// </remarks>
        public string Resource { get; set; }

        /// <summary>
        /// Gets or sets the resource to authorize, 
        /// </summary>
        /// <remarks>
        /// If not set will default to the current controller Action
        /// </remarks>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the URL to use on failure of authorization.
        /// </summary>
        public string RedirectUrl { get; set; }
        
        /// <summary>
        /// Gets or sets the activity to authorize, if not set will default to Controller.Action
        /// </summary>
        public string Activity { get; set; }

        public IActivityAuthorizer Authorizer
        {
            // NB Not great, but avoids coupling this class to a specific IoC container.
            get { return authorizer ?? (authorizer = DependencyResolver.Current.GetService<IActivityAuthorizer>()); }
            set { authorizer = value; }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {           
            // Check for basic user and role authentication
            if (!base.AuthorizeCore(httpContext))
            {
                return false; 
            }

            // Work out which activity to check, can't override Resource/Action as we might be a global filter.
            var resource = !string.IsNullOrEmpty(Resource) ? Resource : InferredResource(httpContext);
            var action = !string.IsNullOrEmpty(Action) ? Action : InferredAction(httpContext);

            // Now check if we are authorised
            var reason = Authorizer.IsAuthorized(resource, action, httpContext.User);

            return reason.IsAuthorised;         
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

        protected virtual string InferredResource(HttpContextBase httpContext)
        {
            var routeData = httpContext.Request.RequestContext.RouteData;
            var controller = routeData.GetRequiredString("controller");
            return controller;
        }

        protected virtual string InferredAction(HttpContextBase httpContext)
        {
            var routeData = httpContext.Request.RequestContext.RouteData;
            var action = routeData.GetRequiredString("action");
            return action;
        }
    }
}