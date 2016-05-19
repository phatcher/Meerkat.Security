using System.Configuration;

using Meerkat.Security.Activities.Configuration;

using NUnit.Framework;

namespace Meerkat.Test.Security.Activities.Configuration
{
    [TestFixture]
    public class ActivityAuthorizationSectionFixture
    {
        [Test]
        public void ParseActivityAuthorization()
        {
            var section = (ActivityAuthorizationSection)ConfigurationManager.GetSection("activityAuthorization");

            Assert.AreEqual(1, section.Activities.Count, "Activity count incorrect");
            Assert.IsFalse(section.Default, "Authorization default incorrect");

            var activity = section.Activities[0];

            Assert.AreEqual("Home.Index", activity.Name, "Name differs");
            Assert.IsNull(activity.Default, "Activity authorization default incorrect");
            Assert.AreEqual("A, D", activity.Deny.Users, "Deny users differs");
            Assert.AreEqual("B, C", activity.Allow.Roles, "Allow roles differs");
        }

        [Test]
        public void ParseDefaultAuthorizationTrue()
        {
            var section = (ActivityAuthorizationSection)ConfigurationManager.GetSection("defaultAuthorizationTrue");

            Assert.AreEqual(1, section.Activities.Count, "Activity count incorrect");
            Assert.IsTrue(section.Default, "Authorization default incorrect");

            var activity = section.Activities[0];

            Assert.AreEqual("Home.Index", activity.Name, "Name differs");
            Assert.IsTrue(activity.Default.Value, "Activity authorization default incorrect");
            Assert.AreEqual("A, C", activity.Deny.Users, "Deny users differs");
            Assert.AreEqual("B", activity.Allow.Roles, "Allow roles differs");
        }

        [Test]
        public void ParseDefaultAuthorizationFalse()
        {
            var section = (ActivityAuthorizationSection)ConfigurationManager.GetSection("defaultAuthorizationFalse");

            Assert.AreEqual(1, section.Activities.Count, "Activity count incorrect");
            Assert.IsFalse(section.Default, "Authorization default incorrect");

            var activity = section.Activities[0];

            Assert.AreEqual("Home.Index", activity.Name, "Name differs");
            Assert.IsFalse(activity.Default.Value, "Activity authorization default incorrect");
            Assert.AreEqual("A", activity.Deny.Users, "Deny users differs");
            Assert.AreEqual("B", activity.Allow.Roles, "Allow roles differs");
        }
    }
}