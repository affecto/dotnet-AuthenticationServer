using System.Configuration;
using Affecto.Configuration.Extensions;
using IdentityServer3.Core.Models;
using System.Linq;

namespace Affecto.AuthenticationServer.Configuration
{
    internal class ScopeConfiguration : ConfigurationElementBase, IScope
    {
        public override string ElementKey => Name;

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string) this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("displayName", IsRequired = true)]
        public string DisplayName
        {
            get { return (string) this["displayName"]; }
            set { this["displayName"] = value; }
        }

        [ConfigurationProperty("includeAllClaimsForUser", IsRequired = false, DefaultValue = false)]
        public bool IncludeAllClaimsForUser
        {
            get { return (bool) this["includeAllClaimsForUser"]; }
            set { this["includeAllClaimsForUser"] = value; }
        }

        protected override void PostDeserialize()
        {
            if (StandardScopes.All.Any(s => s.Name == Name))
            {
                throw new ConfigurationErrorsException($"Configured scope '{Name}' is a standard scope that is already included.");
            }
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new ConfigurationErrorsException("Scope name is required.");
            }
            if (string.IsNullOrWhiteSpace(DisplayName))
            {
                throw new ConfigurationErrorsException($"Scope display name is required for scope '{Name}'.");
            }
        }
    }
}