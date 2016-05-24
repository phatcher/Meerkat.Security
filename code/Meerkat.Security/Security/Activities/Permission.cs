using System.Collections.Generic;
using System.Security.Claims;

namespace Meerkat.Security.Activities
{
    /// <summary>
    /// Holds the users, roles and claims
    /// </summary>
    public class Permission
    {
        private IList<Claim> claims;
        private IList<string> users;
        private IList<string> roles;

        /// <summary>
        /// Get or sets the <see cref="Claim"/>s list
        /// </summary>
        public IList<Claim> Claims
        {
            get { return claims ?? (claims = new List<Claim>()); }
            set { claims = value; }
        }

        /// <summary>
        /// Get or sets the user list
        /// </summary>
        public IList<string> Users
        {
            get { return users ?? (users = new List<string>()); }
            set { users = value; }
        }

        /// <summary>
        /// Get or sets the roles list
        /// </summary>
        public IList<string> Roles
        {
            get { return roles ?? (roles = new List<string>()); }
            set { roles = value; }
        }
    }
}