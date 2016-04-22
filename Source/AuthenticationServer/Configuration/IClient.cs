using System;
using System.Collections.Generic;

namespace Affecto.AuthenticationServer.Configuration
{
    public interface IClient
    {
        string Id { get; }
        string Name { get; }
        string Secret { get; }
        bool Enabled { get; }
        Flow Flow { get; }
        IReadOnlyCollection<string> AllowedScopes { get; } 
        TimeSpan AccessTokenLifetime { get; }
        Uri RedirectUri { get; }
    }
}