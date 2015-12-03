using Affecto.AuthenticationServer.Configuration;
using IdentityServer3.Core.Configuration;

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
                RequireSsl = configuration.RequireHttps
            };

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
    }
}