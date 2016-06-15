namespace Affecto.AuthenticationServer.Plugins.Infrastructure.Configuration
{
    public interface IFederatedAuthenticationConfiguration
    {
        string UserAccountNameClaim { get; set; }
        string UserDisplayNameClaim { get; set; }
        string GroupsClaim { get; set; }
    }
}