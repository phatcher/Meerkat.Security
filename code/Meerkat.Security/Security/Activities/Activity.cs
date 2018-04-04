namespace Meerkat.Security.Activities
{
    /// <summary>
    /// An activity is secured action against a resource and may be allowed or denied.
    /// </summary>
    public class Activity
    {
        private Permission allow;
        private Permission deny;

        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(Action))
                {
                    // NB Naked resources don't have a trailing .
                    return Resource;
                }
                return Resource + "." + Action;
            }
        }

        /// <summary>
        /// Get or set the resource name.
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// Get or set the action name.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Get or set whether we allow unauthenticated users
        /// </summary>
        public bool? AllowUnauthenticated { get; set; }

        /// <summary>
        /// Gets or sets the default authorization.
        /// </summary>
        public bool? Default { get; set; }

        /// <summary>
        /// Gets or sets the allow grants.
        /// </summary>
        public Permission Allow
        {
            get => allow ?? (allow = new Permission());
            set => allow = value;
        }

        /// <summary>
        /// Gets or sets the deny grants.
        /// </summary>
        public Permission Deny
        {
            get => deny ?? (deny = new Permission());
            set => deny = value;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}