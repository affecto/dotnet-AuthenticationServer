using System;
using System.IO;
using Affecto.AuthenticationServer.Plugins.Infrastructure.Configuration;
using Affecto.Testing.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AuthenticationServer.Plugins.Infrastructure.Tests.Configuration
{
    [DeploymentItem("Configuration\\ConfigFiles", "Plugins.Infrastructure.Tests.Configuration")]
    public abstract class ConfigurationTestsBase
    {
        protected IFederatedAuthenticationConfiguration federatedAuthenticationConfiguration;

        private readonly ConfigSectionReader configSectionReader =
            new ConfigSectionReader(Path.Combine(Environment.CurrentDirectory, "Plugins.Infrastructure.Tests.Configuration"));
        

        protected void SetupFederatedAuthenticationConfiguration(string configFileName)
        {
            federatedAuthenticationConfiguration = configSectionReader.GetConfigSection<FederatedAuthenticationConfiguration>(configFileName,
                "federatedAuthentication");
        }
    }
}