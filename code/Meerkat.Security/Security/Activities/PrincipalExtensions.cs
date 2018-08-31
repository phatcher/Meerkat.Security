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
                reason.Reason = "Default: " + activity.Default.Value;
                reason.IsAuthorized = activity.Default.Value;
            }

            return reason;
        }

        public static bool? IsAuthorized(this ClaimsIdentity identity, Activity activity, AuthorizationReason reason)
        {
            // Authentication takes precedence over everything            
            if (identity.IsAuthenticated == false && activity.AllowUnauthenticated.HasValue)
            {
                reason.NoDecision = false;
                reason.Reason = "IsAuthenticated: false";
                // Determined by the allowUnauthenticated
                reason.IsAuthorized = activity.AllowUnauthenticated.Value;
                reason.Identity = identity;

                return reason.IsAuthorized;
            }

            // Check the denies first, must take precedence over the allows
            if (identity.HasPermission(activity.Deny, reason))
            {
                // Have an explicit deny.
                reason.NoDecision = false;
                reason.IsAuthorized = false;
                reason.Identity = identity;

                return reason.IsAuthorized;
            }

            if (identity.HasPermission(activity.Allow, reason))
            {
                // Have an explity allow.
                reason.NoDecision = false;
                reason.IsAuthorized = true;
                reason.Identity = identity;

                return reason.IsAuthorized;
            }

            return null;
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
            if (!(principal is ClaimsPrincipal claimsPrincipal))
            {
                return false;
            }

            // Check user over roles
            foreach (var identity in claimsPrincipal.Identities)
            {
                var user = permission.HasUser(identity);
                if (!string.IsNullOrEmpty(user))
                {
                    reason.Reason = "User: " + user;
                    reason.Identity = identity;
                    return true;
                }
            }

            foreach (var identity in claimsPrincipal.Identities)
            {
                var role = permission.HasRole(identity);
                if (!string.IsNullOrEmpty(role))
                {
                    // Hit due to this role so record it.
                    reason.Reason = "Role: " + role;
                    reason.Identity = identity;
                    return true;
                }
            }

            foreach (var identity in claimsPrincipal.Identities)
            {
                var claim = permission.HasClaim(identity);
                if (!string.IsNullOrEmpty(claim))
                {
                    // Record the claim that passes
                    reason.Reason = "Claim: " + claim;
                    reason.Identity = identity;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check whether the principal has the specified <see cref="Permission"/>
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="permission"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public static bool HasPermission(this ClaimsIdentity identity, Permission permission, AuthorizationReason reason)
        {
            // Check user over roles
            var user = permission.HasUser(identity);
            if (!string.IsNullOrEmpty(user))
            {
                reason.Reason = "User: " + user;
                return true;
            }

            var role = permission.HasRole(identity);
            if (!string.IsNullOrEmpty(role))
            {
                // Hit due to this role so record it.
                reason.Reason = "Role: " + role;
                return true;
            }

            var claim = permission.HasClaim(identity);
            if (!string.IsNullOrEmpty(claim))
            {
                // Record the claim that passes
                reason.Reason = "Claim: " + claim;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check whether the principal has the specified claim type.
        /// </summary>
        /// <param name="principal">Principal to use</param>
        /// <param name="claimType">Claim type to check</param>
        /// <returns>true if the claim type exists, otherwise false</returns>
        public static bool ClaimExists(this ClaimsPrincipal principal, string claimType)
        {
            var claim = principal?.Claims.FirstOrDefault(x => x.Type == claimType);

            return claim != null;
        }

        /// <summary>
        /// Check whether the principal has the specified claim type.
        /// </summary>
        /// <param name="identity">Principal to use</param>
        /// <param name="claimType">Claim type to check</param>
        /// <returns>true if the claim type exists, otherwise false</returns>
        public static bool ClaimExists(this ClaimsIdentity identity, string claimType)
        {
            var claim = identity?.Claims.FirstOrDefault(x => x.Type == claimType);

            return claim != null;
        }

        /// <summary>
        /// Check whether the principal has the specified claim type.
        /// </summary>
        /// <param name="principal">Principal to use</param>
        /// <param name="claimType">Claim type to check</param>
        /// <param name="claimValue">Value to check</param>
        /// <param name="issuer">Issuer to check</param>
        /// <returns>true if the claim type exists and the value/issues match, otherwise false</returns>
        public static bool HasClaim(this ClaimsPrincipal principal, string claimType, string claimValue, string issuer = null)
        {
            var claim = principal?.Claims.FirstOrDefault(x => x.Type == claimType && x.Value == claimValue && (issuer == null || x.Issuer == issuer));

            return claim != null;
        }

        /// <summary>
        /// Check whether the principal has the specified claim type.
        /// </summary>
        /// <param name="identity">Identity to use</param>
        /// <param name="claimType">Claim type to check</param>
        /// <param name="claimValue">Value to check</param>
        /// <param name="issuer">Issuer to check</param>
        /// <returns>true if the claim type exists and the value/issues match, otherwise false</returns>
        public static bool HasClaim(this ClaimsIdentity identity, string claimType, string claimValue, string issuer = null)
        {
            var claim = identity?.Claims.FirstOrDefault(x => x.Type == claimType && x.Value == claimValue && (issuer == null || x.Issuer == issuer));

            return claim != null;
        }
    }
}
