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

    /// <summary>
    /// 首页控制类
    /// </summary>
    public class MainCenter : BaseDialogCenter<MaterialDesignMainWindow, MainViewModel>
    {
        public override void SubscribeMessenger()
        {
            //执行菜单模块动画
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
            Messenger.Default.Register<bool>(View, "DisplayView", arg =>
              {
                  ViewModel.DialogIsOpen = arg;
              });
            Messenger.Default.Register<Module>(View, "NavigationNewPage", async m =>
            {
                try
                {
                    ViewModel.DialogIsOpen = true;
                    //临时方案,反射创建实例,缺陷:每次都需要进行反射...
                    var ass = System.Reflection.Assembly.GetEntryAssembly();
                    if (ass.CreateInstance(m.TypeName) is IModule dialog)
                    {
                        if (m.TypeName == View.page.Tag?.ToString()) return;
                        await dialog.BindDefaultModel(m.Auth);
                        View.page.Tag = m.TypeName;
                        View.page.Content = dialog.GetView();
                    }
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
            //Messenger.Default.Register<string>(View, "UpdateBackground", arg =>
            //{
            //    ViewModel.StyleConfig.Url = arg;
            //    //保存用户配置...
            //});
            //Messenger.Default.Register<double>(View, "UpdateTrans", arg =>
            //{
            //    ViewModel.StyleConfig.Trans = arg / 100;
            //    //保存用户配置...
            //});
            //Messenger.Default.Register<double>(View, "UpdateGaussian", arg =>
            //{
            //    ViewModel.StyleConfig.Radius = arg;
            //    //保存用户配置...
            //});
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
        /// 临时固定,后期修改动态绑定 2020-07-10
        /// </summary>
        private void GoHomeView()
        {
            View.page.Tag = "首页";
            View.page.Content = new HomeView();
        }
    }
}
