using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Meerkat.Security.Activities;
using Moq;
using NUnit.Framework;

namespace Meerkat.Test.Web.Mvc
{
    [TestFixture]
    public class ActivityAuthorizeAttributeFixture : Fixture
    {
        [Test]
        public void UnauthenticateDelegatesToAuthorizer()
        {
            var context = new AuthorizationContext();
            var authorizer = new Mock<IActivityAuthorizer>();

        }
    }
}
