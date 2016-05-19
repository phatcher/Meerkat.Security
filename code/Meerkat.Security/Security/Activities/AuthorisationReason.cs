namespace Meerkat.Security.Activities
{
    using System.Security.Principal;

    public class AuthorisationReason
    {
        public IPrincipal Principal { get; set; }

        public string Resource { get; set; }

        public string Action { get; set; }

        public bool NoDecision { get; set; }

        public bool IsAuthorised { get; set; }

        public string Role { get; set; }

        public AuthorisationReason PrincipalReason { get; set; }

        public override string ToString()
        {
            if (IsAuthorised)
            {

            }
            else
            {
                
            }
            return base.ToString();
        }
    }
}