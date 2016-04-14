namespace Affecto.AuthenticationServer.IdentityManagement.Configuration
{
    public interface IFederatedAuthenticationConfiguration
    {
        string UserAccountNameClaim { get; set; }
        string UserDisplayNameClaim { get; set; }
        string GroupsClaim { get; set; }
    }
}
