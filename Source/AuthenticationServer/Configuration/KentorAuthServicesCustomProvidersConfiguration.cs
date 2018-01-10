using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Affecto.Configuration.Extensions;

namespace Affecto.AuthenticationServer.Configuration
{

    /*
      <kentor.authServices.customProviders entityId="https://localhost:44305/">
        <identityProviders>
            <identityProvider modulePath="/SAML/local" AuthenticationType="saml-local" caption="SAML ADFS Local" entityId="http://fs.dev.local/adfs/services/trust" metadataLocation="https://fs.dev.local/FederationMetadata/2007-06/FederationMetadata.xml" >
              <signingCertificate storeName="My" storeLocation="LocalMachine" findValue="1976d6c2c5fbb3b54d727e290d2ab6ab" x509FindType="FindBySerialNumber" />
            </identityProvider>
            <identityProvider modulePath="/SAML/azure" AuthenticationType="saml-azure" caption="SAML ADFS AZure" entityId="http://adfstest-tpr.westeurope.cloudapp.azure.com/adfs/services/trust" metadataLocation="https://adfstest-tpr.westeurope.cloudapp.azure.com/FederationMetadata/2007-06/FederationMetadata.xml" >
              <signingCertificate storeName="My" storeLocation="LocalMachine" findValue="2601a9aacf3c56a3406fd5a216cf5049" x509FindType="FindBySerialNumber" />
            </identityProvider>
        <identityProviders>
      </kentor.authServices.customProviders>

     * */
    internal class KentorAuthServicesCustomProvidersConfiguration : ConfigurationSection, IKentorAuthServicesCustomProvidersConfiguration
    {
        private static readonly KentorAuthServicesCustomProvidersConfiguration SettingsInstance =
            ConfigurationManager.GetSection("kentor.authServices.customProviders") as KentorAuthServicesCustomProvidersConfiguration;

        public static IKentorAuthServicesCustomProvidersConfiguration Settings => SettingsInstance;

        [ConfigurationProperty("entityId", IsRequired = true)]
        public string EntityId
        {
            get { return (string)this["entityId"]; }
            set { this["entityId"] = value; }
        }

        public IReadOnlyCollection<ICustomIdentityProvider> IdentityProviders => IdentityProvidersInternal.ToList();

        [ConfigurationProperty("identityProviders", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(ConfigurationElementCollection<CustomIdentityProviderConfiguration>), AddItemName = "identityProvider")]
        private ConfigurationElementCollection<CustomIdentityProviderConfiguration> IdentityProvidersInternal
        {
            get { return (ConfigurationElementCollection<CustomIdentityProviderConfiguration>) base["identityProviders"]; }
        }


        protected override void PostDeserialize()
        {
            if (string.IsNullOrWhiteSpace(EntityId))
                throw new ConfigurationErrorsException("EntityId is required.");
            
            if(IdentityProviders.Count == 0)
                throw new ConfigurationErrorsException("At least one IdentityProvider is required.");
            
        }


    }
}