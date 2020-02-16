using NoteMoblie.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoteMoblie.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserDetailChart : ContentView
    {
        public UserDetailChart()
        {
            InitializeComponent();
            this.BindingContext = new UserDetailChartViewModel();
        }
    }
}