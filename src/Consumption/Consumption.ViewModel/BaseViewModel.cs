/*
*
* 文件名    ：BaseViewModel                             
* 程序说明  : MVVM基类
* 更新时间  : 2020-05-30 14：27
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
    using Consumption.Core.Common;
    using Consumption.Core.Interfaces;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// MVVM基类
    /// </summary>
    public class BaseViewModel : ViewModelBase, IDataInitialize
    {
        public BaseViewModel()
        {
            CloseCommand = new RelayCommand(() =>
              {
                  Messenger.Default.Send(true, "Exit");
              });
        }
        public RelayCommand CloseCommand { get; private set; }

        public virtual Task InitDefaultViewData()
        {
            return Task.FromResult(true);
        }

        private bool isOpen;
        private string dialogMsg;

        /// <summary>
        /// 窗口是否显示
        /// </summary>
        public bool DialogIsOpen
        {
            get { return isOpen; }
            set { isOpen = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 弹出窗口
        /// </summary>
        public string DialogMsg
        {
            get { return dialogMsg; }
            set { dialogMsg = value; RaisePropertyChanged(); }
        }

        #region Loading

        /// <summary>
        /// 更新状态信息
        /// </summary>
        /// <param name="isOpen"></param>
        /// <param name="msg"></param>
        public void UpdateDialog(bool isOpen = true, string msg = "")
        {
            Messenger.Default.Send(new MsgInfo() { IsOpen = isOpen, Msg = msg }, "UpdateDialog");
        }

        #endregion
    }
}
