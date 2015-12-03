using System;
using System.Collections.Generic;
using System.Linq;

namespace Affecto.AuthenticationServer.Claims
{
    public class AuthenticatedUser
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string AccountName { get; private set; }
        public IReadOnlyCollection<CustomProperty> CustomProperties { get; private set; }
        public IReadOnlyCollection<string> Roles { get; private set; }
        public IReadOnlyCollection<Guid> Groups { get; private set; }
        public IReadOnlyCollection<string> Permissions { get; private set; }
        public IReadOnlyCollection<string> Organizations { get; private set; }

        public AuthenticatedUser(Guid id, string name, string accountName, IEnumerable<string> permissions = null, IEnumerable<string> roles = null,
            IEnumerable<CustomProperty> customProperties = null, IEnumerable<Guid> groups = null, IEnumerable<string> organizations = null)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Id must be provided.", "name");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name must be provided.", "name");
            }
            if (string.IsNullOrWhiteSpace(accountName))
            {
                throw new ArgumentException("Account name must be provided.", "accountName");
            }

            Id = id;
            Name = name;
            AccountName = accountName;
            Permissions = (permissions == null) ? new List<string>(0) : permissions.ToList();
            Roles = (roles == null) ? new List<string>(0) : roles.ToList();
            CustomProperties = (customProperties == null) ? new List<CustomProperty>(0) : customProperties.ToList();
            Groups = (groups == null) ? new List<Guid>(0) : groups.ToList();
            Organizations = (organizations == null) ? new List<string>(0) : organizations.ToList();
        }
    }
}