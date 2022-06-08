namespace AppFramework.ViewModels
{
    using Prism.Ioc;
    using Prism.Services.Dialogs;
    using Prism.Commands;

    public class NavigationCurdViewModel : NavigationViewModel
    {
        public NavigationCurdViewModel()
        {
            AddCommand = new DelegateCommand(Add);
            //数据分页服务
            dataPager = ContainerLocator.Container.Resolve<IDataPagerService>();
        }

        public DelegateCommand AddCommand { get; private set; }
        public IDataPagerService dataPager { get; private set; }

        /// <summary>
        /// 新增
        /// </summary>
        public async void Add()
        {
            var dialogResult = await dialog.ShowDialogAsync(GetPageName("Details"));
            if (dialogResult.Result == ButtonResult.OK)
                await RefreshAsync();
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
                await RefreshAsync();
        }

        /// <summary>
        /// 获取弹出页名称
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        private string GetPageName(string methodName) => this.GetType().Name.Replace("ViewModel", $"{methodName}View");
    }
}
