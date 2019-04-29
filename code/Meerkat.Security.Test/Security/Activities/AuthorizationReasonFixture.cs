using System.Collections.Generic;

using Meerkat.Security.Activities;
using NUnit.Framework;

namespace Meerkat.Test.Security.Activities
{
    [TestFixture]
    public class AuthorizationReasonFixture : PrincipalFixture
    {
        public void AuthorizedRole()
        {
            var reason = new AuthorizationReason
            {
                Resource = "Entity",
                Action = "Read",
                Principal = CreatePrincipal("John", new List<string> { "Admin", "User"}),               
                NoDecision = false,
                IsAuthorized = true,
                Reason = "Role: User"
            };
        }
    }
}