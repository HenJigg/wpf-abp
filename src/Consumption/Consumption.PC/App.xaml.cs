using Autofac;
using Autofac.Builder;
using Consumption.Core.Common;
using Consumption.Core.Interfaces;
using Consumption.Core.IService;
using Consumption.PC.View;
using Consumption.PC.ViewCenter;
using Consumption.Service;
using Consumption.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Consumption.PC
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.ConfigureServices();
            var view = AutofacProvider.Get<IModuleDialog>("LoginCenter");
            view.ShowDialog();
        }

        protected void ConfigureServices()
        {
            AutofacLocator locator = new AutofacLocator();
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType(typeof(LoginCenter)).Named("LoginCenter", typeof(IModuleDialog));
            builder.RegisterType(typeof(MainCenter)).Named("MainCenter", typeof(IModuleDialog));
            builder.RegisterType(typeof(SkinCenter)).Named("SkinCenter", typeof(IModule));
            locator.Register(builder);
            AutofacProvider.RegisterServiceLocator(locator);
        }
    }
}
