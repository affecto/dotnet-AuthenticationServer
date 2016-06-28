using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Affecto.AuthenticationServer.Configuration.Tests
{
    [TestClass]
    public class IncompleteAuthenticationServerConfigurationTests : ConfigurationTestsBase
    {
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void AllowedOriginsCanNotBeEmpty()
        {
            SetupAuthenticationServerConfiguration("AllowedOriginsEmpty.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void AllowedOriginsCanNotBeMissing()
        {
            SetupAuthenticationServerConfiguration("AllowedOriginsMissing.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ServiceUserNameCanNotBeEmpty()
        {
            SetupAuthenticationServerConfiguration("ServiceUserNameEmpty.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ServiceUserNameCanNotBeMissing()
        {
            SetupAuthenticationServerConfiguration("ServiceUserNameMissing.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void SigningCertificateStoreCanNotBeEmpty()
        {
            SetupAuthenticationServerConfiguration("SigningCertificateStoreEmpty.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void SigningCertificateStoreCanNotBeMissing()
        {
            SetupAuthenticationServerConfiguration("SigningCertificateStoreMissing.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void SigningCertificateThumbprintCanNotBeEmpty()
        {
            SetupAuthenticationServerConfiguration("SigningCertificateThumbprintEmpty.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void SigningCertificateThumbprintCanNotBeMissing()
        {
            SetupAuthenticationServerConfiguration("SigningCertificateThumbprintMissing.config");
        }

        [TestMethod]
        public void SecondarySigningCertificateThumbprintIsNullByDefaultWhenMissing()
        {
            SetupAuthenticationServerConfiguration("SecondarySigningCertificateThumbprintIsMissing.config");
            Assert.IsNull(authenticationServerConfiguration.SecondarySigningCertificateThumbprint);
        }

        [TestMethod]
        public void SecondarySigningCertificateThumbprintIsNullByDefaultWhenEmpty()
        {
            SetupAuthenticationServerConfiguration("SecondarySigningCertificateThumbprintIsEmpty.config");
            Assert.IsNull(authenticationServerConfiguration.SecondarySigningCertificateThumbprint);
        }

        [TestMethod]
        public void RequireHttpsIsEnabledByDefault()
        {
            SetupAuthenticationServerConfiguration("RequireHttpsIsEnabledByDefault.config");
            Assert.IsTrue(authenticationServerConfiguration.RequireHttps);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void PublicOriginIsNotUri()
        {
            SetupAuthenticationServerConfiguration("PublicOriginIsNotUri.config");
        }

        [TestMethod]
        public void PublicOriginIsNullByDefault()
        {
            SetupAuthenticationServerConfiguration("PublicOriginIsNullByDefault.config");
            Assert.IsNull(authenticationServerConfiguration.PublicOrigin);
        }
    }
}