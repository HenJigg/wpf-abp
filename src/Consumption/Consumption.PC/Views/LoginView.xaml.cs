namespace Consumption.PC.Views
{
    using Consumption.ViewModel.Common.Events;
    using Prism.Events;
    using System.Windows;

    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            this.MouseDown += (s, e) =>
            {
                if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                    this.DragMove();
            };
            eventAggregator.GetEvent<LoginEvent>().Subscribe(LoginEvent);
        }

        /// <summary>
        /// 登录验证返回事件
        /// </summary>
        /// <param name="sender"></param>
        void LoginEvent(bool result)
        {
            this.DialogResult = result;
        }
    }
}
