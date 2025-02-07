using Abp.Localization;
using AppFramework.ApiClient;
using AppFramework.Authorization.Accounts;
using AppFramework.Authorization.Accounts.Dto;
using AppFramework.Shared;
using AppFramework.Admin.Services;
using AppFramework.Admin.Services.Account;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AppFramework.Shared.Services;

namespace AppFramework.Admin.ViewModels
{
    public class LoginViewModel : DialogViewModel
    {
        #region 字段/属性

        public DelegateCommand<string> ExecuteCommand { get; }
        public DelegateCommand<LanguageInfo> ChangeLanguageCommand { get; }

        private readonly IHostDialogService dialogService;
        private readonly IAccountService accountService;
        private readonly IAccountAppService accountAppService;
        private readonly IApplicationContext applicationContext;
        private readonly IAccountStorageService dataStorageService;
        private readonly IDataStorageService storageService;
        private string tenancyName;
        private bool isLoginEnabled;
        private bool isRememberMe;
        private string currentTenancyNameOrDefault;

        public string CurrentTenancyNameOrDefault
        {
            get => currentTenancyNameOrDefault;
            set
            {
                currentTenancyNameOrDefault = value;
                OnPropertyChanged();
            }
        }
        private LanguageInfo selectedLanguage;
        private ObservableCollection<LanguageInfo> languages;

        public bool IsMultiTenancyEnabled { get; set; }

        public LanguageInfo SelectedLanguage
        {
            get => selectedLanguage;
            set
            {
                selectedLanguage = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<LanguageInfo> Languages
        {
            get => languages;
            set
            {
                languages = value;
                OnPropertyChanged();
            }
        }

        public string TenancyName
        {
            get => tenancyName;
            set
            {
                tenancyName = value;
                OnPropertyChanged();
            }
        }

        public string UserName
        {
            get => accountService.AuthenticateModel.UserNameOrEmailAddress;
            set
            {
                accountService.AuthenticateModel.UserNameOrEmailAddress = value;
                SetLoginButtonEnabled();
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => accountService.AuthenticateModel.Password;
            set
            {
                accountService.AuthenticateModel.Password = value;
                SetLoginButtonEnabled();
                OnPropertyChanged();
            }
        }

        public bool IsLoginEnabled
        {
            get { return isLoginEnabled; }
            set { isLoginEnabled = value; OnPropertyChanged(); }
        }

        public bool IsRememberMe
        {
            get { return isRememberMe; }
            set { isRememberMe = value; OnPropertyChanged(); }
        }

        #endregion

        public LoginViewModel(IHostDialogService dialogService,
            IAccountService accountService,
            IAccountAppService accountAppService,
            IApplicationContext applicationContext,
            IAccountStorageService dataStorageService,
            IDataStorageService storageService)
        {
            this.dialogService = dialogService;
            this.accountService = accountService;
            this.accountAppService = accountAppService;
            this.applicationContext = applicationContext;
            this.dataStorageService = dataStorageService;
            this.storageService = storageService;
            ExecuteCommand = new DelegateCommand<string>(Execute);
            ChangeLanguageCommand = new DelegateCommand<LanguageInfo>(ChangeLanguage);
        }

        private async void Execute(string arg)
        {
            switch (arg)
            {
                case "LoginUser": await LoginUserAsync(); break;
                case "ChangeTenant": ChangeTenantAsync(); break;
                case "ForgotPassword": ForgotPasswordAsync(); break;
                case "EmailActivation": EmailActivationAsync(); break;
            }
        }

        #region 忘记密码/激活邮件/修改语言/登录/租户

        public void SetLoginButtonEnabled()
        {
            IsLoginEnabled = !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password);
        }

        public async void ForgotPasswordAsync()
        {
            await dialogService.ShowDialogAsync(AppViews.ForgotPassword, null, AppSharedConsts.LoginIdentifier);
        }

        public async void EmailActivationAsync()
        {
            await dialogService.ShowDialogAsync(AppViews.EmailActivation, null, AppSharedConsts.LoginIdentifier);
        }

        public async void ChangeLanguage(LanguageInfo languageInfo)
        {
            applicationContext.CurrentLanguage = languageInfo;

            await SetBusyAsync(async () =>
             {
                 await UserConfigurationManager.GetAsync().WebAsync(async () =>
                  {
                      OnDialogClosed(ButtonResult.Retry);
                      await Task.CompletedTask;
                  });
             });
        }

        public async void ChangeTenantAsync()
        {
            var dialogResult = await dialogService.ShowDialogAsync(AppViews.SelectTenant, null, AppSharedConsts.LoginIdentifier);

            if (dialogResult.Result == ButtonResult.OK)
            {
                CurrentTenancyNameOrDefault = dialogResult.Parameters.GetValue<string>("Value");
                await SetTenantAsync(CurrentTenancyNameOrDefault);
            }
            else
                applicationContext.SetAsTenant(null, 0);
        }

        private async Task LoginUserAsync()
        { 
            await SetBusyAsync(async () =>
            {
                await accountService.LoginUserAsync().WebAsync(LoginUserSuccessed);
            });
        }

        private async Task LoginUserSuccessed()
        {
            //记住密码？ 
            storageService.SetValue(nameof(UserName), IsRememberMe ? UserName : null);
            storageService.SetValue(nameof(Password), IsRememberMe ? Password : null, true);

            //清理
            UserName = string.Empty;
            Password = string.Empty;

            OnDialogClosed();
            await Task.CompletedTask;
        }

        public async Task SetTenantAsync(string tenancyName)
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => accountAppService.IsTenantAvailable(
                        new IsTenantAvailableInput { TenancyName = tenancyName }),
                    result => IsTenantAvailableExecuted(result, tenancyName)
                );
            });
        }

        public async Task IsTenantAvailableExecuted(IsTenantAvailableOutput result, string tenancyName)
        {
            var tenantAvailableResult = result;
            IsBusy = false;

            switch (tenantAvailableResult.State)
            {
                case TenantAvailabilityState.Available:
                    applicationContext.SetAsTenant(tenancyName, tenantAvailableResult.TenantId.Value);
                    ApiUrlConfig.ChangeBaseUrl(tenantAvailableResult.ServerRootAddress);
                    OnPropertyChanged(CurrentTenancyNameOrDefault);
                    break;

                case TenantAvailabilityState.InActive:
                    await dialogService.Question("InActive",
                        Local.Localize("TenantIsNotActive", tenancyName), "Login");
                    break;

                case TenantAvailabilityState.NotFound:
                    await dialogService.Question("NotFound",
                        Local.Localize("ThereIsNoTenantDefinedWithName{0}", tenancyName), "Login");
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            await Task.CompletedTask;
        }

        #endregion 

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            await SetBusyAsync(async () =>
             {
                 SetAppSettings();
                 PopulateLoginInfoFromStorage();
                 await Task.CompletedTask;
             });
        }

        /// <summary>
        /// 设置应用程序信息  语言/租户选项
        /// </summary>
        private void SetAppSettings()
        {
            var configuration = applicationContext.Configuration;
            if (configuration != null)
            {
                Languages = new ObservableCollection<LanguageInfo>(configuration.Localization.Languages);

                var currentLanguage = Languages.FirstOrDefault(l => l.Name == applicationContext.Configuration.Localization.CurrentLanguage.Name);
                if (currentLanguage != null)
                    SelectedLanguage = currentLanguage;

                if (applicationContext.CurrentTenant != null)
                {
                    IsMultiTenancyEnabled = configuration.MultiTenancy.IsEnabled;
                    CurrentTenancyNameOrDefault = applicationContext.CurrentTenant.TenancyName;
                }
            }
            else
            {
                CurrentTenancyNameOrDefault = Local.Localize(AppLocalizationKeys.NotSelected);
            }
        }

        /// <summary>
        /// 从本地存储当中读取账户信息, 可能是用户之前登陆的信息 
        /// </summary>
        private void PopulateLoginInfoFromStorage()
        {
            UserName = storageService.GetValue(nameof(UserName), "");
            Password = storageService.GetValue(nameof(Password), "", true);

            var loginInfo = dataStorageService.RetrieveLoginInfo();
            if (loginInfo == null) return;

            if (loginInfo.Tenant != null)
                TenancyName = loginInfo.Tenant.TenancyName;

            if (loginInfo.Tenant == null)
                applicationContext.SetAsHost();
            else
                applicationContext.SetAsTenant(TenancyName, loginInfo.Tenant.Id);

            OnPropertyChanged("CurrentTenancyNameOrDefault");
        }
    }
}