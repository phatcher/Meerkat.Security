using System.Collections.Generic;
using System.Security.Claims;

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
            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, true);

            Assert.That(authorizer.DefaultAuthorization, Is.True, "Default authorization differs");

            var principal = CreatePrincipal("charlie", new List<string>());
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
        }

        [Test]
        public void AuthorizerDefaultAuthorizationFalse()
        {
            var activities = new List<Activity>();
            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, false);

            Assert.AreEqual(false, authorizer.DefaultAuthorization, "Default authorization differs");

            var principal = CreatePrincipal("charlie", new List<string>());
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
        }

        [Test]
        public void ActivtyDefaultAuthorizationFalse()
        {
            var activities = SampleActivities(false);
            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("charlie", new List<string>());
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
        }

        [Test]
        public void ActivityDefaultAuthorizationTrue()
        {
            var activities = SampleActivities(true);
            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("charlie", new List<string>());
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
        }

        [Test]
        public void ActivityDefaultAuthorizationFallback()
        {
            var activities = SampleActivities(null);
            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, true, "Test");

            var principal = CreatePrincipal("charlie", new List<string>());
            var candidate = authorizer.IsAuthorized("Default", null, principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
        }

        [Test]
        public void DefaultActivityDenyUserTakesPrecedence()
        {
            var activities = SampleActivities(null);
            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, true, "Test");

            var principal = CreatePrincipal("alice", new List<string> { "b" });
            var candidate = authorizer.IsAuthorized("Default", null, principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
        }

        [Test]
        public void DefaultActivityDenyRoleTakesPrecendence()
        {
            var activities = SampleActivities(null);
            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, true, "Test");

            var principal = CreatePrincipal("bob", new List<string> { "a" });
            var candidate = authorizer.IsAuthorized("Default", null, principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
        }

        [Test]
        public void DefaultActivityDenyClaimTakesPrecendence()
        {
            var activities = SampleActivities(null);
            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, true, "Test");

            var principal = CreatePrincipal("bob", new List<string>(), new List<string> { "p" });
            var candidate = authorizer.IsAuthorized("Default", null, principal);

            Assert.AreEqual(false, candidate.IsAuthorized, "IsAuthorized differs");
        }

        [Test]
        public void DefaultActivityGrantUser()
        {
            var activities = SampleActivities(null);
            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, true, "Test");

            var principal = CreatePrincipal("bob", new List<string> { "c" });
            var candidate = authorizer.IsAuthorized("Default", null, principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
        }

        [Test]
        public void DefaultActivityGrantRole()
        {
            var activities = SampleActivities(null);
            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("charlie", new List<string> { "b" });
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
        }

        [Test]
        public void DefaultActivityGrantClaime()
        {
            var activities = SampleActivities(null);
            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("charlie", new List<string> { "b" });
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
        }

        [Test]
        public void ActivityDenyUserTakesPrecedence()
        {
            var activities = SampleActivities(null);
            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("alice", new List<string> { "b" });
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("User: alice"), "Reason differs");
        }

        [Test]
        public void ActivityDenyRoleTakesPrecendence()
        {
            var activities = SampleActivities(null);
            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("bob", new List<string> { "a" });
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("Role: a"), "Reason differs");
        }

        [Test]
        public void ActivityDenyClaimTakesPrecendence()
        {
            var activities = SampleActivities(null);
            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("bob", new List<string>(), new List<string> { "p" });
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("Claim: team/p"), "Reason differs");
        }

        [Test]
        public void ActivityGrantUser()
        {
            var activities = SampleActivities(null);
            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("bob", new List<string> { "c" });
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("User: bob"), "Reason differs");
        }

        [Test]
        public void ActivityGrantRole()
        {
            var activities = SampleActivities(null);
            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("charlie", new List<string> { "b" });
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("Role: b"), "Reason differs");
        }

        [Test]
        public void ActivityGrantClaim()
        {
            var activities = SampleActivities(null);
            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("charlie", new List<string>(), new List<string> { "q" });
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("Claim: team/q"), "Reason differs");
        }

        [Test]
        public void ActivityHierarchyDenyUserTakesPrecedence()
        {
            var activities = SampleActivities(null);
            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("alice", new List<string> { "b" });
            var candidate = authorizer.IsAuthorized("Test", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            // NB Proves we got the "Test" activity
            Assert.IsNotNull(candidate.PrincipalReason);
            Assert.IsNull(candidate.PrincipalReason.Action);
        }

        [Test]
        public void ActivityHierarchyExplicitActionDenyUserTakesPrecedence()
        {
            var activities = SampleActivities(null);
            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("alice", new List<string> { "b" });
            var candidate = authorizer.IsAuthorized("Test", "Foo", principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            // NB Proves we got the "Test.Foo" activity
            Assert.IsNull(candidate.PrincipalReason);
        }

        [Test]
        public void ActivityHierarchyDenyRoleTakesPrecendence()
        {
            var activities = SampleActivities(null);
            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("bob", new List<string> { "a" });
            var candidate = authorizer.IsAuthorized("Test", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            // NB Proves we got the "Test" activity
            Assert.IsNotNull(candidate.PrincipalReason);
            Assert.IsNull(candidate.PrincipalReason.Action);
        }

        [Test]
        public void ActivityHierarchyExplicitActionDenyRoleTakesPrecendence()
        {
            var activities = SampleActivities(null);
            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("bob", new List<string> { "a" });
            var candidate = authorizer.IsAuthorized("Test", "Foo", principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            // NB Proves we got the "Test.Foo" activity
            Assert.IsNull(candidate.PrincipalReason);
        }

        [Test]
        public void ActivityHierarchyGrantUser()
        {
            var activities = SampleActivities(null);
            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("bob", new List<string> { "c" });
            var candidate = authorizer.IsAuthorized("Test", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
        }

        [Test]
        public void ActivityHierarchyGrantRole()
        {
            var activities = SampleActivities(null);
             var provider = new StaticActivityProvider(activities);
           var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("charlie", new List<string> { "b" });
            var candidate = authorizer.IsAuthorized("Test", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
        }

        private ClaimsPrincipal CreatePrincipal(string name, IList<string> roles, IList<string> teams = null)
        {
            if (teams == null)
            {
                teams = new List<string>();
            }

            var claims = new List<Claim>
            {
                new Claim("name", name)
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }
            foreach (var team in teams)
            {
                claims.Add(new Claim("team", team));
            }
            var identity = new ClaimsIdentity(claims, "custom", "name", "role");
            var principal = new ClaimsPrincipal(identity);

            return principal;
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
                    Roles = new List<string> { "a" },
                    Claims = new List<Claim> {
                        new Claim("team", "p" )
                    }
                },
                Allow = new Permission
                {
                    Users = new List<string> { "bob" },
                    Roles = new List<string> { "b" },
                    Claims = new List<Claim> {
                        new Claim("team", "q" )
                    }
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
                    Roles = new List<string> { "a" },
                    Claims = new List<Claim> {
                        new Claim("team", "p" )
                    }
                },
                Allow = new Permission
                {
                    Users = new List<string> { "bob" },
                    Roles = new List<string> { "b" },
                    Claims = new List<Claim> {
                        new Claim("team", "q" )
                    }
                }
            });

            return activities;
        }
    }
}