using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

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
        /// <param name="values">Additional values to determine the authorization</param>
        /// <returns>The authorization reason</returns>
        AuthorizationReason IsAuthorized(string resource, string action, IPrincipal principal, IDictionary<string, object> values = null);

        /// <summary>
        /// Checks authorization of a user for an activity.
        /// </summary>
        /// <param name="resource">Resource to use</param>
        /// <param name="action">Action to use</param>
        /// <param name="principal">User to use</param>
        /// <param name="values">Additional values to determine the authorization</param>
        /// <returns>The authorization reason</returns>
        Task<AuthorizationReason> IsAuthorizedAsync(string resource, string action, IPrincipal principal, IDictionary<string, object> values = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}