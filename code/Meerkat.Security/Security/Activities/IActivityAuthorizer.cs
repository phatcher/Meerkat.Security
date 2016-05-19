using System.Security.Principal;

namespace Meerkat.Security.Activities
{
    /// <summary>
    /// Checks authorization of a user for an activity.
    /// </summary>
    public interface IActivityAuthorizer
    {
        /// <summary>
        /// Checks authorization of a user for an activity.
        /// </summary>
        /// <param name="resource">Resource to use</param>
        /// <param name="action">Action to use</param>
        /// <param name="principal">User to use</param>
        /// <returns>The authorization reason</returns>
        AuthorisationReason IsAuthorized(string resource, string action, IPrincipal principal);
    }
}