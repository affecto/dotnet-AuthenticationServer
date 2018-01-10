using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Affecto.AuthenticationServer.Configuration
{
    public interface IKentorAuthServicesCustomProvidersConfiguration
    {
        string EntityId { get; }
        IReadOnlyCollection<ICustomIdentityProvider> IdentityProviders { get; }
    }
}