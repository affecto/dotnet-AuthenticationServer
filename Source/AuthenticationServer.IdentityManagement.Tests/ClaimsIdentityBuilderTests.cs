using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using Affecto.Authentication.Claims;
using Affecto.IdentityManagement.Interfaces;
using Affecto.IdentityManagement.Interfaces.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Affecto.AuthenticationServer.IdentityManagement.Tests
{
    [TestClass]
    public class ClaimsIdentityBuilderTests
    {
        private ClaimsIdentityBuilder sut;
        private IUserService userService;
        private AccountType accountType = AccountType.ActiveDirectory;

        [TestInitialize]
        public void Setup()
        {
            userService = Substitute.For<IUserService>();
            sut = new ClaimsIdentityBuilder(userService);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildWhenTheAuthenticationTypeIsNull()
        {
            sut.Build(null, "user", accountType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildWhenTheAuthenticationTypeIsEmpty()
        {
            sut.Build(string.Empty, "user", accountType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildWhenTheUserAccountNameIsNull()
        {
            sut.Build("type", null, accountType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildWhenTheUserAccountNameIsEmpty()
        {
            sut.Build("type", string.Empty, accountType);
        }

        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void BuildWhenTheIdentityManagementServiceHasNoUser()
        {
            const string userAccountName = "user";
            userService.GetUser(userAccountName, accountType).Returns((IUser)null);

            sut.Build("type", userAccountName, accountType);
        }

        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void BuildWhenTheIdentityManagementServiceHasADisabledUser()
        {
            const string userAccountName = "user";
            IUser user = Substitute.For<IUser>();
            user.IsDisabled.Returns(true);
            userService.GetUser(userAccountName, accountType).Returns(user);

            sut.Build("type", userAccountName, accountType);
        }

        [TestMethod]
        public void BuiltClaimsAreOfCorrectAuthenticationType()
        {
            const string authenticationType = "type";

            ClaimsIdentity identity = sut.Build(authenticationType, "user", accountType);

            Assert.AreEqual(authenticationType, identity.AuthenticationType);
        }

        [TestMethod]
        public void BuiltClaimsContainUserId()
        {
            const string userAccountName = "user";
            var userId = Guid.NewGuid();
            IUser user = Substitute.For<IUser>();
            user.Id.Returns(userId);
            userService.GetUser(userAccountName, accountType).Returns(user);

            ClaimsIdentity identity = sut.Build("type", userAccountName, accountType);

            Assert.AreEqual(userId.ToString(), identity.Claims.Single(c => c.Type.Equals(ClaimType.Id)).Value);
        }

        [TestMethod]
        public void BuiltClaimsContainUserName()
        {
            const string userAccountName = "user";
            const string userName = "name";
            IUser user = Substitute.For<IUser>();
            user.Name.Returns(userName);
            userService.GetUser(userAccountName, accountType).Returns(user);

            ClaimsIdentity identity = sut.Build("type", userAccountName, accountType);

            Assert.AreEqual(userName, identity.Claims.Single(c => c.Type.Equals(ClaimType.Name)).Value);
        }

        [TestMethod]
        public void BuiltClaimsContainUserAccountName()
        {
            const string userAccountName = "user";

            ClaimsIdentity identity = sut.Build("type", userAccountName, accountType);

            Assert.AreEqual(userAccountName, identity.Claims.Single(c => c.Type.Equals(ClaimType.AccountName)).Value);
        }

        [TestMethod]
        public void BuiltClaimsContainUserCustomProperties()
        {
            const string userAccountName = "user";
            const string organizationName = "organization";
            const string organizationValue = "Affecto";
            const string emailName = "email";
            const string emailValue = "foo@bar.com";

            ICustomProperty organizationProperty = CreateCustomPropertyMock(organizationName, organizationValue);
            ICustomProperty emailProperty = CreateCustomPropertyMock(emailName, emailValue);
            IUser user = Substitute.For<IUser>();
            user.CustomProperties.Returns(new List<ICustomProperty> { organizationProperty, organizationProperty, emailProperty });
            userService.GetUser(userAccountName, accountType).Returns(user);

            ClaimsIdentity identity = sut.Build("type", userAccountName, accountType);

            List<Claim> organizationClaims = identity.Claims.Where(c => c.Type == ClaimTypePrefix.CustomProperty + organizationName).ToList();
            Assert.AreEqual(2, organizationClaims.Count);
            Assert.IsTrue(organizationClaims.All(c => c.Value.Equals(organizationValue)));

            List<Claim> emailClaims = identity.Claims.Where(c => c.Type == ClaimTypePrefix.CustomProperty + emailName).ToList();
            Assert.AreEqual(1, emailClaims.Count);
            Assert.IsTrue(emailClaims.All(c => c.Value.Equals(emailValue)));
        }

        [TestMethod]
        public void BuiltClaimsContainUserOrganizations()
        {
            const string userAccountName = "user";
            const string firstOrganizationName = "org";
            const string secondOrganizationName = "another org";

            IOrganization firstOrganization = CreateOrganizationMock(firstOrganizationName);
            IOrganization secondOrganization = CreateOrganizationMock(secondOrganizationName);
            IUser user = Substitute.For<IUser>();
            user.Organizations.Returns(new List<IOrganization> { firstOrganization, firstOrganization, secondOrganization });
            userService.GetUser(userAccountName, accountType).Returns(user);

            ClaimsIdentity identity = sut.Build("type", userAccountName, accountType);

            List<Claim> organizationClaims = identity.Claims.Where(c => c.Type.Equals(ClaimType.Organization)).ToList();
            Assert.AreEqual(2, organizationClaims.Count);
            Assert.IsTrue(organizationClaims.Any(c => c.Value.Equals(firstOrganizationName)));
            Assert.IsTrue(organizationClaims.Any(c => c.Value.Equals(secondOrganizationName)));
        }

        [TestMethod]
        public void BuiltClaimsContainUserGroupGuids()
        {
            const string userAccountName = "user";
            const string firstGroupName = "group";
            const string secondGroupName = "another group";

            IGroup firstGroup = CreateGroupMock(firstGroupName);
            IGroup secondGroup = CreateGroupMock(secondGroupName);
            IUser user = Substitute.For<IUser>();
            user.Groups.Returns(new List<IGroup> { firstGroup, secondGroup, secondGroup });
            userService.GetUser(userAccountName, accountType).Returns(user);

            ClaimsIdentity identity = sut.Build("type", userAccountName, accountType);

            List<Claim> groupClaims = identity.Claims.Where(c => c.Type.Equals(ClaimType.Group)).ToList();
            Assert.AreEqual(2, groupClaims.Count);
            Assert.IsTrue(groupClaims.Any(c => c.Value.Equals(firstGroup.Id.ToString())));
            Assert.IsTrue(groupClaims.Any(c => c.Value.Equals(secondGroup.Id.ToString())));
        }

        [TestMethod]
        public void BuiltClaimsContainUserRoles()
        {
            const string userAccountName = "user";
            const string firstRoleName = "role";
            const string secondRoleName = "another role";

            IRole firstRole = CreateRoleMock(firstRoleName);
            IRole secondRole = CreateRoleMock(secondRoleName);
            IUser user = Substitute.For<IUser>();
            user.Roles.Returns(new List<IRole> { firstRole, secondRole, secondRole });
            userService.GetUser(userAccountName, accountType).Returns(user);

            ClaimsIdentity identity = sut.Build("type", userAccountName, accountType);

            List<Claim> roleClaims = identity.Claims.Where(c => c.Type.Equals(ClaimType.Role)).ToList();
            Assert.AreEqual(2, roleClaims.Count);
            Assert.IsTrue(roleClaims.Any(c => c.Value.Equals(firstRoleName)));
            Assert.IsTrue(roleClaims.Any(c => c.Value.Equals(secondRoleName)));
        }

        [TestMethod]
        public void BuiltClaimsContainUserRolePermissions()
        {
            const string userAccountName = "user";
            const string firstRoleName = "role";
            const string secondRoleName = "another role";
            const string firstPermissionName = "read";
            const string secondPermissionName = "write";
            const string thirdPermissionName = "all";

            IRole firstRole = CreateRoleMock(firstRoleName);
            IRole secondRole = CreateRoleMock(secondRoleName);
            IPermission firstPermission = CreatePermissionMock(firstPermissionName);
            IPermission secondPermission = CreatePermissionMock(secondPermissionName);
            IPermission thirdPermission = CreatePermissionMock(thirdPermissionName);
            firstRole.Permissions.Returns(new List<IPermission> { firstPermission, secondPermission });
            secondRole.Permissions.Returns(new List<IPermission> { thirdPermission, thirdPermission });

            IUser user = Substitute.For<IUser>();
            user.Roles.Returns(new List<IRole> { firstRole, secondRole, secondRole });
            userService.GetUser(userAccountName, accountType).Returns(user);

            ClaimsIdentity identity = sut.Build("type", userAccountName, accountType);

            List<Claim> permissionClaims = identity.Claims.Where(c => c.Type.Equals(ClaimType.Permission)).ToList();
            Assert.AreEqual(3, permissionClaims.Count);
            Assert.IsTrue(permissionClaims.Any(c => c.Value.Equals(firstPermissionName)));
            Assert.IsTrue(permissionClaims.Any(c => c.Value.Equals(secondPermissionName)));
            Assert.IsTrue(permissionClaims.Any(c => c.Value.Equals(thirdPermissionName)));
        }

        private static IPermission CreatePermissionMock(string name)
        {
            IPermission permission = Substitute.For<IPermission>();
            permission.Name.Returns(name);
            return permission;
        }

        private static ICustomProperty CreateCustomPropertyMock(string name, string value)
        {
            ICustomProperty customProperty = Substitute.For<ICustomProperty>();
            customProperty.Name.Returns(name);
            customProperty.Value.Returns(value);
            return customProperty;
        }

        private static IOrganization CreateOrganizationMock(string name)
        {
            IOrganization organization = Substitute.For<IOrganization>();
            organization.Name.Returns(name);
            return organization;
        }

        private static IGroup CreateGroupMock(string name)
        {
            IGroup group = Substitute.For<IGroup>();
            group.Name.Returns(name);
            group.Id.Returns(Guid.NewGuid());
            return group;
        }

        private static IRole CreateRoleMock(string name)
        {
            IRole role = Substitute.For<IRole>();
            role.Name.Returns(name);
            return role;
        }
    }
}
