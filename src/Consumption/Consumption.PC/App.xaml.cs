using Consumption.Core.Response;
using Consumption.Core.Interfaces;
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
using Consumption.Common.Contract;
using Consumption.ViewModel.Common;
using Autofac;
using System.Reflection;

namespace Consumption.PC
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            Contract.serverUrl = ConfigurationManager.AppSettings["serverAddress"];
            var container = ConfigureServices();
            NetCoreProvider.RegisterServiceLocator(container);
            LoginCenter viewCenter = new LoginCenter();
            await viewCenter.ShowDialog();
        }

        private IContainer ConfigureServices()
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
                        builder.RegisterType(t).Named(t.Name, firstInterface);
                }
            }
            return builder.Build();
        }

    }
}
