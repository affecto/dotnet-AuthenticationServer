using System.Configuration;

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
    }
}
