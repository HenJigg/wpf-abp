using AppFramework.Admin.HandyUI.Themes.Controls;
using AppFramework.Admin.Services;
using AppFramework.Shared;
using AppFramework.Shared.Services;
using AppFramework.Shared.Services.App;
using AppFramework.Shared.Services.Mapper;
using Hardcodet.Wpf.TaskbarNotification;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Regions; 
using System.Windows; 

namespace AppFramework.Admin.HandyUI
{
    public partial class App : PrismApplication, IAppTaskBar
    {
        private TaskbarIcon? taskBar;

        protected override Window? CreateShell() => null;

        protected override async void OnInitialized()
        {
            Initialization();

            var appVersionService = ContainerLocator.Container.Resolve<IUpdateService>();
            await appVersionService.CheckVersion();

            var appStart = ContainerLocator.Container.Resolve<IAppStartService>();
            appStart.CreateShell();

            base.OnInitialized();
        }

        protected override void RegisterTypes(IContainerRegistry container)
        {
            container.AddViews();
            container.AddAdminsServices();
            container.AddSharedServices();

            container.RegisterSingleton<IHostDialogService, DialogHostService>();
            container.RegisterSingleton<IAppStartService, HandyUIStartService>();
            container.RegisterScoped<IPermissionTreesService, AppFramework.Admin.HandyUI.Services.PermissionTreesService>();
            container.RegisterScoped<IFeaturesService, AppFramework.Admin.HandyUI.Services.FeaturesService>();
             
            container.RegisterSingleton<ILocaleCulture, LocaleCulture>(); 
            //container.RegisterSingleton<INavigationMenuService, NavigationSingleMenuService>();
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            regionAdapterMappings.RegisterMapping<TabControl, TabControlRegionAdapter>();
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
        }

        public void ShowBalloonTip(string title, string message, BalloonIcon balloonIcon)
        {
            taskBar.ShowBalloonTip(title, message, balloonIcon);
        }

        public void Initialization()
        {
            taskBar = (TaskbarIcon)FindResource("taskBar");
        }

        public void Dispose() => taskBar?.Dispose();
    }
}
