using System.Collections.Generic;

namespace Affecto.AuthenticationServer.Configuration
{
    public interface IScope
    {
        string Name { get; }
        string DisplayName { get; }
        bool IncludeAllClaimsForUser { get; }
        IReadOnlyCollection<string> ScopeSecrets { get; }
    }
}