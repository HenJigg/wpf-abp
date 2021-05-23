
namespace Consumption.ViewModel
{
    using Consumption.Shared.Common.Events;
    using Microsoft.Toolkit.Mvvm.ComponentModel;
    using Microsoft.Toolkit.Mvvm.Input;
    using Prism.Events;
    using Prism.Ioc;

    /// <summary>
    /// MVVM基类
    /// </summary>
    public class BaseDialogViewModel : ObservableObject
    {
        public BaseDialogViewModel(IContainerProvider containerProvider)
        {
            ExecuteCommand = new RelayCommand<string>(Execute);
            aggregator = containerProvider.Resolve<IEventAggregator>();
            this.containerProvider = containerProvider;
        }

        public virtual void Execute(string arg)
        {
            switch (arg)
            {
                case "最小化": ExecuteEvent("Min"); break;
                case "最大化": ExecuteEvent("Max"); break;
                case "退出系统": ExecuteEvent("Exit"); break;
            }
        }

        /// <summary>
        /// 执行事件消息
        /// </summary>
        /// <param name="command"></param>
        protected void ExecuteEvent(string command) => aggregator.GetEvent<StringMessageEvent>().Publish(command);

        public RelayCommand<string> ExecuteCommand { get; private set; }

        private bool isOpen;
        protected IEventAggregator aggregator;
        private readonly IContainerProvider containerProvider;

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
