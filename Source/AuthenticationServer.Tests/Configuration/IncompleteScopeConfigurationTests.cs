using System.Configuration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Affecto.AuthenticationServer.Tests.Configuration
{
    [TestClass]
    public class IncompleteScopeConfigurationTests : ConfigurationTestsBase
    {
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ScopeNameCanNotBeEmpty()
        {
            SetupSut("ScopeNameEmpty.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ScopeNameCanNotBeMissing()
        {
            SetupSut("ScopeNameMissing.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ScopeDisplayNameCanNotBeEmpty()
        {
            SetupSut("ScopeDisplayNameEmpty.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ScopeDisplayNameCanNotBeMissing()
        {
            SetupSut("ScopeDisplayNameMissing.config");
        }

        [TestMethod]
        public void ScopeIsEnabledByDefault()
        {
            SetupSut("ScopeIsEnabledByDefault.config");
            Assert.IsTrue(sut.Scopes.Single().Enabled);
        }

        [TestMethod]
        public void ScopeDoesNotIncludeAllClaimsForUserByDefault()
        {
            SetupSut("ScopeDoesNotIncludeAllClaimsForUserByDefault.config");
            Assert.IsFalse(sut.Scopes.Single().IncludeAllClaimsForUser);
        }
    }
}