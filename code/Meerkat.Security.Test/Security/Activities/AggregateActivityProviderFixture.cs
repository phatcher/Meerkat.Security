using System.Collections.Generic;

using Meerkat.Security.Activities;

using Moq;

using NUnit.Framework;

namespace Meerkat.Test.Security.Activities
{
    [TestFixture]
    public class AggregateActivityProviderFixture
    {
        [Test]
        public void MultipleProviderActivities()
        {
            var p1 = new Mock<IActivityProvider>();
            var p2 = new Mock<IActivityProvider>();
            var providers = new [] { p1.Object, p2.Object };

            var a1 = new Activity();
            var a2 = new Activity();
            p1.Setup(x => x.Activities()).Returns(new List<Activity> { a1 });
            p2.Setup(x => x.Activities()).Returns(new List<Activity> { a2 });

            var provider = new AggregateActivityProvider(providers);

            var expected = provider.Activities();

            Assert.That(expected.Count, Is.EqualTo(2), "Activity count differ");
            Assert.That(expected[0], Is.SameAs(a1), "First activity differs");
        }

        [Test]
        public void FirstProviderDefaultActivity()
        {
            var p1 = new Mock<IActivityProvider>();
            var p2 = new Mock<IActivityProvider>();
            var providers = new[] { p1.Object, p2.Object };

            p1.Setup(x => x.DefaultActivity()).Returns("Foo");

            var provider = new AggregateActivityProvider(providers);

            var expected = provider.DefaultActivity();

            Assert.That(expected, Is.EqualTo("Foo"), "Default activity differ");
        }

        [Test]
        public void LastProviderDefaultActivity()
        {
            var p1 = new Mock<IActivityProvider>();
            var p2 = new Mock<IActivityProvider>();
            var providers = new[] { p1.Object, p2.Object };

            p1.Setup(x => x.DefaultActivity()).Returns("Foo");
            p1.Setup(x => x.DefaultActivity()).Returns("Bar");

            var provider = new AggregateActivityProvider(providers);

            var expected = provider.DefaultActivity();

            Assert.That(expected, Is.EqualTo("Bar"), "Default activity differ");
        }

        [Test]
        public void FirstProviderDefaultAuthorization()
        {
            var p1 = new Mock<IActivityProvider>();
            var p2 = new Mock<IActivityProvider>();
            var providers = new[] { p1.Object, p2.Object };

            p1.Setup(x => x.DefaultAuthorization()).Returns(false);

            var provider = new AggregateActivityProvider(providers);

            var expected = provider.DefaultAuthorization();

            Assert.That(expected, Is.EqualTo(false), "Default authorization differ");
        }

        [Test]
        public void LastProviderDefaultAuthorization()
        {
            var p1 = new Mock<IActivityProvider>();
            var p2 = new Mock<IActivityProvider>();
            var providers = new[] { p1.Object, p2.Object };

            p1.Setup(x => x.DefaultAuthorization()).Returns(false);
            p2.Setup(x => x.DefaultAuthorization()).Returns(true);

            var provider = new AggregateActivityProvider(providers);

            var expected = provider.DefaultAuthorization();

            Assert.That(expected, Is.EqualTo(true), "Default authorization differ");
        }
    }
}