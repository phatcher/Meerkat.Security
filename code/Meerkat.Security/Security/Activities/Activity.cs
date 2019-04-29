using Newtonsoft.Json;

namespace Meerkat.Security.Activities
{
    /// <summary>
    /// An activity is secured action against a resource and may be allowed or denied.
    /// </summary>
    public class Activity
    {
        private Permission allow;
        private Permission deny;

        [JsonIgnore]
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
        [JsonProperty("resource")]
        public string Resource { get; set; }

        /// <summary>
        /// Get or set the action name.
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }

        /// <summary>
        /// Get or set whether we allow unauthenticated users
        /// </summary>
        [JsonProperty("allowUnauthenticated")]
        public bool? AllowUnauthenticated { get; set; }

        /// <summary>
        /// Gets or sets the default authorization.
        /// </summary>
        [JsonProperty("authorized")]
        public bool? Default { get; set; }

        /// <summary>
        /// Gets or sets the allow grants.
        /// </summary>
        [JsonProperty("allow")]
        public Permission Allow
        {
            get => allow ?? (allow = new Permission());
            set => allow = value;
        }

        /// <summary>
        /// Gets or sets the deny grants.
        /// </summary>
        [JsonProperty("deny")]
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