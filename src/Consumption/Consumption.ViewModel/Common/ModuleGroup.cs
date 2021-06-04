
namespace Consumption.ViewModel.Common
{
    using Microsoft.Toolkit.Mvvm.ComponentModel;
    using Prism.Mvvm;
    using System.Collections.ObjectModel;

    /// <summary>
    /// 模块分组
    /// </summary>
    public class ModuleGroup : BindableBase
    {
        private string groupName;
        private bool contractionTemplate = true;
        private ObservableCollection<Module> modules;

        /// <summary>
        /// 组名称
        /// </summary>
        public string GroupName
        {
            get { return groupName; }
            set { groupName = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 收缩面板-模板
        /// </summary>
        public bool ContractionTemplate
        {
            get { return contractionTemplate; }
            set { contractionTemplate = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 包含的子模块
        /// </summary>
        public ObservableCollection<Module> Modules
        {
            get { return modules; }
            set { modules = value; RaisePropertyChanged(); }
        }
    }
}
