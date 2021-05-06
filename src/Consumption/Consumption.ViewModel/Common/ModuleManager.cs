
namespace Consumption.ViewModel.Common
{
    using Consumption.Shared.Common;
    using Consumption.Shared.Common.Enums;
    using Microsoft.Toolkit.Mvvm.ComponentModel;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 模块管理器
    /// </summary>
    public class ModuleManager : ObservableObject
    {
        private ObservableCollection<Module> modules;
        /// <summary>
        /// 已加载模块
        /// </summary>
        public ObservableCollection<Module> Modules
        {
            get { return modules; }
            set { modules = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ModuleGroup> moduleGroups;

        /// <summary>
        /// 已加载模块-分组
        /// </summary>
        public ObservableCollection<ModuleGroup> ModuleGroups
        {
            get { return moduleGroups; }
            set { moduleGroups = value; OnPropertyChanged(); }
        }
    }
}
