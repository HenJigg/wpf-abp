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

    /// <summary>
    /// 应用首页
    /// </summary>
    public class MainViewModel : BaseDialogViewModel
    {
        public MainViewModel(IContainerProvider containerProvider) : base(containerProvider)
        {
            OpenPageCommand = new AsyncRelayCommand<string>(OpenPage);
            ClosePageCommand = new RelayCommand<string>(ClosePage);
            ExpandMenuCommand = new RelayCommand(() =>
            {
                for (int i = 0; i < ModuleManager.ModuleGroups.Count; i++)
                {
                    var arg = ModuleManager.ModuleGroups[i];
                    arg.ContractionTemplate = !arg.ContractionTemplate;
                }
                WeakReferenceMessenger.Default.Send("", "ExpandMenu");
            });
        }

        #region Property

        private ModuleUIComponent currentModule;

        /// <summary>
        /// 当前选中模块
        /// </summary>
        public ModuleUIComponent CurrentModule
        {
            get { return currentModule; }
            set { currentModule = value; OnPropertyChanged(); }
        }


        private ObservableCollection<ModuleUIComponent> moduleList;

        /// <summary>
        /// 所有展开的模块
        /// </summary>
        public ObservableCollection<ModuleUIComponent> ModuleList
        {
            get { return moduleList; }
            set { moduleList = value; OnPropertyChanged(); }
        }

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
        /// 菜单栏收缩
        /// </summary>
        public RelayCommand ExpandMenuCommand { get; private set; }

        /// <summary>
        /// 返回首页
        /// </summary>
        public RelayCommand GoHomeCommand { get; private set; }

        /// <summary>
        /// 打开新页面，string: 模块名称
        /// </summary>
        public AsyncRelayCommand<string> OpenPageCommand { get; private set; }

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
        public async virtual Task OpenPage(string pageName)
        {
            await Task.Delay(1);
        }

        /// <summary>
        /// 关闭页面
        /// </summary>
        /// <param name="pageName"></param>
        public void ClosePage(string pageName)
        {
            var module = this.ModuleList.FirstOrDefault(t => t.Name.Equals(pageName));
            if (module != null)
            {
                this.ModuleList.Remove(module);
                if (this.ModuleList.Count > 0)
                    this.CurrentModule = this.ModuleList.Last();
                else
                    this.CurrentModule = null;
            }
        }

        /// <summary>
        /// 初始化页面上下文内容
        /// </summary>
        /// <returns></returns>
        public void InitDefaultView()
        {
            //创建模块管理器
            ModuleManager = new ModuleManager();
            ModuleList = new ObservableCollection<ModuleUIComponent>();
        }
    }
}
