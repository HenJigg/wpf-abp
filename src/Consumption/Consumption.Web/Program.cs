using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Consumption.Common.Contract;
using Consumption.Core.Interfaces;
using Consumption.Core.Response;
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
            var container = ConfigureServices();
            NetCoreProvider.RegisterServiceLocator(container);
            CreateHostBuilder(args).Build().Run();
        }

        private static IContainer ConfigureServices()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsumptionService>().As<IConsumptionService>();

            var assembly = Assembly.GetCallingAssembly();
            var types = assembly.GetTypes();
            foreach (var t in types)
            {
                if (t.Name.EndsWith("Center")) //简陋判断一下,一般而言,该定义仅仅注册Center结尾的类依赖关系。
                {
                    var firstInterface = t.GetInterfaces().FirstOrDefault();
                    if (firstInterface != null)
                        builder.RegisterType(t).Named(t.Name, firstInterface).SingleInstance();
                }
            }
            return builder.Build();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
