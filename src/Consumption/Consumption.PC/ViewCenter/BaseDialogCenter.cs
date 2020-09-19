/*
*
* 文件名    ：BaseDialogCenter                             
* 程序说明  : View/ViewModel 弹出式控制基类
* 更新时间  : 2020-05-21 17：25
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
    using Consumption.Core.Aop;
    using Consumption.ViewModel;
    using Consumption.ViewModel.Common;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Messaging;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Consumption.Core.Interfaces;
    using Consumption.ViewModel.Interfaces;

    public class BaseDialogCenter<TView, TViewModel> : IModuleDialog
        where TView : Window, new()
        where TViewModel : class, new()
    {
        public readonly TView view = new TView();
        public readonly TViewModel viewModel = new TViewModel();

        /// <summary>
        /// 绑定默认ViewModel
        /// </summary>
        protected void BindDefaultViewModel()
        {
            view.DataContext = viewModel;
        }

        /// <summary>
        /// 打开窗口
        /// </summary>
        /// <returns></returns>
        public virtual async Task<bool> ShowDialog()
        {
            this.SubscribeMessenger();
            this.SubscribeEvent();
            this.BindDefaultViewModel();
            var result = view.ShowDialog();
            return await Task.FromResult((bool)result);
        }

        /// <summary>
        /// 注册默认事件
        /// </summary>
        public void SubscribeEvent()
        {
            view.MouseDown += (sender, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    view.DragMove();
            };
        }

        public virtual void SubscribeMessenger()
        {
            //最小化
            Messenger.Default.Register<string>(view, "WindowMinimize", arg =>
            {
                view.WindowState = System.Windows.WindowState.Minimized;
            });
            //最大化
            Messenger.Default.Register<string>(view, "WindowMaximize", arg =>
            {
                if (view.WindowState == System.Windows.WindowState.Maximized)
                    view.WindowState = System.Windows.WindowState.Normal;
                else
                    view.WindowState = System.Windows.WindowState.Maximized;
            });
            //关闭系统
            Messenger.Default.Register<bool>(view, "Exit", async r =>
            {
                if (r)
                    if (!await Msg.Question("确认退出系统?")) return;
                Environment.Exit(0);
            });
        }

        public void UnsubscribeMessenger()
        {
            Messenger.Default.Unregister(view);
        }
    }
}
