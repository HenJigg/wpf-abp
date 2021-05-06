using System.Threading.Tasks;
using Consumption.ViewModel.Interfaces;
using Consumption.Shared.Common;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using Prism.Ioc;

namespace Consumption.ViewModel
{
    /// <summary>
    /// 登录模块
    /// </summary>
    public class LoginViewModelBase : BaseDialogViewModel, ILoginViewModel
    {
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

        public LoginViewModelBase(IUserRepository repository, IContainerProvider containerProvider):base(containerProvider)
        {
            this.repository = repository;
        }

        public override async void Execute(string arg)
        {
            switch (arg)
            {
                case "登录": await Login(); break;
            }
            base.Execute(arg);
        }

        /// <summary>
        /// 登录系统
        /// </summary>
        public virtual async Task Login()
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
                #region 关联用户信息/缓存

                Contract.Account = loginResult.Result.User.Account;
                Contract.UserName = loginResult.Result.User.UserName;
                Contract.IsAdmin = loginResult.Result.User.FlagAdmin == 1;
                Contract.Menus = loginResult.Result.Menus; //用户包含的权限信息
                Contract.AuthItems = authResult.Result;

                #endregion
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
    }
}
