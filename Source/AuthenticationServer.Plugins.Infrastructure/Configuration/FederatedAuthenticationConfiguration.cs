using System.Configuration;

namespace Affecto.AuthenticationServer.Plugins.Infrastructure.Configuration
{
    public class FederatedAuthenticationConfiguration : ConfigurationSection, IFederatedAuthenticationConfiguration
    {
        private static readonly FederatedAuthenticationConfiguration SettingsInstance =
            ConfigurationManager.GetSection("federatedAuthentication") as FederatedAuthenticationConfiguration;

        public static IFederatedAuthenticationConfiguration Settings
        {
            get { return SettingsInstance; }
        }

        [ConfigurationProperty("userAccountNameClaim", IsRequired = true)]
        public string UserAccountNameClaim
        {
            get { return (string)this["userAccountNameClaim"]; }
            set { this["userAccountNameClaim"] = value; }
        }

        [ConfigurationProperty("userDisplayNameClaim", IsRequired = true)]
        public string UserDisplayNameClaim
        {
            get { return (string)this["userDisplayNameClaim"]; }
            set { this["userDisplayNameClaim"] = value; }
        }

        [ConfigurationProperty("groupsClaim", IsRequired = false)]
        public string GroupsClaim
        {
            get { return (string)this["groupsClaim"]; }
            set { this["groupsClaim"] = value; }
        }

        protected override void PostDeserialize()
        {
            if (string.IsNullOrWhiteSpace(UserAccountNameClaim))
            {
                throw new ConfigurationErrorsException("User account name claim is required.");
            }
            if (string.IsNullOrWhiteSpace(UserDisplayNameClaim))
            {
                throw new ConfigurationErrorsException("User display name claim is required.");
            }
        }
    }
}