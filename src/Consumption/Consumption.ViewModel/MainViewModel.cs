/*
*
* 文件名    ：MainViewModel                             
* 程序说明  : 首页模块
* 更新时间  : 2020-05-12 21:50
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

namespace Consumption.ViewModel
{
    using Consumption.ViewModel.Common;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;
    using System.Threading.Tasks;

    /// <summary>
    /// 应用首页
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            OpenPageCommand = new RelayCommand<string>(arg => { Messenger.Default.Send(arg, "NavigationNewPage"); });
        }

        public RelayCommand<string> OpenPageCommand { get; private set; }
        private ModuleManager moduleManager;
        private StyleConfig styleConfig;
        private Module currentModule;

        /// <summary>
        /// 个性化设置
        /// </summary>
        public StyleConfig StyleConfig
        {
            get { return styleConfig; }
            set { styleConfig = value; RaisePropertyChanged(); }
        }

        public Module CurrentModule
        {
            get { return currentModule; }
            set { currentModule = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 模块管理器
        /// </summary>
        public ModuleManager ModuleManager
        {
            get { return moduleManager; }
            set { moduleManager = value; RaisePropertyChanged(); }
        }

        public async Task InitDefaultView()
        {
            //个性化配置
            StyleConfig = UserManager.GetStyleConfig();
            //加载模块
            ModuleManager = new ModuleManager();
            await ModuleManager.LoadAssemblyModule();
        }
    }
}
