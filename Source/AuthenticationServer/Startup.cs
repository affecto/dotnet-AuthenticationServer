using System;
using System.Collections.Generic;
using Affecto.AuthenticationServer.Configuration;
using Affecto.Logging;
using Autofac;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using Owin;
using System.Linq;

namespace Affecto.AuthenticationServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AuthenticationServerModule>();
            IContainer container = builder.Build();

            ILoggerFactory loggerFactory = container.Resolve<ILoggerFactory>();
            ILogger logger = loggerFactory.CreateLogger(this);

            logger.LogVerbose("Initializing AuthenticationServer. This is required for IdentityServer logging to work. Do not remove this!");

            app.Map("/core", coreApp =>
            {
                IAuthenticationServerConfiguration configuration = container.Resolve<IAuthenticationServerConfiguration>();

                var serviceFactory = new IdentityServerServiceFactory()
                    .UseInMemoryClients(configuration.Clients.MapToIdentityServerClients())
                    .UseInMemoryScopes(StandardScopes.All.Concat(configuration.Scopes.MapToIdentityServerScopes()));

                serviceFactory.UserService = new Registration<IUserService>(resolver => container.Resolve<IUserService>());
                serviceFactory.CorsPolicyService = new Registration<ICorsPolicyService>(CorsPolicyServiceFactory.Create(configuration));

                if (container.IsRegistered<ITokenService>())
                {
                    serviceFactory.TokenService =
                        new Registration<ITokenService>(resolver => container.Resolve<ITokenService>(new PositionalParameter(0, resolver)));
                }

                IEnumerable<Func<ICustomGrantValidator>> customGrantValidatorFactories = container.Resolve<IEnumerable<Func<ICustomGrantValidator>>>();
                foreach (Func<ICustomGrantValidator> validatorFactory in customGrantValidatorFactories)
                {
                    serviceFactory.CustomGrantValidators.Add(new Registration<ICustomGrantValidator>(resolver => validatorFactory()));
                }

                coreApp.UseIdentityServer(IdentityServerOptionsFactory.Create(configuration, serviceFactory));
            });
        }
    }
}