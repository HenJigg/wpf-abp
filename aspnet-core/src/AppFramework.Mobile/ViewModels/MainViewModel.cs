using AppFramework.ApiClient;
using Prism.Commands;
using Prism.Regions.Navigation;
using AppFramework.Shared.Services;
using AppFramework.Shared.Services.Account;
using AppFramework.Shared.Models;
using AppFramework.Shared.Core;
using AppFramework.Shared.Services.Navigation;
using AppFramework.Shared.Services.Messenger;
using AppFramework.Shared.Views;

namespace AppFramework.Shared.ViewModels
{
    public class MainViewModel : RegionViewModel
    {
        public MainViewModel(IRegionNavigateService regionService, 
            IApplicationContext applicationContext,
            IApplicationService appService,
            IMessenger messenger)
        {
            this.regionService = regionService; 
            this.applicationContext = applicationContext;
            this.appService = appService; 

            GoChatCommand = new DelegateCommand(() =>
            {
                regionService.Navigate(AppRegions.Main, AppViews.Friends);
            });
            ExecuteCommand = new DelegateCommand<string>(Execute);

            messenger.Sub(AppMessengerKeys.LanguageRefresh, appService.RefreshAuthMenus);
        }

        #region 字段/属性

        private readonly IRegionNavigateService regionService; 
        private readonly IApplicationContext applicationContext; 

        public IApplicationService appService { get; }

        public DelegateCommand GoChatCommand { get; private set; }
        public DelegateCommand<string> ExecuteCommand { get; private set; }

        #endregion 字段/属性

        #region 方法

        public async void Execute(string arg)
        {
            switch (arg)
            {
                case "ShowProfilePhoto": await appService.ShowProfilePhoto(); break;
                case "Home": SetDefaultPage(); break;
                case "MyProfile": await appService.ShowMyProfile(); break;
            }
        }

        public override async void OnNavigatedTo(INavigationContext navigationContext)
        {
            if (applicationContext.LoginInfo == null) return;
            //设置应用程序信息
            await appService.GetApplicationInfo();
            //初始化系统首页
            SetDefaultPage();
        }

        public void Navigate(NavigationItem item)
        {
            regionService.Navigate(AppRegions.Main, item.PageViewName);
        }

        private void SetDefaultPage()
        {
            regionService.Navigate(AppRegions.Main, AppViews.Dashboard);
        }

        #endregion
    }
}