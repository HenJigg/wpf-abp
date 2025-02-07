using Xamarin.Forms;

namespace AppFramework.Shared.Views
{
    public partial class RoleDetailsView : ContentPage
    {
        public RoleDetailsView()
        {
            InitializeComponent();
            sfTreeView.NodeChecked+=SfTreeView_NodeChecked;
        }

        private void SfTreeView_NodeChecked(object sender, Syncfusion.XForms.TreeView.NodeCheckedEventArgs e)
        {
            if (e.Node != null && bool.TryParse(e.Node.IsChecked.ToString(), out bool result))
            {
                foreach (var item in e.Node.ChildNodes)
                    item.IsChecked = result;
            }
        }
    }
}