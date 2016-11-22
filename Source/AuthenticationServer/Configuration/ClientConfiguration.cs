using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Affecto.Configuration.Extensions;

namespace Affecto.AuthenticationServer.Configuration
{
    internal class ClientConfiguration : ConfigurationElementBase, IClient
    {
        public override string ElementKey => Id;

        [ConfigurationProperty("id", IsRequired = true)]
        public string Id
        {
            get { return (string) this["id"]; }
            set { this["id"] = value; }
        }

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string) this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("secret", IsRequired = true)]
        public string Secret
        {
            get { return (string) this["secret"]; }
            set { this["secret"] = value; }
        }

        [ConfigurationProperty("flow", IsRequired = true)]
        public Flow Flow
        {
            get { return (Flow) this["flow"]; }
            set { this["flow"] = value; }
        }

        [ConfigurationProperty("accessTokenLifetime", IsRequired = true)]
        public TimeSpan AccessTokenLifetime
        {
            get { return (TimeSpan) this["accessTokenLifetime"]; }
            set { this["accessTokenLifetime"] = value; }
        }

        [ConfigurationProperty("redirectUri", IsRequired = false)]
        public Uri RedirectUri
        {
            get { return (Uri) this["redirectUri"]; }
            set { this["redirectUri"] = value; }
        }

        public IReadOnlyCollection<string> AllowedScopes
        {
            get { return AllowedScopesInternal.Select(s => s.Name).ToList(); }
        }

        public IReadOnlyCollection<string> AllowedCustomGrantTypes
        {
            get { return AllowedCustomGrantTypesInternal.Select(s => s.Name).ToList(); }
        }

        [ConfigurationProperty("allowedScopes", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(ConfigurationElementCollection<ScopeConfiguration>), AddItemName = "allowedScope")]
        private ConfigurationElementCollection<AllowedScopeConfiguration> AllowedScopesInternal
        {
            get { return (ConfigurationElementCollection<AllowedScopeConfiguration>) base["allowedScopes"]; }
        }

        [ConfigurationProperty("allowedCustomGrantTypes", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(ConfigurationElementCollection<AllowedCustomGrantTypeConfiguration>), AddItemName = "allowedCustomGrantType")]
        private ConfigurationElementCollection<AllowedCustomGrantTypeConfiguration> AllowedCustomGrantTypesInternal
        {
            get { return (ConfigurationElementCollection<AllowedCustomGrantTypeConfiguration>) base["allowedCustomGrantTypes"]; }
        }

        protected override void PostDeserialize()
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                throw new ConfigurationErrorsException("Client id is required.");
            }
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new ConfigurationErrorsException($"Client name is required for client '{Id}'.");
            }
            if (string.IsNullOrWhiteSpace(Secret))
            {
                throw new ConfigurationErrorsException($"Client secret is required for client '{Id}'.");
            }
            if (!(this["flow"] is Flow))
            {
                throw new ConfigurationErrorsException($"Client flow is required for client '{Id}'.");
            }
            if (!(this["accessTokenLifetime"] is TimeSpan))
            {
                throw new ConfigurationErrorsException($"Client access token lifetime is required for client '{Id}'.");
            }
            if (RedirectUri != null && RedirectUri.OriginalString == string.Empty)
            {
                RedirectUri = null;
            }
        }
    }
}