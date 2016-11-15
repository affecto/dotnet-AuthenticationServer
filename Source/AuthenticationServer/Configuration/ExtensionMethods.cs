using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer3.Core.Models;

namespace Affecto.AuthenticationServer.Configuration
{
    public static class ExtensionMethods
    {
        public static IReadOnlyCollection<Scope> MapToIdentityServerScopes(this IEnumerable<IScope> scopes)
        {
            return scopes.Select(scope => new Scope
            {
                Name = scope.Name,
                DisplayName = scope.DisplayName,
                IncludeAllClaimsForUser = scope.IncludeAllClaimsForUser,
                Enabled = true
            }).ToList();
        }

        public static IReadOnlyCollection<Client> MapToIdentityServerClients(this IEnumerable<IClient> clients)
        {
            return clients.Select(client => new Client
            {
                ClientId = client.Id,
                ClientName = client.Name,
                ClientSecrets = new List<Secret> { new Secret(client.Secret.Sha256()) },
                Flow = (Flows) Enum.Parse(typeof(Flows), client.Flow.ToString()),
                AccessTokenLifetime = (int) client.AccessTokenLifetime.TotalSeconds,
                Enabled = true,
                AllowedScopes = StandardScopes.All.Select(o => o.Name).Concat(client.AllowedScopes).ToList(),
                AllowedCustomGrantTypes = client.AllowedCustomGrantTypes.ToList(),
                RedirectUris = client.RedirectUri != null ? new List<string> { client.RedirectUri.OriginalString } : null,
                RequireConsent = false
            }).ToList();
        }
    }
}