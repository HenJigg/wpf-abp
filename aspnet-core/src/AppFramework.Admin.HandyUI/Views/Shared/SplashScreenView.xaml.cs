using Prism.Services.Dialogs;
using System.Windows; 

namespace AppFramework.Admin.HandyUI.Views
{ 
    public partial class SplashScreenView : Window, IDialogWindow
    {
        public SplashScreenView()
        {
            InitializeComponent();
        }

        public IDialogResult Result { get; set; }
    }
}
