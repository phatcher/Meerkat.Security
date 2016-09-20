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

            Assert.That(section.Activities.Count, Is.EqualTo(2), "Activity count incorrect");
            Assert.That(section.DefaultActivity, Is.EqualTo("Foo"), "Default activity incorrect");
            Assert.That(section.DefaultAllowUnauthenticated, Is.Null, "Default AllowUnauthenticated incorrect");
            Assert.That(section.Default, Is.False, "Authorization default incorrect");

            var activity = section.Activities[0];

            Assert.That("Home.Index", Is.EqualTo(activity.Name), "Name differs");
            Assert.That(activity.Default, Is.Null, "Activity authorization default incorrect");
            Assert.That("A, D", Is.EqualTo(activity.Deny.Users), "Deny users differs");
            Assert.That("B, C", Is.EqualTo(activity.Allow.Roles), "Allow roles differs");

            Assert.That(1, Is.EqualTo(activity.Allow.Claims.Count), "Allow claim count differs");
            Assert.That("team", Is.EqualTo(activity.Allow.Claims[0].Name), "Allow claim type differs");
            Assert.That("foo", Is.EqualTo(activity.Allow.Claims[0].Issuer), "Allow claim issuer differs");
            Assert.That("F, G", Is.EqualTo(activity.Allow.Claims[0].Claims), "Allow claim differs");

            Assert.That(1, Is.EqualTo(activity.Deny.Claims.Count), "Deny claim count differs");
            Assert.That("team", Is.EqualTo(activity.Deny.Claims[0].Name), "Deny claim type differs");
            Assert.That("bar", Is.EqualTo(activity.Deny.Claims[0].Issuer), "Deny claim issuer differs");
            Assert.That("E", Is.EqualTo(activity.Deny.Claims[0].Claims), "Denu claim differs");
        }

        [Test]
        public void ParseDefaultAuthorizationTrue()
        {
            var section = (ActivityAuthorizationSection)ConfigurationManager.GetSection("defaultAuthorizationTrue");

            Assert.AreEqual(1, section.Activities.Count, "Activity count incorrect");
            Assert.That(section.DefaultAllowUnauthenticated, Is.True, "Default AllowUnauthenticated incorrect");
            Assert.IsTrue(section.Default, "Authorization default incorrect");

            var activity = section.Activities[0];

            Assert.That("Home.Index", Is.EqualTo(activity.Name), "Name differs");
            Assert.That(activity.Default.Value, Is.True, "Activity authorization default incorrect");
            Assert.That("A, C", Is.EqualTo(activity.Deny.Users), "Deny users differs");
            Assert.That("B", Is.EqualTo(activity.Allow.Roles), "Allow roles differs");
        }

        [Test]
        public void ParseDefaultAuthorizationFalse()
        {
            var section = (ActivityAuthorizationSection)ConfigurationManager.GetSection("defaultAuthorizationFalse");

            Assert.AreEqual(1, section.Activities.Count, "Activity count incorrect");
            Assert.That(section.DefaultAllowUnauthenticated, Is.False, "Default AllowUnauthenticated incorrect");
            Assert.IsFalse(section.Default, "Authorization default incorrect");

            var activity = section.Activities[0];

            Assert.That("Home.Index", Is.EqualTo(activity.Name), "Name differs");
            Assert.That(activity.Default.Value, Is.False, "Activity authorization default incorrect");
            Assert.That("A", Is.EqualTo(activity.Deny.Users), "Deny users differs");
            Assert.That("B", Is.EqualTo(activity.Allow.Roles), "Allow roles differs");
        }
    }
}