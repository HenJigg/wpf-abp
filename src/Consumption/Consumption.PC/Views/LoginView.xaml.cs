namespace Consumption.PC.Views
{
    using Consumption.ViewModel.Common.Events;
    using Prism.Events;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView(IEventAggregator eventAggregator)
        {
            InitializeComponent();
        }
    }
}
