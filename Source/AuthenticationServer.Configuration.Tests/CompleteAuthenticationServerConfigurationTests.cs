using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Affecto.AuthenticationServer.Configuration.Tests
{
    [TestClass]
    public class CompleteAuthenticationServerConfigurationTests : ConfigurationTestsBase
    {
        [TestInitialize]
        public void Setup()
        {
            SetupAuthenticationServerConfiguration("ValidConfiguration.config");
        }

        [TestMethod]
        public void AllowedOriginsAreRetrieved()
        {
            Assert.AreEqual(2, authenticationServerConfiguration.AllowedOrigins.Count);
            Assert.IsTrue(authenticationServerConfiguration.AllowedOrigins.Contains("http://server.com"));
            Assert.IsTrue(authenticationServerConfiguration.AllowedOrigins.Contains("https://secureserver.com"));
            Assert.IsTrue(authenticationServerConfiguration.IsAllowedOrigin("http://server.com"));
            Assert.IsTrue(authenticationServerConfiguration.IsAllowedOrigin("https://secureserver.com"));
        }

        [TestMethod]
        public void NotAllowedOrigin()
        {
            Assert.IsFalse(authenticationServerConfiguration.IsAllowedOrigin("http://server2.com"));
        }

        [TestMethod]
        public void RequireHttpsIsRetrieved()
        {
            bool result = authenticationServerConfiguration.RequireHttps;
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ServiceUserNameIsRetrieved()
        {
            string result = authenticationServerConfiguration.ServiceUserName;
            Assert.AreEqual("VIRTA autentikointipalvelu", result);
        }

        [TestMethod]
        public void SigningCertificateThumbprintIsRetrieved()
        {
            string result = authenticationServerConfiguration.SigningCertificateThumbprint;
            Assert.AreEqual("thumb", result);
        }

        [TestMethod]
        public void SecondarySigningCertificateThumbprintIsRetrieved()
        {
            string result = authenticationServerConfiguration.SecondarySigningCertificateThumbprint;
            Assert.AreEqual("secondary thumb", result);
        }

        [TestMethod]
        public void PublicOriginIsRetrieved()
        {
            Uri result = authenticationServerConfiguration.PublicOrigin;
            Assert.AreEqual("http://someurl.com:12837/dir/", result.ToString());
        }

        [TestMethod]
        public void ScopesAreRetrieved()
        {
            Assert.AreEqual(1, authenticationServerConfiguration.Scopes.Count);
            IScope scope = authenticationServerConfiguration.Scopes.Single();

            Assert.IsNotNull(scope);
            Assert.AreEqual("FirstScope", scope.Name);
            Assert.AreEqual("First Scope", scope.DisplayName);
            Assert.IsTrue(scope.IncludeAllClaimsForUser);
        }

        [TestMethod]
        public void ClientsAreRetrieved()
        {
            Assert.AreEqual(1, authenticationServerConfiguration.Clients.Count);
            IClient client = authenticationServerConfiguration.Clients.Single();

            Assert.IsNotNull(client);
            Assert.AreEqual("SomeClient", client.Id);
            Assert.AreEqual("Some Client", client.Name);
            Assert.AreEqual("9809DBC8-E72B-47EC-BE4F-42122C2965E1", client.Secret);
            Assert.AreEqual(Flow.ResourceOwner, client.Flow);
            Assert.AreEqual(new TimeSpan(23, 59, 59), client.AccessTokenLifetime);
            Assert.AreEqual(new Uri("http://localhost:49612/"), client.RedirectUri);

            Assert.AreEqual(1, client.AllowedScopes.Count);
            string allowedScope = client.AllowedScopes.Single();
            Assert.AreEqual("FirstScope", allowedScope);
        }
    }
}