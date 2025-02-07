using AppFramework.ApiClient;
using AppFramework.ApiClient.Models;
using AppFramework.Shared.Services;
using AppFramework.Sessions;
using AppFramework.Sessions.Dto;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Threading.Tasks;
using AppFramework.Shared;
using AppFramework.Shared.Services.App;
using AppFramework.Admin.Models;

namespace AppFramework.Admin.Services.Account
{
    public class AccountService : BindableBase, IAccountService
    {
        private readonly IHostDialogService dialog;
        private readonly IAppStartService appStart;
        private readonly IChatService chatService;
        public readonly IAccountStorageService dataStorageService;
        public readonly IApplicationContext applicationContext;
        public readonly ISessionAppService sessionAppService;
        public readonly IAccessTokenManager accessTokenManager;

        public AccountService(
            IHostDialogService dialog,
            IAppStartService appStart,
            IChatService chatService,
            IApplicationContext applicationContext,
            ISessionAppService sessionAppService,
            IAccessTokenManager accessTokenManager,
            IAccountStorageService dataStorageService,
            AbpAuthenticateModel authenticateModel)
        {
            this.dialog = dialog;
            this.appStart = appStart;
            this.chatService=chatService;
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
            await chatService.CloseAsync();
            appStart.Logout();
        }

        private async Task AuthenticateSucceed(AbpAuthenticateResultModel result)
        {
            AuthenticateResultModel = result;

            if (AuthenticateResultModel.ShouldResetPassword)
            {
                DialogParameters param = new DialogParameters();
                param.Add("Value", result);
                var pwdResult = await dialog.ShowDialogAsync(AppViews.FirstChangedPwd,
                    param, AppSharedConsts.LoginIdentifier);

                if (pwdResult.Result != ButtonResult.OK)
                    return;
            }

            if (AuthenticateResultModel.RequiresTwoFactorVerification)
            {
                DialogParameters param = new DialogParameters();
                param.Add("Value", AuthenticateResultModel);
                await dialog.ShowDialogAsync(AppViews.SendTwoFactorCode, param, AppSharedConsts.LoginIdentifier);
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
            await WebRequest.Execute(() => sessionAppService.GetCurrentLoginInformations(),
                GetCurrentUserInfoExecuted);
        }

        public async Task GetCurrentUserInfoExecuted(GetCurrentLoginInformationsOutput result)
        {
            applicationContext.SetLoginInfo(result);
            await dataStorageService.StoreLoginInformationAsync(applicationContext.LoginInfo);
        }
    }
}