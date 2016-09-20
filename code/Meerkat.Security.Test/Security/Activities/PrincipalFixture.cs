using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Meerkat.Test.Security.Activities
{
    public class PrincipalFixture
    {
        protected ClaimsPrincipal CreatePrincipal(string name, IList<string> roles, IList<string> teams = null, bool authenticated = true)
        {
            if (teams == null)
            {
                teams = new List<string>();
            }

            var claims = new List<Claim>
            {
                new Claim("name", name)
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }
            foreach (var team in teams)
            {
                claims.Add(new Claim("team", team));
            }
            var identity = new ClaimsIdentity(claims, authenticated ? "custom" : null, "name", "role");
            var principal = new ClaimsPrincipal(identity);

            return principal;
        }
    }
}
