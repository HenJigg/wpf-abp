using System.Windows.Controls; 

namespace AppFramework.Admin.SyncUI
{ 
    public partial class RoleDetailsView : UserControl
    {
        public RoleDetailsView()
        {
            InitializeComponent();
            sfTreeView.NodeChecked += SfTreeView_NodeChecked;
        }

        private void SfTreeView_NodeChecked(object? sender, Syncfusion.UI.Xaml.TreeView.NodeCheckedEventArgs e)
        {
            if (e.Node != null && bool.TryParse(e.Node.IsChecked.ToString(), out bool result))
            {
                foreach (var item in e.Node.ChildNodes)
                    item.IsChecked = result;
            }
        }
    }
}
