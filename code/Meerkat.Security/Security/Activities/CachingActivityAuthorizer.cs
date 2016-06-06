using System;
using System.Security.Principal;

using Meerkat.Caching;

namespace Meerkat.Security.Activities
{
    public class CachingActivityAuthorizer : IActivityAuthorizer
    {
        private readonly IActivityAuthorizer authorizer;
        private readonly ICache cache;

        public CachingActivityAuthorizer(IActivityAuthorizer authorizer, ICache cache)
        {
            this.authorizer = authorizer;
            this.cache = cache;
        }

        public AuthorizationReason IsAuthorized(string resource, string action, IPrincipal principal)
        {
            var key = string.Format("(0}.{1}:{2}", resource, action, principal);

            return cache.AddOrGetExisting(key, () => authorizer.IsAuthorized(resource, action, principal), DateTimeOffset.UtcNow.AddMinutes(5), "authorizer");
        }
    }
}