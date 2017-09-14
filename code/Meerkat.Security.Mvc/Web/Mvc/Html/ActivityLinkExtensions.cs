﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

using Meerkat.Security.Activities;
using Meerkat.Security.Web;

namespace Meerkat.Web.Mvc.Html
{
    /// <summary>
    /// Provides HTML helpers to avoid displaying links that the user is not authorized to use.
    /// </summary>
    public static class ActivityLinkExtensions
    {
        private static IActivityAuthorizer authorizer;
        private static IControllerActivityMapper inferrer;

        public static IActivityAuthorizer Authorizer
        {
            // NB Not great, but avoids coupling this class to a specific IoC container.
            get { return authorizer ?? (authorizer = DependencyResolver.Current.GetService<IActivityAuthorizer>()); }
            set { authorizer = value; }
        }

        public static IControllerActivityMapper Inferrer
        {
            // NB Not great, but avoids coupling this class to a specific IoC container.
            get { return inferrer ?? (inferrer = DependencyResolver.Current.GetService<IControllerActivityMapper>()); }
            set { inferrer = value; }
        }

        /// <summary>
        /// Generate a HTML link to a controller action, conditional on whether the user has the right to invoke the controller/action
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="entity">Entity to use, type determines which controller is invoked</param>
        /// <param name="linkTextFunc">Function to generate the link text</param>
        /// <param name="actionName">Action to use</param>
        /// <param name="routeValues">Route values to use</param>
        /// <param name="htmlAttributes">Html attributes to use</param>
        /// <param name="linkTextOnUnauthorized">Whether to display the </param>
        /// <returns></returns>
        public static MvcHtmlString ActionLink<T>(this HtmlHelper htmlHelper, T entity, Func<T, string> linkTextFunc, string actionName, object routeValues, object htmlAttributes = null, bool linkTextOnUnauthorized = true)
        {
            if (entity == null)
            {
                // No link
                return MvcHtmlString.Empty;
            }

            var controller = entity.GetType().Name;

            var rvd = new RouteValueDictionary(routeValues);
            var hto = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            return htmlHelper.ActivityActionLink(linkTextFunc(entity), actionName, controller, rvd, hto, linkTextOnUnauthorized);
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

        public static MvcHtmlString ActivityActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes, bool linkTextOnUnauthorized = false)
        {
            if (string.IsNullOrEmpty(linkText))
            {
                throw new ArgumentException("Null or empty", nameof(linkText));
            }

            var reason = htmlHelper.LinkAuthorisationReason(actionName, controllerName);
            return reason.IsAuthorized 
                 ? MvcHtmlString.Create(HtmlHelper.GenerateLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkText, null /* routeName */, actionName, controllerName, routeValues, htmlAttributes)) 
                 : linkTextOnUnauthorized ? MvcHtmlString.Create(linkText) : MvcHtmlString.Empty;
        }

        public static MvcHtmlString ActivityActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
        {
            return ActivityActionLink(htmlHelper, linkText, actionName, controllerName, protocol, hostName, fragment, new RouteValueDictionary(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString ActivityActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes, bool linkTextOnUnauthorized = false)
        {
            if (string.IsNullOrEmpty(linkText))
            {
                throw new ArgumentException("Null or empty", nameof(linkText));
            }

            var reason = htmlHelper.LinkAuthorisationReason(actionName, controllerName);
            return reason.IsAuthorized
                 ? MvcHtmlString.Create(HtmlHelper.GenerateLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkText, null /* routeName */, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes))
                 : linkTextOnUnauthorized ? MvcHtmlString.Create(linkText) : MvcHtmlString.Empty;
        }

        public static AuthorizationReason LinkAuthorisationReason(this HtmlHelper htmlHelper, string actionName, string controllerName)
        {
            // Get the controller
            var cn = !string.IsNullOrEmpty(controllerName) ? controllerName : htmlHelper.ViewContext.RequestContext.RouteData.Values["controller"].ToString();

            // Now map to the underlying resource/action e.g. Details -> Read
            var activity = Inferrer.Map(cn, actionName);
            cn = activity.Item1;
            actionName = activity.Item2;

            // And ask the authorizer for its opinion.
            return Authorizer.IsAuthorized(cn, actionName, htmlHelper.ViewContext.HttpContext.User);
        }
    }
}