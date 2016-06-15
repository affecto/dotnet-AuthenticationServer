using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;

namespace Affecto.AuthenticationServer.Plugins.Infrastructure
{
    public abstract class UserServiceBase : IdentityServer3.Core.Services.Default.UserServiceBase
    {
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
        /// Override this method to implement user password check.
        /// </summary>
        /// <param name="userName">Received user name.</param>
        /// <param name="password">Received user password.</param>
        /// <returns></returns>
        protected virtual bool IsMatchingPassword(string userName, string password)
        {
            return false;
        }

        /// <summary>
        /// Override this method to create authentication result containing data about the user as claims.
        /// </summary>
        /// <param name="userName">Received user name.</param>
        /// <param name="authenticationType">Authentication type used.</param>
        /// <param name="identityProvider">Identity provider id, "idsrv" as default. Can be replaced with e.g. external identity provider id.</param>
        /// <returns></returns>
        protected abstract AuthenticateResult CreateAuthenticateResult(string userName, string authenticationType, string identityProvider = "idsrv");
    }
}