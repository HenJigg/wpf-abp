using Consumption.Service;
using Consumption.ViewModel;
using System.Configuration;
using System.Linq;
using System.Windows;
using Consumption.ViewModel.Common;
using Autofac;
using System.Reflection;
using Consumption.Core.Common;
using Consumption.ViewModel.Core;
using Consumption.ViewModel.Interfaces;
using Consumption.Shared.DataInterfaces;
using Consumption.Shared.Common;
using System;
using Microsoft.Extensions.DependencyInjection;
using Consumption.PC.ViewCenter;

namespace Consumption.PC
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            App.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
            NetCoreProvider.Resolve<ILog>()?.Warn(e.Exception, e.Exception.Message);
            e.Handled = true;
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            Contract.serverUrl = ConfigurationManager.AppSettings["serverAddress"];
            this.ConfigureServices();
            var login = NetCoreProvider.ResolveNamed<ILoginCenter>("LoginCenter");
            await login.ShowDialog();
        }

        private void ConfigureServices()
        {
            var service = new ContainerBuilder();

            service.AddRepository<UserService, IUserRepository>()
            .AddRepository<GroupService, IGroupRepository>()
            .AddRepository<MenuService, IMenuRepository>()
            .AddRepository<BasicService, IBasicRepository>()
            .AddRepository<ConsumptionNLog, ILog>();

            service.AddViewModel<UserViewModel, IUserViewModel>()
            .AddViewModel<LoginViewModel, ILoginViewModel>()
            .AddViewModel<MainViewModel, IMainViewModel>()
            .AddViewModel<GroupViewModel, IGroupViewModel>()
            .AddViewModel<MenuViewModel, IMenuViewModel>()
            .AddViewModel<BasicViewModel, IBasicViewModel>()
            .AddViewModel<SkinViewModel, ISkinViewModel>()
            .AddViewModel<HomeViewModel, IHomeViewModel>()
            .AddViewModel<DashboardViewModel, IDashboardViewModel>();

            service.AddViewCenter<LoginCenter, ILoginCenter>()
                .AddViewCenter<MainCenter, IMainCenter>()
                .AddViewCenter<MsgCenter, IMsgCenter>()
                 .AddViewCenter<HomeCenter, IHomeCenter>()
            .AddViewCenter<UserCenter, IBaseCenter>()
            .AddViewCenter<MenuCenter, IBaseCenter>()
            .AddViewCenter<SkinCenter, IBaseCenter>()
            .AddViewCenter<GroupCenter, IBaseCenter>()
            .AddViewCenter<BasicCenter, IBaseCenter>()
            .AddViewCenter<DashboardCenter, IBaseCenter>();

            NetCoreProvider.RegisterServiceLocator(service.Build());
        }
    }
}
