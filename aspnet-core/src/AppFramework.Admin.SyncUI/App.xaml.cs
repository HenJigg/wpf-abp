using AppFramework.Admin.SyncUI.Services.Sessions;
using AppFramework.Shared;
using AppFramework.Shared.Services;
using AppFramework.Shared.Services.App;
using Hardcodet.Wpf.TaskbarNotification;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Regions;
using Syncfusion.Windows.Tools.Controls;
using System.Windows;
using System.Windows.Forms;

namespace AppFramework.Admin.SyncUI
{
    public partial class App : PrismApplication, IAppTaskBar
    {
        private TaskbarIcon taskBar;

        protected override Window? CreateShell() => null;

        protected override void RegisterTypes(IContainerRegistry container)
        {
            container.AddViews();
            container.AddAdminsServices();
            container.AddSharedServices();

            container.RegisterSingleton<IThemeService, ThemeService>();
            container.RegisterSingleton<IHostDialogService, DialogHostService>();
            container.RegisterSingleton<IAppStartService, SyncUIStartService>();

            container.RegisterSingleton<ILocaleCulture, LocaleCulture>();
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            regionAdapterMappings.RegisterMapping<TabControlExt, SfTabControlRegionAdapter>();
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
        }

        protected override async void OnInitialized()
        {
            Initialization();

            var appVersionService = ContainerLocator.Container.Resolve<IUpdateService>();
            await appVersionService.CheckVersion();

            var appStart = ContainerLocator.Container.Resolve<IAppStartService>();
            appStart.CreateShell();

            base.OnInitialized();
        }

        public void ShowBalloonTip(string title, string message, BalloonIcon balloonIcon)
        {
            taskBar.ShowBalloonTip(title, message, balloonIcon);
        }

        public void Initialization()
        {
            taskBar = (TaskbarIcon)FindResource("taskBar"); 

            Syncfusion.SfSkinManager.SfSkinManager.ApplyStylesOnApplication = true;
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjM1NDUyOUAzMjMxMmUzMDJlMzBvTWpMSjdPcWlJYzRlS2RRamlVNkxWSE8rL202M2lUaWp5Z1dzSmc0ZXo4PQ==");
        }

        public void Dispose() => taskBar?.Dispose(); 
    }
}