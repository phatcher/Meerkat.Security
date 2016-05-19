using System.Collections.Generic;
using System.Security.Principal;

using Moq;

namespace Meerkat.Test
{
    public static class MoqExtensions
    {
        /// <summary>
        /// Create a mock for <see cref="IPrincipal"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public static Mock<IPrincipal> MockPrincipal(string name, ICollection<string> roles)
        {
            var mockIdentity = new Mock<IIdentity>();
            mockIdentity.SetupGet(x => x.Name).Returns(name);
            mockIdentity.SetupGet(x => x.IsAuthenticated).Returns(true);

            var mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.SetupGet(x => x.Identity).Returns(mockIdentity.Object);
            mockPrincipal.Setup(x => x.IsInRole(It.IsAny<string>())).Returns<string>(roles.Contains);

            return mockPrincipal;
        }
    }
}