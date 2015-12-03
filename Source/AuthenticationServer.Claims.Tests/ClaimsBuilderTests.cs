using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Affecto.Authentication.Claims;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Affecto.AuthenticationServer.Claims.Tests
{
    [TestClass]
    public class ClaimsBuilderTests
    {
        private ClaimsBuilder sut;

        [TestInitialize]
        public void Setup()
        {
            sut = new ClaimsBuilder();
        }

        [TestMethod]
        public void NoClaimsAreReturnedAfterInitiliaze()
        {
            IReadOnlyCollection<Claim> claims = sut.GetClaims();

            Assert.IsNotNull(claims);
            Assert.AreEqual(0, claims.Count);
        }

        [TestMethod]
        public void IdIsSet()
        {
            Guid expectedValue = Guid.NewGuid();

            sut.SetId(expectedValue);
            IReadOnlyCollection<Claim> claims = sut.GetClaims();

            Assert.AreEqual(1, claims.Count);
            Claim claim = claims.Single();
            Assert.AreEqual(ClaimType.Id, claim.Type);
            Assert.AreEqual(expectedValue.ToString("D"), claim.Value);
        }

        [TestMethod]
        public void IdIsOverwritten()
        {
            Guid expectedValue = Guid.NewGuid();

            sut.SetId(Guid.NewGuid());
            sut.SetId(expectedValue);
            IReadOnlyCollection<Claim> claims = sut.GetClaims();

            Assert.AreEqual(1, claims.Count);
            Claim claim = claims.Single();
            Assert.AreEqual(ClaimType.Id, claim.Type);
            Assert.AreEqual(expectedValue.ToString("D"), claim.Value);
        }

        [TestMethod]
        public void NameIsSet()
        {
            const string expectedValue = "JohnDoe";

            sut.SetName(expectedValue);
            IReadOnlyCollection<Claim> claims = sut.GetClaims();

            Assert.AreEqual(1, claims.Count);
            Claim claim = claims.Single();
            Assert.AreEqual(ClaimType.Name, claim.Type);
            Assert.AreEqual(expectedValue, claim.Value);
        }

        [TestMethod]
        public void NameIsOverwritten()
        {
            const string expectedValue = "JohnDoe";

            sut.SetName("Foo");
            sut.SetName(expectedValue);
            IReadOnlyCollection<Claim> claims = sut.GetClaims();

            Assert.AreEqual(1, claims.Count);
            Claim claim = claims.Single();
            Assert.AreEqual(ClaimType.Name, claim.Type);
            Assert.AreEqual(expectedValue, claim.Value);
        }

        [TestMethod]
        public void AccountNameIsSet()
        {
            const string expectedValue = "domain\\JohnDoe";

            sut.SetAccountName(expectedValue);
            IReadOnlyCollection<Claim> claims = sut.GetClaims();

            Assert.AreEqual(1, claims.Count);
            Claim claim = claims.Single();
            Assert.AreEqual(ClaimType.AccountName, claim.Type);
            Assert.AreEqual(expectedValue, claim.Value);
        }

        [TestMethod]
        public void AccountNameIsOverwritten()
        {
            const string expectedValue = "domain\\JohnDoe";

            sut.SetAccountName("SomeAccount");
            sut.SetAccountName(expectedValue);
            IReadOnlyCollection<Claim> claims = sut.GetClaims();

            Assert.AreEqual(1, claims.Count);
            Claim claim = claims.Single();
            Assert.AreEqual(ClaimType.AccountName, claim.Type);
            Assert.AreEqual(expectedValue, claim.Value);
        }

        [TestMethod]
        public void CustomPropertiesAreSet()
        {
            const string expectedName1 = "email";
            const string expectedValue1 = "e@mail.com";
            const string expectedName2 = "LastName";
            const string expectedValue2 = "Doe";

            sut.AddCustomProperty(expectedName1, expectedValue1);
            sut.AddCustomProperty(expectedName2, expectedValue2);
            IReadOnlyCollection<Claim> claims = sut.GetClaims();

            Assert.AreEqual(2, claims.Count);
            Assert.IsTrue(claims.Any(c => c.Type == ClaimTypePrefix.CustomProperty + expectedName1 && c.Value == expectedValue1));
            Assert.IsTrue(claims.Any(c => c.Type == ClaimTypePrefix.CustomProperty + expectedName2 && c.Value == expectedValue2));
        }

        [TestMethod]
        public void RolesAreSet()
        {
            const string expectedValue1 = "admin";
            const string expectedValue2 = "basic";

            sut.AddRole(expectedValue1);
            sut.AddRole(expectedValue2);
            IReadOnlyCollection<Claim> claims = sut.GetClaims();

            Assert.AreEqual(2, claims.Count);
            Assert.IsTrue(claims.Any(c => c.Type == ClaimType.Role && c.Value == expectedValue1));
            Assert.IsTrue(claims.Any(c => c.Type == ClaimType.Role && c.Value == expectedValue2));
        }

        [TestMethod]
        public void PermissionsAreSet()
        {
            const string expectedValue1 = "READ";
            const string expectedValue2 = "EDIT";

            sut.AddPermission(expectedValue1);
            sut.AddPermission(expectedValue2);
            IReadOnlyCollection<Claim> claims = sut.GetClaims();

            Assert.AreEqual(2, claims.Count);
            Assert.IsTrue(claims.Any(c => c.Type == ClaimType.Permission && c.Value == expectedValue1));
            Assert.IsTrue(claims.Any(c => c.Type == ClaimType.Permission && c.Value == expectedValue2));
        }

        [TestMethod]
        public void OrganizationsAreSet()
        {
            const string expectedValue1 = "HR";
            const string expectedValue2 = "IT";

            sut.AddOrganization(expectedValue1);
            sut.AddOrganization(expectedValue2);
            IReadOnlyCollection<Claim> claims = sut.GetClaims();

            Assert.AreEqual(2, claims.Count);
            Assert.IsTrue(claims.Any(c => c.Type == ClaimType.Organization && c.Value == expectedValue1));
            Assert.IsTrue(claims.Any(c => c.Type == ClaimType.Organization && c.Value == expectedValue2));
        }

        [TestMethod]
        public void GroupsAreSet()
        {
            Guid expectedValue1 = Guid.NewGuid();
            Guid expectedValue2 = Guid.NewGuid();

            sut.AddGroup(expectedValue1);
            sut.AddGroup(expectedValue2);
            IReadOnlyCollection<Claim> claims = sut.GetClaims();

            Assert.AreEqual(2, claims.Count);
            Assert.IsTrue(claims.Any(c => c.Type == ClaimType.Group && c.Value == expectedValue1.ToString("D")));
            Assert.IsTrue(claims.Any(c => c.Type == ClaimType.Group && c.Value == expectedValue2.ToString("D")));
        }
    }
}