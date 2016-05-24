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

            // Check the denies first, must take precedence over the allows
            if (principal.HasPermission(activity.Deny, reason))
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
