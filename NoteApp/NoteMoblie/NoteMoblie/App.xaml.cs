using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoteMoblie
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new MainWindow();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
