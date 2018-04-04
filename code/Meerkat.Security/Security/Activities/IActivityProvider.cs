using System.Collections.Generic;
using System.Threading.Tasks;

namespace Meerkat.Security.Activities
{
    /// <summary>
    /// Provides activities to be secured.
    /// </summary>
    public interface IActivityProvider
    {
        /// <summary>
        /// Gets the current set of activities.
        /// </summary>
        /// <returns></returns>
        IList<Activity> Activities();

        /// <summary>
        /// Gets the current set of activities asynchronously
        /// </summary>
        /// <returns></returns>
        Task<IList<Activity>> ActivitiesAsync();

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