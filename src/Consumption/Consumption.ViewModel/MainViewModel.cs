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
            OpenPageCommand = new RelayCommand<Module>(arg => { Messenger.Default.Send(arg, "NavigationNewPage"); });
            GoHomeCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send("", "GoHomePage");
            });
            ExpandMenuCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send("", "ExpandMenu");
            });
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

        /// <summary>
        /// 菜单栏收缩
        /// </summary>
        public RelayCommand ExpandMenuCommand { get; private set; }

        /// <summary>
        /// 返回首页
        /// </summary>
        public RelayCommand GoHomeCommand { get; private set; }

        /// <summary>
        /// 打开新页面
        /// </summary>
        public RelayCommand<Module> OpenPageCommand { get; private set; }

        public async Task InitDefaultView()
        {
            /*
             *  加载首页的程序集模块
             *  1.首先获取本机的所有可用模块
             *  2.利用服务器验证,过滤掉不可用模块
             *
             *  注:理论上管理员应该可用本机的所有模块, 
             *  当检测本机用户属于管理员,则不向服务器验证
             */ 

            //创建模块管理器
            ModuleManager = new ModuleManager();

            //加载自身的程序集模块
            await ModuleManager.LoadAssemblyModule();
        }
    }
}
