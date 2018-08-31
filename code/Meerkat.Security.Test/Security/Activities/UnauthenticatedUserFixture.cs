using System.Collections.Generic;

using Meerkat.Security.Activities;

using NUnit.Framework;

namespace Meerkat.Test.Security.Activities
{
    [TestFixture]
    public class UnauthenticatedUserFixture : PrincipalFixture
    {
        [Test]
        public void DefaultTrueUnauthenticatedTrue()
        {
            var scope = new AuthorizationScope();
            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, true, null, true);

            var principal = CreatePrincipal("fred", new List<string>(), null, false);
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.Null, "Reason differs");
        }

        [Test]
        public void DefaultTrueUnauthenticatedFalse()
        {
            var scope = new AuthorizationScope();
            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, true, null, false);

            var principal = CreatePrincipal("fred", new List<string>(), null, false);
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("IsAuthenticated: false"), "Reason differs");
        }

        [Test]
        public void DefaultFalseUnauthenticatedTrue()
        {
            var scope = new AuthorizationScope();
            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, false, null, true);

            var principal = CreatePrincipal("fred", new List<string>(), null, false);
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.Null, "Reason differs");
        }

        [Test]
        public void DefaultFalseUnauthenticatedFalse()
        {
            var scope = new AuthorizationScope();
            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, false, null, false);

            var principal = CreatePrincipal("fred", new List<string>(), null, false);
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("IsAuthenticated: false"), "Reason differs");
        }

        [Test]
        public void UnauthenticatedActivityNullDefaultTrue()
        {
            var scope = new AuthorizationScope
            {
                Activities =
                {
                    new Activity
                    {
                        Resource = "Home",
                        Action = "Index",
                        //AllowUnauthenticated = true,
                        // NB This allows us to override the top level default
                        Default = true,
                    }
                }
            };

            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, false, null, false);

            var principal = CreatePrincipal("fred", new List<string>(), null, false);
            var candidate = authorizer.IsAuthorized("Home", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("Default: True"), "Reason differs");
        }

        [Test]
        public void UnauthenticatedActivityNullDefaultFalse()
        {
            var scope = new AuthorizationScope
            {
                Activities =
                {
                    new Activity
                    {
                        Resource = "Home",
                        Action = "Index",
                        //AllowUnauthenticated = true,
                        // NB This allows us to override the top level default
                        Default = false,
                    }
                }
            };

            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, false, null, false);

            var principal = CreatePrincipal("fred", new List<string>(), null, false);
            var candidate = authorizer.IsAuthorized("Home", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("Default: False"), "Reason differs");
        }

        [Test]
        public void UnauthenticatedActivityTrueDefaultTrue()
        {
            var scope = new AuthorizationScope
            {
                Activities =
                {
                    new Activity
                    {
                        Resource = "Home",
                        Action = "Index",
                        AllowUnauthenticated = true,
                        // NB This allows us to override the top level default
                        Default = true,
                    }
                }
            };

            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, false, null, false);

            var principal = CreatePrincipal("fred", new List<string>(), null, false);
            var candidate = authorizer.IsAuthorized("Home", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("IsAuthenticated: false"), "Reason differs");
        }

        [Test]
        public void UnauthenticatedActivityTrueDefaultFalse()
        {
            var scope = new AuthorizationScope
            {
                Activities =
                {
                    new Activity
                    {
                        Resource = "Home",
                        Action = "Index",
                        AllowUnauthenticated = true,
                        // NB This allows us to override the top level default
                        Default = false,
                    }
                }
            };

            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, false, null, false);

            var principal = CreatePrincipal("fred", new List<string>(), null, false);
            var candidate = authorizer.IsAuthorized("Home", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("IsAuthenticated: false"), "Reason differs");
        }

        [Test]
        public void UnauthenticatedActivityFalseDefaultTrue()
        {
            var scope = new AuthorizationScope
            {
                Activities =
                {
                    new Activity
                    {
                        Resource = "Home",
                        Action = "Index",
                        AllowUnauthenticated = false,
                        // NB This allows us to override the top level default
                        Default = true,
                    }
                }
            };

            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, false, null, false);

            var principal = CreatePrincipal("fred", new List<string>(), null, false);
            var candidate = authorizer.IsAuthorized("Home", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("IsAuthenticated: false"), "Reason differs");
        }

        [Test]
        public void UnauthenticatedActivityFalseDefaultFalse()
        {
            var scope = new AuthorizationScope
            {
                Activities =
                {
                    new Activity
                    {
                        Resource = "Home",
                        Action = "Index",
                        AllowUnauthenticated = false,
                        // NB This allows us to override the top level default
                        Default = false,
                    }
                }
            };

            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, false, null, false);

            var principal = CreatePrincipal("fred", new List<string>(), null, false);
            var candidate = authorizer.IsAuthorized("Home", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("IsAuthenticated: false"), "Reason differs");
        }

        [Test]
        public void AuthenticatedActivityNullDefaultTrue()
        {
            var scope = new AuthorizationScope
            {
                Activities =
                {
                    new Activity
                    {
                        Resource = "Home",
                        Action = "Index",
                        //AllowUnauthenticated = true,
                        // NB This allows us to override the top level default
                        Default = true,
                    }
                }
            };

            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, false, null, false);

            var principal = CreatePrincipal("charlie", new List<string>());
            var candidate = authorizer.IsAuthorized("Home", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("Default: True"), "Reason differs");
        }

        [Test]
        public void AuthenticatedActivityNullDefaultFalse()
        {
            var scope = new AuthorizationScope
            {
                Activities =
                {
                    new Activity
                    {
                        Resource = "Home",
                        Action = "Index",
                        //AllowUnauthenticated = true,
                        // NB This allows us to override the top level default
                        Default = false,
                    }
                }
            };

            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, false, null, false);

            var principal = CreatePrincipal("charlie", new List<string>());
            var candidate = authorizer.IsAuthorized("Home", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("Default: False"), "Reason differs");
        }

        [Test]
        public void AuthenticatedActivityTrueDefaultTrue()
        {
            var scope = new AuthorizationScope
            {
                Activities =
                {
                    new Activity
                    {
                        Resource = "Home",
                        Action = "Index",
                        AllowUnauthenticated = true,
                        // NB This allows us to override the top level default
                        Default = true,
                    }
                }
            };

            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, false, null, false);

            var principal = CreatePrincipal("charlie", new List<string>());
            var candidate = authorizer.IsAuthorized("Home", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("Default: True"), "Reason differs");
        }

        [Test]
        public void AuthenticatedActivityTrueDefaultFalse()
        {
            var scope = new AuthorizationScope
            {
                Activities =
                {
                    new Activity
                    {
                        Resource = "Home",
                        Action = "Index",
                        AllowUnauthenticated = true,
                        // NB This allows us to override the top level default
                        Default = false,
                    }
                }
            };

            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, false, null, false);

            var principal = CreatePrincipal("charlie", new List<string>());
            var candidate = authorizer.IsAuthorized("Home", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("Default: False"), "Reason differs");
        }

        [Test]
        public void AuthenticatedActivityFalseDefaultTrue()
        {
            var scope = new AuthorizationScope
            {
                Activities =
                {
                    new Activity
                    {
                        Resource = "Home",
                        Action = "Index",
                        AllowUnauthenticated = false,
                        // NB This allows us to override the top level default
                        Default = true,
                    }
                }
            };

            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, false, null, false);

            var principal = CreatePrincipal("charlie", new List<string>());
            var candidate = authorizer.IsAuthorized("Home", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("Default: True"), "Reason differs");
        }

        [Test]
        public void AuthenticatedActivityFalseDefaultFalse()
        {
            var scope = new AuthorizationScope
            {
                Activities =
                {
                    new Activity
                    {
                        Resource = "Home",
                        Action = "Index",
                        AllowUnauthenticated = false,
                        // NB This allows us to override the top level default
                        Default = false,
                    }
                }
            };

            var provider = new StaticAuthorizationScopeProvider(scope);
            var authorizer = new ActivityAuthorizer(provider, false, null, false);

            var principal = CreatePrincipal("charlie", new List<string>());
            var candidate = authorizer.IsAuthorized("Home", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("Default: False"), "Reason differs");
        }
    }
}