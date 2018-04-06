using System.Threading;
using System.Threading.Tasks;

namespace Meerkat.Security.Activities
{
    /// <summary>
    /// Acquires an <see cref="AuthorizationScope"/> to allow authorization
    /// </summary>
    public interface IAuthorizationScopeProvider
    {
        /// <summary>
        /// Gets a <see cref="AuthorizationScope"/>
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<AuthorizationScope> AuthorizationScopeAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}