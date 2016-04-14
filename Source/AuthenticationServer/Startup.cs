using Affecto.AuthenticationServer.Configuration;
using Affecto.AuthenticationServer.IdentityManagement;
using Affecto.Logging;
using Autofac;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using Owin;

namespace Affecto.AuthenticationServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AuthenticationServerModule>();
            builder.RegisterModule<ConfigurationModule>();
            builder.RegisterModule<IdentityManagementModule>();
            IContainer container = builder.Build();

            ILoggerFactory loggerFactory = container.Resolve<ILoggerFactory>();
            ILogger logger = loggerFactory.CreateLogger(this);

            logger.LogVerbose("Initializing AuthenticationServer. This is required for IdentityServer logging to work. Do not remove this!");

            app.Map("/core", coreApp =>
            {
                IAuthenticationServerConfiguration configuration = container.Resolve<IAuthenticationServerConfiguration>();

                var serviceFactory = new IdentityServerServiceFactory()
                    .UseInMemoryClients(configuration.Clients.MapToIdentityServerClients())
                    .UseInMemoryScopes(configuration.Scopes.MapToIdentityServerScopes());

                serviceFactory.UserService = new Registration<IUserService>(resolver => container.Resolve<IUserService>());
                serviceFactory.CorsPolicyService = new Registration<ICorsPolicyService>(CorsPolicyServiceFactory.Create(configuration));

                coreApp.UseIdentityServer(IdentityServerOptionsFactory.Create(configuration, serviceFactory));
            });
        }
    }
}