using System.Collections.Generic;

using Newtonsoft.Json;

namespace Meerkat.Security.Activities
{
    /// <summary>
    /// A coherent set of <see cref="Activity"/>
    /// </summary>
    public class AuthorizationScope
    {
        public AuthorizationScope()
        {
            Activities = new List<Activity>();
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the default activity
        /// </summary>
        [JsonProperty("defaultActivity")]
        public string DefaultActivity { get; set; }

        /// <summary>
        /// Gets or sets the default authorization.
        /// </summary>
        [JsonProperty("defaultAuthorization")]
        public bool? DefaultAuthorization { get; set; }

        /// <summary>
        /// Gets or sets whether we default to allowing unauthenticated
        /// </summary>
        [JsonProperty("allowUnauthenticated")]
        public bool? AllowUnauthenticated { get; set; }

        /// <summary>
        /// Gets or sets the activities.
        /// </summary>
        [JsonProperty("activities")]
        public IList<Activity> Activities { get; set; }
    }
}
