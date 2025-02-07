using AppFramework.ApiClient;
using AppFramework.Shared;
using AppFramework.Shared.Services;
using AppFramework.Admin.Services.Account;
using Prism.Services.Dialogs;
using System.Threading.Tasks;
using System;

namespace AppFramework.Admin.ViewModels
{
    public class SplashScreenViewModel : DialogViewModel
    {
        private readonly IAccessTokenManager accessTokenManager;
        private readonly IAccountStorageService dataStorageService;
        private readonly IApplicationContext applicationContext;

        private string displayText;

        public string DisplayText
        {
            get { return displayText; }
            set { displayText = value; OnPropertyChanged(); }
        }

        public SplashScreenViewModel(
           IApplicationContext applicationContext,
           IAccessTokenManager accessTokenManager,
           IAccountStorageService dataStorageService)
        {
            this.applicationContext = applicationContext;
            this.accessTokenManager = accessTokenManager;
            this.dataStorageService = dataStorageService;
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            await SetBusyAsync(async () =>
            {
                await Task.Delay(200);

                //加载本地的缓存信息
                DisplayText = LocalTranslationHelper.Localize("Initializing");
                accessTokenManager.AuthenticateResult = dataStorageService.RetrieveAuthenticateResult();
                applicationContext.Load(dataStorageService.RetrieveTenantInfo(), dataStorageService.RetrieveLoginInfo());

                //加载系统资源
                DisplayText = LocalTranslationHelper.Localize("LoadResource");
                await UserConfigurationManager.GetIfNeedsAsync(GetIfNeedsFailCallback);

                //如果本地授权存在,直接进入系统首页
                if (accessTokenManager.IsUserLoggedIn && applicationContext.Configuration != null)
                    OnDialogClosed();
                else if (applicationContext.Configuration != null)
                    OnDialogClosed(ButtonResult.Ignore);
                else
                    OnDialogClosed(ButtonResult.No);
            });
        }

        private async Task GetIfNeedsFailCallback(Exception ex)
        {
            OnDialogClosed(ButtonResult.No);
            await Task.CompletedTask;
        }
    }
}
