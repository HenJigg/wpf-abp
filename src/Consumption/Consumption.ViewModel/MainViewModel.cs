/*
*
* 文件名    ：MainViewModel                             
* 程序说明  : 首页模块
* 更新时间  : 2020-05-12 21:50
* 
* 
*
*/

namespace Consumption.ViewModel
{
    using Consumption.ViewModel.Common;
    using GalaSoft.MvvmLight;

    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            ModuleManager = new ModuleManager();
        }

        private ModuleManager moduleManager;

        /// <summary>
        /// 模块管理器
        /// </summary>
        public ModuleManager ModuleManager
        {
            get { return moduleManager; }
            set { moduleManager = value; RaisePropertyChanged(); }
        }
    }
}
