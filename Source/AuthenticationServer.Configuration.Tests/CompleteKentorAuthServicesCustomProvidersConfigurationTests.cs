using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Affecto.AuthenticationServer.Configuration.Tests
{
    [TestClass]
    public class CompleteKentorAuthServicesCustomProvidersConfigurationTests : ConfigurationTestsBase
    {
        [TestInitialize]
        public void Setup()
        {
            SetupKentorAuthServicesCustomProvidersConfiguration("ValidKentorCustomProvidersConfiguration.config");
        }


        [TestMethod]
        public void EntityIdIsRetrieved()
        {
            Assert.AreEqual("https://localhost:44305/", kentorAuthServicesCustomProvidersConfiguration.EntityId);
        }


        [TestMethod]
        public void IdentityProvidersAreRetrieved()
        {
            Assert.AreEqual(2, kentorAuthServicesCustomProvidersConfiguration.IdentityProviders.Count);
            var firstIdp = kentorAuthServicesCustomProvidersConfiguration.IdentityProviders.First();
            Assert.AreEqual("/SAML/local", firstIdp.ModulePath);
            Assert.AreEqual("saml-local", firstIdp.AuthenticationType);
            Assert.AreEqual("SAML ADFS Local", firstIdp.Caption);
            Assert.AreEqual("http://localhost/adfs/services/trust", firstIdp.EntityId);
            Assert.AreEqual("https://fs.dev.local/FederationMetadata/2007-06/FederationMetadata.xml", firstIdp.MetadataLocation);

            var secondIdp = kentorAuthServicesCustomProvidersConfiguration.IdentityProviders.Skip(1).First();
            Assert.AreEqual("/SAML/azure", secondIdp.ModulePath);
            Assert.AreEqual("saml-azure", secondIdp.AuthenticationType);
            Assert.AreEqual("SAML ADFS Azure", secondIdp.Caption);
            Assert.AreEqual("http://some-adfs.westeurope.cloudapp.azure.com/adfs/services/trust", secondIdp.EntityId);
            Assert.AreEqual("https://some-adfs.westeurope.cloudapp.azure.com/FederationMetadata/2007-06/FederationMetadata.xml", secondIdp.MetadataLocation);


        }


        [TestMethod]
        public void IdpSigningCertificateIsRetrieved()
        {

            var cert = kentorAuthServicesCustomProvidersConfiguration.IdentityProviders.First().SigningCertificate;
            Assert.AreEqual("My", cert.StoreName);
            Assert.AreEqual("LocalMachine", cert.StoreLocation);
            Assert.AreEqual("11110000222233334444555566667777", cert.FindValue);
            Assert.AreEqual("FindBySerialNumber", cert.X509FindType);

            var secondCert = kentorAuthServicesCustomProvidersConfiguration.IdentityProviders.Skip(1).First().SigningCertificate;
            Assert.AreEqual("My", secondCert.StoreName);
            Assert.AreEqual("LocalMachine", secondCert.StoreLocation);
            Assert.AreEqual("22220000222233334444555566667777", secondCert.FindValue);
            Assert.AreEqual("FindBySerialNumber", secondCert.X509FindType);

        }


    }
}