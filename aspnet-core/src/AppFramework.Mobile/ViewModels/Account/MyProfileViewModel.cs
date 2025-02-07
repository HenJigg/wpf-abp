using AppFramework.ApiClient; 
using Prism.Services.Dialogs; 
using AppFramework.Shared.Models;

namespace AppFramework.Shared.ViewModels
{
    public class MyProfileViewModel : DialogViewModel
    {
        private UserLoginInfoModel userInfo;

        public UserLoginInfoModel UserInfo
        {
            get { return userInfo; }
            set { userInfo = value; RaisePropertyChanged(); }
        }


        private readonly IApplicationContext applicationContext;

        public MyProfileViewModel(IApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            UserInfo = Map<UserLoginInfoModel>(applicationContext.LoginInfo.User);
            base.OnDialogOpened(parameters);
        }
    }
}
