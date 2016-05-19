using System;
using System.Collections.Generic;
using System.Configuration;

using AngloAmerican.TradingSystems.Security.Activities;
using AngloAmerican.TradingSystems.Security.Activities.Configuration;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AngloAmerican.TradingSystems.Test.Security.Activities
{
    [TestClass]
    public class ActivityExtensionsTests : Fixture
    {
        [TestMethod]
        public void Convert()
        {
            var section = (ActivityAuthorizationSection)ConfigurationManager.GetSection("activityAuthorization");

            var activities = section.ToActivitites();

            Assert.AreEqual(1, activities.Count, "Activity count incorrect");
            Assert.IsFalse(section.Default, "Authorization default incorrect");

            var expected = new Activity
            {
                Resource = "Home",
                Action = "Index",
                Deny = new Permission
                {
                    Users = new List<string> { "A", "D" }
                },
                Allow = new Permission
                {
                    Roles = new List<string> { "B", "C" }
                }
            };

            var candidate = activities[0];

            Check(expected, candidate);
        }

        [TestMethod]
        public void ToActivity()
        {
            var element = new ActivityElement
            {
                Name = "Home.Index"
            };

            var expected = new Activity
            {
                Resource = "Home",
                Action = "Index"
            };

            var candidate = element.ToActivity();

            Check(expected, candidate);
        }

        [TestMethod]
        public void Resource()
        {
            foreach (var activity in ActivityExtensions.Activities("Invoice", null))
            {
                Console.WriteLine(activity);
            }
        }

        [TestMethod]
        public void ResourceAction()
        {
            foreach (var activity in ActivityExtensions.Activities("Invoice", "Update"))
            {
                Console.WriteLine(activity);
            }
        }

        [TestMethod]
        public void ActionHierarchy()
        {
            foreach (var activity in ActivityExtensions.Activities("Invoice", "Update/HighValue"))
            {
                Console.WriteLine(activity);
            }
        }

        [TestMethod]
        public void ResourceHierarchy()
        {
            foreach (var activity in ActivityExtensions.Activities("Invoice/Amount", "Update"))
            {
                Console.WriteLine(activity);
            }
        }

        [TestMethod]
        public void DualHierarchy()
        {
            foreach (var activity in ActivityExtensions.Activities("Invoice/Amount", "Update/HighValue"))
            {
                Console.WriteLine(activity);
            }
        }
    }
}