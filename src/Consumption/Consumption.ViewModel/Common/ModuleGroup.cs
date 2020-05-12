/*
*
* 文件名    ：ModuleGroup                             
* 程序说明  : 定义分组模块的数据结构
* 更新时间  : 2020-05-12 21：09
* 
* 
*
*/


namespace Consumption.ViewModel.Common
{
    using GalaSoft.MvvmLight;
    using System.Collections.ObjectModel;

    /// <summary>
    /// 模块分组
    /// </summary>
    public class ModuleGroup : ViewModelBase
    {
        private string groupName;
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
        /// 包含的子模块
        /// </summary>
        public ObservableCollection<Module> Modules
        {
            get { return modules; }
            set { modules = value; RaisePropertyChanged(); }
        }
    }
}
