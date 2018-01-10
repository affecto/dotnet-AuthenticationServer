using System.Configuration;
using System.Security.Claims;
using Affecto.AuthenticationServer.Configuration;
using IdentityServer3.Core.Configuration;
using Kentor.AuthServices.Owin;
using Owin;
using Microsoft.Owin.Security;
using Kentor.AuthServices.Configuration;
using System.IdentityModel.Metadata;
using Kentor.AuthServices;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Globalization;
using System.IdentityModel.Tokens;
using Autofac;

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
            else
            {
                if (IsKentorAuthServicesCustomProvidersConfigured())
                {
                    options.AuthenticationOptions = new IdentityServer3.Core.Configuration.AuthenticationOptions
                    {
                        IdentityProviders = ConfigureCustomIdentityProviders
                    };

                }
            }

            if (configuration.PublicOrigin != null)
            {
                options.PublicOrigin = configuration.PublicOrigin.ToString();
            }

            if (configuration.Issuer != null)
            {
                options.IssuerUri = configuration.Issuer.ToString();
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

        public static void ConfigureCustomIdentityProviders(IAppBuilder app, string signInAsType)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AuthenticationServerModule>();
            IContainer container = builder.Build();

            IKentorAuthServicesCustomProvidersConfiguration config = container.Resolve<IKentorAuthServicesCustomProvidersConfiguration>();
            IAuthenticationServerConfiguration authConfig = container.Resolve<IAuthenticationServerConfiguration>();

            app.SetDefaultSignInAsAuthenticationType(AuthenticationTypes.Federation);

            foreach (var customIdp in config.IdentityProviders)
            {
                var authServicesOptions = new KentorAuthServicesAuthenticationOptions(false)
                {
                    SPOptions = new SPOptions
                    {
                        EntityId = new EntityId(config.EntityId),
                        ModulePath = customIdp.ModulePath,
                    },
                    SignInAsAuthenticationType = signInAsType,
                    AuthenticationType = customIdp.AuthenticationType,
                    Caption = customIdp.Caption
                };

                var cert = LoadCertificate(customIdp.SigningCertificate.StoreName, customIdp.SigningCertificate.StoreLocation, customIdp.SigningCertificate.X509FindType, customIdp.SigningCertificate.FindValue);

                var idp = new IdentityProvider(new System.IdentityModel.Metadata.EntityId(customIdp.EntityId), authServicesOptions.SPOptions)
                {
                    LoadMetadata = true,
                    MetadataLocation = customIdp.MetadataLocation
                };


                idp.SigningKeys.AddConfiguredKey(
                        new X509RawDataKeyIdentifierClause(cert));

                authServicesOptions.IdentityProviders.Add(idp);
                app.UseKentorAuthServicesAuthentication(authServicesOptions);

            }
            

        }

        private static X509Certificate2 LoadCertificate(string name, string location, string type, string findValue)
        {

            StoreName storeName;
            StoreLocation storeLocation;
            X509FindType findType;

            try
            {
                storeName = (StoreName)Enum.Parse(typeof(StoreName), name);
                storeLocation = (StoreLocation)Enum.Parse(typeof(StoreLocation), location);
                findType = (X509FindType)Enum.Parse(typeof(X509FindType), type);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    string.Format(CultureInfo.InvariantCulture,
                    "Invalid certificate store config values. Cannot load cert through {0} in {1}:{2}.",
                    type, location, name));
            }


            var store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.ReadOnly);
            try
            {
                var certs = store.Certificates.Find(findType, findValue, false);

                if (certs.Count != 1)
                {
                    throw new InvalidOperationException(
                        string.Format(CultureInfo.InvariantCulture,
                        "Finding cert through {0} in {1}:{2} with value {3} matched {4} certificates. A unique match is required.",
                        findType, storeLocation, storeName, findValue, certs.Count));
                }

                return certs[0];
            }
            finally
            {
                store.Close();
            }

        }

        private static bool IsKentorAuthServicesConfigured()
        {
            return ConfigurationManager.GetSection("kentor.authServices") != null && ConfigurationManager.GetSection("system.identityModel") != null;
        }

        private static bool IsKentorAuthServicesCustomProvidersConfigured()
        {
            return ConfigurationManager.GetSection("kentor.authServices.customProviders") != null && ConfigurationManager.GetSection("system.identityModel") != null;
        }
    }
}