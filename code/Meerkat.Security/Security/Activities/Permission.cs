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
            get => claims ?? (claims = new List<Claim>());
            set => claims = value;
        }

        /// <summary>
        /// Get or sets the user list
        /// </summary>
        public IList<string> Users
        {
            get => users ?? (users = new List<string>());
            set => users = value;
        }

        /// <summary>
        /// Get or sets the roles list
        /// </summary>
        public IList<string> Roles
        {
            get => roles ?? (roles = new List<string>());
            set => roles = value;
        }

        internal bool IsEmpty()
        {
            return Claims.Count == 0 && Users.Count == 0 && Roles.Count == 0;
        }
    }
}