namespace Meerkat.Security.Activities
{
    using System.Collections.Generic;

    public class Permission
    {
        private IList<string> users;
        private IList<string> roles;

        public IList<string> Users
        {
            get { return users ?? (users = new List<string>()); }
            set { users = value; }
        }

        public IList<string> Roles
        {
            get { return roles ?? (roles = new List<string>()); }
            set { roles = value; }
        }
    }
}