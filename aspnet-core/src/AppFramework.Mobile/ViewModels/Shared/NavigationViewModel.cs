namespace AppFramework.Shared.ViewModels
{ 
    using Prism.Commands;
    using Prism.Ioc;
    using Prism.Navigation;
    using System.Threading.Tasks;

    /// <summary>
    /// 页面详细信息导航
    /// </summary>
    public class NavigationViewModel : ViewModelBase, INavigationAware
    {
        public readonly INavigationService navigationService;
        public DelegateCommand GoBackCommand { get; private set; }

        public NavigationViewModel()
        {
            navigationService = ContainerLocator.Container.Resolve<INavigationService>();
            GoBackCommand = new DelegateCommand(async () => await GoBackAsync());
        }

        /// <summary>
        /// 导航开始
        /// </summary>
        /// <param name="parameters"></param>
        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        { }

        /// <summary>
        /// 导航到达
        /// </summary>
        /// <param name="parameters"></param>
        public virtual void OnNavigatedTo(INavigationParameters parameters)
        { }

        /// <summary>
        /// 返回上一页
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public virtual async Task GoBackAsync()
        {
            await navigationService.GoBackAsync(null);
        }
    }
}