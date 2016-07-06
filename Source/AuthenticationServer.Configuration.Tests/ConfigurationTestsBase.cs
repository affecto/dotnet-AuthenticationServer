using System;
using System.IO;
using Affecto.Testing.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Affecto.AuthenticationServer.Configuration.Tests
{
    [DeploymentItem("ConfigFiles", "AuthenticationServer.Configuration.Tests")]
    public abstract class ConfigurationTestsBase
    {
        protected IAuthenticationServerConfiguration authenticationServerConfiguration;

        private readonly ConfigSectionReader configSectionReader =
            new ConfigSectionReader(Path.Combine(Environment.CurrentDirectory, "AuthenticationServer.Configuration.Tests"));

        protected void SetupAuthenticationServerConfiguration(string configFileName)
        {
            authenticationServerConfiguration = configSectionReader.GetConfigSection<AuthenticationServerConfiguration>(configFileName, "authenticationServer");
        }
    }
}