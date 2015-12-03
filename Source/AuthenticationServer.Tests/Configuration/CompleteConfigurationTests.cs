using System;
using System.Linq;
using Affecto.AuthenticationServer.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Affecto.AuthenticationServer.Tests.Configuration
{
    [TestClass]
    public class CompleteConfigurationTests : ConfigurationTestsBase
    {
        [TestInitialize]
        public void Setup()
        {
            SetupSut("ValidConfiguration.config");
        }

        [TestMethod]
        public void AllowedOriginsAreRetrieved()
        {
            Assert.AreEqual(2, sut.AllowedOrigins.Count);
            Assert.IsTrue(sut.AllowedOrigins.Contains("http://server.com"));
            Assert.IsTrue(sut.AllowedOrigins.Contains("https://secureserver.com"));
            Assert.IsTrue(sut.IsAllowedOrigin("http://server.com"));
            Assert.IsTrue(sut.IsAllowedOrigin("https://secureserver.com"));
        }

        [TestMethod]
        public void NotAllowedOrigin()
        {
            Assert.IsFalse(sut.IsAllowedOrigin("http://server2.com"));
        }

        [TestMethod]
        public void RequireHttpsIsRetrieved()
        {
            bool result = sut.RequireHttps;
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ServiceUserNameIsRetrieved()
        {
            string result = sut.ServiceUserName;
            Assert.AreEqual("VIRTA autentikointipalvelu", result);
        }

        [TestMethod]
        public void AutoCreateUserIsRetrieved()
        {
            bool result = sut.AutoCreateUser;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SigningCertificateThumbprintIsRetrieved()
        {
            string result = sut.SigningCertificateThumbprint;
            Assert.AreEqual("thumb", result);
        }

        [TestMethod]
        public void SecondarySigningCertificateThumbprintIsRetrieved()
        {
            string result = sut.SecondarySigningCertificateThumbprint;
            Assert.AreEqual("secondary thumb", result);
        }

        [TestMethod]
        public void PublicOriginIsRetrieved()
        {
            Uri result = sut.PublicOrigin;
            Assert.AreEqual("http://someurl.com:12837/dir/", result.ToString());
        }

        [TestMethod]
        public void ScopesAreRetrieved()
        {
            Assert.AreEqual(1, sut.Scopes.Count);
            IScope scope = sut.Scopes.Single();

            Assert.IsNotNull(scope);
            Assert.AreEqual("FirstScope", scope.Name);
            Assert.AreEqual("First Scope", scope.DisplayName);
            Assert.IsTrue(scope.IncludeAllClaimsForUser);
            Assert.IsFalse(scope.Enabled);
        }

        [TestMethod]
        public void ClientsAreRetrieved()
        {
            Assert.AreEqual(1, sut.Clients.Count);
            IClient client = sut.Clients.Single();

            Assert.IsNotNull(client);
            Assert.AreEqual("SomeClient", client.Id);
            Assert.AreEqual("Some Client", client.Name);
            Assert.AreEqual("9809DBC8-E72B-47EC-BE4F-42122C2965E1", client.Secret);
            Assert.AreEqual(Flow.ResourceOwner, client.Flow);
            Assert.AreEqual(new TimeSpan(23, 59, 59), client.AccessTokenLifetime);
            Assert.IsFalse(client.Enabled);

            Assert.AreEqual(1, client.AllowedScopes.Count);
            string allowedScope = client.AllowedScopes.Single();
            Assert.AreEqual("FirstScope", allowedScope);
        }
    }
}