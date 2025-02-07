using Acr.UserDialogs;
using AppFramework.Shared.Services.Account;
using AppFramework.ApiClient.Models;
using AppFramework.Authorization.Accounts;
using AppFramework.Shared.Localization.Resources;
using Prism.Commands;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Linq;
using AppFramework.Shared.Extensions;

namespace AppFramework.Shared.ViewModels
{
    public class SendTwoFactorCodeViewModel : DialogViewModel
    {
        private readonly ProxyTokenAuthControllerService _proxyTokenAuthControllerService;
        private readonly IAccountService _accountService;

        public DelegateCommand SendSecurityCodeCommand { get; private set; }

        public SendTwoFactorCodeViewModel(
            ProxyTokenAuthControllerService proxyTokenAuthControllerService,
            IAccountService accountService)
        {
            _proxyTokenAuthControllerService = proxyTokenAuthControllerService;
            _accountService = accountService;
            _twoFactorAuthProviders = new List<string>();

            SendSecurityCodeCommand=new DelegateCommand(SendSecurityCodeAsync);
        }

        private List<string> _twoFactorAuthProviders;

        public List<string> TwoFactorAuthProviders
        {
            get => _twoFactorAuthProviders;
            set
            {
                _twoFactorAuthProviders = value;
                RaisePropertyChanged();
            }
        }

        private string _selectedProvider;

        public string SelectedProvider
        {
            get => _selectedProvider;
            set
            {
                _selectedProvider = value;
                RaisePropertyChanged();
            }
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            _accountService.AuthenticateResultModel = parameters
              .GetValue<AbpAuthenticateResultModel>("Value");

            TwoFactorAuthProviders = _accountService
                .AuthenticateResultModel.TwoFactorAuthProviders.ToList();
            SelectedProvider = TwoFactorAuthProviders.FirstOrDefault();
        }
         
        private async void SendSecurityCodeAsync()
        {
            await SetBusyAsync(async () =>
            {
                await _proxyTokenAuthControllerService.SendTwoFactorAuthCode(
                    _accountService.AuthenticateResultModel.UserId,
                    _selectedProvider
                );
            }, LocalTranslation.Authenticating);

            var promptResult = await UserDialogs.Instance.PromptAsync(new PromptConfig
            {
                Message = Local.Localize(LocalizationKeys.VerifySecurityCode_Information),
                Text = "",
                OkText = Local.Localize(LocalizationKeys.Ok),
                CancelText = Local.Localize(LocalizationKeys.Cancel),
                Title = Local.Localize(LocalizationKeys.VerifySecurityCode),
                Placeholder = Local.LocalizeWithThreeDots(LocalizationKeys.Code)
            });

            if (!promptResult.Ok)
            {
                return;
            }

            if (!string.IsNullOrEmpty(promptResult.Text))
            {
                _accountService.AuthenticateModel.TwoFactorVerificationCode = promptResult.Text;
                _accountService.AuthenticateModel.RememberClient = true;
                await SetBusyAsync(async () =>
                {
                    await _accountService.LoginUserAsync();
                }, LocalTranslation.Authenticating);
            }
        }
    }
}