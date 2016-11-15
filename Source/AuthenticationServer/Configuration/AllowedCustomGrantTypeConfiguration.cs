using System.Configuration;
using Affecto.Configuration.Extensions;

namespace Affecto.AuthenticationServer.Configuration
{
    internal class AllowedCustomGrantTypeConfiguration : ConfigurationElementBase
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
                throw new ConfigurationErrorsException("Allowed grant type name is required.");
            }
        }
    }
}