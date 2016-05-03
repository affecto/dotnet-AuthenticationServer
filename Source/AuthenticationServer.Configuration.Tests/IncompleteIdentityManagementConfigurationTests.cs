using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Affecto.AuthenticationServer.Configuration.Tests
{
    [TestClass]
    public class IncompleteIdentityManagementConfigurationTests : ConfigurationTestsBase
    {
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void MissingNewUserCustomPropertyName()
        {
            SetupIdentityManagementConfiguration("MissingIdentityManagementNewUserPropertyName.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void MissingNewUserCustomPropertyValue()
        {
            SetupIdentityManagementConfiguration("MissingIdentityManagementNewUserPropertyValue.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void EmptyNewUserCustomPropertyName()
        {
            SetupIdentityManagementConfiguration("EmptyIdentityManagementNewUserPropertyName.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void EmptyNewUserCustomPropertyValue()
        {
            SetupIdentityManagementConfiguration("EmptyIdentityManagementNewUserPropertyValue.config");
        }
    }
}
