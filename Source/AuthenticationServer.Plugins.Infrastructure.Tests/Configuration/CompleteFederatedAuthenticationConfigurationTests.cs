using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AuthenticationServer.Plugins.Infrastructure.Tests.Configuration
{
    [TestClass]
    public class CompleteFederatedAuthenticationConfigurationTests : ConfigurationTestsBase
    {
        [TestMethod]
        public void UserAccountNameClaimIsRetrieved()
        {
            SetupFederatedAuthenticationConfiguration("ValidConfiguration.config");
            Assert.AreEqual("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", federatedAuthenticationConfiguration.UserAccountNameClaim);
        }

        [TestMethod]
        public void UserDisplayNameClaimIsRetrieved()
        {
            SetupFederatedAuthenticationConfiguration("ValidConfiguration.config");
            Assert.AreEqual("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", federatedAuthenticationConfiguration.UserDisplayNameClaim);
        }

        [TestMethod]
        public void GroupsClaimIsRetrieved()
        {
            SetupFederatedAuthenticationConfiguration("ValidConfiguration.config");
            Assert.AreEqual("http://affecto.com/claims/group", federatedAuthenticationConfiguration.GroupsClaim);
        }

        [TestMethod]
        public void GroupsClaimIsEmpty()
        {
            SetupFederatedAuthenticationConfiguration("EmptyFederatedGroupClaim.config");
            Assert.AreEqual(string.Empty, federatedAuthenticationConfiguration.GroupsClaim);
        }

        [TestMethod]
        public void GroupsClaimIsMissing()
        {
            SetupFederatedAuthenticationConfiguration("MissingFederatedGroupClaim.config");
            Assert.AreEqual(string.Empty, federatedAuthenticationConfiguration.GroupsClaim);
        }
    }
}