using Prism.Mvvm;

namespace AppFramework.Shared.Models
{
    public class UserEditModel : BindableBase
    {
        private string phoneNumber;
        private string userName;
        private string passWord;
        private string name;
        private string surnName;
        private string emailAddress;
        private bool isActive;
        private bool shouldChangePasswordOnNextLogin;
        private bool isTwoFactorEnabled;
        private bool isLockoutEnabled;

        public long? Id { get; set; }

        public string Name
        {
            get { return name; }
            set { name = value; RaisePropertyChanged(); }
        }

        public string Surname
        {
            get { return surnName; }
            set { surnName = value; RaisePropertyChanged(); }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; RaisePropertyChanged(); }
        }

        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value; RaisePropertyChanged(); }
        }

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; RaisePropertyChanged(); }
        }

        public string Password
        {
            get { return passWord; }
            set { passWord = value; RaisePropertyChanged(); }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; RaisePropertyChanged(); }
        }

        public bool ShouldChangePasswordOnNextLogin
        {
            get { return shouldChangePasswordOnNextLogin; }
            set { shouldChangePasswordOnNextLogin = value; RaisePropertyChanged(); }
        }

        public bool IsTwoFactorEnabled
        {
            get { return isTwoFactorEnabled; }
            set { isTwoFactorEnabled = value; RaisePropertyChanged(); }
        }

        public bool IsLockoutEnabled
        {
            get { return isLockoutEnabled; }
            set { isLockoutEnabled = value; RaisePropertyChanged(); }
        }
    }
}