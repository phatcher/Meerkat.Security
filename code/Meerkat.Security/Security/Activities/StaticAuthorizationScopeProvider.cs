using System.Threading;
using System.Threading.Tasks;

namespace Meerkat.Security.Activities
{
    /// <summary>
    /// A <see cref="IAuthorizationScopeProvider"/> that is initialized statically.
    /// </summary>
    public class StaticAuthorizationScopeProvider : IAuthorizationScopeProvider
    {
        private readonly AuthorizationScope scope;

        /// <summary>
        /// Creates a new instance of the <see cref="StaticAuthorizationScopeProvider"/> class.
        /// </summary>
        /// <param name="scope"></param>
        public StaticAuthorizationScopeProvider(AuthorizationScope scope)
        {
            this.scope = scope;
        }

        /// <copydoc cref="IAuthorizationScopeProvider.AuthorizationScopeAsync" />
        public Task<AuthorizationScope> AuthorizationScopeAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(scope);
        }
    }
}