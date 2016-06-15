using System;
using Affecto.AuthenticationServer.Plugins.Infrastructure.Configuration;
using Affecto.Testing.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AuthenticationServer.Plugins.Infrastructure.Tests.Configuration
{
    [DeploymentItem("Configuration\\ConfigFiles")]
    public abstract class ConfigurationTestsBase
    {
        private readonly ConfigSectionReader configSectionReader = new ConfigSectionReader(Environment.CurrentDirectory);
        protected IFederatedAuthenticationConfiguration federatedAuthenticationConfiguration;

        protected void SetupFederatedAuthenticationConfiguration(string configFileName)
        {
            federatedAuthenticationConfiguration = configSectionReader.GetConfigSection<FederatedAuthenticationConfiguration>(configFileName, "federatedAuthentication");
        }
    }
}