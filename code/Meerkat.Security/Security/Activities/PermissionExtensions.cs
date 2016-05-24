using System.Linq;
using System.Security.Principal;

namespace Meerkat.Security.Activities
{
    public static class PermissionExtensions
    {
        public static string HasUser(this Permission permission, IPrincipal principal)
        {
            foreach (var user in permission.Users)
            {
                if (user == principal.Identity.Name)
                {
                    return user;
                }
            }

            return null;
        }

        public static string HasRole(this Permission permission, IPrincipal principal)
        {
            foreach (var role in permission.Roles.Where(principal.IsInRole))
            {
                // Hit due to this role so record it.
                return role;
            }

            return null;
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