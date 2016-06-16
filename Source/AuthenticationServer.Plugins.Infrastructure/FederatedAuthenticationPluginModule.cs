using Affecto.AuthenticationServer.Plugins.Infrastructure.Configuration;
using Autofac;

namespace Affecto.AuthenticationServer.Plugins.Infrastructure
{
    public abstract class FederatedAuthenticationPluginModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.Register(CreateFederatedAuthenticationConfiguration).SingleInstance().As<IFederatedAuthenticationConfiguration>();
        }

        private static IFederatedAuthenticationConfiguration CreateFederatedAuthenticationConfiguration(IComponentContext componentContext)
        {
            return FederatedAuthenticationConfiguration.Settings;
        }
    }
}