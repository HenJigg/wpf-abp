using Consumption.Core.Common;
using Consumption.Core.Interfaces;
using Consumption.Core.IService;
using Consumption.PC.View;
using Consumption.PC.ViewCenter;
using Consumption.Service;
using Consumption.ViewModel;
using Microsoft.Extensions.DependencyInjection;
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
        public IServiceProvider ServiceProvider { get; private set; }
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.ConfigureServices();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            NetCoreProvider.RegisterServiceLocator(serviceProvider);

            LoginCenter viewCenter = new LoginCenter();
            await viewCenter.ShowDialog();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(typeof(MainCenter));
            services.AddScoped<IConsumptionService, ConsumptionService>();
        }

        protected void ConfigureServices()
        {
            //AutofacLocator locator = new AutofacLocator();
            //ContainerBuilder builder = new ContainerBuilder();
            //builder.RegisterType<ConsumptionService>().As<IConsumptionService>();
            //builder.RegisterType(typeof(MainCenter)).Named("MainCenter", typeof(IModuleDialog));
            //builder.RegisterType(typeof(SkinCenter)).Named("SkinCenter", typeof(IModule));
            //builder.RegisterType(typeof(UserCenter)).Named("UserCenter", typeof(IModule));
            //builder.RegisterType(typeof(MenuCenter)).Named("MenuCenter", typeof(IModule));
            //builder.RegisterType(typeof(BasicCenter)).Named("BasicCenter", typeof(IModule));
            //locator.Register(builder);
            //NetCoreProvider.RegisterServiceLocator(locator);
        }
    }
}
