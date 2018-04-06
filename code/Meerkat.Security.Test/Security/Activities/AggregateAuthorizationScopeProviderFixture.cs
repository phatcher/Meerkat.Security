using System.Threading;
using System.Threading.Tasks;

using Meerkat.Security.Activities;

using Moq;

using NUnit.Framework;

namespace Meerkat.Test.Security.Activities
{
    [TestFixture]
    public class AggregateAuthorizationScopeProviderFixture
    {
        [Test]
        public async Task MultipleProviderActivities()
        {
            var p1 = new Mock<IAuthorizationScopeProvider>();
            var p2 = new Mock<IAuthorizationScopeProvider>();
            var providers = new [] { p1.Object, p2.Object };

            var a1 = new Activity();
            var a2 = new Activity();
            p1.Setup(x => x.AuthorizationScopeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new AuthorizationScope { Activities = { a1 }});
            p2.Setup(x => x.AuthorizationScopeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new AuthorizationScope { Activities = { a2 }});

            var provider = new AggregateAuthorizationScopeProvider("Aggregate", providers);

            var expected = await provider.AuthorizationScopeAsync().ConfigureAwait(false);

            Assert.That(expected.Activities.Count, Is.EqualTo(2), "Activity count differ");
            Assert.That(expected.Activities[0], Is.SameAs(a1), "First activity differs");
        }

        [Test]
        public async Task FirstProviderDefaultActivity()
        {
            var p1 = new Mock<IAuthorizationScopeProvider>();
            var p2 = new Mock<IAuthorizationScopeProvider>();
            var providers = new[] { p1.Object, p2.Object };

            // NB Deliberately don't setup for p2 to check null handling
            p1.Setup(x => x.AuthorizationScopeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new AuthorizationScope { DefaultActivity = "Foo" });

            var provider = new AggregateAuthorizationScopeProvider("Aggregate", providers);

            var expected = await provider.AuthorizationScopeAsync().ConfigureAwait(false);

            Assert.That(expected.DefaultActivity, Is.EqualTo("Foo"), "Default activity differ");
        }

        [Test]
        public async Task LastProviderDefaultActivity()
        {
            var p1 = new Mock<IAuthorizationScopeProvider>();
            var p2 = new Mock<IAuthorizationScopeProvider>();
            var providers = new[] { p1.Object, p2.Object };

            p1.Setup(x => x.AuthorizationScopeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new AuthorizationScope { DefaultActivity = "Foo" });
            p2.Setup(x => x.AuthorizationScopeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new AuthorizationScope { DefaultActivity = "Bar" });

            var provider = new AggregateAuthorizationScopeProvider("Aggregate", providers);

            var expected = await provider.AuthorizationScopeAsync().ConfigureAwait(false);

            Assert.That(expected.DefaultActivity, Is.EqualTo("Bar"), "Default activity differ");
        }

        [Test]
        public async Task FirstProviderDefaultAuthorization()
        {
            var p1 = new Mock<IAuthorizationScopeProvider>();
            var p2 = new Mock<IAuthorizationScopeProvider>();
            var providers = new[] { p1.Object, p2.Object };

            p1.Setup(x => x.AuthorizationScopeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new AuthorizationScope { DefaultAuthorization = false });
            p2.Setup(x => x.AuthorizationScopeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new AuthorizationScope());

            var provider = new AggregateAuthorizationScopeProvider("Aggregate", providers);

            var expected = await provider.AuthorizationScopeAsync().ConfigureAwait(false);

            Assert.That(expected.DefaultAuthorization, Is.EqualTo(false), "Default authorization differ");
        }

        [Test]
        public async Task LastProviderDefaultAuthorization()
        {
            var p1 = new Mock<IAuthorizationScopeProvider>();
            var p2 = new Mock<IAuthorizationScopeProvider>();
            var providers = new[] { p1.Object, p2.Object };

            p1.Setup(x => x.AuthorizationScopeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new AuthorizationScope { DefaultAuthorization = false });
            p2.Setup(x => x.AuthorizationScopeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new AuthorizationScope { DefaultAuthorization = true });

            var provider = new AggregateAuthorizationScopeProvider("Aggregate", providers);

            var expected = await provider.AuthorizationScopeAsync().ConfigureAwait(false);

            Assert.That(expected.DefaultAuthorization, Is.EqualTo(true), "Default authorization differ");
        }
    }
}