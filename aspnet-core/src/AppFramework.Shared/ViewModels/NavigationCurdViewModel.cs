namespace AppFramework.Shared
{
    using Prism.Ioc;
    using Prism.Services.Dialogs;
    using Prism.Commands; 
    using AppFramework.Shared.Services.Permission;
    using AppFramework.Shared.Services;
    using CommunityToolkit.Mvvm.Input;

    public partial class NavigationCurdViewModel : NavigationViewModel
    {
        public NavigationCurdViewModel()
        { 
            //数据分页服务
            dataPager = ContainerLocator.Container.Resolve<IDataPagerService>();
            proxyService = ContainerLocator.Container.Resolve<IPermissionPorxyService>();
              
            ExecuteCommand = new DelegateCommand<string>(proxyService.Execute);
            proxyService.Generate(CreatePermissionItems());
        }
         
        public DelegateCommand<string> ExecuteCommand { get; private set; }
        public IDataPagerService dataPager { get; private set; }
        public IPermissionPorxyService proxyService { get; private set; }

        /// <summary>
        /// 新增
        /// </summary>
        [RelayCommand]
        public async void Add()
        {
            var dialogResult = await dialog.ShowDialogAsync(GetPageName("Details"));
            if (dialogResult.Result == ButtonResult.OK)
                await OnNavigatedToAsync();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        public async void Edit()
        {
            DialogParameters param = new DialogParameters();
            param.Add("Value", dataPager.SelectedItem);

            var dialogResult = await dialog.ShowDialogAsync(GetPageName("Details"), param);
            if (dialogResult.Result == ButtonResult.OK)
                await OnNavigatedToAsync();
        }

        /// <summary>
        /// 获取弹出页名称
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        private string GetPageName(string methodName) => GetType().Name.Replace("ViewModel", $"{methodName}View");

        /// <summary>
        /// 创建模块具备的默认权限选项清单
        /// </summary>
        /// <returns></returns>
        public virtual PermissionItem[] CreatePermissionItems() => new PermissionItem[0];
    }
}
