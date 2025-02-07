namespace AppFramework.Configuration.Host.Dto
{
    public class HostUserManagementSettingsEditDto
    {
        public bool IsEmailConfirmationRequiredForLogin { get; set; }

        public bool SmsVerificationEnabled { get; set; }

        public bool IsCookieConsentEnabled { get; set; }

        public bool IsQuickThemeSelectEnabled { get; set; }

        public bool UseCaptchaOnLogin { get; set; }

        public bool AllowUsingGravatarProfilePicture { get; set; }
        
        public SessionTimeOutSettingsEditDto SessionTimeOutSettings { get; set; }
        
        public UserPasswordSettingsEditDto UserPasswordSettings { get; set; }
    }
}