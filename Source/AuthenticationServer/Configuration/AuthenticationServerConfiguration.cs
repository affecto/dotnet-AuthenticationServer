using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Affecto.Configuration.Extensions;

namespace Affecto.AuthenticationServer.Configuration
{
    internal class AuthenticationServerConfiguration : ConfigurationSection, IAuthenticationServerConfiguration
    {
        private static readonly AuthenticationServerConfiguration SettingsInstance =
            ConfigurationManager.GetSection("authenticationServer") as AuthenticationServerConfiguration;

        public static IAuthenticationServerConfiguration Settings => SettingsInstance;
        public IReadOnlyCollection<IScope> Scopes => ScopesInternal.ToList();
        public IReadOnlyCollection<IClient> Clients => ClientsInternal.ToList();

        public IReadOnlyCollection<string> AllowedOrigins
        {
            get
            {
                return string.IsNullOrWhiteSpace(AllowedOriginsInternal)
                    ? new List<string>(0)
                    : AllowedOriginsInternal.Split(',').Select(o => o.Trim()).ToList();
            }
        }

        public bool IsAllowedOrigin(string origin)
        {
            return AllowedOrigins.Any(o => o.Equals(origin, StringComparison.InvariantCultureIgnoreCase) || o.Equals("*"));
        }

        [ConfigurationProperty("requireHttps", IsRequired = false, DefaultValue = true)]
        public bool RequireHttps
        {
            get { return (bool) this["requireHttps"]; }
            set { this["requireHttps"] = value; }
        }

        [ConfigurationProperty("serviceUserName", IsRequired = true)]
        public string ServiceUserName
        {
            get { return (string) this["serviceUserName"]; }
            set { this["serviceUserName"] = value; }
        }

        [ConfigurationProperty("autoCreateUser", IsRequired = false, DefaultValue = false)]
        public bool AutoCreateUser
        {
            get { return (bool) this["autoCreateUser"]; }
            set { this["autoCreateUser"] = value; }
        }

        [ConfigurationProperty("signingCertificateStore", IsRequired = true)]
        public StoreName SigningCertificateStore
        {
            get { return (StoreName) this["signingCertificateStore"]; }
            set { this["signingCertificateStore"] = value; }
        }

        [ConfigurationProperty("signingCertificateThumbprint", IsRequired = true)]
        public string SigningCertificateThumbprint
        {
            get { return (string) this["signingCertificateThumbprint"]; }
            set { this["signingCertificateThumbprint"] = value; }
        }

        [ConfigurationProperty("secondarySigningCertificateThumbprint", IsRequired = false)]
        public string SecondarySigningCertificateThumbprint
        {
            get
            {
                string value = (string) this["secondarySigningCertificateThumbprint"];
                return string.IsNullOrWhiteSpace(value) ? null : value;
            }
            set { this["secondarySigningCertificateThumbprint"] = value; }
        }

        [ConfigurationProperty("publicOrigin", IsRequired = false)]
        public Uri PublicOrigin
        {
            get { return (Uri) this["publicOrigin"]; }
            set { this["publicOrigin"] = value; }
        }

        [ConfigurationProperty("allowedOrigins", IsRequired = true)]
        private string AllowedOriginsInternal
        {
            get { return (string) this["allowedOrigins"]; }
            set { this["allowedOrigins"] = value; }
        }

        [ConfigurationProperty("scopes", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(ConfigurationElementCollection<ScopeConfiguration>), AddItemName = "scope")]
        private ConfigurationElementCollection<ScopeConfiguration> ScopesInternal
        {
            get { return (ConfigurationElementCollection<ScopeConfiguration>) base["scopes"]; }
        }

        [ConfigurationProperty("clients", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(ConfigurationElementCollection<ClientConfiguration>), AddItemName = "client")]
        private ConfigurationElementCollection<ClientConfiguration> ClientsInternal
        {
            get { return (ConfigurationElementCollection<ClientConfiguration>) base["clients"]; }
        }

        protected override void PostDeserialize()
        {
            if (string.IsNullOrWhiteSpace(ServiceUserName))
            {
                throw new ConfigurationErrorsException("Service username is required.");
            }
            if (string.IsNullOrWhiteSpace(AllowedOriginsInternal))
            {
                throw new ConfigurationErrorsException("Allowed origins are required.");
            }
            if (string.IsNullOrWhiteSpace(SigningCertificateThumbprint))
            {
                throw new ConfigurationErrorsException("Signing certificate thumbprint is required.");
            }
            if (!(this["signingCertificateStore"] is StoreName))
            {
                throw new ConfigurationErrorsException("Signing certificate store is required.");
            }

            object publicOrigin = this["publicOrigin"];
            if (publicOrigin != null && (!(publicOrigin is Uri) || !Uri.IsWellFormedUriString(publicOrigin.ToString(), UriKind.Absolute)))
            {
                throw new ConfigurationErrorsException("Public origin must be a well formed absolute URI.");
            }

            CheckAllowedScopesMatch();
        }

        private void CheckAllowedScopesMatch()
        {
            foreach (IClient client in Clients)
            {
                foreach (string allowedScope in client.AllowedScopes)
                {
                    if (Scopes.All(s => s.Name != allowedScope))
                    {
                        throw new ConfigurationErrorsException($"Allowed scope '{allowedScope}' of client '{client.Id}' does not match to configured scopes.");
                    }
                }
            }
        }
    }
}