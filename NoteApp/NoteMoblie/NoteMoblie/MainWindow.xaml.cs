using NoteMoblie.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NoteMoblie.View;

namespace NoteMoblie
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainWindow : Xamarin.Forms.TabbedPage
    {
        public MainWindow()
        {
            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            //On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom)
            // .SetBarItemColor(Color.Black)
            // .SetBarSelectedItemColor(Color.Red)
            // .SetIsLegacyColorModeEnabled(false);
            this.Children.Clear();
            InitializeComponent();
            BindingContext = new MainViewModel();
        }
    }
}