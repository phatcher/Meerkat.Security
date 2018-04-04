using System.Collections.Generic;

namespace Meerkat.Security.Activities
{
    /// <summary>
    /// A coherent set of <see cref="Activity"/>
    /// </summary>
    public class Authorizations
    {
        public Authorizations()
        {
            Activities = new List<Activity>();
        }

        /// <summary>
        /// Gets or sets the scope of the authorizations
        /// </summary>
        public string Scope { get; set; }
        
        /// <summary>
        /// Gets or sets the default activity
        /// </summary>         
        public string DefaultActivity { get; set; }

        /// <summary>
        /// Gets or sets whether we default to allowing unauthenticated
        /// </summary>
        public bool? DefaultAllowUnauthenticated { get; set; }

        /// <summary>
        /// Gets or sets the default authentication rule
        /// </summary>
        public bool? DefaultAuthorization { get; set; }

        /// <summary>
        /// Gets or sets the activities.
        /// </summary>
        public IList<Activity> Activities { get; set; }
    }
}
