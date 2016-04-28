using System.Configuration;
using System.Security.Claims;
using Affecto.AuthenticationServer.Configuration;
using IdentityServer3.Core.Configuration;
using Kentor.AuthServices.Owin;
using Owin;
using Microsoft.Owin.Security;

namespace Affecto.AuthenticationServer
{
    internal class IdentityServerOptionsFactory
    {
        public static IdentityServerOptions Create(IAuthenticationServerConfiguration configuration, IdentityServerServiceFactory serviceFactory)
        {
            var certificate = new Certificate(configuration.SigningCertificateStore, configuration.SigningCertificateThumbprint);

            var options = new IdentityServerOptions
            {
                SiteName = "Authentication Server",
                SigningCertificate = certificate.Load(),
                Factory = serviceFactory,
                RequireSsl = configuration.RequireHttps,
            };

            if (IsKentorAuthServicesConfigured())
            {
                options.AuthenticationOptions = new IdentityServer3.Core.Configuration.AuthenticationOptions
                {
                    IdentityProviders = ConfigureIdentityProviders
                };
            }

            if (configuration.PublicOrigin != null)
            {
                options.PublicOrigin = configuration.PublicOrigin.ToString();
            }

            if (configuration.SecondarySigningCertificateThumbprint != null)
            {
                var secondaryCertificate = new Certificate(configuration.SigningCertificateStore, configuration.SecondarySigningCertificateThumbprint);
                options.SecondarySigningCertificate = secondaryCertificate.Load();
            }

            return options;
        }

        public static void ConfigureIdentityProviders(IAppBuilder app, string signInAsType)
        {
            var authServicesOptions = new KentorAuthServicesAuthenticationOptions(true)
            {
                SignInAsAuthenticationType = signInAsType,
                AuthenticationType = "saml" // this is the "idp" - identity provider - that you can refer to throughout identity server
            };
            app.SetDefaultSignInAsAuthenticationType(AuthenticationTypes.Federation);
            app.UseKentorAuthServicesAuthentication(authServicesOptions);
        }

        private static bool IsKentorAuthServicesConfigured()
        {
            return ConfigurationManager.GetSection("kentor.authServices") != null && ConfigurationManager.GetSection("system.identityModel") != null;
        }
    }
}