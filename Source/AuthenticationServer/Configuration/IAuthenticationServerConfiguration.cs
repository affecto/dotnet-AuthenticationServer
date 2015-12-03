using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Affecto.AuthenticationServer.Configuration
{
    public interface IAuthenticationServerConfiguration
    {
        bool RequireHttps { get; }
        string ServiceUserName { get; }
        bool AutoCreateUser { get; }
        IReadOnlyCollection<string> AllowedOrigins { get; }
        StoreName SigningCertificateStore { get; }
        string SigningCertificateThumbprint { get; }
        string SecondarySigningCertificateThumbprint { get; }
        IReadOnlyCollection<IScope> Scopes { get; }
        IReadOnlyCollection<IClient> Clients { get; }
        bool IsAllowedOrigin(string origin);
        Uri PublicOrigin { get; }
    }
}