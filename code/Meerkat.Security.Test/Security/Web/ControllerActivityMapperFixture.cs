using Meerkat.Security.Web;

using NUnit.Framework;

namespace Meerkat.Test.Security.Web
{
    [TestFixture]
    public class ControllerActivityMapperFixture : Fixture
    {
        private ControllerActivityMapper mapper;

        [Test]
        public void GetResource()
        {
            var candidate = mapper.Resource("Customer", "Get");

            Assert.That(candidate, Is.EqualTo("Customer"), "Resource differs");
        }

        [Test]
        public void GetAction()
        {
            var candidate = mapper.Action("Customer", "Get");

            Assert.That(candidate, Is.EqualTo("Read"), "Action differs");
        }

        [Test]
        public void GetPluralsResource()
        {
            var candidate = mapper.Resource("Alert", "GetAlerts");

            Assert.That(candidate, Is.EqualTo("Alert"), "Resource differs");
        }

        [Test]
        public void GetPluralsAction()
        {
            var candidate = mapper.Action("Alert", "GetAlerts");

            Assert.That(candidate, Is.EqualTo("Read"), "Action differs");
        }

        [Test]
        public void GetPluraliesResource()
        {
            var candidate = mapper.Resource("Party", "GetCompanies");

            Assert.That(candidate, Is.EqualTo("Company"), "Resource differs");
        }

        [Test]
        public void GetPluraliesAction()
        {
            var candidate = mapper.Action("Party", "GetCompanies");

            Assert.That(candidate, Is.EqualTo("Read"), "Action differs");
        }

        [Test]
        public void DetailsResource()
        {
            var candidate = mapper.Resource("Customer", "Details");

            Assert.That(candidate, Is.EqualTo("Customer"), "Resource differs");
        }

        [Test]
        public void DetailsAction()
        {
            var candidate = mapper.Action("Customer", "Details");

            Assert.That(candidate, Is.EqualTo("Read"), "Action differs");
        }

        [Test]
        public void PostResource()
        {
            var candidate = mapper.Resource("Customer", "Post");

            Assert.That(candidate, Is.EqualTo("Customer"), "Resource differs");
        }

        [Test]
        public void PostAction()
        {
            var candidate = mapper.Action("Customer", "Post");

            Assert.That(candidate, Is.EqualTo("Create"), "Action differs");
        }

        [Test]
        public void PatchResource()
        {
            var candidate = mapper.Resource("Customer", "Patch");

            Assert.That(candidate, Is.EqualTo("Customer"), "Resource differs");
        }

        [Test]
        public void PatchAction()
        {
            var candidate = mapper.Action("Customer", "Patch");

            Assert.That(candidate, Is.EqualTo("Update"), "Action differs");
        }

        [Test]
        public void PutResource()
        {
            var candidate = mapper.Resource("Customer", "Put");

            Assert.That(candidate, Is.EqualTo("Customer"), "Resource differs");
        }

        [Test]
        public void PutAction()
        {
            var candidate = mapper.Action("Customer", "Put");

            Assert.That(candidate, Is.EqualTo("Update"), "Action differs");
        }

        [Test]
        public void GetListFromResource()
        {
            var candidate = mapper.Resource("Party", "GetFromPerson");

            Assert.That(candidate, Is.EqualTo("Person"), "Resource differs");
        }

        [Test]
        public void GetListFromAction()
        {
            var candidate = mapper.Action("Party", "GetFromPerson");

            Assert.That(candidate, Is.EqualTo("Read"), "Action differs");
        }

        [Test]
        public void GetFromResource()
        {
            var candidate = mapper.Resource("Party", "GetPerson");

            Assert.That(candidate, Is.EqualTo("Person"), "Resource differs");
        }

        [Test]
        public void GetFromAction()
        {
            var candidate = mapper.Action("Party", "GetPerson");

            Assert.That(candidate, Is.EqualTo("Read"), "Action differs");
        }

        [Test]
        public void PostFromResource()
        {
            var candidate = mapper.Resource("Party", "PostFromPerson");

            Assert.That(candidate, Is.EqualTo("Person"), "Resource differs");
        }

        [Test]
        public void PostFromAction()
        {
            var candidate = mapper.Action("Party", "PostFromPerson");

            Assert.That(candidate, Is.EqualTo("Create"), "Action differs");
        }

        [Test]
        public void PatchFromResource()
        {
            var candidate = mapper.Resource("Party", "PatchPerson");

            Assert.That(candidate, Is.EqualTo("Person"), "Resource differs");
        }

        [Test]
        public void PatchFromAction()
        {
            var candidate = mapper.Action("Party", "PatchPerson");

            Assert.That(candidate, Is.EqualTo("Update"), "Action differs");
        }

        [Test]
        public void PutFromResource()
        {
            var candidate = mapper.Resource("Party", "PutPerson");

            Assert.That(candidate, Is.EqualTo("Person"), "Resource differs");
        }

        [Test]
        public void PutFromAction()
        {
            var candidate = mapper.Action("Party", "PutPerson");

            Assert.That(candidate, Is.EqualTo("Update"), "Action differs");
        }

        protected override void OnSetup()
        {
            mapper = new ControllerActivityMapper();
        }
    }
}
