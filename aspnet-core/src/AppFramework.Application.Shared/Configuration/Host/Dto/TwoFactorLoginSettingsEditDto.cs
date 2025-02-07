namespace AppFramework.Configuration.Host.Dto
{
    public class TwoFactorLoginSettingsEditDto
    {
        public bool IsEnabledForApplication { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsEmailProviderEnabled { get; set; }

        public bool IsSmsProviderEnabled { get; set; }

        public bool IsRememberBrowserEnabled { get; set; }

        public bool IsGoogleAuthenticatorEnabled { get; set; }
    }
}