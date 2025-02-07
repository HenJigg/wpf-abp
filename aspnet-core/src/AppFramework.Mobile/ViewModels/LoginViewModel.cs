using Abp.Localization;
using Abp.MultiTenancy;
using Acr.UserDialogs;
using AppFramework.Shared.Services.Account;
using AppFramework.Shared.Services.Storage;
using AppFramework.ApiClient;
using AppFramework.Authorization.Accounts;
using AppFramework.Authorization.Accounts.Dto;
using MvvmHelpers;
using Prism.Commands;
using Prism.Regions.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AppFramework.Shared.Extensions;
using AppFramework.Shared.Views;

namespace AppFramework.Shared.ViewModels
{
    public class LoginViewModel : RegionViewModel
    {
        public LoginViewModel(IAccountStorageService dataStorageService,
            IAccountService accountService,
            IAccountAppService accountAppService, 
            IApplicationContext appContext)
        {
            this.appContext = appContext;  
            this.accountService = accountService;
            this.dataStorageService = dataStorageService;
            this.accountAppService = accountAppService; 

            ExecuteCommand = new DelegateCommand<string>(Execute);
        }

        #region 字段/属性

        private readonly IAccountService accountService;
        private readonly IAccountAppService accountAppService;
        private readonly IAccountStorageService dataStorageService; 
        private readonly IApplicationContext appContext;  
        private LanguageInfo selectedLanguage;
        private ObservableCollection<LanguageInfo> languages;
        private string tenancyName;
        private bool isLoginEnabled;
        private bool initialize;
        public bool IsMultiTenancyEnabled { get; set; }
        public string CurrentTenancyNameOrDefault { get; set; }

        public LanguageInfo SelectedLanguage
        {
            get => selectedLanguage;
            set
            {
                selectedLanguage = value;
                if (initialize) AsyncRunner.Run(ChangeLanguage(value));
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<LanguageInfo> Languages
        {
            get => languages;
            set
            {
                languages = value;
                RaisePropertyChanged();
            }
        }

        public string TenancyName
        {
            get => tenancyName;
            set
            {
                tenancyName = value;
                RaisePropertyChanged();
            }
        }

        public string UserName
        {
            get => accountService.AuthenticateModel.UserNameOrEmailAddress;
            set
            {
                accountService.AuthenticateModel.UserNameOrEmailAddress = value;
                SetLoginButtonEnabled();
                RaisePropertyChanged();
            }
        }

        public string Password
        {
            get => accountService.AuthenticateModel.Password;
            set
            {
                accountService.AuthenticateModel.Password = value;
                SetLoginButtonEnabled();
                RaisePropertyChanged();
            }
        }

        public bool IsLoginEnabled
        {
            get { return isLoginEnabled; }
            set { isLoginEnabled = value; RaisePropertyChanged(); }
        }

        public DelegateCommand<string> ExecuteCommand { get; }
        public DelegateCommand<LanguageInfo> ChangeLanguageCommand { get; }

        #endregion 字段/属性

        #region 用户登录

        private void PopulateLoginInfoFromStorage()
        {
            //从本地存储当中读取账户信息, 可能是用户之前登陆的信息 
            var loginInfo = dataStorageService.RetrieveLoginInfo();
            if (loginInfo == null) return;

            if (loginInfo.User != null)
                UserName = loginInfo.User.UserName;

            if (loginInfo.Tenant != null)
                TenancyName = loginInfo.Tenant.TenancyName;

            if (loginInfo.Tenant == null)
                appContext.SetAsHost();
            else
                appContext.SetAsTenant(TenancyName, loginInfo.Tenant.Id);

            RaisePropertyChanged("CurrentTenancyNameOrDefault");
        }

        private void SetLoginButtonEnabled()
        {
            IsLoginEnabled = !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password);
        }

        private async Task LoginUserAsync()
        {
            await SetBusyAsync(accountService.LoginUserAsync);
        }

        #endregion 用户登录

        #region 系统语言

        protected async Task ChangeLanguage(LanguageInfo languageInfo)
        {
            /*
             *  设置当前的上下文语言,再次向Web服务中以当前语言请求资源,获取对应语言类型的本地化资源
             */
            appContext.CurrentLanguage = languageInfo;
            await AppConfigurationManager.GetAsync();
        }

        #endregion 系统语言

        #region 租户

        private async Task ChangeTenantAsync()
        {
            var promptResult = await UserDialogs.Instance.PromptAsync(new PromptConfig
            {
                Message = Local.Localize(LocalizationKeys.LeaveEmptyToSwitchToHost),
                Text = appContext.CurrentTenant?.TenancyName ?? "",
                OkText = Local.Localize(LocalizationKeys.Ok),
                CancelText = Local.Localize(LocalizationKeys.Cancel),
                Title = Local.Localize(LocalizationKeys.ChangeTenant),
                Placeholder = Local.LocalizeWithThreeDots(LocalizationKeys.TenancyName),
                MaxLength = AbpTenantBase.MaxTenancyNameLength
            });

            if (!promptResult.Ok)
            {
                return;
            }

            if (string.IsNullOrEmpty(promptResult.Text))
            {
                appContext.SetAsHost();
                ApiUrlConfig.ResetBaseUrl();
                RaisePropertyChanged("CurrentTenancyNameOrDefault");
            }
            else
            {
                await SetTenantAsync(promptResult.Text);
            }

            await dataStorageService.StoreTenantInfoAsync(appContext.CurrentTenant);
        }

        private async Task SetTenantAsync(string tenancyName)
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(
                    async () => await accountAppService.IsTenantAvailable(
                        new IsTenantAvailableInput { TenancyName = tenancyName }),
                    result => IsTenantAvailableExecuted(result, tenancyName)
                );
            });
        }

        private async Task IsTenantAvailableExecuted(IsTenantAvailableOutput result, string tenancyName)
        {
            var tenantAvailableResult = result;

            switch (tenantAvailableResult.State)
            {
                case TenantAvailabilityState.Available:
                    appContext.SetAsTenant(tenancyName, tenantAvailableResult.TenantId.Value);
                    ApiUrlConfig.ChangeBaseUrl(tenantAvailableResult.ServerRootAddress);
                    RaisePropertyChanged(CurrentTenancyNameOrDefault);
                    break;

                case TenantAvailabilityState.InActive:
                    UserDialogs.Instance.HideLoading();
                    await UserDialogs.Instance.AlertAsync(Local.Localize(LocalizationKeys.TenantIsNotActive, tenancyName));
                    break;

                case TenantAvailabilityState.NotFound:
                    UserDialogs.Instance.HideLoading();
                    await UserDialogs.Instance.AlertAsync(Local.Localize(LocalizationKeys.ThereIsNoTenantDefinedWithName, tenancyName));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            await Task.CompletedTask;
        }

        #endregion 租户

        #region 忘记密码/发送邮件

        private void ForgotPassword()
        {
            dialogService.ShowDialog(AppViews.ForgotPassword);
        }

        private void EmailActivation()
        {
            dialogService.ShowDialog(AppViews.EmailActivation);
        }

        #endregion 忘记密码/发送邮件

        #region IRegionAware 接口实现

        public override void OnNavigatedTo(INavigationContext navigationContext)
        {
            SetAppLanguageAndSettings();
            PopulateLoginInfoFromStorage();
            initialize = true;
        }

        #endregion

        /// <summary>
        /// 通用的执行命令方法
        /// </summary>
        /// <param name="arg">命名指向类型</param>
        private async void Execute(string arg)
        {
            switch (arg)
            {
                case "LoginUser": await LoginUserAsync(); break;
                case "ChangeTenant": await ChangeTenantAsync(); break;
                case "ForgotPassword": ForgotPassword(); break;
                case "EmailActivation": EmailActivation(); break;
            }
        }

        /// <summary>
        /// 设置应用程序语言及设置
        /// </summary>
        private void SetAppLanguageAndSettings()
        {
            var configuration = appContext.Configuration;
            if (configuration != null)
            {
                Languages = new ObservableRangeCollection<LanguageInfo>(configuration.Localization.Languages);
                SelectedLanguage = Languages.FirstOrDefault(l => l.Name == appContext.CurrentLanguage.Name);

                if (appContext.CurrentTenant != null)
                {
                    IsMultiTenancyEnabled = configuration.MultiTenancy.IsEnabled;
                    CurrentTenancyNameOrDefault = appContext.CurrentTenant.TenancyName;
                }
            }
            else
            {
                CurrentTenancyNameOrDefault = Local.Localize(LocalizationKeys.NotSelected);
            }
        }
    }
}