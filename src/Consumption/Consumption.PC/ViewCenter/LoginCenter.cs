/*
*
* 文件名    ：LoginCenter                             
* 程序说明  : 登录控制类 
* 更新时间  : 2020-05-21 17：26
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
    using Consumption.PC.View;
    using Consumption.Shared.Common;
    using Consumption.Shared.DataInterfaces;
    using Consumption.ViewModel;
    using GalaSoft.MvvmLight.Messaging;
    using MaterialDesignThemes.Wpf;
    using System;

    /// <summary>
    /// 登录控制类
    /// </summary>
    public class LoginCenter : BaseDialogCenter<LoginView, LoginViewModel>
    {
        public override void SubscribeMessenger()
        {
            Messenger.Default.Register<string>(view, "Snackbar", arg =>
            {
                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    var messageQueue = view.SnackbarThree.MessageQueue;
                    messageQueue.Enqueue(arg);
                }));
            });
            Messenger.Default.Register<bool>(view, "NavigationPage", async arg =>
              {
                  var dialog = NetCoreProvider.Get<IModuleDialog>("MainCenter");
                  view.Close();
                  this.UnsubscribeMessenger();
                  await dialog.ShowDialog();
              });
            base.SubscribeMessenger();
        }
    }
}
