using System.Collections.Generic;

using Meerkat.Security.Activities;

using NUnit.Framework;

namespace Meerkat.Test.Security.Activities
{
    [TestFixture]
    public class ToDictionaryFixture
    {
        [Test]
        public void ActivitiesPresent()
        {
            var a1 = new Activity { Resource = "Home", Action = "Index" };
            var a2 = new Activity { Resource = "Invoice" };
            var activities = new List<Activity> { a1, a2 };

            var dictionary = activities.ToDictionary();

            Assert.That(dictionary.Keys.Count, Is.EqualTo(2), "Count differs");
            Assert.That(dictionary["Home.Index"], Is.SameAs(a1), "Home.Index differs");
            Assert.That(dictionary["Invoice"], Is.SameAs(a2), "Home.Index differs");
        }

        [Test]
        public void LastActivtyWins()
        {
            var a1 = new Activity { Resource = "Home", Action = "Index" };
            var a2 = new Activity { Resource = "Home", Action = "Index" };
            var activities = new List<Activity> { a1, a2 };

            var dictionary = activities.ToDictionary();

            Assert.That(dictionary.Keys.Count, Is.EqualTo(1), "Count differs");
            Assert.That(dictionary["Home.Index"], Is.SameAs(a2), "Home.Index differs");
        }
    }
}