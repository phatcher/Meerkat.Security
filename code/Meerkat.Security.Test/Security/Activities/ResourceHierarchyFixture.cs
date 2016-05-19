using System.Collections.Generic;
using System.Linq;

using Meerkat.Security.Activities;

using NUnit.Framework;

namespace Meerkat.Test.Security.Activities
{
    [TestFixture]
    public class ResourceHierarchyFixture : Fixture
    {
        [Test]
        public void Resource()
        {
            IList<string> expected = new[]
            {
                "Resource"
            };

            var candidate = ActivityExtensions.Activities("Resource", null).ToList();

            Check(expected, candidate);
        }

        [Test]
        public void ResourceAction()
        {
            IList<string> expected = new[]
            {
                "Resource.Action",
                "Resource",
                ".Action"
            };

            var candidate = ActivityExtensions.Activities("Resource", "Action").ToList();

            Check(expected, candidate);
        }

        [Test]
        public void ResourceHiearchyAction()
        {
            IList<string> expected = new[]
            {
                "Resource/Foo.Action",
                "Resource/Foo",
                "Resource.Action",
                "Resource",
                ".Action"
            };

            var candidate = ActivityExtensions.Activities("Resource/Foo", "Action").ToList();

            Check(expected, candidate);
        }

        [Test]
        public void ResourceActionHierarchy()
        {
            IList<string> expected = new[]
            {
                "Resource.Action/Bar",
                "Resource.Action",
                "Resource",
                ".Action/Bar",
                ".Action"
            };

            var candidate = ActivityExtensions.Activities("Resource", "Action/Bar").ToList();

            Check(expected, candidate);
        }

        [Test]
        public void ResourceHierarchyActionHierarchy()
        {
            IList<string> expected = new[]
            {
                "Resource/Foo.Action/Bar",
                "Resource/Foo.Action",
                "Resource/Foo",
                "Resource.Action/Bar",
                "Resource.Action",
                "Resource",
                ".Action/Bar",
                ".Action"
            };

            var candidate = ActivityExtensions.Activities("Resource/Foo", "Action/Bar").ToList();

            Check(expected, candidate);
        }
    }
}
