using System;
using System.Security.Principal;

namespace Meerkat.Security.Activities
{
    /// <summary>
    /// Describes the reason for an authorization decision
    /// </summary>
    public class AuthorizationReason
    {
        /// <summary>
        /// Gets or sets the principal that is being considered
        /// </summary>
        public IPrincipal Principal { get; set; }

        /// <summary>
        /// Gets or sets the identity that determines the reason.
        /// </summary>
        public IIdentity Identity { get; set; }

        /// <summary>
        /// Gets or sets the resource that is being checked.
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// Gets or sets the action that is being checked.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets whether a decision has been made.
        /// </summary>
        public bool NoDecision { get; set; }

        /// <summary>
        /// Gets or sets whether the principal is authorized for the activity.
        /// </summary>
        public bool IsAuthorized { get; set; }

        /// <summary>
        /// Gets or sets the reason for the <see cref="IsAuthorized"/> decision.
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Gets or sets when the validity of the decision expires.
        /// </summary>
        public DateTime? Expiry { get; set; }

        /// <summary>
        /// Gets or sets the principal reason the decision was made.
        /// </summary>
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