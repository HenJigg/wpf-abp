/*
 * 应用启动初始化操作
 * 用途:初始化应用程序的配置文件,如:本地化、当前语言等
 *
 *
 * 说明:在Android应用启动过程中, 避免在主线程中执行耗时操作,
 * 通过加载初始化的显示屏幕, 触发页面的Appearing事件,来执行耗时操作。
 * Appearing函数为Page类的实现, ContentPage间接继承于Xamarin.Forms.Page类。
 */

namespace AppFramework.Shared.ViewModels
{
    using AppFramework.Shared.Core;
    using AppFramework.Shared.Services.Storage;
    using AppFramework.ApiClient;
    using AppFramework.Shared.Services.Account;
    using Prism.Commands;
    using AppFramework.Shared.Services.Navigation;
    using AppFramework.Shared.Services.Messenger;
    using AppFramework.Shared.Views;

    /// <summary>
    /// 应用启动初始化操作
    /// </summary>
    public class InitialScreenViewModel : ViewModelBase
    {
        private readonly IRegionNavigateService regionNavigateService;
        private readonly IAccessTokenManager accessTokenManager;
        private readonly IApplicationContext applicationContext;
        private readonly IAccountStorageService dataStorageService;
        public DelegateCommand AppearingCommand { get; private set; }

        private bool initialize;

        private bool isDisplayLayer = true;

        public bool IsDisplayLayer
        {
            get { return isDisplayLayer; }
            set { isDisplayLayer = value; RaisePropertyChanged(); }
        }

        public InitialScreenViewModel(
            IRegionNavigateService regionNavigateService,
            IAccessTokenManager accessTokenManager,
            IApplicationContext applicationContext,
            IAccountStorageService dataStorageService,
            IMessenger messenger)
        {
            this.regionNavigateService = regionNavigateService;
            this.accessTokenManager = accessTokenManager;
            this.applicationContext = applicationContext;
            this.dataStorageService = dataStorageService;
            AppearingCommand = new DelegateCommand(Appearing);

            messenger.Sub(AppMessengerKeys.Logout, () =>
            {
                initialize = false;
                this.Appearing();
            });
        }

        private async void Appearing()
        {
            /*
            * 导航过程中,当页面返回时,会触发原来的导航起始页的
            * PageAppearing 防止页面重新刷新覆盖原有的导航内容
            */
            if (initialize) return;

            //加载本地的缓存信息
            accessTokenManager.AuthenticateResult = dataStorageService.RetrieveAuthenticateResult();
            applicationContext.Load(dataStorageService.RetrieveTenantInfo(), dataStorageService.RetrieveLoginInfo());

            //获取应用程序资源数据(本地化资源、设置、用户信息权限等...)
            await AppConfigurationManager.GetIfNeedsAsync(); 
            IsDisplayLayer = false;

            if (accessTokenManager.IsUserLoggedIn)
                regionNavigateService.Navigate(AppRegions.Index, AppViews.Main);
            else
                regionNavigateService.Navigate(AppRegions.Index, AppViews.Login);

            initialize = true;
        }
    }
}