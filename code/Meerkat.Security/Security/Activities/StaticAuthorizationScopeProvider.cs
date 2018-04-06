using System.Threading;
using System.Threading.Tasks;

namespace Meerkat.Security.Activities
{
    /// <summary>
    /// Implementation of <see cref="IAuthorizationScopeProvider"/> that is initialized statically.
    /// </summary>
    public class StaticAuthorizationScopeProvider : IAuthorizationScopeProvider
    {
        private readonly AuthorizationScope scope;

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