using System;
using System.Collections.Generic;

using Meerkat.Caching;
using Meerkat.Security.Activities;

using Moq;

using NUnit.Framework;

namespace Meerkat.Test.Security.Activities
{
    [TestFixture]
    public class CachingActivityProviderFixture
    {
        [Test]
        public void ProviderActivities()
        {
            // Arrange
            var p1 = new Mock<IActivityProvider>();
            var cache = new Mock<ICache>();

            p1.Setup(x => x.Activities()).Returns(new List<Activity> { new Activity() });

            var provider = new CachingActivityProvider(p1.Object, cache.Object);

            // Act
            var expected = provider.Activities();

            // Assert
            Assert.That(expected.Count, Is.EqualTo(1), "Activity count differs");
            cache.Verify(x => x.Set("activities", It.IsAny<object>(), It.IsAny<DateTimeOffset>(), "activityProvider"));
        }

        [Test]
        public void ProviderDefaultActivity()
        {
            // Arrange
            var p1 = new Mock<IActivityProvider>();
            var cache = new Mock<ICache>();

            p1.Setup(x => x.DefaultActivity()).Returns("Foo");

            var provider = new CachingActivityProvider(p1.Object, cache.Object);

            // Act
            var expected = provider.DefaultActivity();

            // Assert
            Assert.That(expected, Is.EqualTo("Foo"), "Default activity differs");
            cache.Verify(x => x.Set("defaultActivity", It.IsAny<object>(), It.IsAny<DateTimeOffset>(), "activityProvider"));
        }

        [Test]
        public void ProviderDefaultAuthorization()
        {
            // Arrange
            var p1 = new Mock<IActivityProvider>();
            var cache = new Mock<ICache>();

            p1.Setup(x => x.DefaultAuthorization()).Returns(false);

            var provider = new CachingActivityProvider(p1.Object, cache.Object);

            // Act
            var expected = provider.DefaultAuthorization();

            // Assert
            Assert.That(expected, Is.EqualTo(false), "Default authorization differs");
            cache.Verify(x => x.Set("defaultAuthorization", It.IsAny<object>(), It.IsAny<DateTimeOffset>(), "activityProvider"));
        }

        [Test]
        public void CachedActivities()
        {
            // Arrange
            var p1 = new Mock<IActivityProvider>();
            var cache = new Mock<ICache>();

            cache.Setup(x => x.Contains("activities", "activityProvider")).Returns(true);
            cache.Setup(x => x.Get("activities", "activityProvider")).Returns(new List<Activity> { new Activity() });
            p1.Setup(x => x.Activities()).Returns(new List<Activity>());

            var provider = new CachingActivityProvider(p1.Object, cache.Object);

            // Act
            var expected = provider.Activities();

            // Assert
            Assert.That(expected.Count, Is.EqualTo(1), "Activity count differs");
        }

        [Test]
        public void CachedDefaultActivity()        
        {
            // Arrange
            var p1 = new Mock<IActivityProvider>();
            var cache = new Mock<ICache>();

            cache.Setup(x => x.Contains("defaultActivity", "activityProvider")).Returns(true);
            cache.Setup(x => x.Get("defaultActivity", "activityProvider")).Returns("Foo");
            p1.Setup(x => x.Activities()).Returns(new List<Activity>());

            var provider = new CachingActivityProvider(p1.Object, cache.Object);

            // Act
            var expected = provider.DefaultActivity();

            // Assert
            Assert.That(expected, Is.EqualTo("Foo"), "Default activity differs");
        }

        [Test]
        public void CachedDefaultAuthorization()
        {
            // Arrange
            var p1 = new Mock<IActivityProvider>();
            var cache = new Mock<ICache>();

            cache.Setup(x => x.Contains("defaultAuthorization", "activityProvider")).Returns(true);
            cache.Setup(x => x.Get("defaultAuthorization", "activityProvider")).Returns(false);
            p1.Setup(x => x.Activities()).Returns(new List<Activity>());

            var provider = new CachingActivityProvider(p1.Object, cache.Object);

            // Act
            var expected = provider.DefaultAuthorization();

            // Assert
            Assert.That(expected, Is.EqualTo(false), "Default authorization differs");

        }
    }
}