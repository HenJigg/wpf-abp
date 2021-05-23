using System.Threading.Tasks;
using Consumption.ViewModel.Interfaces;
using Consumption.Shared.Common;
using System;
using Prism.Ioc;
using Prism.Events;
using Consumption.ViewModel.Common.Events;
using Consumption.ViewModel.Common;
using Prism.Services.Dialogs;

namespace Consumption.ViewModel
{
    /// <summary>
    /// 登录模块
    /// </summary>
    public class LoginBaseViewModel : BaseDialogViewModel, IDialogAware, ILoginViewModel
    {
        #region Property

        private string userName;
        private string passWord;
        private string report;
        private string isCancel;
        private readonly IUserRepository repository;
        private readonly IEventAggregator eventAggregator;



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

        public LoginBaseViewModel(IUserRepository repository, IContainerProvider containerProvider)
            : base(containerProvider)
        {
            this.repository = repository;
            this.eventAggregator = containerProvider.Resolve<IEventAggregator>();
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
                Contract.AuthItems = authResult.Result;

                #endregion

                RequestClose?.Invoke(new DialogResult
                    (ButtonResult.OK, new DialogParameters()
                    {
                        { "Value", loginResult.Result.Menus}
                    }));
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

        #region IDialogService

        public string Title => "Login";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        { }

        public void OnDialogOpened(IDialogParameters parameters)
        { }

        #endregion
    }
}
