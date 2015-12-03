using System;
using System.Collections.Generic;
using Affecto.AuthenticationServer.Configuration;
using IdentityServer3.Core.Services.Default;

namespace Affecto.AuthenticationServer
{
    internal class CorsPolicyServiceFactory
    {
        public static DefaultCorsPolicyService Create(IAuthenticationServerConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            DefaultCorsPolicyService corsPolicyService = new DefaultCorsPolicyService();

            if (configuration.IsAllowedOrigin("*"))
            {
                corsPolicyService.AllowAll = true;
            }
            else
            {
                corsPolicyService.AllowedOrigins = new List<string>(configuration.AllowedOrigins);
            }
            return corsPolicyService;
        }
    }
}