using System.Threading.Tasks;

using Meerkat.Security.Activities;

using NUnit.Framework;

namespace Meerkat.Test.Security.Activities
{
    [TestFixture]
    public class ConfigurationAuthorizationScopeProviderFixture
    {
        [Test]
        public async Task ActivitySectionActivities()
        {
            var provider = new ConfigurationAuthorizationScopeProvider();
            var candidate = await provider.AuthorizationScopeAsync().ConfigureAwait(false);

            Assert.That(candidate.Activities.Count, Is.EqualTo(2), "Activity count differs");
        }

        [Test]
        public async Task ActivitySectionDefaultAuthorization()
        {
            var provider = new ConfigurationAuthorizationScopeProvider();
            var candidate = await provider.AuthorizationScopeAsync().ConfigureAwait(false);

            Assert.That(candidate.DefaultAuthorization, Is.EqualTo(false), "Default authorization differs");
        }

        [Test]
        public async Task ActivitySectionDefaultAllowUnauthenticated()
        {
            var provider = new ConfigurationAuthorizationScopeProvider();
            var candidate = await provider.AuthorizationScopeAsync().ConfigureAwait(false);

            Assert.That(candidate.AllowUnauthenticated, Is.Null, "Default allowUnauthenticated differs");
        }

        [Test]
        public async Task ActivitySectionDefaultActivity()
        {
            var provider = new ConfigurationAuthorizationScopeProvider();
            var candidate = await provider.AuthorizationScopeAsync().ConfigureAwait(false);

            Assert.That(candidate.DefaultActivity, Is.EqualTo("Foo"), "Default activity differs");
        }

        [Test]
        public async Task MissingSectionNoActivities()
        {
            var provider = new ConfigurationAuthorizationScopeProvider("foo");
            var candidate = await provider.AuthorizationScopeAsync().ConfigureAwait(false);

            Assert.That(candidate.Activities.Count, Is.EqualTo(0), "Activity count differs");
        }

        [Test]
        public async Task MissingSectionNoDefaultAuthorization()
        {
            var provider = new ConfigurationAuthorizationScopeProvider("foo");
            var candidate = await provider.AuthorizationScopeAsync().ConfigureAwait(false);

            Assert.That(candidate.DefaultAuthorization, Is.Null, "Default authorization differs");
        }

        [Test]
        public async Task MissingsectionNoDefaultActivity()
        {
            var provider = new ConfigurationAuthorizationScopeProvider("foo");
            var candidate = await provider.AuthorizationScopeAsync().ConfigureAwait(false);

            Assert.That(candidate.DefaultActivity, Is.Null, "Default authorization differs");
        }
    }
}
