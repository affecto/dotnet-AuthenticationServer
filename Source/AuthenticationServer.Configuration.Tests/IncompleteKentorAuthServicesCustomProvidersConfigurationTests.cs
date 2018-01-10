using System.Configuration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Affecto.AuthenticationServer.Configuration.Tests
{
    [TestClass]
    public class IncompleteKentorAuthServicesCustomProvidersConfigurationTests : ConfigurationTestsBase
    {
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void EntityIdCannotBeMissing()
        {
            SetupKentorAuthServicesCustomProvidersConfiguration("KentorCustomProvidersEntityIdMissing.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void EntityIdCannotBeEmpty()
        {
            SetupKentorAuthServicesCustomProvidersConfiguration("KentorCustomProvidersEntityIdEmpty.config");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void IdpCannotBeMissing()
        {
            SetupKentorAuthServicesCustomProvidersConfiguration("KentorCustomProvidersIdpMissing.config");            
        }

    //    [TestMethod]
    //    [ExpectedException(typeof(ConfigurationErrorsException))]
    //    public void ClientNameCanNotBeEmpty()
    //    {
    //        SetupKentorAuthServicesCustomProvidersConfiguration("ClientNameEmpty.config");
    //    }

    //    [TestMethod]
    //    [ExpectedException(typeof(ConfigurationErrorsException))]
    //    public void ClientNameCanNotBeMissing()
    //    {
    //        SetupKentorAuthServicesCustomProvidersConfiguration("ClientNameMissing.config");
    //    }

    //    [TestMethod]
    //    [ExpectedException(typeof(ConfigurationErrorsException))]
    //    public void ClientSecretCanNotBeEmpty()
    //    {
    //        SetupKentorAuthServicesCustomProvidersConfiguration("ClientSecretEmpty.config");
    //    }

    //    [TestMethod]
    //    [ExpectedException(typeof(ConfigurationErrorsException))]
    //    public void ClientSecretCanNotBeMissing()
    //    {
    //        SetupKentorAuthServicesCustomProvidersConfiguration("ClientSecretMissing.config");
    //    }

    //    [TestMethod]
    //    [ExpectedException(typeof(ConfigurationErrorsException))]
    //    public void ClientFlowCanNotBeEmpty()
    //    {
    //        SetupKentorAuthServicesCustomProvidersConfiguration("ClientFlowEmpty.config");
    //    }

    //    [TestMethod]
    //    [ExpectedException(typeof(ConfigurationErrorsException))]
    //    public void ClientFlowCanNotBeMissing()
    //    {
    //        SetupKentorAuthServicesCustomProvidersConfiguration("ClientFlowMissing.config");
    //    }

    //    [TestMethod]
    //    [ExpectedException(typeof(ConfigurationErrorsException))]
    //    public void ClientAccessTokenLifetimeCanNotBeEmpty()
    //    {
    //        SetupKentorAuthServicesCustomProvidersConfiguration("ClientAccessTokenLifetimeEmpty.config");
    //    }

    //    [TestMethod]
    //    [ExpectedException(typeof(ConfigurationErrorsException))]
    //    public void ClientAccessTokenLifetimeCanNotBeMissing()
    //    {
    //        SetupKentorAuthServicesCustomProvidersConfiguration("ClientAccessTokenLifetimeMissing.config");
    //    }

    //    [TestMethod]
    //    [ExpectedException(typeof(ConfigurationErrorsException))]
    //    public void ClientAllowedScopeNameCanNotBeEmpty()
    //    {
    //        SetupKentorAuthServicesCustomProvidersConfiguration("ClientAllowedScopeNameEmpty.config");
    //    }

    //    [TestMethod]
    //    [ExpectedException(typeof(ConfigurationErrorsException))]
    //    public void ClientAllowedScopeNameCanNotBeMissing()
    //    {
    //        SetupKentorAuthServicesCustomProvidersConfiguration("ClientAllowedScopeNameMissing.config");
    //    }

    //    [TestMethod]
    //    [ExpectedException(typeof(ConfigurationErrorsException))]
    //    public void ClientAllowedScopeNameMustMatchToConfiguredScopes()
    //    {
    //        SetupKentorAuthServicesCustomProvidersConfiguration("ClientAllowedScopeNameDoesNotMatch.config");
    //    }

    //    [TestMethod]
    //    public void ClientRedirectUriMissing()
    //    {
    //        SetupKentorAuthServicesCustomProvidersConfiguration("ClientRedirectUriMissing.config");

    //        ICustomIdentityProvider idp = kentorAuthServicesCustomProvidersConfiguration.IdentityProviders.SingleOrDefault();
    //        Assert.IsNull(idp);
    //    }

    //    [TestMethod]
    //    public void ClientRedirectUriEmpty()
    //    {
    //        SetupKentorAuthServicesCustomProvidersConfiguration("ClientRedirectUriEmpty.config");

    //        IClient client = authenticationServerConfiguration.Clients.Single();
    //        Assert.IsNull(client.RedirectUri);
    //    }

    //    [TestMethod]
    //    [ExpectedException(typeof(ConfigurationErrorsException))]
    //    public void ClientRedirectUriInvalid()
    //    {
    //        SetupKentorAuthServicesCustomProvidersConfiguration("ClientRedirectUriInvalid.config");
    //    }



    }
}