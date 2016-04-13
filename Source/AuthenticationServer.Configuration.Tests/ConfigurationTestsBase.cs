using System;
using Affecto.Testing.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Affecto.AuthenticationServer.Configuration.Tests
{
    [DeploymentItem("ConfigFiles")]
    public abstract class ConfigurationTestsBase
    {
        private readonly ConfigSectionReader configSectionReader = new ConfigSectionReader(Environment.CurrentDirectory);
        protected IAuthenticationServerConfiguration authenticationServerConfiguration;
        protected IFederatedAuthenticationConfiguration federatedAuthenticationConfiguration;

        protected void SetupAuthenticationServerConfiguration(string configFileName)
        {
            authenticationServerConfiguration = configSectionReader.GetConfigSection<AuthenticationServerConfiguration>(configFileName, "authenticationServer");
        }

        protected void SetupFederatedAuthenticationConfiguration(string configFileName)
        {
            federatedAuthenticationConfiguration = configSectionReader.GetConfigSection<FederatedAuthenticationConfiguration>(configFileName, "federatedAuthentication");
        }
    }
}