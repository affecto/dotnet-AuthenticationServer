// ReSharper disable PossibleMultipleEnumeration

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Affecto.Authentication.Claims;
using Affecto.IdentityManagement.Interfaces;
using Affecto.IdentityManagement.Interfaces.Model;
using IdentityServer3.Core.Extensions;
using IdentityServer3.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

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
            identityManagementUserService.GetUser(AccountName, AccountType.Password).Returns(expectedUser);
            identityManagementUserService.IsMatchingPassword(AccountName, Password).Returns(true);

            sut = new UserService(new Lazy<IUserService>(() => identityManagementUserService));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IdentityManagementServiceCannotBeNull()
        {
            sut = new UserService(null);
        }

        [TestMethod]
        public void EmptyResultIsReturnedWhenPasswordDoesNotMatch()
        {
            var context = new LocalAuthenticationContext { UserName = AccountName, Password = Password };
            identityManagementUserService.IsMatchingPassword(AccountName, Password).Returns(false);

            Task task = sut.AuthenticateLocalAsync(context);

            Assert.IsNotNull(task);
            AuthenticateResult result = context.AuthenticateResult;
            Assert.IsNull(result);
        }

        [TestMethod]
        public void UserIsAuthenticatedWhenPasswordMatches()
        {
            var context = new LocalAuthenticationContext { UserName = AccountName, Password = Password };
            Task task = sut.AuthenticateLocalAsync(context);

            Assert.IsNotNull(task);
            AuthenticateResult result = context.AuthenticateResult;
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsError);
            Assert.IsFalse(result.IsPartialSignIn);
            Assert.IsNotNull(result.User);
            Assert.IsNotNull(result.User.Identity);
            Assert.IsTrue(result.User.Identity.IsAuthenticated);
        }

        [TestMethod]
        public void AuthenticatedUserClaimsAreGenerated()
        {
            var context = new LocalAuthenticationContext { UserName = AccountName, Password = Password };
            Task task = sut.AuthenticateLocalAsync(context);

            Assert.IsNotNull(task);
            Assert.IsNotNull(context.AuthenticateResult);
            Assert.IsNotNull(context.AuthenticateResult.User);
            
            IIdentity identity = context.AuthenticateResult.User.Identity;
            IEnumerable<Claim> claims = context.AuthenticateResult.User.Claims;
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
    }
}