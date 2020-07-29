/*
*
* 文件名    ：MainCenter                             
* 程序说明  : 首页控制类 
* 更新时间  : 2020-05-21 17：31
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

namespace Consumption.PC.ViewCenter
{
    using Consumption.Core.Response;
    using Consumption.Core.Interfaces;
    using Consumption.PC.View;
    using Consumption.ViewModel;
    using GalaSoft.MvvmLight.Messaging;
    using System;
    using System.Runtime;
    using System.Threading.Tasks;
    using Module = ViewModel.Common.Module;
    using Consumption.PC.Common;
    using Consumption.Core.Common;
    using Consumption.Common.Contract;
    using Consumption.PC.Template;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// 首页控制类
    /// </summary>
    public class MainCenter : BaseDialogCenter<MaterialDesignMainWindow, MainViewModel>
    {
        public override void SubscribeMessenger()
        {
            //执行菜单模块动画
            Messenger.Default.Register<string>(View, "WindowMinimized", arg =>
            {
                View.WindowState = System.Windows.WindowState.Minimized;
            });
            Messenger.Default.Register<string>(View, "ExpandMenu", arg =>
            {
                if (View.grdLeftMenu.Width < 200)
                    AnimationHelper
                    .CreateWidthChangedAnimation(View.grdLeftMenu, 60, 200, new TimeSpan(0, 0, 0, 0, 250));
                else
                    AnimationHelper
                    .CreateWidthChangedAnimation(View.grdLeftMenu, 200, 60, new TimeSpan(0, 0, 0, 0, 250));
            });
            //执行返回首页
            Messenger.Default.Register<string>(View, "GoHomePage", arg =>
            {
                GoHomeView();
            });
            Messenger.Default.Register<Module>(View, "NavigationNewPage", async m =>
            {
                try
                {
                    if (m.TypeName == View.page.Tag?.ToString()) return;
                    _ = DialogHost.Show(new SplashScreenView()
                    {
                        DataContext = new { Msg = "正在打开页面..." }
                    }, "Root");
                    ViewModel.DialogIsOpen = true;
                    await Task.Delay(100);
                    //将数据库中获取的菜单Namespace在容器当中查找依赖关系的实例
                    NetCoreProvider.Get(m.TypeName, out IModule dialog);
                    if (dialog == null) return;
                    await dialog.BindDefaultModel(m.Auth);
                    View.page.Tag = m.TypeName;
                    View.page.Content = dialog.GetView();
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                }
                finally
                {
                    ViewModel.DialogIsOpen = false; //关闭等待窗口
                    GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                    GC.Collect();
                }
            });
        }

        /// <summary>
        /// 首页重写弹窗-
        /// </summary>
        /// <returns></returns>
        public override async Task<bool> ShowDialog()
        {
            await ViewModel.InitDefaultView();
            GoHomeView();
            return await base.ShowDialog();
        }

        /// <summary>
        /// 临时固定,后期修改动态绑定 2020-07-19
        /// </summary>
        private void GoHomeView()
        {
            HomeCenter center = new HomeCenter();
            center.BindDefaultModel();
            View.page.Tag = center.ViewModel.SelectPageTitle;
            View.page.Content = center.View;
        }
    }
}
