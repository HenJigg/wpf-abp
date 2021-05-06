
namespace Consumption.ViewModel
{
    using Consumption.Shared.Common.Events;
    using Microsoft.Toolkit.Mvvm.ComponentModel;
    using Microsoft.Toolkit.Mvvm.Input;
    using Microsoft.Toolkit.Mvvm.Messaging;
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
                case "最小化": aggregator.GetEvent<StringMessageEvent>().Publish("Min"); break;
                case "最大化": aggregator.GetEvent<StringMessageEvent>().Publish("Max"); break;
                case "关闭": aggregator.GetEvent<StringMessageEvent>().Publish("Exit"); break;
            }
        }

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
