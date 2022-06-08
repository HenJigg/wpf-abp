using AppFramework.Services;
using Prism.Services.Dialogs;
using Syncfusion.Windows.Shared;

namespace AppFramework.Views
{
    public partial class SplashScreenView : ChromelessWindow, IDialogWindow
    {
        public SplashScreenView(IThemeService service)
        {
            InitializeComponent();
            service.SetCurrentTheme(this);
        }

        public IDialogResult Result { get; set; }
    }
}
