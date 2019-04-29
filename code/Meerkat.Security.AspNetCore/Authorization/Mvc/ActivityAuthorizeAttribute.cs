using System;
using System.Security.Claims;

using Meerkat.Security.Activities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Meerkat.Security.Authorization.Mvc
{
    /// <summary>
    /// An authorization filter that verifies the request's <see cref="ClaimsPrincipal"/> against a <see cref="IActivityAuthorizer"/>
    /// </summary>
    /// <remarks>
    /// You can declare multiples of these attributes per action. You can also use <see cref="AllowAnonymousAttribute"/> to disable
    /// authorization for a specific action.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ActivityAuthorizeAttribute : TypeFilterAttribute
    {
        public ActivityAuthorizeAttribute(string resource = null, string action = null) : base(typeof(ActivityFilter))
        {
            Arguments = new object[] { resource, action };
        }
    }
}