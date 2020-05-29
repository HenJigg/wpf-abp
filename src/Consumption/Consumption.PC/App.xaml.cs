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
            LoginView view = new LoginView();
            view.DataContext = new LoginViewModel();
            view.ShowDialog();
            base.OnStartup(e);
        }
    }
}
