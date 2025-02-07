using AppFramework.Shared.Models;
using AppFramework.Shared.Services.Account;
using AppFramework.Shared.Services.Storage;
using AppFramework.Shared.Views;
using DryIoc; 
using Prism.DryIoc;
using Prism.Ioc;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: ExportFont("Montserrat-Bold.ttf", Alias = "Montserrat-Bold")]
[assembly: ExportFont("Montserrat-Medium.ttf", Alias = "Montserrat-Medium")]
[assembly: ExportFont("Montserrat-Regular.ttf", Alias = "Montserrat-Regular")]
[assembly: ExportFont("Montserrat-SemiBold.ttf", Alias = "Montserrat-SemiBold")]
[assembly: ExportFont("UIFontIcons.ttf", Alias = "FontIcons")]
[assembly: ExportFont("iconfont.ttf", Alias = "iconfont")]

namespace AppFramework.Shared
{
    public partial class App
    {
        public static Action ExitApplication;

        public static async Task OnSessionTimeout()
        {
            await ContainerLocator.Container.Resolve<IAccountService>()
                .LogoutAsync();
        }

        public static async Task OnAccessTokenRefresh(string newAccessToken)
        {
            await ContainerLocator.Container.Resolve<IAccountStorageService>()
                .StoreAccessTokenAsync(newAccessToken);
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            AppSettings.OnInitialized();

            await NavigationService.NavigateAsync(AppViews.InitialScreen);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //注册应用程序服务
            containerRegistry.ConfigureSerivces();

            //注册Prism区域服务及导航页
            containerRegistry.RegisterRegionServices();
            containerRegistry.RegisterForNavigation<NavigationPage>();
        }
         
        public bool OnBackPressedHandler()
        {
            //当导航堆栈中存在模块,则允许返回
            if (MainPage.Navigation.ModalStack.Count > 0)
                return true;

            return false;
        }
    }
}