using Autofac;
using Consumption.Common.Contract;
using Consumption.Core.Interfaces;
using Consumption.Core.Response;
using Consumption.Mobile.View;
using Consumption.Mobile.ViewCenter;
using Consumption.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Consumption.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            XF.Material.Forms.Material.Init(this);
        }

        protected override void OnStart()
        {
            var serviceCollection = new ServiceCollection();
            var container = ConfigureServices();
            NetCoreProvider.RegisterServiceLocator(container);
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private IContainer ConfigureServices()
        {
            var builder = new ContainerBuilder();
            //builder.RegisterType<ConsumptionService>().As<IConsumptionService>();

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

    }
}
