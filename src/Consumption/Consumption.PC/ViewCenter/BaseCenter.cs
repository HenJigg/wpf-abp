/*
*
* 文件名    ：BaseCenter                             
* 程序说明  : View/ViewModel 控制基类
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
    using Autofac.Core;
    using Consumption.Core.Entity;
    using GalaSoft.MvvmLight;
    using NLog.Filters;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using IModule = Core.Interfaces.IModule;

    /// <summary>
    /// View/ViewModel 控制基类
    /// </summary>
    /// <typeparam name="TView"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    public class BaseCenter<TView, TViewModel> : IModule
        where TView : UserControl, new()
        where TViewModel : ViewModelBase, new()
    {
        public TView View;
        public TViewModel ViewModel;
        public virtual void BindDefaultModel()
        {
            if (ViewModel == null) ViewModel = new TViewModel();
            GetView().DataContext = ViewModel;
        }

        public void BindViewModel<BViewModel>(BViewModel viewModel) where BViewModel : class, new()
        {
            this.GetView().DataContext = viewModel;
        }

        public UserControl GetView()
        {
            if (View == null)
                View = new TView();
            return View;
        }

        object IModule.GetView()
        {
            return GetView();
        }
    }
}
