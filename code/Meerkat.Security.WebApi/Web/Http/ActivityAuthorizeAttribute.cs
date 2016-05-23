using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.ServiceModel.Channels;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

using Meerkat.Security.Activities;

using AllowAnonymousAttribute = System.Web.Http.AllowAnonymousAttribute;

namespace Meerkat.Security.Web.Http
{
    /// <summary>
    /// An authorization filter that verifies the request's <see cref="IPrincipal"/> against a <see cref="IActivityAuthorizer"/>
    /// </summary>
    /// <remarks>
    /// You can declare multiples of these attributes per action. You can also use <see cref="System.Web.Http.AllowAnonymousAttribute"/> to disable
    /// authorization for a specific action.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ActivityAuthorizeAttribute : AuthorizationFilterAttribute
    {
        private readonly object typeId;

        public ActivityAuthorizeAttribute()
        {
            typeId = new object();
        }

        /// <summary>
        /// Gets or sets the activity to authorize, 
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
        /// Get a unique identifier for this <see cref="Attribute"/>
        /// </summary>
        /// <returns>The unique identifier for the attribute.</returns>
        public override object TypeId
        {
            get { return typeId; }
        }

        /// <summary>
        /// Determines whether access for this particular <paramref name="actionContext"/> is authorized. This method uses te user
        /// <see cref="IPrincipal"/> returned via <see cref="M:HttpRequestMessageExtensions.GetUserPrincipal"/>.
        /// Authorization is denied if
        /// <list type="bullet">
        /// <item>
        /// <description>the request is not associated with any user.</description>
        /// </item>
        /// <item>
        /// <description>the user is not authenticated,</description>
        /// </item>
        /// <item>
        /// <description>the user is authenticated but it not authorized for the activity by the <see cref="IActivityAuthorizer"/></description>
        /// </item>
        /// </list>
        /// <para>
        /// If the <see cref="P:Activity"/> is empty a value is contructed from the controller and action names.
        /// </para>
        /// <para>
        /// If authorization is denied then this method will invoke <see cref="M:HandleUnauthorizedRequest"/> to process the unauthorized request.
        /// </para>
        /// </summary>
        /// <remarks>
        /// You can use he <see cref="System.Web.Http.AllowAnonymousAttribute"/> to cause authorization checks to be skipped for a particular action or controller.
        /// </remarks>
        /// <param name="actionContext">The action context</param>
        /// <returns><c>true</c> if access is authorized; otherwise <c>false</c>.</returns>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }

            if (SkipAuthorization(actionContext))
            {
                return;
            }

            if (!AuthorizeCore(actionContext))
            {
                HandleUnauthorizedRequest(actionContext);
            }
        }

        protected virtual bool AuthorizeCore(HttpActionContext actionContext)
        {
            var request = actionContext.ControllerContext.Request;

            var principal = request.GetUserPrincipal() ?? Thread.CurrentPrincipal;
            if (principal == null || !principal.Identity.IsAuthenticated)
            {
                return false;
            }

            // Work out which activity to check, can't override Resource/Action as we might be a global filter.
            var resource = !string.IsNullOrEmpty(Resource) ? Resource : InferredResource(request);
            var action = !string.IsNullOrEmpty(Action) ? Action : InferredAction(request);

            // Now check if we are authorized.
            var authorizer = Authorizer(actionContext);
            var reason = authorizer.IsAuthorized(resource, action, principal);

            return reason.IsAuthorized;         
        }

        protected virtual string InferredResource(HttpRequestMessage request)
        {
            var routeData = request.GetRouteData();
            var controller = routeData.Values["controller"] as string;
            return controller;
        }

        protected virtual string InferredAction(HttpRequestMessage request)
        {
            var routeData = request.GetRouteData();
            var action = routeData.Values["action"] as string;
            return action;
        }

        protected virtual void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.ControllerContext.Request.CreateResponse(HttpStatusCode.Forbidden);
        }

        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                   || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }

        private IActivityAuthorizer Authorizer(HttpActionContext actionContext)
        {
            return (IActivityAuthorizer) actionContext.ControllerContext.Configuration.DependencyResolver.GetService(typeof(IActivityAuthorizer));
        }
    }
}