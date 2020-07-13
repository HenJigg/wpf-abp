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
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

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

    }
}
