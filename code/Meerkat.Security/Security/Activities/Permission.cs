using System.Collections.Generic;
using System.Security.Claims;

using Newtonsoft.Json;

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
        [JsonProperty("claims")]
        public IList<Claim> Claims
        {
            get => claims ?? (claims = new List<Claim>());
            set => claims = value;
        }

        /// <summary>
        /// Get or sets the user list
        /// </summary>
        [JsonProperty("users")]
        public IList<string> Users
        {
            get => users ?? (users = new List<string>());
            set => users = value;
        }

        /// <summary>
        /// Get or sets the roles list
        /// </summary>
        [JsonProperty("roles")]
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