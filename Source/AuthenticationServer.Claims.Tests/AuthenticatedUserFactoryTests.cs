// ReSharper disable ObjectCreationAsStatement

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Affecto.Authentication.Claims;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Affecto.AuthenticationServer.Claims.Tests
{
    [TestClass]
    public class AuthenticatedUserFactoryTests
    {
        private AuthenticatedUserFactory sut;

        private static readonly Guid ExpectedId = Guid.NewGuid();
        private static readonly string ExpectedName = "JohnDoe";
        private static readonly string ExpectedAccountName = "domain\\JohnDoe";

        private static readonly List<string> ExpectedPermissions = new List<string> { "READ", "WRITE" };
        private static readonly List<string> ExpectedRoles = new List<string> { "ADMIN", "BASIC" };
        private static readonly List<string> ExpectedOrganizations = new List<string> { "HR", "IT" };
        private static readonly List<Guid> ExpectedGroups = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        private static readonly List<CustomProperty> ExpectedCustomProperties = new List<CustomProperty>
        {
            new CustomProperty("Email", "john@doe.com"),
            new CustomProperty("Surname", "Doe")
        };

        private static readonly List<Claim> Claims = new List<Claim>
        {
            new Claim(ClaimType.Id, ExpectedId.ToString("D")),
            new Claim(ClaimType.Name, ExpectedName),
            new Claim(ClaimType.AccountName, ExpectedAccountName),
            new Claim(ClaimType.Permission, ExpectedPermissions[0]),
            new Claim(ClaimType.Permission, ExpectedPermissions[1]),
            new Claim(ClaimType.Role, ExpectedRoles[0]),
            new Claim(ClaimType.Role, ExpectedRoles[1]),
            new Claim(ClaimTypePrefix.CustomProperty + ExpectedCustomProperties[0].Name, ExpectedCustomProperties[0].Value),
            new Claim(ClaimTypePrefix.CustomProperty + ExpectedCustomProperties[1].Name, ExpectedCustomProperties[1].Value),
            new Claim(ClaimType.Group, ExpectedGroups[0].ToString("D")),
            new Claim(ClaimType.Group, ExpectedGroups[1].ToString("D")),
            new Claim(ClaimType.Organization, ExpectedOrganizations[0]),
            new Claim(ClaimType.Organization, ExpectedOrganizations[1])
        };

        [TestInitialize]
        public void Setup()
        {
            sut = new AuthenticatedUserFactory(Claims);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ClaimsCannotBeNull()
        {
            new AuthenticatedUserFactory(null);
        }

        [TestMethod]
        [ExpectedException(typeof(MultipleClaimsFoundException))]
        public void IdCannotBeSetTwice()
        {
            var claims = new List<Claim>(Claims);
            claims.Add(new Claim(ClaimType.Id, Guid.NewGuid().ToString("D")));

            sut = new AuthenticatedUserFactory(claims);
            sut.Create();
        }

        [TestMethod]
        [ExpectedException(typeof(MultipleClaimsFoundException))]
        public void NameCannotBeSetTwice()
        {
            var claims = new List<Claim>(Claims);
            claims.Add(new Claim(ClaimType.Name, "JaneDoe"));

            sut = new AuthenticatedUserFactory(claims);
            sut.Create();
        }

        [TestMethod]
        [ExpectedException(typeof(MultipleClaimsFoundException))]
        public void AccountNameCannotBeSetTwice()
        {
            var claims = new List<Claim>(Claims);
            claims.Add(new Claim(ClaimType.AccountName, "domain\\JaneDoe"));

            sut = new AuthenticatedUserFactory(claims);
            sut.Create();
        }

        [TestMethod]
        public void IdIsSet()
        {
            AuthenticatedUser user = sut.Create();

            Assert.AreEqual(ExpectedId, user.Id);
        }

        [TestMethod]
        public void NameIsSet()
        {
            AuthenticatedUser user = sut.Create();

            Assert.AreEqual(ExpectedName, user.Name);
        }

        [TestMethod]
        public void AccountNameIsSet()
        {
            AuthenticatedUser user = sut.Create();

            Assert.AreEqual(ExpectedAccountName, user.AccountName);
        }

        [TestMethod]
        public void RolesAreSet()
        {
            AuthenticatedUser user = sut.Create();

            Assert.AreEqual(ExpectedRoles[0], user.Roles.ElementAt(0));
            Assert.AreEqual(ExpectedRoles[1], user.Roles.ElementAt(1));
        }

        [TestMethod]
        public void CustomPropertiesAreSet()
        {
            AuthenticatedUser user = sut.Create();

            Assert.AreEqual(ExpectedCustomProperties[0].Name, user.CustomProperties.ElementAt(0).Name);
            Assert.AreEqual(ExpectedCustomProperties[0].Value, user.CustomProperties.ElementAt(0).Value);
            Assert.AreEqual(ExpectedCustomProperties[1].Name, user.CustomProperties.ElementAt(1).Name);
            Assert.AreEqual(ExpectedCustomProperties[1].Value, user.CustomProperties.ElementAt(1).Value);
        }

        [TestMethod]
        public void GroupsAreSet()
        {
            AuthenticatedUser user = sut.Create();

            Assert.AreEqual(ExpectedGroups[0], user.Groups.ElementAt(0));
            Assert.AreEqual(ExpectedGroups[1], user.Groups.ElementAt(1));
        }

        [TestMethod]
        public void OrganizationsAreSet()
        {
            AuthenticatedUser user = sut.Create();

            Assert.AreEqual(ExpectedOrganizations[0], user.Organizations.ElementAt(0));
            Assert.AreEqual(ExpectedOrganizations[1], user.Organizations.ElementAt(1));
        }
    }
}