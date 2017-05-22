using System;
using System.Security.Principal;

using Meerkat.Caching;

namespace Meerkat.Security.Activities
{
    /// <summary>
    /// Caching version of a <see cref="IActivityAuthorizer"/>.
    /// </summary>
    public class CachingActivityAuthorizer : IActivityAuthorizer
    {
        private readonly IActivityAuthorizer authorizer;
        private readonly ICache cache;
        private readonly TimeSpan duration;

        /// <summary>
        /// Creates a new instance of the <see cref="CachingActivityAuthorizer"/> class
        /// </summary>
        /// <param name="authorizer">Authorizer to use</param>
        /// <param name="cache">Cache to use</param>
        /// <param name="duration">Cache duration (default: 5 mins)</param>
        public CachingActivityAuthorizer(IActivityAuthorizer authorizer, ICache cache, TimeSpan? duration = null)
        {
            this.authorizer = authorizer;
            this.cache = cache;
            this.duration = duration ?? TimeSpan.FromMinutes(5);
        }

        /// <copydoc cref="IActivityAuthorizer.IsAuthorized" />
        public AuthorizationReason IsAuthorized(string resource, string action, IPrincipal principal)
        {
            var key = string.Format("(0}.{1}:{2}", resource, action, principal);

            return cache.AddOrGetExisting(key, () => authorizer.IsAuthorized(resource, action, principal), DateTimeOffset.UtcNow.Add(duration), "authorizer");
        }
    }
}