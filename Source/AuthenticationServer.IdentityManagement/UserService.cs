using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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

        public UserService(Lazy<IUserService> userService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }

            this.userService = userService;
        }

        /// <summary>
        /// This method gets called for local authentication (whenever the user uses the username and password dialog).
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns/>
        public override Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            bool success = userService.Value.IsMatchingPassword(context.UserName, context.Password);

            if (success)
            {
                var identityBuilder = new ClaimsIdentityBuilder(userService.Value);
                ClaimsIdentity identity = identityBuilder.Build(AuthenticationTypes.Password, context.UserName, AccountType.Password);
                context.AuthenticateResult = new AuthenticateResult(identity.GetUserId(), identity.Name, identity.Claims, AuthenticationTypes.Password);
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
    }
}