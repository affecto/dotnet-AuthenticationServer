using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using Affecto.AuthenticationServer.Claims;
using Affecto.IdentityManagement.Interfaces;
using Affecto.IdentityManagement.Interfaces.Model;

namespace Affecto.AuthenticationServer.IdentityManagement
{
    internal class ClaimsIdentityBuilder
    {
        private readonly IUserService userService;

        public ClaimsIdentityBuilder(IUserService userService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }
            this.userService = userService;
        }

        public ClaimsIdentity Build(string authenticationType, string userAccountName, AccountType accountType)
        {
            if (string.IsNullOrWhiteSpace(authenticationType))
            {
                throw new ArgumentException("Authentication type cannot be null or empty.", nameof(authenticationType));
            }
            if (string.IsNullOrWhiteSpace(userAccountName))
            {
                throw new ArgumentException("User account name cannot be null or empty.", nameof(userAccountName));
            }

            IUser user = userService.GetUser(userAccountName, accountType);
            if (user == null)
            {
                throw new AuthenticationException($"Invalid user '{userAccountName}'.");
            }
            if (user.IsDisabled)
            {
                throw new AuthenticationException($"User '{userAccountName}' is disabled.");
            }

            var claimsBuilder = new ClaimsBuilder();
            claimsBuilder
                .SetId(user.Id)
                .SetName(user.Name)
                .SetAccountName(userAccountName);

            SetUserCustomPropertyClaims(user, claimsBuilder);
            SetUserOrganizationClaims(user, claimsBuilder);
            SetUserGroupClaims(user, claimsBuilder);
            SetUserRoleAndPermissionClaims(user, claimsBuilder);

            var identity = new ClaimsIdentity(authenticationType);
            identity.AddClaims(claimsBuilder.GetClaims());

            return identity;
        }

        private static void SetUserCustomPropertyClaims(IUser user, ClaimsBuilder builder)
        {
            foreach (ICustomProperty customProperty in user.CustomProperties.Where(customProperty => customProperty.Value != null))
            {
                builder.AddCustomProperty(customProperty.Name, customProperty.Value);
            }
        }

        private static void SetUserOrganizationClaims(IUser user, ClaimsBuilder builder)
        {
            foreach (string organizationName in user.Organizations.Select(o => o.Name).Distinct())
            {
                if (organizationName != null)
                {
                    builder.AddOrganization(organizationName);
                }
            }
        }

        private static void SetUserGroupClaims(IUser user, ClaimsBuilder builder)
        {
            foreach (Guid groupId in user.Groups.Select(g => g.Id).Distinct())
            {
                if (groupId != Guid.Empty)
                {
                    builder.AddGroup(groupId);
                }
            }
        }

        private static void SetUserRoleAndPermissionClaims(IUser user, ClaimsBuilder builder)
        {
            foreach (string roleName in user.Roles.Select(r => r.Name).Distinct())
            {
                if (roleName != null)
                {
                    builder.AddRole(roleName);
                }
            }

            IEnumerable<IPermission> permissions = user.Roles.SelectMany(r => r.Permissions);
            foreach (string permissionName in permissions.Select(p => p.Name).Distinct())
            {
                if (permissionName != null)
                {
                    builder.AddPermission(permissionName);
                }
            }
        }
    }
}