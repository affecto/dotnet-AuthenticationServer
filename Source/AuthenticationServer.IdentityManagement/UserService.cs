using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Affecto.AuthenticationServer.IdentityManagement.Configuration;
using Affecto.IdentityManagement.Interfaces;
using Affecto.IdentityManagement.Interfaces.Model;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.Default;
using Microsoft.AspNet.Identity;

namespace Affecto.AuthenticationServer.IdentityManagement
{
    internal class UserService : UserServiceBase
    {
        private readonly Lazy<IUserService> userService;
        private readonly Lazy<IFederatedAuthenticationConfiguration> federatedAuthenticationConfiguration;
        private readonly Lazy<IIdentityManagementConfiguration> configuration;

        public UserService(Lazy<IUserService> userService, Lazy<IFederatedAuthenticationConfiguration> federatedAuthenticationConfiguration, 
            Lazy<IIdentityManagementConfiguration> configuration)
        {
            if (userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }
            if (federatedAuthenticationConfiguration == null)
            {
                throw new ArgumentNullException(nameof(federatedAuthenticationConfiguration));
            }
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.userService = userService;
            this.federatedAuthenticationConfiguration = federatedAuthenticationConfiguration;
            this.configuration = configuration;
        }

        /// <summary>
        /// This method gets called for local authentication (whenever the user uses the username and password dialog).
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns/>
        public override Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            if (userService.Value.IsMatchingPassword(context.UserName, context.Password))
            {
                context.AuthenticateResult = CreateAuthenticateResult(context.UserName, AuthenticationTypes.Password, AccountType.Password);
            }

            return Task.FromResult(0);
        }

        /// <summary>
        /// This method is called whenever claims about the user are requested (e.g. during token creation or via the userinfo endpoint)
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns/>
        public override Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.IssuedClaims = context.Subject.Claims.ToList();
            return Task.FromResult(0);
        }

        /// <summary>
        /// This method gets called when the user uses an external identity provider to authenticate.
        ///             The user's identity from the external provider is passed via the `externalUser` parameter which contains the
        ///             provider identifier, the provider's identifier for the user, and the claims from the provider for the external user.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns/>
        public override Task AuthenticateExternalAsync(ExternalAuthenticationContext context)
        {
            Claim userAccountName = context.ExternalIdentity.Claims.SingleOrDefault(c => c.Type == federatedAuthenticationConfiguration.Value.UserAccountNameClaim);
            Claim userDisplayName = context.ExternalIdentity.Claims.SingleOrDefault(c => c.Type == federatedAuthenticationConfiguration.Value.UserDisplayNameClaim);
            if (userAccountName != null && userDisplayName != null)
            {
                IEnumerable<Claim> userGroups = Enumerable.Empty<Claim>();
                if (!string.IsNullOrWhiteSpace(federatedAuthenticationConfiguration.Value.GroupsClaim))
                {
                    userGroups = context.ExternalIdentity.Claims.Where(c => c.Type == federatedAuthenticationConfiguration.Value.GroupsClaim);
                }
                CreateOrUpdateUser(userAccountName.Value, userDisplayName.Value, userGroups.Select(c => c.Value), AccountType.Federated);
                context.AuthenticateResult = CreateAuthenticateResult(userAccountName.Value, AuthenticationTypes.Federation, AccountType.Federated);
            }

            return Task.FromResult(0);
        }

        private AuthenticateResult CreateAuthenticateResult(string userName, string authenticationType, AccountType accountType)
        {
            var identityBuilder = new ClaimsIdentityBuilder(userService.Value);
            ClaimsIdentity identity = identityBuilder.Build(authenticationType, userName, accountType);
            return new AuthenticateResult(identity.GetUserId(), identity.Name, identity.Claims, authenticationType);
        }

        private void CreateOrUpdateUser(string accountName, string displayName, IEnumerable<string> groups, AccountType accountType)
        {
            if (!userService.Value.IsExistingUserAccount(accountName, accountType))
            {
                if (configuration.Value.AutoCreateUser)
                {
                    userService.Value.AddUser(accountName, accountType, displayName, groups);
                }
            }
            else
            {
                userService.Value.UpdateUserGroupsAndRoles(accountName, accountType, groups);
            }
        }
    }
}