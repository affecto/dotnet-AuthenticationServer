using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Affecto.AuthenticationServer.Infrastructure.Configuration;
using IdentityServer3.Core.Models;

namespace Affecto.AuthenticationServer.Infrastructure
{
    public abstract class UserServiceBase : IdentityServer3.Core.Services.Default.UserServiceBase
    {
        private readonly Lazy<IFederatedAuthenticationConfiguration> federatedAuthenticationConfiguration;

        protected UserServiceBase(Lazy<IFederatedAuthenticationConfiguration> federatedAuthenticationConfiguration)
        {
            if (federatedAuthenticationConfiguration == null)
            {
                throw new ArgumentNullException(nameof(federatedAuthenticationConfiguration));
            }
            this.federatedAuthenticationConfiguration = federatedAuthenticationConfiguration;
        }

        /// <summary>
        /// This method gets called for local authentication (whenever the user uses the username and password dialog).
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns/>
        public override Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            if (IsMatchingPassword(context.UserName, context.Password))
            {
                context.AuthenticateResult = CreateAuthenticateResult(context.UserName, AuthenticationTypes.Password);
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
                CreateOrUpdateExternallyAuthenticatedUser(userAccountName.Value, userDisplayName.Value, userGroups.Select(c => c.Value));
                context.AuthenticateResult = CreateAuthenticateResult(userAccountName.Value, AuthenticationTypes.Federation, context.SignInMessage.IdP);
            }

            return Task.FromResult(0);
        }

        protected virtual void CreateOrUpdateExternallyAuthenticatedUser(string userName, string userDisplayName, IEnumerable<string> groups)
        {
        }

        protected virtual bool IsMatchingPassword(string userName, string password)
        {
            return false;
        }

        protected abstract AuthenticateResult CreateAuthenticateResult(string userName, string authenticationType, string identityProvider = "idsrv");
    }
}
