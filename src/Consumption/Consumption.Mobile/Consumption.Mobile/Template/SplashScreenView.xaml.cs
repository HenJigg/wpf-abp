using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Consumption.Mobile.Template
{
    public partial class SplashScreenView : PopupPage
    {
        public SplashScreenView(string loadingMsg)
        {
            InitializeComponent();
            LoadingMsg.Text = loadingMsg;
        }
    }
}