using Prism.Mvvm;

namespace AppFramework.Shared.Models.Configuration
{
    public class UserLockOutSettingsEditModel : BindableBase
    {
        private bool isEnabled;
        private int maxFailedAccessAttemptsBeforeLockout;
        private int defaultAccountLockoutSeconds;

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; RaisePropertyChanged(); }
        }

        public int MaxFailedAccessAttemptsBeforeLockout
        {
            get { return maxFailedAccessAttemptsBeforeLockout; }
            set { maxFailedAccessAttemptsBeforeLockout = value; RaisePropertyChanged(); }
        }

        public int DefaultAccountLockoutSeconds
        {
            get { return defaultAccountLockoutSeconds; }
            set { defaultAccountLockoutSeconds = value; RaisePropertyChanged(); }
        }
    }
}