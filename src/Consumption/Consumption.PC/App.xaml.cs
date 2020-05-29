using Consumption.Core.Interfaces;
using Consumption.PC.Core;
using Consumption.PC.View;
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
            locator.Register();
            AutofacProvider.RegisterServiceLocator(locator);
        }
    }
}
