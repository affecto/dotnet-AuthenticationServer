using System.Configuration;
using Affecto.Configuration.Extensions;

namespace Affecto.AuthenticationServer.Configuration
{
    internal class AllowedScopeConfiguration : ConfigurationElementBase
    {
        public override string ElementKey => Name;

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string) this["name"]; }
            set { this["name"] = value; }
        }

        protected override void PostDeserialize()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new ConfigurationErrorsException("Allowed scope name is required.");
            }
        }
    }
}