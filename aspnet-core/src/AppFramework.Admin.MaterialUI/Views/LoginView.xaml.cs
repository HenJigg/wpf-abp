using Prism.Services.Dialogs;
using System;
using System.Windows; 

namespace AppFramework.Admin.MaterialUI.Views
{ 
    public partial class LoginView : Window, IDialogWindow
    {
        public LoginView()
        {
            InitializeComponent();
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
