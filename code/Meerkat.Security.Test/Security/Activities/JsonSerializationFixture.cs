using System.Collections.Generic;
using System.Security.Claims;

using Meerkat.Security.Activities;

using Newtonsoft.Json;

using NUnit.Framework;

namespace Meerkat.Test.Security.Activities
{
    [TestFixture]
    public class JsonSerializationFixture : Fixture
    {
        [Test]
        public void Basic()
        {
            var source = new Authorizations
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
                            Users = {"A", "D"},
                            Claims = new List<Claim>
                            {
                                new Claim("team", "E", string.Empty, "bar")
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