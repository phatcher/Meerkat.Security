using System.Collections.Generic;

using Meerkat.Security.Activities;

using NUnit.Framework;

namespace Meerkat.Test.Security.Activities
{
    public class UnauthenticatedUserFixture : PrincipalFixture
    {
        [Test]
        public void DefaultTrueUnauthenticatedTrue()
        {
            var activities = new List<Activity>();
            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, true, null, true);

            var principal = CreatePrincipal("fred", new List<string>(), null, false);
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.Null, "Reason differs");
        }

        [Test]
        public void DefaultTrueUnauthenticatedFalse()
        {
            var activities = new List<Activity>();
            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, true, null, false);

            var principal = CreatePrincipal("fred", new List<string>(), null, false);
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("IsAuthenticated: false"), "Reason differs");
        }

        [Test]
        public void DefaultFalseUnauthenticatedTrue()
        {
            var activities = new List<Activity>();
            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, false, null, true);

            var principal = CreatePrincipal("fred", new List<string>(), null, false);
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.Null, "Reason differs");
        }

        [Test]
        public void DefaultFalseUnauthenticatedFalse()
        {
            var activities = new List<Activity>();
            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, false, null, false);

            var principal = CreatePrincipal("fred", new List<string>(), null, false);
            var candidate = authorizer.IsAuthorized("Test", null, principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("IsAuthenticated: false"), "Reason differs");
        }

        [Test]
        public void UnauthenticatedActivityTrueDefaultTrue()
        {
            var activities = new List<Activity>
            {
                new Activity
                {
                    Resource = "Home",
                    Action = "Index",
                    AllowUnauthenticated = true,
                    // NB This allows us to override the top level default
                    Default = true,
                }
            }; 
            
            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, false, null, false);

            var principal = CreatePrincipal("fred", new List<string>(), null, false);
            var candidate = authorizer.IsAuthorized("Home", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.Null, "Reason differs");
        }

        [Test]
        public void UnauthenticatedActivityTrueDefaultFalse()
        {
            var activities = new List<Activity>
            {
                new Activity
                {
                    Resource = "Home",
                    Action = "Index",
                    AllowUnauthenticated = true,
                    // NB This allows us to override the top level default
                    Default = false,
                }
            };

            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, false, null, false);

            var principal = CreatePrincipal("fred", new List<string>(), null, false);
            var candidate = authorizer.IsAuthorized("Home", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.Null, "Reason differs");
        }

        [Test]
        public void UnauthenticatedActivityFalseDefaultTrue()
        {
            var activities = new List<Activity>
            {
                new Activity
                {
                    Resource = "Home",
                    Action = "Index",
                    AllowUnauthenticated = false,
                    // NB This allows us to override the top level default
                    Default = true,
                }
            };

            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, false, null, false);

            var principal = CreatePrincipal("fred", new List<string>(), null, false);
            var candidate = authorizer.IsAuthorized("Home", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("IsAuthenticated: false"), "Reason differs");
        }

        [Test]
        public void UnauthenticatedActivityFalseDefaultFalse()
        {
            var activities = new List<Activity>
            {
                new Activity
                {
                    Resource = "Home",
                    Action = "Index",
                    AllowUnauthenticated = false,
                    // NB This allows us to override the top level default
                    Default = false,
                }
            };

            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, false, null, false);

            var principal = CreatePrincipal("fred", new List<string>(), null, false);
            var candidate = authorizer.IsAuthorized("Home", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.EqualTo("IsAuthenticated: false"), "Reason differs");
        }

        [Test]
        public void AuthenticatedActivityTrueDefaultTrue()
        {
            var activities = new List<Activity>
            {
                new Activity
                {
                    Resource = "Home",
                    Action = "Index",
                    AllowUnauthenticated = true,
                    // NB This allows us to override the top level default
                    Default = true,
                }
            };

            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, false, null, false);

            var principal = CreatePrincipal("charlie", new List<string>());
            var candidate = authorizer.IsAuthorized("Home", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.Null, "Reason differs");
        }

        [Test]
        public void AuthenticatedActivityTrueDefaultFalse()
        {
            var activities = new List<Activity>
            {
                new Activity
                {
                    Resource = "Home",
                    Action = "Index",
                    AllowUnauthenticated = true,
                    // NB This allows us to override the top level default
                    Default = false,
                }
            };

            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, false, null, false);

            var principal = CreatePrincipal("charlie", new List<string>());
            var candidate = authorizer.IsAuthorized("Home", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.Null, "Reason differs");
        }

        [Test]
        public void AuthenticatedActivityFalseDefaultTrue()
        {
            var activities = new List<Activity>
            {
                new Activity
                {
                    Resource = "Home",
                    Action = "Index",
                    AllowUnauthenticated = false,
                    // NB This allows us to override the top level default
                    Default = true,
                }
            };

            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, false, null, false);

            var principal = CreatePrincipal("charlie", new List<string>());
            var candidate = authorizer.IsAuthorized("Home", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.True, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.Null, "Reason differs");
        }

        [Test]
        public void AuthenticatedActivityFalseDefaultFalse()
        {
            var activities = new List<Activity>
            {
                new Activity
                {
                    Resource = "Home",
                    Action = "Index",
                    AllowUnauthenticated = false,
                    // NB This allows us to override the top level default
                    Default = false,
                }
            };

            var provider = new StaticActivityProvider(activities);
            var authorizer = new ActivityAuthorizer(provider, false, null, false);

            var principal = CreatePrincipal("charlie", new List<string>());
            var candidate = authorizer.IsAuthorized("Home", "Index", principal);

            Assert.That(candidate.IsAuthorized, Is.False, "IsAuthorized differs");
            Assert.That(candidate.Reason, Is.Null, "Reason differs");
        }
    }
}
