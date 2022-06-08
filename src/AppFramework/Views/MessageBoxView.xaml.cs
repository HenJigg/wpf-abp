using AppFramework.Services;
using System.Windows.Controls;

namespace AppFramework.Views
{
    public partial class MessageBoxView : UserControl
    {
        public MessageBoxView(IThemeService themeService)
        {
            InitializeComponent(); 
            themeService.SetCurrentTheme(this); 
        }
    }
}
