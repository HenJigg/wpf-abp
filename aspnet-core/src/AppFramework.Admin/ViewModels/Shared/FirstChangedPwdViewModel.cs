using AppFramework.ApiClient;
using AppFramework.ApiClient.Models;
using AppFramework.Authorization.Accounts;
using AppFramework.Authorization.Accounts.Dto;
using AppFramework.Shared;
using Prism.Services.Dialogs;
using System.Threading.Tasks;

namespace AppFramework.Admin.ViewModels.Shared
{
    public class FirstChangedPwdViewModel : HostDialogViewModel
    {
        private readonly IAccountAppService appService;
        private readonly IAccessTokenManager accessTokenManager;
        private readonly AbpAuthenticateModel authenticateModel;

        public FirstChangedPwdViewModel(
            IAccountAppService appService,
            IAccessTokenManager accessTokenManager,
            AbpAuthenticateModel authenticateModel)
        {
            this.appService = appService;
            this.accessTokenManager = accessTokenManager;
            this.authenticateModel = authenticateModel;
        }

        private string passWord;
        private string newpassWord;
        private string errorMessage;

        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; ValidationPassWord(); OnPropertyChanged(); }
        }

        public string NewPassWord
        {
            get { return newpassWord; }
            set
            {
                newpassWord = value;
                ValidationPassWord();
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged();
            }
        }

        private AbpAuthenticateResultModel model;

        public override async Task Save()
        {
            if (ValidationPassWord())
            {
                //密码策略验证...

                await SetBusyAsync(async () =>
                {
                    await WebRequest.Execute(() => appService.ResetPassword(
                          new ResetPasswordInput()
                          {
                              UserId = model.UserId,
                              Password = PassWord,
                              ResetCode = model.PasswordResetCode
                          }), ResetPasswordSuccessed);
                });
            }
        }
        private async Task ResetPasswordSuccessed(ResetPasswordOutput output)
        {
            if (output.CanLogin)
            {
                authenticateModel.Password = PassWord;
                await accessTokenManager.LoginAsync().WebAsync(base.Save);
            }
            else
                base.Cancel();
        }

        private bool ValidationPassWord()
        {
            if (!string.IsNullOrWhiteSpace(PassWord) && !string.IsNullOrWhiteSpace(NewPassWord))
            {
                if (PassWord != NewPassWord)
                    ErrorMessage = Local.Localize("PasswordsDontMatch");
                else
                {
                    ErrorMessage = string.Empty;
                    return true;
                }
            }
            return false;
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            model = parameters.GetValue<AbpAuthenticateResultModel>("Value");
        }
    }
}
