using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

using Meerkat.Security.Activities;

namespace Meerkat.Web.Mvc.Html
{
    public static class ActivityLinkExtensions
    {
        private static IActivityAuthorizer authorizer;

        public static IActivityAuthorizer Authorizer
        {
            // NB Not great, but avoids coupling this class to a specific IoC container.
            get { return authorizer ?? (authorizer = DependencyResolver.Current.GetService<IActivityAuthorizer>()); }
            set { authorizer = value; }
        }

        public static MvcHtmlString ActivityActionLink(this HtmlHelper htmlHelper, string linkText, string actionName)
        {
            return ActivityActionLink(htmlHelper, linkText, actionName, null /* controllerName */, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public static MvcHtmlString ActivityActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues)
        {
            return ActivityActionLink(htmlHelper, linkText, actionName, null /* controllerName */, new RouteValueDictionary(routeValues), new RouteValueDictionary());
        }

        public static MvcHtmlString ActivityActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues, object htmlAttributes)
        {
            return ActivityActionLink(htmlHelper, linkText, actionName, null /* controllerName */, new RouteValueDictionary(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString ActivityActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues)
        {
            return ActivityActionLink(htmlHelper, linkText, actionName, null /* controllerName */, routeValues, new RouteValueDictionary());
        }

        public static MvcHtmlString ActivityActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return ActivityActionLink(htmlHelper, linkText, actionName, null /* controllerName */, routeValues, htmlAttributes);
        }

        public static MvcHtmlString ActivityActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
        {
            return ActivityActionLink(htmlHelper, linkText, actionName, controllerName, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public static MvcHtmlString ActivityActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            return ActivityActionLink(htmlHelper, linkText, actionName, controllerName, new RouteValueDictionary(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString ActivityActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            if (string.IsNullOrEmpty(linkText))
            {
                throw new ArgumentException("Null or empty", "linkText");
            }

            var reason = htmlHelper.LinkAuthorisationReason(actionName, controllerName);
            return reason.IsAuthorised 
                 ? MvcHtmlString.Create(HtmlHelper.GenerateLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkText, null /* routeName */, actionName, controllerName, routeValues, htmlAttributes)) 
                 : MvcHtmlString.Empty;
        }

        public static MvcHtmlString ActivityActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
        {
            return ActivityActionLink(htmlHelper, linkText, actionName, controllerName, protocol, hostName, fragment, new RouteValueDictionary(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString ActivityActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            if (string.IsNullOrEmpty(linkText))
            {
                throw new ArgumentException("Null or empty", "linkText");
            }

            var reason = htmlHelper.LinkAuthorisationReason(actionName, controllerName);
            return reason.IsAuthorised
                 ? MvcHtmlString.Create(HtmlHelper.GenerateLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkText, null /* routeName */, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes))
                 : MvcHtmlString.Empty;
        }

        public static AuthorisationReason LinkAuthorisationReason(this HtmlHelper htmlHelper, string actionName, string controllerName)
        {
            // Get the controller
            var cn = !string.IsNullOrEmpty(controllerName) ? controllerName : htmlHelper.ViewContext.RequestContext.RouteData.Values["controller"].ToString();

            return Authorizer.IsAuthorized(cn, actionName, htmlHelper.ViewContext.HttpContext.User);
        }
    }
}