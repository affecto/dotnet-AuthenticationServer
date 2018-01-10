using System.Configuration;
using Affecto.Configuration.Extensions;

namespace Affecto.AuthenticationServer.Configuration
{
    internal class CustomIdentityProviderConfiguration : ConfigurationElementBase, ICustomIdentityProvider
    {
        public override string ElementKey => EntityId;

        [ConfigurationProperty("modulePath", IsRequired = true)]
        public string ModulePath
        {
            get { return (string) this["modulePath"]; }
            set { this["modulePath"] = value; }
        }

        [ConfigurationProperty("authenticationType", IsRequired = true)]
        public string AuthenticationType
        {
            get { return (string) this["authenticationType"]; }
            set { this["authenticationType"] = value; }
        }

        [ConfigurationProperty("caption", IsRequired = true)]
        public string Caption
        {
            get { return (string)this["caption"]; }
            set { this["caption"] = value; }
        }

        [ConfigurationProperty("entityId", IsRequired = true)]
        public string EntityId
        {
            get { return (string)this["entityId"]; }
            set { this["entityId"] = value; }
        }

        [ConfigurationProperty("metadataLocation", IsRequired = true)]
        public string MetadataLocation
        {
            get { return (string)this["metadataLocation"]; }
            set { this["metadataLocation"] = value; }
        }

        public ISigningCertificate SigningCertificate => SigningCertificateInternal;

        [ConfigurationProperty("signingCertificate", IsRequired = false)]
        private SigningCertificateConfiguration SigningCertificateInternal
        {
            get { return (SigningCertificateConfiguration)this["signingCertificate"]; }
            set { this["signingCertificate"] = value; }
        }

        protected override void PostDeserialize()
        {
            if (string.IsNullOrWhiteSpace(EntityId))
            {
                throw new ConfigurationErrorsException($"Identity provider EntityId is required.");
            }
            if (string.IsNullOrWhiteSpace(ModulePath))
            {
                throw new ConfigurationErrorsException($"Identity provider ModulePath is required for entity '{EntityId}'.");
            }
            if (string.IsNullOrWhiteSpace(AuthenticationType))
            {
                throw new ConfigurationErrorsException($"Identity provider AuthenticationType is required for entity '{EntityId}'.");
            }
            if (string.IsNullOrWhiteSpace(Caption))
            {
                throw new ConfigurationErrorsException($"Identity provider Caption is required for entity '{EntityId}'.");
            }
            if (string.IsNullOrWhiteSpace(MetadataLocation))
            {
                throw new ConfigurationErrorsException($"Identity provider MetadataLocation is required for entity '{EntityId}'.");
            }

        }
    }
}