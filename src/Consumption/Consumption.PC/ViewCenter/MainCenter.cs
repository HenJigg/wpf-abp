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
    using Consumption.ViewModel;
    using System;
    using System.Runtime;
    using System.Threading.Tasks;
    using Consumption.PC.Common;
    using Consumption.PC.Template;
    using MaterialDesignThemes.Wpf;
    using System.Linq;
    using Consumption.ViewModel.Common;
    using Consumption.ViewModel.Interfaces;
    using Consumption.Shared.Common;
    using Consumption.ViewModel.Common.Aop;
    using Microsoft.Toolkit.Mvvm.Messaging;

    /// <summary>
    /// 首页控制类
    /// </summary>
    public class MainCenter : BaseDialogCenter<MaterialDesignMainWindow, MainViewModel>
    {
        public override void SubscribeMessenger()
        {
            //非阻塞式窗口提示消息
            WeakReferenceMessenger.Default.Register<string, string>(view, "Snackbar", (sender, arg) =>
             {
                 var messageQueue = view.SnackbarThree.MessageQueue;
                 messageQueue.Enqueue(arg);
             });
            //阻塞式窗口提示消息
            WeakReferenceMessenger.Default.Register<MsgInfo, string>(view, "UpdateDialog", (sender, m) =>
                {
                    if (m.IsOpen)
                        _ = DialogHost.Show(new SplashScreenView()
                        {
                            DataContext = new { Msg = m.Msg }
                        }, "Root");
                    else
                    {
                        DialogHost.Close("Root");
                    }
                });
            //菜单执行相关动画及模板切换
            WeakReferenceMessenger.Default.Register<string, string>(view, "ExpandMenu", (sender, arg) =>
              {
                  if (view.MENU.Width < 200)
                      AnimationHelper.CreateWidthChangedAnimation(view.MENU, 60, 200, new TimeSpan(0, 0, 0, 0, 300));
                  else
                      AnimationHelper.CreateWidthChangedAnimation(view.MENU, 200, 60, new TimeSpan(0, 0, 0, 0, 300));

                  //由于...
                  var template = view.IC.ItemTemplateSelector;
                  view.IC.ItemTemplateSelector = null;
                  view.IC.ItemTemplateSelector = template;
              });
            base.SubscribeMessenger();
        }

        /// <summary>
        /// 首页重写弹窗-
        /// </summary>
        /// <returns></returns>
        public override async Task<bool> ShowDialog()
        {
            await viewModel.InitDefaultView();
            return await base.ShowDialog();
        }
    }
}
