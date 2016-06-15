using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Affecto.AuthenticationServer.Plugins.Infrastructure.Configuration;
using IdentityServer3.Core.Models;

namespace Affecto.AuthenticationServer.Plugins.Infrastructure
{
    public abstract class FederatedAuthenticationUserServiceBase : UserServiceBase
    {
        private readonly Lazy<IFederatedAuthenticationConfiguration> federatedAuthenticationConfiguration;

        protected FederatedAuthenticationUserServiceBase(Lazy<IFederatedAuthenticationConfiguration> federatedAuthenticationConfiguration)
        {
            if (federatedAuthenticationConfiguration == null)
            {
                throw new ArgumentNullException(nameof(federatedAuthenticationConfiguration));
            }
            this.federatedAuthenticationConfiguration = federatedAuthenticationConfiguration;
        }

        /// <summary>
        /// This method gets called when the user uses an external identity provider to authenticate.
        /// The user's identity from the external provider is passed via the `externalUser` parameter which contains the
        /// provider identifier, the provider's identifier for the user, and the claims from the provider for the external user.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns/>
        public override Task AuthenticateExternalAsync(ExternalAuthenticationContext context)
        {
            Claim userAccountName = context.ExternalIdentity.Claims.SingleOrDefault(c => c.Type == federatedAuthenticationConfiguration.Value.UserAccountNameClaim);
            Claim userDisplayName = context.ExternalIdentity.Claims.SingleOrDefault(c => c.Type == federatedAuthenticationConfiguration.Value.UserDisplayNameClaim);

            if (userAccountName != null && userDisplayName != null)
            {
                IEnumerable<string> userGroups = Enumerable.Empty<string>();
                if (!string.IsNullOrWhiteSpace(federatedAuthenticationConfiguration.Value.GroupsClaim))
                {
                    userGroups = context.ExternalIdentity.Claims
                        .Where(c => c.Type == federatedAuthenticationConfiguration.Value.GroupsClaim)
                        .Select(c => c.Value);
                }

                CreateOrUpdateExternallyAuthenticatedUser(userAccountName.Value, userDisplayName.Value, userGroups.ToList());
                context.AuthenticateResult = CreateAuthenticateResult(userAccountName.Value, AuthenticationTypes.Federation, context.SignInMessage.IdP);
            }
            else
            {
                context.AuthenticateResult = new AuthenticateResult("One or more required claims were missing from the IDP's message.");
            }

            return Task.FromResult(0);
        }

        /// <summary>
        /// Override this method if you need to update externally authenticated user's information to another identity management system.
        /// </summary>
        /// <param name="userAccountName">User's account name from external IDP claims.</param>
        /// <param name="userDisplayName">User's display name from external IDP claims..</param>
        /// <param name="groups">User's groups from external IDP claims.</param>
        protected virtual void CreateOrUpdateExternallyAuthenticatedUser(string userAccountName, string userDisplayName, IReadOnlyCollection<string> groups)
        {
        }
    }
}