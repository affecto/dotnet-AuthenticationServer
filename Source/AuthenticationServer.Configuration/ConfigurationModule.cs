using Autofac;

namespace Affecto.AuthenticationServer.Configuration
{
    public class ConfigurationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterInstance(AuthenticationServerConfiguration.Settings).As<IAuthenticationServerConfiguration>();
            builder.RegisterInstance(FederatedAuthenticationConfiguration.Settings).As<IFederatedAuthenticationConfiguration>();
        }
    }
}
