using System.Security.Claims;
using Affecto.Authentication.Claims;
using Affecto.AuthenticationServer.Configuration;
using Affecto.Logging;
using Affecto.Logging.Log4Net;
using Autofac;
using Autofac.Configuration;

namespace Affecto.AuthenticationServer
{
    public class AuthenticationServerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new ConfigurationSettingsReader());
            builder.RegisterType<Log4NetLoggerFactory>().As<ILoggerFactory>();
            builder.RegisterInstance(AuthenticationServerConfiguration.Settings).As<IAuthenticationServerConfiguration>();
            builder.Register(CreateAuthenticatedUserContext).SingleInstance().As<IAuthenticatedUserContext>();
            builder.RegisterInstance(KentorAuthServicesCustomProvidersConfiguration.Settings).As<IKentorAuthServicesCustomProvidersConfiguration>();
        }

        private static IAuthenticatedUserContext CreateAuthenticatedUserContext(IComponentContext componentContext)
        {
            var identity = new ClaimsIdentity();
            IAuthenticationServerConfiguration configuration = componentContext.Resolve<IAuthenticationServerConfiguration>();

            identity.AddClaim(new Claim(ClaimType.Name, configuration.ServiceUserName));
            identity.AddClaim(new Claim(ClaimType.IsSystemUser, bool.TrueString, ClaimValueTypes.Boolean));

            return new AuthenticatedUserContext(identity);
        }
    }
}