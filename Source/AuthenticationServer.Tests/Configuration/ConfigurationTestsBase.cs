using System;
using Affecto.AuthenticationServer.Configuration;
using Affecto.Testing.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Affecto.AuthenticationServer.Tests.Configuration
{
    [DeploymentItem("Configuration\\ConfigFiles")]
    public abstract class ConfigurationTestsBase
    {
        private readonly ConfigSectionReader configSectionReader = new ConfigSectionReader(Environment.CurrentDirectory);
        protected IAuthenticationServerConfiguration sut;

        protected void SetupSut(string configFileName)
        {
            sut = configSectionReader.GetConfigSection<AuthenticationServerConfiguration>(configFileName, "authenticationServer");
        }
    }
}