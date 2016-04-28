using System.Configuration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Affecto.AuthenticationServer.Configuration.Tests
{
    [TestClass]
    public class IncompleteScopeConfigurationTests : ConfigurationTestsBase
    {
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ScopeNameCanNotBeEmpty()
        {
            SetupAuthenticationServerConfiguration("ScopeNameEmpty.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ScopeNameCanNotBeMissing()
        {
            SetupAuthenticationServerConfiguration("ScopeNameMissing.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ScopeDisplayNameCanNotBeEmpty()
        {
            SetupAuthenticationServerConfiguration("ScopeDisplayNameEmpty.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ScopeDisplayNameCanNotBeMissing()
        {
            SetupAuthenticationServerConfiguration("ScopeDisplayNameMissing.config");
        }

        [TestMethod]
        public void ScopeDoesNotIncludeAllClaimsForUserByDefault()
        {
            SetupAuthenticationServerConfiguration("ScopeDoesNotIncludeAllClaimsForUserByDefault.config");
            Assert.IsFalse(authenticationServerConfiguration.Scopes.Single().IncludeAllClaimsForUser);
        }
    }
}