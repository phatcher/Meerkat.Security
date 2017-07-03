using System.Linq;
using System.Security.Principal;

namespace Meerkat.Security.Activities
{
    public static class PermissionExtensions
    {
        public static string HasUser(this Permission permission, IPrincipal principal)
        {
            // Return the user with matching name
            return permission.Users.FirstOrDefault(user => user == principal.Identity.Name);
        }

        public static string HasRole(this Permission permission, IPrincipal principal)
        {
            // Return the first role that has permission
            return permission.Roles.FirstOrDefault(principal.IsInRole);
        }

        public static string HasClaim(this Permission permission, IPrincipal principal)
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
    }
}