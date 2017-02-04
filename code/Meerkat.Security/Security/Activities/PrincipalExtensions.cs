using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Meerkat.Security.Activities
{
    public static class PrincipalExtensions
    {
        /// <summary>
        /// Check the authorization for a <see cref="IPrincipal"/> for an <see cref="Activity"/>.
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="principal"></param>
        /// <param name="defaultAuthorization"></param>
        /// <returns></returns>
        public static AuthorizationReason IsAuthorized(this IPrincipal principal, Activity activity, bool defaultAuthorization)
        {
            var reason = new AuthorizationReason
            {
                Resource = activity.Resource,
                Action = activity.Action,
                Principal = principal,
                NoDecision = true,
                IsAuthorized = defaultAuthorization,
            };

            // Authentication takes precedence over everything
            if (principal.Identity.IsAuthenticated == false && activity.AllowUnauthenticated.HasValue)
            {
                reason.NoDecision = false;
                reason.Reason = "IsAuthenticated: false";
                // Determined by the allowUnauthenticated
                reason.IsAuthorized = activity.AllowUnauthenticated.Value;
            }
            // Check the denies first, must take precedence over the allows
            else if (principal.HasPermission(activity.Deny, reason))
            {
                // Have an explicit deny.
                reason.NoDecision = false;
                reason.IsAuthorized = false;
            }
            else if (principal.HasPermission(activity.Allow, reason))
            {
                // Have an explity allow.
                reason.NoDecision = false;
                reason.IsAuthorized = true;
            }
            else if (activity.Default.HasValue)
            {
                // Default authorization on the activity.
                reason.NoDecision = false;
                reason.IsAuthorized = activity.Default.Value;
            }

            return reason;
        }

        /// <summary>
        /// Check whether the principal has the specified <see cref="Permission"/>
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="permission"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public static bool HasPermission(this IPrincipal principal, Permission permission, AuthorizationReason reason)
        {
            // Check user over roles
            var user = permission.HasUser(principal);
            if (!string.IsNullOrEmpty(user))
            {
                reason.Reason = "User: " + user;
                return true;
            }

            var role = permission.HasRole(principal);
            if (!string.IsNullOrEmpty(role))
            {
                // Hit due to this role so record it.
                reason.Reason = "Role: " + role;
                return true;
            }

            var claim = permission.HasClaim(principal);
            if (!string.IsNullOrEmpty(claim))
            {
                // Record the claim that passes
                reason.Reason = "Claim: " + claim;
                return true;
            }

            return false;
        }

        public static bool ClaimExists(this IPrincipal principal, string claimType)
        {
            var ci = principal as ClaimsPrincipal;
            if (ci == null)
            {
                return false;
            }

            var claim = ci.Claims.FirstOrDefault(x => x.Type == claimType);

            return claim != null;
        }

        public static bool HasClaim(this IPrincipal principal, string claimType, string claimValue, string issuer = null)
        {
            var ci = principal as ClaimsPrincipal;
            if (ci == null)
            {
                return false;
            }

            var claim = ci.Claims.FirstOrDefault(x => x.Type == claimType && x.Value == claimValue && (issuer == null || x.Issuer == issuer));

            return claim != null;
        }
    }
}
