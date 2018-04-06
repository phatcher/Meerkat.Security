using System.Security.Claims;

using Meerkat.Security.Activities;

using NUnit.Framework;

namespace Meerkat.Test.Security.Activities
{
    [TestFixture]
    public class JsonSerializationFixture : Fixture
    {
        [Test]
        public void Standard()
        {
            var source = new AuthorizationScope
            {
                DefaultActivity = "Foo",
                Activities =
                {
                    new Activity
                    {
                        Resource = "Home",
                        Action = "Index",
                        Deny = new Permission
                        {
                            Users = { "A", "D" },
                            Claims = 
                            {
                                new Claim("team", "E", string.Empty, "bar")
                            }
                        },
                        Allow = new Permission
                        {
                            Roles = { "B", "C" },
                            Claims =
                            {
                                new Claim("team", "F", string.Empty, "bar"),
                                new Claim("team", "G", string.Empty, "bar")
                            }
                        }
                    },
                    new Activity
                    {
                        Resource = "Foo",
                        Deny = new Permission
                        {
                            Users = { "A", "D" },
                            Claims =
                            {
                                new Claim("team", "E", string.Empty, "bar")
                            }
                        },
                        Allow = new Permission
                        {
                            Roles = { "B", "C" },
                            Claims =
                            {
                                new Claim("team", "F", string.Empty, "bar"),
                                new Claim("team", "G", string.Empty, "bar")
                            }
                        }
                    }
                }
            };

            var json = source.ToJson();

            var candidate = json.ToAuthorizations();

            Check(source, candidate);
        }

        [Test]

        public void DefaultAuthorizationTrue()
        {
            var source = new AuthorizationScope
            {
                DefaultAuthorization = true,
                AllowUnauthenticated = true,
                Activities =
                {
                    new Activity
                    {
                        Default = true,
                        AllowUnauthenticated = true,
                        Resource = "Home",
                        Action = "Index",
                        Deny = new Permission
                        {
                            Users = { "A", "C" },
                            Claims =
                            {
                                new Claim("team", "E")
                            }
                        },
                        Allow = new Permission
                        {
                            Roles = { "B" },
                            Claims =
                            {
                                new Claim("team", "F")
                            }
                        }
                    }
                }
            };

            var json = source.ToJson();

            var candidate = json.ToAuthorizations();

            Check(source, candidate);
        }

        [Test]

        public void DefaultAuthorizationFalse()
        {
            var source = new AuthorizationScope
            {
                DefaultAuthorization = false,
                AllowUnauthenticated = false,
                Activities =
                {
                    new Activity
                    {
                        Default = false,
                        AllowUnauthenticated = false,
                        Resource = "Home",
                        Action = "Index",
                        Deny = new Permission
                        {
                            Users = { "A", "C" },
                            Claims =
                            {
                                new Claim("team", "E")
                            }
                        },
                        Allow = new Permission
                        {
                            Roles = { "B" },
                            Claims =
                            {
                                new Claim("team", "F")
                            }
                        }
                    }
                }
            };

            var json = source.ToJson();

            var candidate = json.ToAuthorizations();

            Check(source, candidate);
        }


        [Test]

        public void DefaultAuthorizationMixed()
        {
            var source = new AuthorizationScope
            {
                DefaultAuthorization = false,
                AllowUnauthenticated = false,
                Activities =
                {
                    new Activity
                    {
                        Default = true,
                        AllowUnauthenticated = true,
                        Resource = "Home",
                        Action = "Index",
                        Deny = new Permission
                        {
                            Users = { "A", "C" },
                            Claims =
                            {
                                new Claim("team", "E")
                            }
                        },
                        Allow = new Permission
                        {
                            Roles = { "B" },
                            Claims =
                            {
                                new Claim("team", "F")
                            }
                        }
                    }
                }
            };

            var json = source.ToJson();

            var candidate = json.ToAuthorizations();

            Check(source, candidate);
        }
    }
}