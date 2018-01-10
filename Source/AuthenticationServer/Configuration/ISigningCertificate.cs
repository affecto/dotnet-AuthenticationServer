namespace Affecto.AuthenticationServer.Configuration
{
    public interface ISigningCertificate
    {
        string StoreName { get; }
        string StoreLocation { get; }
        string FindValue { get; }
        string X509FindType { get; }
        
    }
}