using Meerkat.Security.Activities;

using NUnit.Framework;

namespace Meerkat.Test.Security.Activities
{
    [TestFixture]
    public class ConfigurationActivityProviderFixture
    {
        [Test]
        public void ActivitySectionActivities()
        {
            var provider = new ConfigurationActivityProvider();
            var activities = provider.Activities();

            Assert.That(activities.Count, Is.EqualTo(2), "Activity count differs");
        }

        [Test]
        public void ActivitySectionDefaultAuthorization()
        {
            var provider = new ConfigurationActivityProvider();

            Assert.That(provider.DefaultAuthorization(), Is.EqualTo(false), "Default authorization differs");
        }

        [Test]
        public void ActivitySectionDefaultAllowUnauthenticated()
        {
            var provider = new ConfigurationActivityProvider();

            Assert.That(provider.DefaultAllowUnauthenticated(), Is.Null, "Default allowUnauthenticated differs");
        }

        [Test]
        public void ActivitySectionDefaultActivity()
        {
            var provider = new ConfigurationActivityProvider();

            Assert.That(provider.DefaultActivity(), Is.EqualTo("Foo"), "Default activity differs");
        }

        [Test]
        public void MissingSectionNoActivities()
        {
            var provider = new ConfigurationActivityProvider("foo");
            var activities = provider.Activities();

            Assert.That(activities.Count, Is.EqualTo(0), "Activity count differs");
        }

        [Test]
        public void MissingSectionNoDefaultAuthorization()
        {
            var provider = new ConfigurationActivityProvider("foo");

            Assert.That(provider.DefaultAuthorization(), Is.Null, "Default authorization differs");
        }

        [Test]
        public void MissingsectionNoDefaultActivity()
        {
            var provider = new ConfigurationActivityProvider("foo");

            Assert.That(provider.DefaultActivity(), Is.Null, "Default authorization differs");
        }
    }
}
