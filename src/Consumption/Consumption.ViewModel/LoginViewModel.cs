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
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Consumption.ViewModel.Interfaces;
    using Consumption.Shared.Common;
    using Consumption.Shared.DataModel;
    using Consumption.Shared.Dto;
    using Newtonsoft.Json;
    using Microsoft.Toolkit.Mvvm.Input;
    using Microsoft.Toolkit.Mvvm.Messaging;
    using Org.BouncyCastle.Crypto.Engines;

    /// <summary>
    /// 登录模块
    /// </summary>
    public class LoginViewModel : BaseDialogViewModel, IBaseDialog
    {
        public LoginViewModel()
        {
            this.repository = NetCoreProvider.Get<IUserRepository>();
            LoginCommand = new RelayCommand(Login);
        }

        #region Property
        private string userName;
        private string passWord;
        private string report;
        private string isCancel;
        private readonly IUserRepository repository;

        public string UserName
        {
            get { return userName; }
            set { userName = value; OnPropertyChanged(); }
        }

        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; OnPropertyChanged(); }
        }

        public string Report
        {
            get { return report; }
            set { report = value; OnPropertyChanged(); }
        }

        public string IsCancel
        {
            get { return isCancel; }
            set { isCancel = value; OnPropertyChanged(); }
        }
        #endregion

        #region Command

        public RelayCommand LoginCommand { get; private set; }

        /// <summary>
        /// 登录系统
        /// </summary>
        private async void Login()
        {
            try
            {
                if (DialogIsOpen) return;
                if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(PassWord))
                {
                    SnackBar("请输入用户名密码!");
                    return;
                }
                DialogIsOpen = true;
                await Task.Delay(300);
                var loginResult = await repository.LoginAsync(UserName, PassWord);

                if (loginResult.StatusCode != 200)
                {
                    SnackBar(loginResult.Message);
                    return;
                }
                var authResult = await repository.GetAuthListAsync();
                if (authResult.StatusCode != 200)
                {
                    SnackBar(authResult.Message);
                    return;
                }

                var userDto = JsonConvert.DeserializeObject<UserInfoDto>(loginResult.Result.ToString());

                #region 关联用户信息/缓存

                Contract.Account = userDto.User.Account;
                Contract.UserName = userDto.User.UserName;
                Contract.IsAdmin = userDto.User.FlagAdmin == 1;
                Contract.Menus = userDto.Menus; //用户包含的权限信息
                Contract.AuthItems = JsonConvert.DeserializeObject<List<AuthItem>>(authResult.Result.ToString());

                #endregion
                //这行代码会发射到首页,Center中会定义所有的Messenger
                WeakReferenceMessenger.Default.Send(string.Empty, "NavigationPage");
            }
            catch (Exception ex)
            {
                SnackBar(ex.Message);
            }
            finally
            {
                DialogIsOpen = false;
            }
        }

        #endregion

        public override void Exit()
        {
            WeakReferenceMessenger.Default.Send(string.Empty, "Exit");
        }
    }
}
