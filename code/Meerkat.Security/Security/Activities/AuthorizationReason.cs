using System.Security.Principal;

namespace Meerkat.Security.Activities
{
    public class AuthorizationReason
    {
        public IPrincipal Principal { get; set; }

        public string Resource { get; set; }

        public string Action { get; set; }

        public bool NoDecision { get; set; }

        public bool IsAuthorized { get; set; }

        public string Role { get; set; }

        public AuthorizationReason PrincipalReason { get; set; }

        public override string ToString()
        {
            if (IsAuthorized)
            {

            }
            else
            {
                
            }
            return base.ToString();
        }
    }
}