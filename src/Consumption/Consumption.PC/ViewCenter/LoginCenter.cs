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
    using Consumption.ViewModel.Interfaces;
    using MaterialDesignThemes.Wpf;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Toolkit.Mvvm.Input;
    using Microsoft.Toolkit.Mvvm.Messaging;
    using System;

    /// <summary>
    /// 登录控制类
    /// </summary>
    public class LoginCenter : BaseDialogCenter<LoginView>, ILoginCenter
    {
        public LoginCenter(ILoginViewModel viewModel) : base(viewModel) { }

        public override void SubscribeMessenger()
        {
            WeakReferenceMessenger.Default.Register<string, string>(view, "Snackbar", (sender, arg) =>
             {
                 App.Current.Dispatcher.BeginInvoke(new Action(() =>
                 {
                     var messageQueue = view.SnackbarThree.MessageQueue;
                     messageQueue.Enqueue(arg);
                 }));
             });
            WeakReferenceMessenger.Default.Register<string, string>(view, "NavigationPage", async (sender, arg) =>
               {
                   var dialog = NetCoreProvider.ResolveNamed<IMainCenter>("MainCenter");
                   this.UnsubscribeMessenger();
                   view.Close();
                   await dialog.ShowDialog();
               });
            base.SubscribeMessenger();
        }

        public override void UnsubscribeMessenger()
        {
            base.UnsubscribeMessenger();
        }
    }
}
