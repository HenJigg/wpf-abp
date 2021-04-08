using Consumption.Service;
using System.Linq;
using System.Windows;
using Consumption.Core.Common;
using Consumption.ViewModel.Interfaces;
using Consumption.Shared.DataInterfaces;
using Prism.DryIoc;
using Prism.Ioc;
using Consumption.PC.Views;
using Consumption.ViewModel;
using System;

namespace Consumption.PC
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public App()
        {
            App.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
            e.Handled = true;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IUserRepository, UserService>();
            containerRegistry.Register<IGroupRepository, GroupService>();
            containerRegistry.Register<IMenuRepository, MenuService>();
            containerRegistry.Register<IBasicRepository, BasicService>();
            containerRegistry.Register<ILog, ConsumptionNLog>();


            containerRegistry.RegisterForNavigation<HomeView>();
            containerRegistry.RegisterForNavigation<SkinView>();
            containerRegistry.RegisterForNavigation<BasicView, BasicViewModel>();
            containerRegistry.RegisterForNavigation<GroupView, GroupViewModel>();
            containerRegistry.RegisterForNavigation<MenuView, MenuViewModel>();
            containerRegistry.RegisterForNavigation<UserView, UserViewModel>();
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void OnInitialized()
        {
            var win = Container.Resolve<LoginView>();
            var result = (bool)win.ShowDialog();
            if (result)
            {
                base.OnInitialized();
            }
            else
                Environment.Exit(0);
        }
    }
}
