using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Affecto.AuthenticationServer.Tests.Configuration
{
    [TestClass]
    public class IncompleteAuthenticationServerConfigurationTests : ConfigurationTestsBase
    {
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void AllowedOriginsCanNotBeEmpty()
        {
            SetupSut("AllowedOriginsEmpty.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void AllowedOriginsCanNotBeMissing()
        {
            SetupSut("AllowedOriginsMissing.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ServiceUserNameCanNotBeEmpty()
        {
            SetupSut("ServiceUserNameEmpty.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ServiceUserNameCanNotBeMissing()
        {
            SetupSut("ServiceUserNameMissing.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void SigningCertificateStoreCanNotBeEmpty()
        {
            SetupSut("SigningCertificateStoreEmpty.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void SigningCertificateStoreCanNotBeMissing()
        {
            SetupSut("SigningCertificateStoreMissing.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void SigningCertificateThumbprintCanNotBeEmpty()
        {
            SetupSut("SigningCertificateThumbprintEmpty.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void SigningCertificateThumbprintCanNotBeMissing()
        {
            SetupSut("SigningCertificateThumbprintMissing.config");
        }

        [TestMethod]
        public void AutoCreateIsDisabledByDefault()
        {
            SetupSut("AutoCreateIsDisabledByDefault.config");
            Assert.IsFalse(sut.AutoCreateUser);
        }

        [TestMethod]
        public void SecondarySigningCertificateThumbprintIsNullByDefaultWhenMissing()
        {
            SetupSut("SecondarySigningCertificateThumbprintIsMissing.config");
            Assert.IsNull(sut.SecondarySigningCertificateThumbprint);
        }

        [TestMethod]
        public void SecondarySigningCertificateThumbprintIsNullByDefaultWhenEmpty()
        {
            SetupSut("SecondarySigningCertificateThumbprintIsEmpty.config");
            Assert.IsNull(sut.SecondarySigningCertificateThumbprint);
        }

        [TestMethod]
        public void RequireHttpsIsEnabledByDefault()
        {
            SetupSut("RequireHttpsIsEnabledByDefault.config");
            Assert.IsTrue(sut.RequireHttps);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void PublicOriginIsNotUri()
        {
            SetupSut("PublicOriginIsNotUri.config");
        }

        [TestMethod]
        public void PublicOriginIsNullByDefault()
        {
            SetupSut("PublicOriginIsNullByDefault.config");
            Assert.IsNull(sut.PublicOrigin);
        }
    }
}