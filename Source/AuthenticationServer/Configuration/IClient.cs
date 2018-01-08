using System;
using System.Collections.Generic;

namespace Affecto.AuthenticationServer.Configuration
{
    public interface IClient
    {
        string Id { get; }
        string Name { get; }
        string Secret { get; }
        Flow Flow { get; }
        AccessTokenType AccessTokenType { get; }
        IReadOnlyCollection<string> AllowedScopes { get; }
        IReadOnlyCollection<string> AllowedCustomGrantTypes { get; }
        TimeSpan AccessTokenLifetime { get; }
        Uri RedirectUri { get; }
    }
}