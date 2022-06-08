using AppFramework.ApiClient;
using AppFramework.Common.Services.Storage;
using AppFramework.Localization;
using AppFramework.Services.Account;
using Prism.Services.Dialogs;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
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
            set { displayText = value; RaisePropertyChanged(); }
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
                await Task.Delay(1000);

                //加载本地的缓存信息
                DisplayText = LocalTranslationHelper.Localize("Initializing");

                accessTokenManager.AuthenticateResult = dataStorageService.RetrieveAuthenticateResult();
                applicationContext.Load(dataStorageService.RetrieveTenantInfo(), dataStorageService.RetrieveLoginInfo());

                //加载系统资源
                DisplayText = LocalTranslationHelper.Localize("LoadResource");
                await UserConfigurationManager.GetIfNeedsAsync();

                //如果本地授权存在,直接进入系统首页
                if (accessTokenManager.IsUserLoggedIn)
                    OnDialogClosed();
                else
                    OnDialogClosed(ButtonResult.No);
            });
        }
    }
}
