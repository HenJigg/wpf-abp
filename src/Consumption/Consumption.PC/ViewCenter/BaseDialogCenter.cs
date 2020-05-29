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
    using GalaSoft.MvvmLight;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class BaseDialogCenter<TView, TViewModel> : Consumption.Core.Interfaces.IModuleDialog
        where TView : Window, new()
        where TViewModel : ViewModelBase, new()
    {
        public TView View;
        public TViewModel ViewModel;
        public void BindDefaultViewModel()
        {
            if (ViewModel == null) ViewModel = new TViewModel();
            GetDialog().DataContext = ViewModel;
        }

        public Window GetDialog()
        {
            if (View == null)
                View = new TView();
            return View;
        }

        public void RegisterDefaultEvent()
        {
            var window = GetDialog();
            window.MouseDown += (sender, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    window.DragMove();
            };
        }

        public Task<bool> ShowDialog()
        {
            var window = GetDialog();
            if (window.DataContext == null)
            {
                this.RegisterMessenger();
                this.RegisterDefaultEvent();
                this.BindDefaultViewModel();
            }
            var result = window.ShowDialog();
            return Task.FromResult((bool)result);
        }

        public void BindViewModel<BViewModel>(BViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public virtual void Close()
        {
            throw new NotImplementedException();
        }

        public void Register()
        {
            throw new NotImplementedException();
        }

        public virtual void RegisterMessenger()
        {
            throw new NotImplementedException();
        }
    }
}
