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
        public void IssuerIsRetrieved()
        {
            Uri result = authenticationServerConfiguration.Issuer;
            Assert.AreEqual("http://someurl.com:12837/core/", result.ToString());
        }

        [TestMethod]
        public void ScopesAreRetrieved()
        {
            Assert.AreEqual(2, authenticationServerConfiguration.Scopes.Count);
            IScope firstScope = authenticationServerConfiguration.Scopes.First();

            Assert.IsNotNull(firstScope);
            Assert.AreEqual("FirstScope", firstScope.Name);
            Assert.AreEqual("First Scope", firstScope.DisplayName);
            Assert.IsTrue(firstScope.IncludeAllClaimsForUser);

            Assert.IsNotNull(firstScope.ScopeSecrets);
            Assert.AreEqual(2, firstScope.ScopeSecrets.Count);
            Assert.AreEqual("SomeSecret", firstScope.ScopeSecrets.First());
            Assert.AreEqual("AnotherSecret", firstScope.ScopeSecrets.Last());

            IScope secondScope = authenticationServerConfiguration.Scopes.Last();

            Assert.IsNotNull(secondScope);
            Assert.AreEqual("SecondScope", secondScope.Name);
            Assert.AreEqual("Second Scope", secondScope.DisplayName);
            Assert.IsFalse(secondScope.IncludeAllClaimsForUser);

            Assert.IsNotNull(secondScope.ScopeSecrets);
            Assert.AreEqual(0, secondScope.ScopeSecrets.Count);
        }

        [TestMethod]
        public void ClientsAreRetrieved()
        {
            Assert.AreEqual(2, authenticationServerConfiguration.Clients.Count);
            IClient referenceClient = authenticationServerConfiguration.Clients.First();

            Assert.IsNotNull(referenceClient);
            Assert.AreEqual("ReferenceClient", referenceClient.Id);
            Assert.AreEqual("Reference Client", referenceClient.Name);
            Assert.AreEqual("9809DBC8-E72B-47EC-BE4F-42122C2965E1", referenceClient.Secret);
            Assert.AreEqual(Flow.ResourceOwner, referenceClient.Flow);
            Assert.AreEqual(AccessTokenType.Reference, referenceClient.AccessTokenType);
            Assert.AreEqual(new TimeSpan(23, 59, 59), referenceClient.AccessTokenLifetime);
            Assert.AreEqual(new Uri("http://localhost:49612/"), referenceClient.RedirectUri);

            Assert.AreEqual(1, referenceClient.AllowedScopes.Count);
            string allowedScope = referenceClient.AllowedScopes.Single();
            Assert.AreEqual("FirstScope", allowedScope);

            IClient jwtClient = authenticationServerConfiguration.Clients.Last();

            Assert.IsNotNull(jwtClient);
            Assert.AreEqual("JwtClient", jwtClient.Id);
            Assert.AreEqual("Jwt Client", jwtClient.Name);
            Assert.AreEqual("9809DBC8-E72B-47EC-BE4F-42122C2965E1", jwtClient.Secret);
            Assert.AreEqual(Flow.Implicit, jwtClient.Flow);
            Assert.AreEqual(AccessTokenType.Jwt, jwtClient.AccessTokenType);
            Assert.AreEqual(new TimeSpan(0, 10, 0), jwtClient.AccessTokenLifetime);
            Assert.AreEqual(new Uri("http://localhost:49613/"), jwtClient.RedirectUri);

            Assert.AreEqual(1, jwtClient.AllowedScopes.Count);
            allowedScope = jwtClient.AllowedScopes.Single();
            Assert.AreEqual("SecondScope", allowedScope);
        }
    }
}