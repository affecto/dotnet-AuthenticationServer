using System.Configuration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Affecto.AuthenticationServer.Tests.Configuration
{
    [TestClass]
    public class IncompleteClientConfigurationTests : ConfigurationTestsBase
    {
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ClientIdCanNotBeEmpty()
        {
            SetupSut("ClientIdEmpty.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ClientIdCanNotBeMissing()
        {
            SetupSut("ClientIdMissing.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ClientNameCanNotBeEmpty()
        {
            SetupSut("ClientNameEmpty.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ClientNameCanNotBeMissing()
        {
            SetupSut("ClientNameMissing.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ClientSecretCanNotBeEmpty()
        {
            SetupSut("ClientSecretEmpty.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ClientSecretCanNotBeMissing()
        {
            SetupSut("ClientSecretMissing.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ClientFlowCanNotBeEmpty()
        {
            SetupSut("ClientFlowEmpty.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ClientFlowCanNotBeMissing()
        {
            SetupSut("ClientFlowMissing.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ClientAccessTokenLifetimeCanNotBeEmpty()
        {
            SetupSut("ClientAccessTokenLifetimeEmpty.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ClientAccessTokenLifetimeCanNotBeMissing()
        {
            SetupSut("ClientAccessTokenLifetimeMissing.config");
        }

        [TestMethod]
        public void ClientIsEnabledByDefault()
        {
            SetupSut("ClientIsEnabledByDefault.config");
            Assert.IsTrue(sut.Clients.Single().Enabled);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ClientAllowedScopeNameCanNotBeEmpty()
        {
            SetupSut("ClientAllowedScopeNameEmpty.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ClientAllowedScopeNameCanNotBeMissing()
        {
            SetupSut("ClientAllowedScopeNameMissing.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ClientAllowedScopeNameMustMatchToConfiguredScopes()
        {
            SetupSut("ClientAllowedScopeNameDoesNotMatch.config");
        }
    }
}