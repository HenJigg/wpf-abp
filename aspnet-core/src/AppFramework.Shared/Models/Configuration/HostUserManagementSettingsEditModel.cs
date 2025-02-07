using Prism.Mvvm;

namespace AppFramework.Shared.Models.Configuration
{
    public class HostUserManagementSettingsEditModel : BindableBase
    {
        private bool isEmailConfirmationRequiredForLogin;
        private bool smsVerificationEnabled;
        private bool isCookieConsentEnabled;
        private bool isQuickThemeSelectEnabled;
        private bool useCaptchaOnLogin;
        private bool allowUsingGravatarProfilePicture;
        private SessionTimeOutSettingsEditModel sessionTimeOutSettings;
        private UserPasswordSettingsEditModel userPasswordSettings;

        public bool IsEmailConfirmationRequiredForLogin
        {
            get { return isEmailConfirmationRequiredForLogin; }
            set { isEmailConfirmationRequiredForLogin = value; RaisePropertyChanged(); }
        }

        public bool SmsVerificationEnabled
        {
            get { return smsVerificationEnabled; }
            set { smsVerificationEnabled = value; RaisePropertyChanged(); }
        }

        public bool IsCookieConsentEnabled
        {
            get { return isCookieConsentEnabled; }
            set { isCookieConsentEnabled = value; RaisePropertyChanged(); }
        }

        public bool IsQuickThemeSelectEnabled
        {
            get { return isQuickThemeSelectEnabled; }
            set { isQuickThemeSelectEnabled = value; RaisePropertyChanged(); }
        }

        public bool UseCaptchaOnLogin
        {
            get { return useCaptchaOnLogin; }
            set { useCaptchaOnLogin = value; RaisePropertyChanged(); }
        }

        public bool AllowUsingGravatarProfilePicture
        {
            get { return allowUsingGravatarProfilePicture; }
            set { allowUsingGravatarProfilePicture = value; RaisePropertyChanged(); }
        }

        public SessionTimeOutSettingsEditModel SessionTimeOutSettings
        {
            get { return sessionTimeOutSettings; }
            set { sessionTimeOutSettings = value; RaisePropertyChanged(); }
        }

        public UserPasswordSettingsEditModel UserPasswordSettings
        {
            get { return userPasswordSettings; }
            set { userPasswordSettings = value; RaisePropertyChanged(); }
        }
    }
}