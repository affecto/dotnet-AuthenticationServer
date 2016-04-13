using System.Configuration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Affecto.AuthenticationServer.Configuration.Tests
{
    [TestClass]
    public class IncompleteClientConfigurationTests : ConfigurationTestsBase
    {
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ClientIdCanNotBeEmpty()
        {
            SetupAuthenticationServerConfiguration("ClientIdEmpty.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ClientIdCanNotBeMissing()
        {
            SetupAuthenticationServerConfiguration("ClientIdMissing.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ClientNameCanNotBeEmpty()
        {
            SetupAuthenticationServerConfiguration("ClientNameEmpty.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ClientNameCanNotBeMissing()
        {
            SetupAuthenticationServerConfiguration("ClientNameMissing.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ClientSecretCanNotBeEmpty()
        {
            SetupAuthenticationServerConfiguration("ClientSecretEmpty.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ClientSecretCanNotBeMissing()
        {
            SetupAuthenticationServerConfiguration("ClientSecretMissing.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ClientFlowCanNotBeEmpty()
        {
            SetupAuthenticationServerConfiguration("ClientFlowEmpty.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ClientFlowCanNotBeMissing()
        {
            SetupAuthenticationServerConfiguration("ClientFlowMissing.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ClientAccessTokenLifetimeCanNotBeEmpty()
        {
            SetupAuthenticationServerConfiguration("ClientAccessTokenLifetimeEmpty.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ClientAccessTokenLifetimeCanNotBeMissing()
        {
            SetupAuthenticationServerConfiguration("ClientAccessTokenLifetimeMissing.config");
        }

        [TestMethod]
        public void ClientIsEnabledByDefault()
        {
            SetupAuthenticationServerConfiguration("ClientIsEnabledByDefault.config");
            Assert.IsTrue(authenticationServerConfiguration.Clients.Single().Enabled);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ClientAllowedScopeNameCanNotBeEmpty()
        {
            SetupAuthenticationServerConfiguration("ClientAllowedScopeNameEmpty.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ClientAllowedScopeNameCanNotBeMissing()
        {
            SetupAuthenticationServerConfiguration("ClientAllowedScopeNameMissing.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ClientAllowedScopeNameMustMatchToConfiguredScopes()
        {
            SetupAuthenticationServerConfiguration("ClientAllowedScopeNameDoesNotMatch.config");
        }
    }
}