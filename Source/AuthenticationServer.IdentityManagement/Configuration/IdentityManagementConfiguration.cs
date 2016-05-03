using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Affecto.Configuration.Extensions;

namespace Affecto.AuthenticationServer.IdentityManagement.Configuration
{
    internal class IdentityManagementConfiguration : ConfigurationSection, IIdentityManagementConfiguration
    {
        private static readonly IdentityManagementConfiguration SettingsInstance =
            ConfigurationManager.GetSection("identityManagement") as IdentityManagementConfiguration;

        public static IIdentityManagementConfiguration Settings => SettingsInstance;

        [ConfigurationProperty("autoCreateUser", IsRequired = false, DefaultValue = false)]
        public bool AutoCreateUser
        {
            get { return (bool)this["autoCreateUser"]; }
            set { this["autoCreateUser"] = value; }
        }

        public IReadOnlyCollection<ICustomProperty> NewUserCustomProperties => CustomPropertiesInternal.ToList();

        [ConfigurationProperty("newUserCustomProperties", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(ConfigurationElementCollection<CustomProperty>), AddItemName = "customProperty")]
        private ConfigurationElementCollection<CustomProperty> CustomPropertiesInternal
        {
            get { return (ConfigurationElementCollection<CustomProperty>)base["newUserCustomProperties"]; }
        }
    }
}
