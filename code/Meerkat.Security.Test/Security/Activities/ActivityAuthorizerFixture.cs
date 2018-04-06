using System.Collections.Generic;
using System.Security.Claims;

using Meerkat.Security.Activities;

using NUnit.Framework;

namespace Meerkat.Test.Security.Activities
{
    [TestFixture]
    public class ActivityAuthorizerFixture : PrincipalFixture
    {
        [Test]
        public void AuthorizerDefaultAuthorizationTrue()
        {
            var scope = new AuthorizationScope();
            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, true);

            Assert.That(authorizer.DefaultAuthorization, Is.True, "Default authorization differs");

            var principal = CreatePrincipal("charlie", new List<string>());
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
        }

        [Test]
        public void AuthorizerDefaultAuthorizationFalse()
        {
            var scope = new AuthorizationScope();
            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, false);

            Assert.AreEqual(false, authorizer.DefaultAuthorization, "Default authorization differs");

            var principal = CreatePrincipal("charlie", new List<string>());
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
        }

        [Test]
        public void ActivtyDefaultAuthorizationFalse()
        {
            var scope = SampleScope(false);
            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("charlie", new List<string>());
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
        }

        [Test]
        public void ActivityDefaultAuthorizationTrue()
        {
            var scope = SampleScope(true);
            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("charlie", new List<string>());
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
        }

        [Test]
        public void ActivityDefaultAuthorizationFallback()
        {
            var scope = SampleScope(null);
            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, true, "Test");

            var principal = CreatePrincipal("charlie", new List<string>());
            var candidate = authorizer.IsAuthorized("Default", null, principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
        }

        [Test]
        public void DefaultActivityDenyUserTakesPrecedence()
        {
            var scope = SampleScope(null);
            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, true, "Test");

            var principal = CreatePrincipal("alice", new List<string> {"b"});
            var candidate = authorizer.IsAuthorized("Default", null, principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
        }

        [Test]
        public void DefaultActivityDenyRoleTakesPrecendence()
        {
            var scope = SampleScope(null);
            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, true, "Test");

            var principal = CreatePrincipal("bob", new List<string> {"a"});
            var candidate = authorizer.IsAuthorized("Default", null, principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
        }

        [Test]
        public void DefaultActivityDenyClaimTakesPrecendence()
        {
            var scope = SampleScope(null);
            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, true, "Test");

            var principal = CreatePrincipal("bob", new List<string>(), new List<string> {"p"});
            var candidate = authorizer.IsAuthorized("Default", null, principal);

            Assert.AreEqual(false, candidate.IsAuthorized, "IsAuthorized differs");
        }

        [Test]
        public void DefaultActivityGrantUser()
        {
            var scope = SampleScope(null);
            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, true, "Test");

            var principal = CreatePrincipal("bob", new List<string> {"c"});
            var candidate = authorizer.IsAuthorized("Default", null, principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
        }

        [Test]
        public void DefaultActivityGrantRole()
        {
            var scope = SampleScope(null);
            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("charlie", new List<string> {"b"});
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
        }

        [Test]
        public void DefaultActivityGrantClaime()
        {
            var scope = SampleScope(null);
            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("charlie", new List<string> {"b"});
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
        }

        [Test]
        public void ActivityDenyUserTakesPrecedence()
        {
            var scope = SampleScope(null);
            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("alice", new List<string> {"b"});
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("User: alice"), "Reason differs");
        }

        [Test]
        public void ActivityDenyRoleTakesPrecendence()
        {
            var scope = SampleScope(null);
            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("bob", new List<string> {"a"});
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("Role: a"), "Reason differs");
        }

        [Test]
        public void ActivityDenyClaimTakesPrecendence()
        {
            var scope = SampleScope(null);
            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("bob", new List<string>(), new List<string> {"p"});
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("Claim: team/p"), "Reason differs");
        }

        [Test]
        public void ActivityGrantUser()
        {
            var scope = SampleScope(null);
            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("bob", new List<string> {"c"});
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("User: bob"), "Reason differs");
        }

        [Test]
        public void ActivityGrantRole()
        {
            var scope = SampleScope(null);
            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("charlie", new List<string> {"b"});
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("Role: b"), "Reason differs");
        }

        [Test]
        public void ActivityGrantClaim()
        {
            var scope = SampleScope(null);
            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("charlie", new List<string>(), new List<string> {"q"});
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("Claim: team/q"), "Reason differs");
        }

        [Test]
        public void ActivityHierarchyDenyUserTakesPrecedence()
        {
            var scope = SampleScope(null);
            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("alice", new List<string> {"b"});
            var candidate = authorizer.IsAuthorized("Test", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            // NB Proves we got the "Test" activity
            Assert.IsNotNull(candidate.PrincipalReason);
            Assert.IsNull(candidate.PrincipalReason.Action);
        }

        [Test]
        public void ActivityHierarchyExplicitActionDenyUserTakesPrecedence()
        {
            var scope = SampleScope(null);
            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("alice", new List<string> {"b"});
            var candidate = authorizer.IsAuthorized("Test", "Foo", principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            // NB Proves we got the "Test.Foo" activity
            Assert.IsNull(candidate.PrincipalReason);
        }

        [Test]
        public void ActivityHierarchyDenyRoleTakesPrecendence()
        {
            var scope = SampleScope(null);
            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("bob", new List<string> {"a"});
            var candidate = authorizer.IsAuthorized("Test", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            // NB Proves we got the "Test" activity
            Assert.IsNotNull(candidate.PrincipalReason);
            Assert.IsNull(candidate.PrincipalReason.Action);
        }

        [Test]
        public void ActivityHierarchyExplicitActionDenyRoleTakesPrecendence()
        {
            var scope = SampleScope(null);
            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("bob", new List<string> {"a"});
            var candidate = authorizer.IsAuthorized("Test", "Foo", principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            // NB Proves we got the "Test.Foo" activity
            Assert.IsNull(candidate.PrincipalReason);
        }

        [Test]
        public void ActivityHierarchyGrantUser()
        {
            var scope = SampleScope(null);
            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("bob", new List<string> {"c"});
            var candidate = authorizer.IsAuthorized("Test", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
        }

        [Test]
        public void ActivityHierarchyGrantRole()
        {
            var scope = SampleScope(null);
            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, true);

            var principal = CreatePrincipal("charlie", new List<string> {"b"});
            var candidate = authorizer.IsAuthorized("Test", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
        }

        private AuthorizationScope SampleScope(bool? defaultAuthorize)
        {
            return new AuthorizationScope
            {
                Activities =
                {
                    new Activity
                    {
                        Resource = "Test",
                        Default = defaultAuthorize,
                        Deny = new Permission
                        {
                            Users = new List<string> {"alice"},
                            Roles = new List<string> {"a"},
                            Claims = new List<Claim>
                            {
                                new Claim("team", "p")
                            }
                        },
                        Allow = new Permission
                        {
                            Users = new List<string> {"bob"},
                            Roles = new List<string> {"b"},
                            Claims = new List<Claim>
                            {
                                new Claim("team", "q")
                            }
                        }
                    },
                    new Activity
                    {
                        Resource = "Test",
                        Action = "Foo",
                        Default = defaultAuthorize,
                        Deny = new Permission
                        {
                            Users = new List<string> {"alice"},
                            Roles = new List<string> {"a"},
                            Claims = new List<Claim>
                            {
                                new Claim("team", "p")
                            }
                        },
                        Allow = new Permission
                        {
                            Users = new List<string> {"bob"},
                            Roles = new List<string> {"b"},
                            Claims = new List<Claim>
                            {
                                new Claim("team", "q")
                            }
                        }
                    }
                }

            };
        }
    }
}