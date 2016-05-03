using System.Configuration;
using Affecto.Configuration.Extensions;

namespace Affecto.AuthenticationServer.IdentityManagement.Configuration
{
    internal class CustomProperty : ConfigurationElementBase, ICustomProperty
    {
        public override string ElementKey => Name;

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }

        /// <summary>
        /// Called after deserialization.
        /// </summary>
        protected override void PostDeserialize()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new ConfigurationErrorsException("Name is required.");
            }
            if (string.IsNullOrWhiteSpace(Value))
            {
                throw new ConfigurationErrorsException("Value is required.");
            }
        }
    }
}
