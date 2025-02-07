using Acr.UserDialogs;
using AppFramework.Shared.Core;
using AppFramework.Shared.Services.Storage;
using AppFramework.ApiClient;
using AppFramework.ApiClient.Models;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Sessions;
using AppFramework.Sessions.Dto;
using Prism.Mvvm;
using Prism.Navigation;
using System.Threading.Tasks;
using AppFramework.Shared.Models;
using AppFramework.Shared.Services.Navigation;
using AppFramework.Shared.Services.Messenger;
using AppFramework.Shared.Views;

namespace AppFramework.Shared.Services.Account
{
    public class AccountService : BindableBase, IAccountService
    {
        public readonly IAccountStorageService dataStorageService;
        public readonly IApplicationContext applicationContext;
        public readonly ISessionAppService sessionAppService;
        public readonly IAccessTokenManager accessTokenManager;
        private readonly INavigationService navigationService;
        private readonly IRegionNavigateService regionNavigateService;
        private readonly IMessenger messenger;
        public readonly IProfileAppService profileAppService;

        public AbpAuthenticateModel AuthenticateModel { get; set; }
        public AbpAuthenticateResultModel AuthenticateResultModel { get; set; }

        public AccountService(IProfileAppService profileAppService,
            IApplicationContext applicationContext,
            ISessionAppService sessionAppService,
            IAccountStorageService dataStorageService,
            IAccessTokenManager accessTokenManager,
            INavigationService navigationService,
            IRegionNavigateService regionNavigateService,
            IMessenger messenger,
            AbpAuthenticateModel authenticateModel)
        {
            this.profileAppService = profileAppService;
            this.applicationContext = applicationContext;
            this.sessionAppService = sessionAppService;
            this.accessTokenManager = accessTokenManager;
            this.navigationService = navigationService;
            this.regionNavigateService = regionNavigateService;
            this.messenger = messenger;
            this.dataStorageService = dataStorageService;
            AuthenticateModel = authenticateModel;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        public async Task<bool> LoginUserAsync()
        {
            await WebRequest.Execute(accessTokenManager.LoginAsync, AuthenticateSucceed);
            return true;
        }

        /// <summary>
        /// 用户注销
        /// </summary>
        /// <returns></returns>
        public async Task LogoutAsync()
        {
            //清空本地的用户会话信息
            accessTokenManager.Logout();
            applicationContext.ClearLoginInfo();
            dataStorageService.ClearSessionPersistance();
            //移除页面中所注册的区域
            messenger.Send(AppMessengerKeys.RemoveAllRegion);
            //返回至登录页
            GoToLoginPageAsync();

            await Task.CompletedTask;
        }

        /// <summary>
        /// 返回登录页
        /// </summary>
        /// <returns></returns>
        protected void GoToLoginPageAsync()
        {
            messenger.Send(AppMessengerKeys.Logout);
        }

        /// <summary>
        /// 授权成功事件
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected async Task<bool> AuthenticateSucceed(AbpAuthenticateResultModel result)
        {
            AuthenticateResultModel = result;

            if (AuthenticateResultModel.ShouldResetPassword)
            {
                await UserDialogs.Instance.AlertAsync(Local.Localize("ChangePasswordToLogin"), Local.Localize("LoginFailed"), Local.Localize("Ok"));
                return true; //待更新...
            }

            if (AuthenticateResultModel.RequiresTwoFactorVerification)
            {
                NavigationParameters param = new NavigationParameters();
                param.Add("Value", AuthenticateResultModel);
                await navigationService.NavigateAsync(AppViews.SendTwoFactorCode, param);
                return true; //待更新...
            }

            if (!AuthenticateModel.IsTwoFactorVerification)
            {
                await dataStorageService.StoreAuthenticateResultAsync(AuthenticateResultModel);
            }

            AuthenticateModel.Password = null;
            await SetCurrentUserInfoAsync();
            await AppConfigurationManager.GetAsync();
            regionNavigateService.Navigate(AppRegions.Index, AppViews.Main);

            return true;
        }

        /// <summary>
        /// 设置当前用户信息
        /// </summary>
        /// <returns></returns>
        public async Task SetCurrentUserInfoAsync()
        {
            await WebRequest.Execute(async () =>
                await sessionAppService.GetCurrentLoginInformations(), GetCurrentUserInfoExecuted);
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public async Task GetCurrentUserInfoExecuted(GetCurrentLoginInformationsOutput result)
        {
            applicationContext.SetLoginInfo(result);
            await dataStorageService.StoreLoginInformationAsync(applicationContext.LoginInfo);
        }

        public async Task LoginCurrentUserAsync(UserListModel user)
        {
            await Task.CompletedTask;
        }
    }
}