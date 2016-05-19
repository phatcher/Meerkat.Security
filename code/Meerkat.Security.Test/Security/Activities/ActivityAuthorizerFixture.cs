using System.Collections.Generic;

using Meerkat.Security.Activities;

using NUnit.Framework;

namespace Meerkat.Test.Security.Activities
{
    [TestFixture]
    public class ActivityAuthorizerFixture
    {
        [Test]
        public void AuthorizerDefaultAuthorizationTrue()
        {
            var activities = new List<Activity>();
            var authorizer = new ActivityAuthorizer(activities, true);

            Assert.AreEqual(true, authorizer.DefaultAuthorization, "Default authorization differs");

            var principal = MoqExtensions.MockPrincipal("charlie", new List<string>());
            var candidate = authorizer.IsAuthorized("Test", null, principal.Object);

            Assert.AreEqual(true, candidate.IsAuthorised, "IsAuthorized differs");
        }

        [Test]
        public void AuthorizerDefaultAuthorizationFalse()
        {
            var activities = new List<Activity>();
            var authorizer = new ActivityAuthorizer(activities, false);

            Assert.AreEqual(false, authorizer.DefaultAuthorization, "Default authorization differs");

            var principal = MoqExtensions.MockPrincipal("charlie", new List<string>());
            var candidate = authorizer.IsAuthorized("Test", null, principal.Object);

            Assert.AreEqual(false, candidate.IsAuthorised, "IsAuthorized differs");
        }

        [Test]
        public void ActivtyDefaultAuthorizationFalse()
        {
            var activities = SampleActivities(false);
            var authorizer = new ActivityAuthorizer(activities, true);

            var principal = MoqExtensions.MockPrincipal("charlie", new List<string>());
            var candidate = authorizer.IsAuthorized("Test", null, principal.Object);

            Assert.AreEqual(false, candidate.IsAuthorised, "IsAuthorized differs"); 
        }

        [Test]
        public void ActivityDefaultAuthorizationTrue()
        {
            var activities = SampleActivities(true);
            var authorizer = new ActivityAuthorizer(activities, true);

            var principal = MoqExtensions.MockPrincipal("charlie", new List<string>());
            var candidate = authorizer.IsAuthorized("Test", null, principal.Object);

            Assert.AreEqual(true, candidate.IsAuthorised, "IsAuthorized differs");
        }

        [Test]
        public void ActivityDefaultAuthorizationFallback()
        {
            var activities = SampleActivities(null);
            var authorizer = new ActivityAuthorizer(activities, true, "Test");

            var principal = MoqExtensions.MockPrincipal("charlie", new List<string>());
            var candidate = authorizer.IsAuthorized("Default", null, principal.Object);

            Assert.AreEqual(true, candidate.IsAuthorised, "IsAuthorized differs");
        }

        [Test]
        public void DefaultActivityDenyUserTakesPrecedence()
        {
            var activities = SampleActivities(null);
            var authorizer = new ActivityAuthorizer(activities, true, "Test");

            var principal = MoqExtensions.MockPrincipal("alice", new List<string> { "b" });
            var candidate = authorizer.IsAuthorized("Default", null, principal.Object);

            Assert.AreEqual(false, candidate.IsAuthorised, "IsAuthorized differs");
        }

        [Test]
        public void DefaultActivityDenyRoleTakesPrecendence()
        {
            var activities = SampleActivities(null);
            var authorizer = new ActivityAuthorizer(activities, true, "Test");

            var principal = MoqExtensions.MockPrincipal("bob", new List<string> { "a" });
            var candidate = authorizer.IsAuthorized("Default", null, principal.Object);

            Assert.AreEqual(false, candidate.IsAuthorised, "IsAuthorized differs");
        }

        [Test]
        public void DefaultActivityGrantUser()
        {
            var activities = SampleActivities(null);
            var authorizer = new ActivityAuthorizer(activities, true, "Test");

            var principal = MoqExtensions.MockPrincipal("bob", new List<string> { "c" });
            var candidate = authorizer.IsAuthorized("Default", null, principal.Object);

            Assert.AreEqual(true, candidate.IsAuthorised, "IsAuthorized differs");
        }

        [Test]
        public void DefaultActivityGrantRole()
        {
            var activities = SampleActivities(null);
            var authorizer = new ActivityAuthorizer(activities, true);

            var principal = MoqExtensions.MockPrincipal("charlie", new List<string> { "b" });
            var candidate = authorizer.IsAuthorized("Test", null, principal.Object);

            Assert.AreEqual(true, candidate.IsAuthorised, "IsAuthorized differs");
        }

        [Test]
        public void ActivityDenyUserTakesPrecedence()
        {
            var activities = SampleActivities(null);
            var authorizer = new ActivityAuthorizer(activities, true);

            var principal = MoqExtensions.MockPrincipal("alice", new List<string> { "b" });
            var candidate = authorizer.IsAuthorized("Test", null, principal.Object);

            Assert.AreEqual(false, candidate.IsAuthorised, "IsAuthorized differs");
        }

        [Test]
        public void ActivityDenyRoleTakesPrecendence()
        {
            var activities = SampleActivities(null);
            var authorizer = new ActivityAuthorizer(activities, true);

            var principal = MoqExtensions.MockPrincipal("bob", new List<string> { "a" });
            var candidate = authorizer.IsAuthorized("Test", null, principal.Object);

            Assert.AreEqual(false, candidate.IsAuthorised, "IsAuthorized differs");
        }

        [Test]
        public void ActivityGrantUser()
        {
            var activities = SampleActivities(null);
            var authorizer = new ActivityAuthorizer(activities, true);

            var principal = MoqExtensions.MockPrincipal("bob", new List<string> { "c" });
            var candidate = authorizer.IsAuthorized("Test", null, principal.Object);

            Assert.AreEqual(true, candidate.IsAuthorised, "IsAuthorized differs");
        }

        [Test]
        public void ActivityGrantRole()
        {
            var activities = SampleActivities(null);
            var authorizer = new ActivityAuthorizer(activities, true);

            var principal = MoqExtensions.MockPrincipal("charlie", new List<string> { "b" });
            var candidate = authorizer.IsAuthorized("Test", null, principal.Object);

            Assert.AreEqual(true, candidate.IsAuthorised, "IsAuthorized differs");
        }

        [Test]
        public void ActivityHierarchyDenyUserTakesPrecedence()
        {
            var activities = SampleActivities(null);
            var authorizer = new ActivityAuthorizer(activities, true);

            var principal = MoqExtensions.MockPrincipal("alice", new List<string> { "b" });
            var candidate = authorizer.IsAuthorized("Test", "Index", principal.Object);

            Assert.AreEqual(false, candidate.IsAuthorised, "IsAuthorized differs");
            // NB Proves we got the "Test" activity
            Assert.IsNotNull(candidate.PrincipalReason);
            Assert.IsNull(candidate.PrincipalReason.Action);
        }

        [Test]
        public void ActivityHierarchyExplicitActionDenyUserTakesPrecedence()
        {
            var activities = SampleActivities(null);
            var authorizer = new ActivityAuthorizer(activities, true);

            var principal = MoqExtensions.MockPrincipal("alice", new List<string> { "b" });
            var candidate = authorizer.IsAuthorized("Test", "Foo", principal.Object);

            Assert.AreEqual(false, candidate.IsAuthorised, "IsAuthorized differs");
            // NB Proves we got the "Test.Foo" activity
            Assert.IsNull(candidate.PrincipalReason);
        }

        [Test]
        public void ActivityHierarchyDenyRoleTakesPrecendence()
        {
            var activities = SampleActivities(null);
            var authorizer = new ActivityAuthorizer(activities, true);

            var principal = MoqExtensions.MockPrincipal("bob", new List<string> { "a" });
            var candidate = authorizer.IsAuthorized("Test", "Index", principal.Object);

            Assert.AreEqual(false, candidate.IsAuthorised, "IsAuthorized differs");
            // NB Proves we got the "Test" activity
            Assert.IsNotNull(candidate.PrincipalReason);
            Assert.IsNull(candidate.PrincipalReason.Action);
        }

        [Test]
        public void ActivityHierarchyExplicitActionDenyRoleTakesPrecendence()
        {
            var activities = SampleActivities(null);
            var authorizer = new ActivityAuthorizer(activities, true);

            var principal = MoqExtensions.MockPrincipal("bob", new List<string> { "a" });
            var candidate = authorizer.IsAuthorized("Test", "Foo", principal.Object);

            Assert.AreEqual(false, candidate.IsAuthorised, "IsAuthorized differs");
            // NB Proves we got the "Test.Foo" activity
            Assert.IsNull(candidate.PrincipalReason);
        }

        [Test]
        public void ActivityHierarchyGrantUser()
        {
            var activities = SampleActivities(null);
            var authorizer = new ActivityAuthorizer(activities, true);

            var principal = MoqExtensions.MockPrincipal("bob", new List<string> { "c" });
            var candidate = authorizer.IsAuthorized("Test", "Index", principal.Object);

            Assert.AreEqual(true, candidate.IsAuthorised, "IsAuthorized differs");
        }

        [Test]
        public void ActivityHierarchyGrantRole()
        {
            var activities = SampleActivities(null);
            var authorizer = new ActivityAuthorizer(activities, true);

            var principal = MoqExtensions.MockPrincipal("charlie", new List<string> { "b" });
            var candidate = authorizer.IsAuthorized("Test", "Index", principal.Object);

            Assert.AreEqual(true, candidate.IsAuthorised, "IsAuthorized differs");
        }

        private List<Activity> SampleActivities(bool? defaultAuthorize)
        {
            var activities = new List<Activity>();

            activities.Add(new Activity
            {
                Resource = "Test",
                Default = defaultAuthorize,
                Deny = new Permission
                {
                    Users = new List<string> { "alice" },
                    Roles = new List<string> { "a" }
                },
                Allow = new Permission
                {
                    Users = new List<string> { "bob" },
                    Roles = new List<string> { "b" }
                }
            });

            activities.Add(new Activity
            {
                Resource = "Test",
                Action = "Foo",
                Default = defaultAuthorize,
                Deny = new Permission
                {
                    Users = new List<string> { "alice" },
                    Roles = new List<string> { "a" }
                },
                Allow = new Permission
                {
                    Users = new List<string> { "bob" },
                    Roles = new List<string> { "b" }
                }
            });

            return activities;
        }
    }
}