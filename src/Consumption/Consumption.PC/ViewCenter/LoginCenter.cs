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
    using Consumption.Core.Common;
    using Consumption.Core.Interfaces;
    using Consumption.PC.View;
    using Consumption.ViewModel;
    using GalaSoft.MvvmLight.Messaging;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    /// <summary>
    /// 登录控制类
    /// </summary>
    public class LoginCenter : BaseDialogCenter<LoginView, LoginViewModel>
    {
        public override Task<bool> ShowDialog()
        {
            if (View.DataContext == null)
            {
                this.RegisterMessenger();
                this.RegisterDefaultEvent();
                this.BindDefaultViewModel();
            }
            var result = View.ShowDialog();
            return Task.FromResult((bool)result);
        }

        public override void RegisterMessenger()
        {
            Messenger.Default.Register<bool>(View, "NavigationHome", arg =>
             {
                 View.Close(); //Close LoginView
                 var mainView = AutofacProvider.Get<IModuleDialog>("MainCenter"); //Get MainView Examples
                 mainView.ShowDialog(); //Show MainView
             });
            Messenger.Default.Register<bool>(View, "Exit", arg =>
            {
                View.Close();
            });
        }
    }
}
