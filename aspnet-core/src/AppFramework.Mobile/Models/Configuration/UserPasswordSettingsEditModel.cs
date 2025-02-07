using Prism.Mvvm;

namespace AppFramework.Shared.Models.Configuration
{
    public class UserPasswordSettingsEditModel : BindableBase
    {
        private bool enableCheckingLastXPasswordWhenPasswordChange;
        private int checkingLastXPasswordCount;
        private bool enablePasswordExpiration;
        private int passwordExpirationDayCount;

        public bool EnableCheckingLastXPasswordWhenPasswordChange
        {
            get { return enableCheckingLastXPasswordWhenPasswordChange; }
            set { enableCheckingLastXPasswordWhenPasswordChange = value; RaisePropertyChanged(); }
        }

        public int CheckingLastXPasswordCount
        {
            get { return checkingLastXPasswordCount; }
            set { checkingLastXPasswordCount = value; RaisePropertyChanged(); }
        }

        public bool EnablePasswordExpiration
        {
            get { return enablePasswordExpiration; }
            set { enablePasswordExpiration = value; RaisePropertyChanged(); }
        }

        public int PasswordExpirationDayCount
        {
            get { return passwordExpirationDayCount; }
            set { passwordExpirationDayCount = value; RaisePropertyChanged(); }
        }
    }
}