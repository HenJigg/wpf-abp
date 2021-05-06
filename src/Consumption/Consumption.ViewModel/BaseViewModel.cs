
namespace Consumption.ViewModel
{
    using Microsoft.Toolkit.Mvvm.ComponentModel;
    using Microsoft.Toolkit.Mvvm.Input;
    using Microsoft.Toolkit.Mvvm.Messaging;

    /// <summary>
    /// MVVM基类
    /// </summary>
    public class BaseDialogViewModel : ObservableObject
    {
        public BaseDialogViewModel()
        {
            ExecuteCommand = new RelayCommand<string>(Execute);
        }

        public virtual void Execute(string arg)
        {
            switch (arg)
            {
                case "关闭": WeakReferenceMessenger.Default.Send("", "Exit"); break;
            }
        }

        public RelayCommand<string> ExecuteCommand { get; private set; }

        private bool isOpen;

        /// <summary>
        /// 窗口是否显示
        /// </summary>
        public bool DialogIsOpen
        {
            get { return isOpen; }
            set { isOpen = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 通知异常
        /// </summary>
        /// <param name="msg"></param>
        public virtual void SnackBar(string msg) { }
    }
}
