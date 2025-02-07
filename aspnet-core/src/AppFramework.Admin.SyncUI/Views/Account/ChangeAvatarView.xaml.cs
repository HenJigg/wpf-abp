using Syncfusion.UI.Xaml.ImageEditor;
using System.Windows;
using System.Windows.Controls; 

namespace AppFramework.Admin.SyncUI
{ 
    public partial class ChangeAvatarView : UserControl
    {
        public ChangeAvatarView()
        {
            InitializeComponent();
            this.Loaded += ChangeAvatarView_Loaded; 
        }

        private void ChangeAvatarView_Loaded(object sender, RoutedEventArgs e)
        {
            sfImageEditor.ToggleCropping(new Rect());
        }
    }
}
