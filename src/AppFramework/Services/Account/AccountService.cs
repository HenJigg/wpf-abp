using AppFramework.ApiClient;
using AppFramework.ApiClient.Models;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services.Account;
using AppFramework.Common.Services.Storage;
using AppFramework.Sessions;
using AppFramework.Sessions.Dto;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Threading.Tasks;

namespace AppFramework.Services.Account
{
    public class AccountService : BindableBase, IAccountService
    {
        private readonly IHostDialogService dialog;
        public readonly IAccountStorageService dataStorageService;
        public readonly IApplicationContext applicationContext;
        public readonly ISessionAppService sessionAppService;
        public readonly IAccessTokenManager accessTokenManager;

        public AccountService(
            IHostDialogService dialog,
            IApplicationContext applicationContext,
            ISessionAppService sessionAppService,
            IAccessTokenManager accessTokenManager,
            IAccountStorageService dataStorageService,
            AbpAuthenticateModel authenticateModel)
        {
            this.dialog = dialog;
            this.applicationContext = applicationContext;
            this.sessionAppService = sessionAppService;
            this.accessTokenManager = accessTokenManager;
            this.dataStorageService = dataStorageService;
            this.AuthenticateModel = authenticateModel;
        }

        public AbpAuthenticateModel AuthenticateModel { get; set; }
        public AbpAuthenticateResultModel AuthenticateResultModel { get; set; }

        public async Task LoginUserAsync()
        {
            var result = await accessTokenManager.LoginAsync();
            await AuthenticateSucceed(result);
        }

        public async Task LoginCurrentUserAsync(UserListModel user)
        {
            accessTokenManager.Logout();
            applicationContext.ClearLoginInfo();
            dataStorageService.ClearSessionPersistance();

            await GoToLoginPageAsync();
        }

        public async Task LogoutAsync()
        {
            accessTokenManager.Logout();
            applicationContext.ClearLoginInfo();
            dataStorageService.ClearSessionPersistance();

            await GoToLoginPageAsync();
        }

        private async Task GoToLoginPageAsync()
        {
            App.LogOut();

            await Task.CompletedTask;
        }

        private async Task AuthenticateSucceed(AbpAuthenticateResultModel result)
        {
            AuthenticateResultModel = result;

            if (AuthenticateResultModel.ShouldResetPassword)
            {
                dialog.ShowMessage("", Local.Localize("ChangePasswordToLogin"));
                return;
            }

            if (AuthenticateResultModel.RequiresTwoFactorVerification)
            {
                DialogParameters param = new DialogParameters();
                param.Add("Value", AuthenticateResultModel);
                await dialog.ShowDialogAsync(AppViewManager.SendTwoFactorCode, param, AppCommonConsts.LoginIdentifier);
            }

            if (!AuthenticateModel.IsTwoFactorVerification)
            {
                await dataStorageService.StoreAuthenticateResultAsync(AuthenticateResultModel);
            }

            await SetCurrentUserInfoAsync();
            await UserConfigurationManager.GetAsync();
        }

        public async Task SetCurrentUserInfoAsync()
        {
            await WebRequest.Execute(async () =>
                await sessionAppService.GetCurrentLoginInformations(), GetCurrentUserInfoExecuted);
        }

        public async Task GetCurrentUserInfoExecuted(GetCurrentLoginInformationsOutput result)
        {
            applicationContext.SetLoginInfo(result);
            await dataStorageService.StoreLoginInformationAsync(applicationContext.LoginInfo);
        }
    }
}