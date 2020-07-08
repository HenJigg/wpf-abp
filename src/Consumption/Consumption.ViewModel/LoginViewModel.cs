/*
*
* 文件名    ：LoginViewModel                             
* 程序说明  : 登录模块
* 更新时间  : 2020-05-27 15:46
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
    using Consumption.Core.IService;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// 登录模块
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        private readonly IConsumptionService service;
        public LoginViewModel()
        {
            service = AutofacProvider.Get<IConsumptionService>();
            LoginCommand = new RelayCommand(Login);
            LogoutCommand = new RelayCommand(LogOut);
        }

        #region Property
        private string userName;
        private string passWord;
        private string report;
        private string isCancel;

        public string UserName
        {
            get { return userName; }
            set { userName = value; RaisePropertyChanged(); }
        }

        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; RaisePropertyChanged(); }
        }

        public string Report
        {
            get { return report; }
            set { report = value; RaisePropertyChanged(); }
        }

        public string IsCancel
        {
            get { return isCancel; }
            set { isCancel = value; RaisePropertyChanged(); }
        }
        #endregion

        #region Command

        public RelayCommand LoginCommand { get; private set; }
        public RelayCommand LogoutCommand { get; private set; }

        /// <summary>
        /// 登录系统
        /// </summary>
        private async void Login()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(PassWord))
                {
                    this.Report = "请输入用户名密码!";
                    return;
                }
                UpdateDialog(true, "验证登陆中...");
                var r = await service.LoginAsync(UserName, PassWord);
                if (r == null || !r.success)
                {
                    this.Report = r == null ? "远程服务器无法连接！" : r.message;
                    return;
                }
                UpdateDialog(true, "加载首页...");
                Messenger.Default.Send(true, "NavigationPage");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            finally
            {
                UpdateDialog(false);
            }
        }

        /// <summary>
        /// 登出系统
        /// </summary>
        private void LogOut()
        {
            Messenger.Default.Send(true, "LogOut");
        }

        #endregion
    }
}
