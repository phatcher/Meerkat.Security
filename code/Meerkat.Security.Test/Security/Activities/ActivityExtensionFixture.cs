using System.Collections.Generic;
using System.Security.Claims;

using Meerkat.Security.Activities;
using Meerkat.Security.Activities.Configuration;

using NUnit.Framework;

namespace Meerkat.Test.Security.Activities
{
    [TestFixture]
    public class ActivityExtensionFixture : Fixture
    {
        [Test]
        public void ToPermission()
        {
            var element = new PermissionElement
            {
                Roles = "A, B, C",
                Users = "Bob, Sue",
                Claims = new ClaimElementCollection
                {
                    new ClaimElement { Name = "team", Issuer = "Me", Claims = "F, G" },
                    new ClaimElement { Name = "department", Claims = "H" }
                }
            };

            var expected = new Permission
            {
                Roles = new List<string> { "A", "B", "C" },
                Users = new List<string> { "Bob", "Sue" },
                Claims = new List<Claim>
                {
                    new Claim("team", "F", null, "Me"),
                    new Claim("team", "G", null, "Me"),
                    new Claim("department", "H"),
                }
            };

            var candidate = element.ToPermission();

            Check(expected, candidate);
        }

        [Test]
        public void ToPermissionIgnoreWhitespace()
        {
            var element = new PermissionElement
            {
                Roles = " A,B, C",
                Users = "  Alice, Bob  ",
                Claims = new ClaimElementCollection
                {
                    new ClaimElement { Name = " team", Claims = " F, G " },
                    new ClaimElement { Name = "department ", Claims = "H " }
                }
            };

            var expected = new Permission
            {
                Roles = new List<string> { "A", "B", "C" },
                Users = new List<string> { "Alice", "Bob" },
                Claims = new List<Claim>
                {
                    new Claim("team", "F"),
                    new Claim("team", "G"),
                    new Claim("department", "H"),
                }
            };

            var candidate = element.ToPermission();

            Check(expected, candidate);
        }

        [Test]
        public void ToActivity()
        {
            var element = new ActivityElement
            {
                Name = "Resource.Action",
                Allow = new PermissionElement
                {
                    Roles = "A",
                    Users = "Alice"
                },
                Deny = new PermissionElement
                {
                    Roles = "B",
                    Users = "Bob"
                }
            };

            var expected = new Activity
            {
                Resource = "Resource",
                Action = "Action",
                Allow = new Permission
                {
                    Roles = new List<string> { "A" },
                    Users = new List<string> { "Alice" }
                },
                Deny = new Permission
                {
                    Roles = new List<string> { "B" },
                    Users = new List<string> { "Bob" }
                },
            };

            var candidate = element.ToActivity();

            Check(expected, candidate);
        }
    }
}