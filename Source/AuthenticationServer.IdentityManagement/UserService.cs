using System;
using System.Collections.Generic;
using System.Security.Claims;
using Affecto.AuthenticationServer.IdentityManagement.Configuration;
using Affecto.AuthenticationServer.Infrastructure;
using Affecto.AuthenticationServer.Infrastructure.Configuration;
using Affecto.IdentityManagement.Interfaces;
using Affecto.IdentityManagement.Interfaces.Model;
using IdentityServer3.Core.Models;
using Microsoft.AspNet.Identity;

namespace Affecto.AuthenticationServer.IdentityManagement
{
    internal class UserService : UserServiceBase
    {
        private readonly Lazy<IUserService> userService;
        private readonly Lazy<IIdentityManagementConfiguration> identityManagementConfiguration;

        public UserService(Lazy<IUserService> userService, Lazy<IIdentityManagementConfiguration> identityManagementConfiguration, 
            Lazy<IFederatedAuthenticationConfiguration> federatedAuthenticationConfiguration)
            : base(federatedAuthenticationConfiguration)
        {
            if (userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }
            if (identityManagementConfiguration == null)
            {
                throw new ArgumentNullException(nameof(identityManagementConfiguration));
            }

            this.userService = userService;
            this.identityManagementConfiguration = identityManagementConfiguration;
        }

        protected override void CreateOrUpdateExternallyAuthenticatedUser(string accountName, string displayName, IEnumerable<string> groups)
        {
            if (!userService.Value.IsExistingUserAccount(accountName, AccountType.Federated))
            {
                if (identityManagementConfiguration.Value.AutoCreateUser)
                {
                    userService.Value.AddUser(accountName, AccountType.Federated, displayName, groups);
                }
            }
            else
            {
                userService.Value.UpdateUserGroupsAndRoles(accountName, AccountType.Federated, groups);
            }
        }

        protected override AuthenticateResult CreateAuthenticateResult(string userName, string authenticationType, string identityProvider = "idsrv")
        {
            var identityBuilder = new ClaimsIdentityBuilder(userService.Value);
            ClaimsIdentity identity;
            switch (authenticationType)
            {
                case AuthenticationTypes.Password:
                    identity = identityBuilder.Build(authenticationType, userName, AccountType.Password);
                    break;
                case AuthenticationTypes.Federation:
                    identity = identityBuilder.Build(authenticationType, userName, AccountType.Federated);
                    break;
                default:
                    throw new ArgumentException(string.Format("Authentication type '{0}' not supported.", authenticationType));

            }
            return new AuthenticateResult(identity.GetUserId(), identity.Name, identity.Claims, identityProvider);
        }

        protected override bool IsMatchingPassword(string userName, string password)
        {
            return userService.Value.IsMatchingPassword(userName, password);
        }
    }
}