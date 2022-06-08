using AppFramework.Services;
using Prism.Services.Dialogs;
using Syncfusion.Windows.Shared;
using System;

namespace AppFramework.Views
{
    public partial class LoginView : ChromelessWindow, IDialogWindow
    {
        public LoginView(IThemeService service)
        {
            InitializeComponent();
            service.SetCurrentTheme(this);
            HeaderBorder.MouseMove += (s, e) =>
            {
                if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                    this.DragMove();
            };
            btnClose.Click += (s, e) => { Environment.Exit(0); };
        }

        public IDialogResult Result { get; set; }
    }
}