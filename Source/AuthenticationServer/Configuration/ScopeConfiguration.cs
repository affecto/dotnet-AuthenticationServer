using System.Configuration;
using Affecto.Configuration.Extensions;

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

        [ConfigurationProperty("enabled", IsRequired = false, DefaultValue = true)]
        public bool Enabled
        {
            get { return (bool) this["enabled"]; }
            set { this["enabled"] = value; }
        }

        protected override void PostDeserialize()
        {
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