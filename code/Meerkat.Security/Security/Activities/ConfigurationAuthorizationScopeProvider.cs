using System;
using System.Configuration;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Meerkat.Logging;
using Meerkat.Security.Activities.Configuration;

namespace Meerkat.Security.Activities
{
    public class ConfigurationAuthorizationScopeProvider : IAuthorizationScopeProvider
    {
        private static readonly ILog Logger = LogProvider.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ActivityAuthorizationSection section;
        private readonly AuthorizationScope scope;

        /// <summary>
        /// Creates a new instance of the <see cref="ConfigurationAuthorizationScopeProvider"/> class.
        /// </summary>
        /// <param name="sectionName"></param>
        public ConfigurationAuthorizationScopeProvider(string sectionName = "activityAuthorization")
        {
            section = ActivitySection(sectionName);
            scope = section.ToAuthorizationScope();
        }

        /// <copydoc cref="IAuthorizationScopeProvider.AuthorizationScopeAsync" />
        public Task<AuthorizationScope> AuthorizationScopeAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Static result so just return.
            return Task.FromResult(scope);
        }

        private ActivityAuthorizationSection ActivitySection(string name)
        {
            try
            {
                return (ActivityAuthorizationSection)ConfigurationManager.GetSection(name);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Failed to load authorization section " + name, ex);
                return null;
            }
        }
    }
}