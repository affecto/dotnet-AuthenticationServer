namespace Affecto.AuthenticationServer.Configuration
{
    public interface ICustomIdentityProvider
    {
        string ModulePath { get; }
        string AuthenticationType { get; }
        string Caption { get; }
        string EntityId { get; }
        string MetadataLocation { get; }

        ISigningCertificate SigningCertificate { get; }


    }
}