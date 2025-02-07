using AppFramework.ApiClient;
using AppFramework.Shared;
using AppFramework.Admin.Models;
using AppFramework.Admin.ViewModels.Shared;
using Prism.Services.Dialogs; 

namespace AppFramework.Admin.ViewModels
{
    public class MyProfileViewModel : HostDialogViewModel
    {
        private readonly IApplicationContext applicationContext;

        public MyProfileViewModel(IApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        private UserLoginInfoModel userInfo;

        public UserLoginInfoModel UserInfo
        {
            get { return userInfo; }
            set { userInfo = value; OnPropertyChanged(); }
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            UserInfo = Map<UserLoginInfoModel>(applicationContext.LoginInfo.User);
        }
    }
}
