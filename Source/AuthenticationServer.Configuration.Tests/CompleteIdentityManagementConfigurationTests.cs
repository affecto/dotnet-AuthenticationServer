using System.Collections.Generic;
using System.Linq;
using Affecto.AuthenticationServer.IdentityManagement.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Affecto.AuthenticationServer.Configuration.Tests
{
    [TestClass]
    public class CompleteIdentityManagementConfigurationTests : ConfigurationTestsBase
    {
        [TestInitialize]
        public void Setup()
        {
            SetupIdentityManagementConfiguration("ValidConfiguration.config");
        }

        [TestMethod]
        public void AutoCreateUserIsRetrieved()
        {
            bool result = identityManagementConfiguration.AutoCreateUser;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void NewUserCustomPropertiesAreRetrieved()
        {
            IReadOnlyCollection<ICustomProperty> properties = identityManagementConfiguration.NewUserCustomProperties;
            Assert.AreEqual(2, properties.Count);
            Assert.IsTrue(properties.Any(p => p.Name.Equals("BusinessId") && p.Value.Equals("1234567-0")));
            Assert.IsTrue(properties.Any(p => p.Name.Equals("OrganizationId") && p.Value.Equals("7b45e3bc-eda9-4f6b-97bb-e9354db660b5")));
        }
    }
}