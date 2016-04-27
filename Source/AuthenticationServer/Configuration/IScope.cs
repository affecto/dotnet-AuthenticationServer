namespace Affecto.AuthenticationServer.Configuration
{
    public interface IScope
    {
        string Name { get; }
        string DisplayName { get; }
        bool IncludeAllClaimsForUser { get; }
    }
}