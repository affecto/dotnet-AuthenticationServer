// ReSharper disable ObjectCreationAsStatement

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Affecto.AuthenticationServer.Claims.Tests
{
    [TestClass]
    public class AuthenticatedUserTests
    {
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

        private AuthenticatedUser sut;

        [TestInitialize]
        public void Setup()
        {
            sut = new AuthenticatedUser(ExpectedId, ExpectedName, ExpectedAccountName, ExpectedPermissions, ExpectedRoles, ExpectedCustomProperties, ExpectedGroups,
                ExpectedOrganizations);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IdCannotBeEmpty()
        {
            new AuthenticatedUser(Guid.Empty, ExpectedName, ExpectedAccountName, ExpectedPermissions, ExpectedRoles, ExpectedCustomProperties, ExpectedGroups,
                ExpectedOrganizations);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NameCannotBeNull()
        {
            new AuthenticatedUser(ExpectedId, null, ExpectedAccountName, ExpectedPermissions, ExpectedRoles, ExpectedCustomProperties, ExpectedGroups,
                ExpectedOrganizations);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NameCannotBeEmpty()
        {
            new AuthenticatedUser(ExpectedId, "", ExpectedAccountName, ExpectedPermissions, ExpectedRoles, ExpectedCustomProperties, ExpectedGroups,
                ExpectedOrganizations);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AccountNameCannotBeNull()
        {
            new AuthenticatedUser(ExpectedId, ExpectedName, null, ExpectedPermissions, ExpectedRoles, ExpectedCustomProperties, ExpectedGroups,
                ExpectedOrganizations);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AccountNameCannotBeEmpty()
        {
            new AuthenticatedUser(ExpectedId, ExpectedName, "", ExpectedPermissions, ExpectedRoles, ExpectedCustomProperties, ExpectedGroups,
                ExpectedOrganizations);
        }

        [TestMethod]
        public void PermissionsCanBeEmpty()
        {
            sut = new AuthenticatedUser(ExpectedId, ExpectedName, ExpectedAccountName, null, ExpectedRoles, ExpectedCustomProperties, ExpectedGroups,
                ExpectedOrganizations);

            Assert.IsNotNull(sut.Permissions);
            Assert.AreEqual(0, sut.Permissions.Count);
        }

        [TestMethod]
        public void RolesCanBeEmpty()
        {
            sut = new AuthenticatedUser(ExpectedId, ExpectedName, ExpectedAccountName, ExpectedPermissions, null, ExpectedCustomProperties, ExpectedGroups,
                ExpectedOrganizations);

            Assert.IsNotNull(sut.Roles);
            Assert.AreEqual(0, sut.Roles.Count);
        }

        [TestMethod]
        public void CustomPropertiesCanBeEmpty()
        {
            sut = new AuthenticatedUser(ExpectedId, ExpectedName, ExpectedAccountName, ExpectedPermissions, ExpectedRoles, null, ExpectedGroups,
                ExpectedOrganizations);

            Assert.IsNotNull(sut.CustomProperties);
            Assert.AreEqual(0, sut.CustomProperties.Count);
        }

        [TestMethod]
        public void GroupsCanBeEmpty()
        {
            sut = new AuthenticatedUser(ExpectedId, ExpectedName, ExpectedAccountName, ExpectedPermissions, ExpectedRoles, ExpectedCustomProperties, null,
                ExpectedOrganizations);

            Assert.IsNotNull(sut.Groups);
            Assert.AreEqual(0, sut.Groups.Count);
        }

        [TestMethod]
        public void OrganizationsCanBeEmpty()
        {
            sut = new AuthenticatedUser(ExpectedId, ExpectedName, ExpectedAccountName, ExpectedPermissions, ExpectedRoles, ExpectedCustomProperties, ExpectedGroups);

            Assert.IsNotNull(sut.Organizations);
            Assert.AreEqual(0, sut.Organizations.Count);
        }

        [TestMethod]
        public void IdIsSet()
        {
            Assert.AreEqual(ExpectedId, sut.Id);
        }

        [TestMethod]
        public void NameIsSet()
        {
            Assert.AreEqual(ExpectedName, sut.Name);
        }

        [TestMethod]
        public void AccountNameIsSet()
        {
            Assert.AreEqual(ExpectedAccountName, sut.AccountName);
        }

        [TestMethod]
        public void PermissionsAreSet()
        {
            Assert.AreNotSame(ExpectedPermissions, sut.Permissions);
            Assert.AreEqual(2, sut.Permissions.Count);
            Assert.AreEqual(ExpectedPermissions[0], sut.Permissions.ElementAt(0));
            Assert.AreEqual(ExpectedPermissions[1], sut.Permissions.ElementAt(1));
        }

        [TestMethod]
        public void RolesAreSet()
        {
            Assert.AreNotSame(ExpectedRoles, sut.Roles);
            Assert.AreEqual(2, sut.Roles.Count);
            Assert.AreEqual(ExpectedRoles[0], sut.Roles.ElementAt(0));
            Assert.AreEqual(ExpectedRoles[1], sut.Roles.ElementAt(1));
        }

        [TestMethod]
        public void CustomPropertiesAreSet()
        {
            Assert.AreNotSame(ExpectedCustomProperties, sut.CustomProperties);
            Assert.AreEqual(2, sut.CustomProperties.Count);
            Assert.AreEqual(ExpectedCustomProperties[0].Name, sut.CustomProperties.ElementAt(0).Name);
            Assert.AreEqual(ExpectedCustomProperties[0].Value, sut.CustomProperties.ElementAt(0).Value);
            Assert.AreEqual(ExpectedCustomProperties[1].Name, sut.CustomProperties.ElementAt(1).Name);
            Assert.AreEqual(ExpectedCustomProperties[1].Value, sut.CustomProperties.ElementAt(1).Value);
        }

        [TestMethod]
        public void GroupsAreSet()
        {
            Assert.AreNotSame(ExpectedGroups, sut.Groups);
            Assert.AreEqual(2, sut.Groups.Count);
            Assert.AreEqual(ExpectedGroups[0], sut.Groups.ElementAt(0));
            Assert.AreEqual(ExpectedGroups[1], sut.Groups.ElementAt(1));
        }

        [TestMethod]
        public void OrganizationsAreSet()
        {
            Assert.AreNotSame(ExpectedOrganizations, sut.Organizations);
            Assert.AreEqual(2, sut.Organizations.Count);
            Assert.AreEqual(ExpectedOrganizations[0], sut.Organizations.ElementAt(0));
            Assert.AreEqual(ExpectedOrganizations[1], sut.Organizations.ElementAt(1));
        }
    }
}