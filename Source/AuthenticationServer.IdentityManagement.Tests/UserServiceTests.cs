// ReSharper disable PossibleMultipleEnumeration

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Affecto.Authentication.Claims;
using Affecto.AuthenticationServer.IdentityManagement.Configuration;
using Affecto.AuthenticationServer.Infrastructure.Configuration;
using Affecto.IdentityManagement.Interfaces;
using Affecto.IdentityManagement.Interfaces.Model;
using IdentityServer3.Core.Extensions;
using IdentityServer3.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using ICustomProperty = Affecto.IdentityManagement.Interfaces.Model.ICustomProperty;

namespace Affecto.AuthenticationServer.IdentityManagement.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        private const string AccountName = "TestUser";
        private const string Password = "Secrets";

        private IUser expectedUser;
        private IAccount expectedAccount;
        private ICustomProperty expectedCustomProperty;
        private IPermission expectedPermission;
        private IRole expectedRole;
        private IGroup expectedGroup;
        private IOrganization expectedOrganization;

        private IUserService identityManagementUserService;
        private IFederatedAuthenticationConfiguration federatedAuthenticationConfiguration;
        private IIdentityManagementConfiguration identityManagementConfiguration;
        private UserService sut;

        [TestInitialize]
        public void Setup()
        {
            expectedAccount = Substitute.For<IAccount>();
            expectedAccount.Name.Returns(AccountName);
            expectedAccount.Type.Returns(AccountType.Password);

            expectedCustomProperty = Substitute.For<ICustomProperty>();
            expectedCustomProperty.Name.Returns("CustomClaim");
            expectedCustomProperty.Value.Returns("ThisIsValue");

            expectedPermission = Substitute.For<IPermission>();
            expectedPermission.Id.Returns(Guid.NewGuid());
            expectedPermission.Name.Returns("AddUsers");

            expectedRole = Substitute.For<IRole>();
            expectedRole.Permissions.Returns(new List<IPermission> { expectedPermission });
            expectedRole.Id.Returns(Guid.NewGuid());
            expectedRole.Name.Returns("Admin");

            expectedGroup = Substitute.For<IGroup>();
            expectedGroup.Id.Returns(Guid.NewGuid());
            expectedGroup.Name.Returns("Administrators");

            expectedOrganization = Substitute.For<IOrganization>();
            expectedOrganization.Id.Returns(Guid.NewGuid());
            expectedOrganization.Name.Returns("Company");

            expectedUser = Substitute.For<IUser>();
            expectedUser.Id.Returns(Guid.NewGuid());
            expectedUser.Name.Returns("Test User");
            expectedUser.Accounts.Returns(new List<IAccount> { expectedAccount });
            expectedUser.CustomProperties.Returns(new List<ICustomProperty> { expectedCustomProperty });
            expectedUser.Roles.Returns(new List<IRole> { expectedRole });
            expectedUser.Groups.Returns(new List<IGroup> { expectedGroup });
            expectedUser.Organizations.Returns(new List<IOrganization> { expectedOrganization });

            identityManagementUserService = Substitute.For<IUserService>();
            identityManagementConfiguration = Substitute.For<IIdentityManagementConfiguration>();
            federatedAuthenticationConfiguration = Substitute.For<IFederatedAuthenticationConfiguration>();
            identityManagementUserService.GetUser(AccountName, AccountType.Password).Returns(expectedUser);
            identityManagementUserService.IsMatchingPassword(AccountName, Password).Returns(true);

            sut = new UserService(identityManagementUserService, new Lazy<IIdentityManagementConfiguration>(() => identityManagementConfiguration),
                new Lazy<IFederatedAuthenticationConfiguration>(() => federatedAuthenticationConfiguration));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IdentityManagementServiceCannotBeNull()
        {
            sut = new UserService(null, new Lazy<IIdentityManagementConfiguration>(() => identityManagementConfiguration),
                new Lazy<IFederatedAuthenticationConfiguration>(() => federatedAuthenticationConfiguration));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IdentityManagementConfigurationCannotBeNull()
        {
            sut = new UserService(identityManagementUserService, null, new Lazy<IFederatedAuthenticationConfiguration>(() => federatedAuthenticationConfiguration));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FederatedAuthenticationConfigurationCannotBeNull()
        {
            sut = new UserService(identityManagementUserService, new Lazy<IIdentityManagementConfiguration>(() => identityManagementConfiguration), null);
        }

        [TestMethod]
        public void EmptyResultIsReturnedWhenPasswordDoesNotMatch()
        {
            var context = new LocalAuthenticationContext { UserName = AccountName, Password = Password };
            identityManagementUserService.IsMatchingPassword(AccountName, Password).Returns(false);

            Task task = sut.AuthenticateLocalAsync(context);

            AssertEmptyResult(task, context.AuthenticateResult);
        }

        [TestMethod]
        public void UserIsAuthenticatedWhenPasswordMatches()
        {
            var context = new LocalAuthenticationContext { UserName = AccountName, Password = Password };
            Task task = sut.AuthenticateLocalAsync(context);

            AssertAuthenticatedUser(task, context.AuthenticateResult);
        }

        [TestMethod]
        public void AuthenticatedUserClaimsAreGeneratedWhenPasswordMatches()
        {
            var context = new LocalAuthenticationContext { UserName = AccountName, Password = Password };
            Task task = sut.AuthenticateLocalAsync(context);

            AssertAuthenticatedUserClaims(task, context.AuthenticateResult);
        }

        [TestMethod]
        public void ErrorMessageIsReturnedWhenFederatedAuthenticationHasNoUserAccountNameClaim()
        {
            const string userAccountNameClaim = "account";
            const string userDisplayNameClaim = "display";
            federatedAuthenticationConfiguration.UserDisplayNameClaim.Returns(userDisplayNameClaim);
            federatedAuthenticationConfiguration.UserAccountNameClaim.Returns(userAccountNameClaim);
            var context = new ExternalAuthenticationContext
            {
                ExternalIdentity = new ExternalIdentity { Claims = new List<Claim> { new Claim(userDisplayNameClaim, "value") } }
            };

            Task task = sut.AuthenticateExternalAsync(context);

            AssertErrorMessage(task, context.AuthenticateResult);
        }

        [TestMethod]
        public void ErrorMessageIsReturnedWhenFederatedAuthenticationHasNoUserDisplayNameClaim()
        {
            const string userAccountNameClaim = "account";
            const string userDisplayNameClaim = "display";
            federatedAuthenticationConfiguration.UserDisplayNameClaim.Returns(userDisplayNameClaim);
            federatedAuthenticationConfiguration.UserAccountNameClaim.Returns(userAccountNameClaim);
            var context = new ExternalAuthenticationContext
            {
                ExternalIdentity = new ExternalIdentity { Claims = new List<Claim> { new Claim(userAccountNameClaim, "value") } }
            };

            Task task = sut.AuthenticateExternalAsync(context);

            AssertErrorMessage(task, context.AuthenticateResult);
        }

        [TestMethod]
        public void UserIsAuthenticatedWhenFederatedAuthenticationHasBothUserDisplayAndAccountNameClaims()
        {
            ExternalAuthenticationContext context = CreateSuccessfulAuthenticationContext();

            Task task = sut.AuthenticateExternalAsync(context);

            AssertAuthenticatedUser(task, context.AuthenticateResult);
        }

        [TestMethod]
        public void AuthenticatedUserClaimsAreGeneratedWhenFederatedAuthenticationHasBothUserDisplayAndAccountNameClaims()
        {
            ExternalAuthenticationContext context = CreateSuccessfulAuthenticationContext();

            Task task = sut.AuthenticateExternalAsync(context);

            AssertAuthenticatedUserClaims(task, context.AuthenticateResult);
        }

        [TestMethod]
        public void NewUserIsAddedWhenFederadedAuthenticationSucceeds()
        {
            const string userDisplayName = "Ted Tester";
            ExternalAuthenticationContext context = CreateSuccessfulAuthenticationContext(userDisplayName);
            identityManagementUserService.IsExistingUserAccount(expectedAccount.Name, AccountType.Federated).Returns(false);
            identityManagementConfiguration.AutoCreateUser.Returns(true);

            sut.AuthenticateExternalAsync(context);

            identityManagementUserService.Received(1).AddUser(expectedAccount.Name, AccountType.Federated, userDisplayName, Arg.Any<IEnumerable<string>>());
        }

        [TestMethod]
        public void NewUserIsNotAddedWhenFederadedAuthenticationSucceedsIfAddingIsConfiguredOff()
        {
            ExternalAuthenticationContext context = CreateSuccessfulAuthenticationContext();
            identityManagementUserService.IsExistingUserAccount(expectedAccount.Name, AccountType.Federated).Returns(false);
            identityManagementConfiguration.AutoCreateUser.Returns(false);

            sut.AuthenticateExternalAsync(context);

            identityManagementUserService.DidNotReceive().AddUser(Arg.Any<string>(), Arg.Any<AccountType>(), Arg.Any<string>(), Arg.Any<IEnumerable<string>>());
        }

        [TestMethod]
        public void ExistingUserIsUpdatedWhenFederadedAuthenticationSucceeds()
        {
            ExternalAuthenticationContext context = CreateSuccessfulAuthenticationContext();
            identityManagementUserService.IsExistingUserAccount(expectedAccount.Name, AccountType.Federated).Returns(true);

            sut.AuthenticateExternalAsync(context);

            identityManagementUserService.Received(1).UpdateUserGroupsAndRoles(expectedAccount.Name, AccountType.Federated, Arg.Any<IEnumerable<string>>());
        }

        private static void AssertEmptyResult(Task task, AuthenticateResult result)
        {
            Assert.IsNotNull(task);
            Assert.IsNull(result);
        }

        private void AssertErrorMessage(Task task, AuthenticateResult result)
        {
            Assert.IsNotNull(task);
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorMessage));
        }

        private static void AssertAuthenticatedUser(Task task, AuthenticateResult result)
        {
            Assert.IsNotNull(task);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsError);
            Assert.IsFalse(result.IsPartialSignIn);
            Assert.IsNotNull(result.User);
            Assert.IsNotNull(result.User.Identity);
            Assert.IsTrue(result.User.Identity.IsAuthenticated);
        }

        private void AssertAuthenticatedUserClaims(Task task, AuthenticateResult result)
        {
            Assert.IsNotNull(task);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.User);

            IIdentity identity = result.User.Identity;
            IEnumerable<Claim> claims = result.User.Claims;
            Assert.IsNotNull(identity);
            Assert.IsNotNull(claims);

            Assert.AreEqual(expectedUser.Name, identity.Name);
            Assert.AreEqual(expectedUser.Id.ToString("D"), identity.GetSubjectId());
            Assert.AreEqual(1, claims.Count(c => c.Type == ClaimType.Id && c.Value == expectedUser.Id.ToString("D")));
            Assert.AreEqual(1, claims.Count(c => c.Type == ClaimType.Name && c.Value == expectedUser.Name));
            Assert.AreEqual(1, claims.Count(c => c.Type == ClaimType.AccountName && c.Value == expectedAccount.Name));
            Assert.AreEqual(1, claims.Count(c => c.Type == ClaimTypePrefix.CustomProperty + expectedCustomProperty.Name && c.Value == expectedCustomProperty.Value));
            Assert.AreEqual(1, claims.Count(c => c.Type == ClaimType.Organization && c.Value == expectedOrganization.Name));
            Assert.AreEqual(1, claims.Count(c => c.Type == ClaimType.Group && c.Value == expectedGroup.Id.ToString("D")));
            Assert.AreEqual(1, claims.Count(c => c.Type == ClaimType.Role && c.Value == expectedRole.Name));
            Assert.AreEqual(1, claims.Count(c => c.Type == ClaimType.Permission && c.Value == expectedPermission.Name));
        }

        private ExternalAuthenticationContext CreateSuccessfulAuthenticationContext(string userDisplayName = "Kenny Koder")
        {
            const string userAccountNameClaim = "account";
            const string userDisplayNameClaim = "display";

            identityManagementUserService.GetUser(expectedAccount.Name, AccountType.Federated).Returns(expectedUser);

            federatedAuthenticationConfiguration.UserDisplayNameClaim.Returns(userDisplayNameClaim);
            federatedAuthenticationConfiguration.UserAccountNameClaim.Returns(userAccountNameClaim);
            return new ExternalAuthenticationContext
            {
                ExternalIdentity = new ExternalIdentity
                {
                    Claims = new List<Claim> { new Claim(userAccountNameClaim, expectedAccount.Name), new Claim(userDisplayNameClaim, userDisplayName) }
                },
                SignInMessage = new SignInMessage
                {
                    IdP = "idsrv"
                }
            };
        }
    }
}