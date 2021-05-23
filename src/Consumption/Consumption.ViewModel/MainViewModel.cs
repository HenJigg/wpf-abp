namespace Consumption.ViewModel
{
    using Consumption.ViewModel.Common;
    using Microsoft.Toolkit.Mvvm.Input;
    using Microsoft.Toolkit.Mvvm.Messaging;
    using Prism.Ioc;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Consumption.Shared.DataModel;
    using System.Collections.Generic;
    using Prism.Regions;

    /// <summary>
    /// 应用首页
    /// </summary>
    public class MainViewModel : BaseDialogViewModel
    {
        private readonly IRegionManager regionManager;
        public MainViewModel(IContainerProvider containerProvider) : base(containerProvider)
        {
            regionManager = containerProvider.Resolve<IRegionManager>();
            OpenPageCommand = new RelayCommand<string>(OpenPage);
            ClosePageCommand = new RelayCommand<string>(ClosePage);
        }

        #region Property

        private ModuleManager moduleManager;

        /// <summary>
        /// 模块管理器
        /// </summary>
        public ModuleManager ModuleManager
        {
            get { return moduleManager; }
            set { moduleManager = value; OnPropertyChanged(); }
        }

        #endregion

        #region Command

        /// <summary>
        /// 返回首页
        /// </summary>
        public RelayCommand GoHomeCommand { get; private set; }

        /// <summary>
        /// 打开新页面，string: 模块名称
        /// </summary>
        public RelayCommand<string> OpenPageCommand { get; private set; }

        /// <summary>
        /// 关闭选择页, string: 模块名称
        /// </summary>
        public RelayCommand<string> ClosePageCommand { get; private set; }

        #endregion

        /// <summary>
        /// 打开页面
        /// </summary>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public void OpenPage(string pageName)
        {
            if (string.IsNullOrWhiteSpace(pageName)) return;

            var region = regionManager.Regions["MainContent"];
            region.RequestNavigate(pageName, arg =>
            {
                //..
            });
        }

        /// <summary>
        /// 关闭页面
        /// </summary>
        /// <param name="pageName"></param>
        public void ClosePage(string pageName)
        {
            //..
        }

        /// <summary>
        /// 初始化页面上下文内容
        /// </summary>
        /// <returns></returns>
        public void InitDefaultView(List<Menu> menus)
        {
            //创建模块管理器
            ModuleManager = new ModuleManager();
            ModuleManager.LoadAssemblyModule(menus);
        }
    }
}
