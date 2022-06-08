namespace AppFramework.ViewModels
{
    using AppFramework.Common;
    using AppFramework.Common.ViewModels;
    using Prism.Commands;
    using Prism.Regions;
    using Prism.Ioc;
    using System.Threading.Tasks;
    using AppFramework.Common.Services.Permission;
    using AppFramework.Services;
    using System;
    using AppFramework.WindowHost;
    using AppFramework.Views;

    public class NavigationViewModel : ViewModelBase, INavigationAware
    {
        public NavigationViewModel()
        {
            proxyService = ContainerLocator.Container.Resolve<IPermissionPorxyService>();
            ExecuteCommand = new DelegateCommand<string>(proxyService.Execute);
            dialog = ContainerLocator.Container.Resolve<IHostDialogService>();
            RefreshCommand = new DelegateCommand(async () => await RefreshAsync());
        }

        public readonly IHostDialogService dialog;
        public IPermissionPorxyService proxyService { get; private set; }

        public DelegateCommand RefreshCommand { get; private set; }
        public DelegateCommand<string> ExecuteCommand { get; private set; }

        #region INavigationAware

        /// <summary>
        /// 导航目标是否重新利用原来的对象
        /// </summary>
        /// <param name="navigationContext"></param>
        /// <returns></returns>
        public virtual bool IsNavigationTarget(NavigationContext navigationContext) => false;

        /// <summary>
        /// 导航发生变更时
        /// </summary>
        /// <param name="navigationContext"></param>
        public virtual void OnNavigatedFrom(NavigationContext navigationContext) { }

        /// <summary>
        /// 导航到达时
        /// </summary>
        /// <param name="navigationContext"></param>
        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            proxyService.Generate(GetDefaultPermissionItems());
            await OnNavigatedToAsync(navigationContext);
        }

        #endregion

        /// <summary>
        /// 创建模块具备的默认权限选项清单
        /// </summary>
        /// <returns></returns>
        public virtual PermissionItem[] GetDefaultPermissionItems() => new PermissionItem[0];

        /// <summary>
        /// 导航到达时的异步方法
        /// </summary>
        /// <param name="navigationContext"></param>
        /// <returns></returns>
        public virtual async Task OnNavigatedToAsync(NavigationContext navigationContext)
        {
            await RefreshAsync();
        }

        /// <summary>
        /// 异步刷新方法,当页面导航到达时触发该方法
        /// </summary>
        /// <returns></returns>
        public virtual async Task RefreshAsync() => await Task.CompletedTask; 
    }
}
