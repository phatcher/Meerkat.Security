using System.Collections.Generic;

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
        public string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the default activity
        /// </summary>         
        public string DefaultActivity { get; set; }

        /// <summary>
        /// Gets or sets the default authorization.
        /// </summary>
        public bool? DefaultAuthorization { get; set; }

        /// <summary>
        /// Gets or sets whether we default to allowing unauthenticated
        /// </summary>
        public bool? AllowUnauthenticated { get; set; }

        /// <summary>
        /// Gets or sets the activities.
        /// </summary>
        public IList<Activity> Activities { get; set; }
    }
}
