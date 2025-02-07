using AppFramework.Security;
using Prism.Mvvm;

namespace AppFramework.Shared.Models.Configuration
{
    public class SecuritySettingsEditModel : BindableBase
    {
        private bool allowOneConcurrentLoginPerUser;
        private bool useDefaultPasswordComplexitySettings;
        private PasswordComplexitySetting passwordComplexity;
        private PasswordComplexitySetting defaultPasswordComplexity;
        private UserLockOutSettingsEditModel userLockOut;
        private TwoFactorLoginSettingsEditModel twoFactorLogin;
         
        public bool AllowOneConcurrentLoginPerUser
        {
            get { return allowOneConcurrentLoginPerUser; }
            set { allowOneConcurrentLoginPerUser = value; RaisePropertyChanged(); }
        }

        public bool UseDefaultPasswordComplexitySettings
        {
            get { return useDefaultPasswordComplexitySettings; }
            set { useDefaultPasswordComplexitySettings = value; RaisePropertyChanged(); }
        }

        public PasswordComplexitySetting PasswordComplexity
        {
            get { return passwordComplexity; }
            set { passwordComplexity = value; RaisePropertyChanged(); }
        }

        public PasswordComplexitySetting DefaultPasswordComplexity
        {
            get { return defaultPasswordComplexity; }
            set { defaultPasswordComplexity = value; RaisePropertyChanged(); }
        }

        public UserLockOutSettingsEditModel UserLockOut
        {
            get { return userLockOut; }
            set { userLockOut = value; RaisePropertyChanged(); }
        }

        public TwoFactorLoginSettingsEditModel TwoFactorLogin
        {
            get { return twoFactorLogin; }
            set { twoFactorLogin = value; RaisePropertyChanged(); }
        }
    }
}