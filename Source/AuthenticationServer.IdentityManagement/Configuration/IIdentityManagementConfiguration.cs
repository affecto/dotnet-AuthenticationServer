using System.Collections.Generic;

namespace Affecto.AuthenticationServer.IdentityManagement.Configuration
{
    public interface IIdentityManagementConfiguration
    {
        bool AutoCreateUser { get; }
        IReadOnlyCollection<ICustomProperty> NewUserCustomProperties { get; }
    }
}
