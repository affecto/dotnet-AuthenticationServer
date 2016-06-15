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

        protected void SetupAuthenticationServerConfiguration(string configFileName)
        {
            authenticationServerConfiguration = configSectionReader.GetConfigSection<AuthenticationServerConfiguration>(configFileName, "authenticationServer");
        }
    }
}