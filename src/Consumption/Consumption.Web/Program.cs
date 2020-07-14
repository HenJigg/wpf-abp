using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Consumption.Core.Common;
using Consumption.Core.IService;
using Consumption.Service;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Consumption.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Contract.serverUrl = ConfigurationManager.AppSettings["serverAddress"];
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            NetCoreProvider.RegisterServiceLocator(serviceProvider);
            CreateHostBuilder(args).Build().Run();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IConsumptionService, ConsumptionService>();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
