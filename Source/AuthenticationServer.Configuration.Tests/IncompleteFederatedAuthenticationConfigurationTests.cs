using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Affecto.AuthenticationServer.Configuration.Tests
{
    [TestClass]
    public class IncompleteFederatedAuthenticationConfigurationTests : ConfigurationTestsBase
    {
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void EmptyUserAccountNameClaim()
        {
            SetupFederatedAuthenticationConfiguration("EmptyFederatedUserAccountNameClaim.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void EmptyUserDisplayNameClaim()
        {
            SetupFederatedAuthenticationConfiguration("EmptyFederatedUserDisplayNameClaim.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void MissingUserAccountNameClaim()
        {
            SetupFederatedAuthenticationConfiguration("MissingFederatedUserAccountNameClaim.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void MissingUserDisplayNameClaim()
        {
            SetupFederatedAuthenticationConfiguration("MissingFederatedUserDisplayNameClaim.config");
        }
    }
}
