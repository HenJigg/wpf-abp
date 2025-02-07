using AppFramework.Admin.ViewModels;
using AppFramework.Shared;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;

namespace AppFramework.Admin.HandyUI
{
    public partial class FriendsChatView : UserControl
    {
        public FriendsChatView()
        {
            InitializeComponent();
            InputText.KeyDown+=SfInputText_KeyDown;
            this.KeyDown+=(s, e) =>
            {
                if (e.KeyStates== Keyboard.GetKeyStates(Key.C)&&Keyboard.Modifiers==ModifierKeys.Alt)
                    (this.DataContext as HostDialogViewModel)?.Cancel();
            };
        }


        private void SfInputText_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key== System.Windows.Input.Key.Enter)
            {
                (this.DataContext as FriendsChatViewModel)?.Send();
            }
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            var url = e.Uri.AbsoluteUri.Replace("&", "^&");
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }
    }
}
