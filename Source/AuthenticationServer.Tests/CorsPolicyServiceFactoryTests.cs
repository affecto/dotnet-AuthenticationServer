using System;
using System.Collections.Generic;
using System.Linq;
using Affecto.AuthenticationServer.Configuration;
using IdentityServer3.Core.Services.Default;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Affecto.AuthenticationServer.Tests
{
    [TestClass]
    public class CorsPolicyServiceFactoryTests
    {
        private IAuthenticationServerConfiguration configuration;

        [TestInitialize]
        public void Setup()
        {
            configuration = Substitute.For<IAuthenticationServerConfiguration>();
            configuration.AllowedOrigins.Returns(new List<string> { "origin", "otherOrigin" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConfigurationCannotBeNull()
        {
            CorsPolicyServiceFactory.Create(null);
        }

        [TestMethod]
        public void AllOriginsAreAllowedByStarInConfiguration()
        {
            configuration.IsAllowedOrigin("*").Returns(true);
            DefaultCorsPolicyService service = CorsPolicyServiceFactory.Create(configuration);

            Assert.IsTrue(service.AllowAll);
        }

        [TestMethod]
        public void OnlyOriginsListedInConfigurationAreAllowed()
        {
            configuration.IsAllowedOrigin("*").Returns(false);
            DefaultCorsPolicyService service = CorsPolicyServiceFactory.Create(configuration);

            Assert.AreEqual(2, service.AllowedOrigins.Count);
            Assert.IsTrue(service.AllowedOrigins.Any(origin => origin == "origin"));
            Assert.IsTrue(service.AllowedOrigins.Any(origin => origin == "otherOrigin"));
        }
    }
}