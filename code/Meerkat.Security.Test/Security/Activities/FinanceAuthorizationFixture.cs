using System.Collections.Generic;
using System.Security.Claims;

using Meerkat.Security.Activities;

using NUnit.Framework;

namespace Meerkat.Test.Security.Activities
{
    [TestFixture]
    public class FinanceAuthorizationFixture 
    {
        [TestCase(Roles.SalesClerk, true)]
        [TestCase(Roles.SalesManager, true)]
        [TestCase(Roles.InvoiceClerk, false)]
        [TestCase(Roles.FinanceManager, true)]
        [TestCase(Roles.FinanceDirector, true)]
        public void OrderRead(string role, bool expected)
        {
            ValidateRole("Order", "Read", role, expected);
        }

        [TestCase(Roles.SalesClerk, true)]
        [TestCase(Roles.SalesManager, false)]
        [TestCase(Roles.InvoiceClerk, false)]
        [TestCase(Roles.FinanceManager, false)]
        [TestCase(Roles.FinanceDirector, false)]
        public void OrderCreate(string role, bool expected)
        {
            ValidateRole("Order", "Create", role, expected);
        }

        [TestCase(Roles.SalesClerk, true)]
        [TestCase(Roles.SalesManager, false)]
        [TestCase(Roles.InvoiceClerk, false)]
        [TestCase(Roles.FinanceManager, false)]
        [TestCase(Roles.FinanceDirector, false)]
        public void OrderEdit(string role, bool expected)
        {
            ValidateRole("Order", "Edit", role, expected);
        }

        [TestCase(Roles.SalesClerk, true)]
        [TestCase(Roles.SalesManager, true)]
        [TestCase(Roles.InvoiceClerk, false)]
        [TestCase(Roles.FinanceManager, false)]
        [TestCase(Roles.FinanceDirector, false)]
        public void OrderShip(string role, bool expected)
        {
            ValidateRole("Order", "Ship", role, expected);
        }

        [TestCase(Roles.SalesClerk, true)]
        [TestCase(Roles.SalesManager, true)]
        [TestCase(Roles.InvoiceClerk, false)]
        [TestCase(Roles.FinanceManager, false)]
        [TestCase(Roles.FinanceDirector, false)]
        public void OrderCancel(string role, bool expected)
        {
            ValidateRole("Order", "Cancel", role, expected);
        }

        [TestCase(Roles.SalesClerk, false)]
        [TestCase(Roles.SalesManager, false)]
        [TestCase(Roles.InvoiceClerk, false)]
        [TestCase(Roles.FinanceManager, false)]
        [TestCase(Roles.FinanceDirector, true)]
        public void OrderDelete(string role, bool expected)
        {
            ValidateRole("Order", "Delete", role, expected);
        }

        [TestCase(Roles.SalesClerk, false)]
        [TestCase(Roles.SalesManager, true)]
        [TestCase(Roles.InvoiceClerk, true)]
        [TestCase(Roles.FinanceManager, true)]
        [TestCase(Roles.FinanceDirector, true)]
        public void InvoiceRead(string role, bool expected)
        {
            ValidateRole("Invoice", "Read", role, expected);
        }

        [TestCase(Roles.SalesClerk, false)]
        [TestCase(Roles.SalesManager, false)]
        [TestCase(Roles.InvoiceClerk, true)]
        [TestCase(Roles.FinanceManager, false)]
        [TestCase(Roles.FinanceDirector, false)]
        public void InvoiceCreate(string role, bool expected)
        {
            ValidateRole("Invoice", "Create", role, expected);
        }

        [TestCase(Roles.SalesClerk, false)]
        [TestCase(Roles.SalesManager, false)]
        [TestCase(Roles.InvoiceClerk, true)]
        [TestCase(Roles.FinanceManager, false)]
        [TestCase(Roles.FinanceDirector, false)]
        public void InvoiceEdit(string role, bool expected)
        {
            ValidateRole("Invoice", "Edit", role, expected);
        }

        [TestCase(Roles.SalesClerk, false)]
        [TestCase(Roles.SalesManager, false)]
        [TestCase(Roles.InvoiceClerk, false)]
        [TestCase(Roles.FinanceManager, true)]
        [TestCase(Roles.FinanceDirector, false)]
        public void InvoiceApprove(string role, bool expected)
        {
            ValidateRole("Invoice", "Approve", role, expected);
        }

        [TestCase(Roles.SalesClerk, false)]
        [TestCase(Roles.SalesManager, false)]
        [TestCase(Roles.InvoiceClerk, false)]
        [TestCase(Roles.FinanceManager, true)]
        [TestCase(Roles.FinanceDirector, false)]
        public void InvoiceCancel(string role, bool expected)
        {
            ValidateRole("Invoice", "Cancel", role, expected);
        }

        [TestCase(Roles.SalesClerk, false)]
        [TestCase(Roles.SalesManager, false)]
        [TestCase(Roles.InvoiceClerk, false)]
        [TestCase(Roles.FinanceManager, false)]
        [TestCase(Roles.FinanceDirector, true)]
        public void InvoiceDelete(string role, bool expected)
        {
            ValidateRole("Invoice", "Delete", role, expected);
        }

        private void ValidateRole(string resource, string action, string role, bool expected)
        {
            var principal = CreatePrincipal(role);
            var authorizer = FinanceAuthorizer();

            var candidate = authorizer.IsAuthorized(resource, action, principal);

            Assert.That(candidate.IsAuthorized, Is.EqualTo(expected), principal.Identity.Name + " " + candidate.Reason);

        }

        private ClaimsPrincipal CreatePrincipal(string role, bool authenticated = true)
        {
            var claims = new List<Claim>
            {
                new Claim("role", role),
                new Claim("name", role)
            };
            var identity = new ClaimsIdentity(claims, "internal", "name", "role");

            return new ClaimsPrincipal(identity);
        }

        private ActivityAuthorizer FinanceAuthorizer()
        {
            var scopeProvider = new ConfigurationAuthorizationScopeProvider("financeAuthorization");
            return new ActivityAuthorizer(scopeProvider, false, null, false);
        }
    }
}
