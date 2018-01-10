using System.Configuration;
using Affecto.Configuration.Extensions;

namespace Affecto.AuthenticationServer.Configuration
{
    internal class SigningCertificateConfiguration : ConfigurationElementBase, ISigningCertificate
    {
        public override string ElementKey => FindValue;

        [ConfigurationProperty("storeName", IsRequired = true)]
        public string StoreName
        {
            get { return (string) this["storeName"]; }
            set { this["storeName"] = value; }
        }

        [ConfigurationProperty("storeLocation", IsRequired = true)]
        public string StoreLocation
        {
            get { return (string) this["storeLocation"]; }
            set { this["storeLocation"] = value; }
        }

        [ConfigurationProperty("findValue", IsRequired = true)]
        public string FindValue
        {
            get { return (string)this["findValue"]; }
            set { this["findValue"] = value; }
        }

        [ConfigurationProperty("x509FindType", IsRequired = true)]
        public string X509FindType
        {
            get { return (string)this["x509FindType"]; }
            set { this["x509FindType"] = value; }
        }


        protected override void PostDeserialize()
        {
            if (string.IsNullOrWhiteSpace(StoreName))
            {
                throw new ConfigurationErrorsException($"SigningCertificate StoreName is required.");
            }
            if (string.IsNullOrWhiteSpace(StoreLocation))
            {
                throw new ConfigurationErrorsException($"SigningCertificate StoreLocation is required.");
            }
            if (string.IsNullOrWhiteSpace(FindValue))
            {
                throw new ConfigurationErrorsException($"SigningCertificate FindValue is required.");
            }
            if (string.IsNullOrWhiteSpace(X509FindType))
            {
                throw new ConfigurationErrorsException($"SigningCertificate X509FindType is required.");
            }


        }
    }
}