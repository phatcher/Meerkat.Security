using System.Collections.Generic;

namespace Meerkat.Security.Activities
{
    /// <summary>
    /// Locates the activities that are available to be secured.
    /// </summary>
    public interface IActivityProvider
    {
        /// <summary>
        /// Gets the current set of activities.
        /// </summary>
        /// <returns></returns>
        IList<Activity> Activities();

        /// <summary>
        /// Gets the default activity.
        /// </summary>
        string DefaultActivity();

        /// <summary>
        /// Get the default allow unauthenticated policy.
        /// </summary>
        /// <returns></returns>
        bool? DefaultAllowUnauthenticated();

        /// <summary>
        /// Get the default authorization.
        /// </summary>
        /// <returns></returns>
        bool? DefaultAuthorization();
    }
}