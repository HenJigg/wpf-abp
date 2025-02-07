using Prism.Mvvm;

namespace AppFramework.Shared.Models.Configuration
{
    public class TwoFactorLoginSettingsEditModel : BindableBase
    {
        private bool isEnabledForApplication;
        private bool isEnabled;
        private bool isEmailProviderEnabled;
        private bool isSmsProviderEnabled;
        private bool isRememberBrowserEnabled;
        private bool isGoogleAuthenticatorEnabled;

        public bool IsEnabledForApplication
        {
            get { return isEnabledForApplication; }
            set { isEnabledForApplication = value; RaisePropertyChanged(); }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; RaisePropertyChanged(); }
        }

        public bool IsEmailProviderEnabled
        {
            get { return isEmailProviderEnabled; }
            set { isEmailProviderEnabled = value; RaisePropertyChanged(); }
        }

        public bool IsSmsProviderEnabled
        {
            get { return isSmsProviderEnabled; }
            set { isSmsProviderEnabled = value; RaisePropertyChanged(); }
        }

        public bool IsRememberBrowserEnabled
        {
            get { return isRememberBrowserEnabled; }
            set { isRememberBrowserEnabled = value; RaisePropertyChanged(); }
        }

        public bool IsGoogleAuthenticatorEnabled
        {
            get { return isGoogleAuthenticatorEnabled; }
            set { isGoogleAuthenticatorEnabled = value; RaisePropertyChanged(); }
        }
    }
}