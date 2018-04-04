using System.Linq;
using System.Security.Claims;

namespace Meerkat.Security.Activities
{
    public static class PermissionExtensions
    {
        /// <summary>
        /// Check whether a <see cref="Permission"/>'s user requirement is satisfied by a <see cref="ClaimsPrincipal"/>.
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string HasUser(this Permission permission, ClaimsPrincipal principal)
        {
            // Return the user with matching name
            return permission.Users.FirstOrDefault(user => user == principal.Identity.Name);
        }

        /// <summary>
        /// Check whether a <see cref="Permission"/>'s role requirement is satisfied by a <see cref="ClaimsPrincipal"/>.
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string HasRole(this Permission permission, ClaimsPrincipal principal)
        {
            // Return the first role that has permission
            return permission.Roles.FirstOrDefault(principal.IsInRole);
        }

        /// <summary>
        /// Check whether a <see cref="Permission"/>'s is satisfied by a <see cref="ClaimsPrincipal"/>.
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string HasClaim(this Permission permission, ClaimsPrincipal principal)
        {
            foreach (var claim in permission.Claims)
            {
                var result = principal.HasClaim(claim.Type, claim.Value, claim.Issuer);
                if (result)
                {
                    // Record the claim that passes
                    return claim.Type + "/" + claim.Value;
                }
            }

            return null;
        }

        /// <summary>
        /// Check whether a <see cref="Permission"/>'s user requirement is satisfied by a <see cref="ClaimsIdentity"/>.
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static string HasUser(this Permission permission, ClaimsIdentity identity)
        {
            return permission.Users.FirstOrDefault(user => user == identity.Name);
        }

        /// <summary>
        /// Check whether a <see cref="Permission"/>'s role requirement is satisfied by a <see cref="ClaimsIdentity"/>
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static string HasRole(this Permission permission, ClaimsIdentity identity)
        {
            return permission.Roles.FirstOrDefault(identity.IsInRole);
        }

        /// <summary>
        /// Determine if a <see cref="ClaimsIdentity"/> is in a specified role.
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static bool IsInRole(this ClaimsIdentity identity, string role)
        {
            return identity.HasClaim(identity.RoleClaimType, role);
        }

        /// <summary>
        /// Check whether a <see cref="ClaimsIdentity"/> has a specified permission
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static string HasClaim(this Permission permission, ClaimsIdentity identity)
        {
            foreach (var claim in permission.Claims)
            {
                var result = identity.HasClaim(claim.Type, claim.Value, claim.Issuer);
                if (result)
                {
                    // Record the claim that passes
                    return claim.Type + "/" + claim.Value;
                }
            }

            return null;
        }
    }
}