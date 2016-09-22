using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Principal;
using System.ServiceModel.Channels;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

using Common.Logging;

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
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly object typeId;

        public ActivityAuthorizeAttribute()
        {
            typeId = new object();
        }

        /// <summary>
        /// Gets or sets the activity to authorize, 
        /// </summary>
        /// <remarks>
        /// If not set will default to the current Controller
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
        /// If the <see cref="P:Resource"/> or <see cref="P:Action"/> properties are empty, the controller and action names are passed to the <see cref="IControllerActivityMapper"/>.
        /// </para>
        /// <para>
        /// If authorization is denied then this method will invoke <see cref="M:HandleUnauthorizedRequest"/> to process the unauthorized request.
        /// </para>
        /// </summary>
        /// <remarks>
        /// You can use he <see cref="System.Web.Http.AllowAnonymousAttribute"/> to cause authorization checks to be skipped for a particular action or controller.
        /// Alternatively the appropriate <see cref="Activity.AllowUnauthenticated"/> property can be set.
        /// </remarks>
        /// <param name="actionContext">The action context</param>
        /// <returns><c>true</c> if access is authorized; otherwise <c>false</c>.</returns>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext));
            }

            if (!AuthorizeCore(actionContext))
            {
                HandleUnauthorizedRequest(actionContext);
            }
        }

        protected virtual bool AuthorizeCore(HttpActionContext actionContext)
        {
            try
            {
                var request = actionContext.ControllerContext.Request;
                var controller = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
                var controllerAction = actionContext.ActionDescriptor.ActionName;

                if (SkipAuthorization(actionContext))
                {
                    Logger.InfoFormat("Skipping authorization check for {0}/{1}", controller, controllerAction);
                    return true;
                }

                var principal = CurrentUser(request);

                // Take a copy, we might be a global filter so can't override the property values
                var resource = Resource;
                var action = Action;

                // If nothing is specified, we need to infer the resource/action
                if (string.IsNullOrEmpty(resource) || string.IsNullOrEmpty(action))
                {
                    var service = GetService<IControllerActivityMapper>(actionContext);
                    var activity = service.Map(controller, controllerAction);
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
                var authorizer = GetService<IActivityAuthorizer>(actionContext);
                var reason = authorizer.IsAuthorized(resource, action, principal);

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

        protected virtual void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            try
            {
                actionContext.Response = actionContext.ControllerContext.Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        protected virtual bool SkipAuthorization(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any() ||
                   actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }

        /// <summary>
        /// Get the current user or the thread identity if not set (un-authenticated)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private IPrincipal CurrentUser(HttpRequestMessage request)
        {
            try
            {
                return request.GetUserPrincipal() ?? Thread.CurrentPrincipal;
            }
            catch (Exception)
            {
                return Thread.CurrentPrincipal;
            }
        }

        private T GetService<T>(HttpActionContext actionContext)
        {
            try
            {
                return (T)actionContext.ControllerContext.Configuration.DependencyResolver.GetService(typeof(T));
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Could not resolve {0} - {1}", typeof(T).Name, ex.Message);
                throw;
            }
        }
    }
}