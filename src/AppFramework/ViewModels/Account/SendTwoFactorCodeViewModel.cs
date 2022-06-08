using AppFramework.ApiClient.Models;
using AppFramework.Authorization.Accounts; 
using AppFramework.Common.Services.Account;
using AppFramework.Services;
using Prism.Commands;
using Prism.Services.Dialogs; 
using System.Collections.Generic;
using System.Linq; 

namespace AppFramework.ViewModels
{
    public class SendTwoFactorCodeViewModel : HostDialogViewModel
    {
        private readonly IHostDialogService dialog;
        private readonly ProxyTokenAuthControllerService _proxyTokenAuthControllerService;
        private readonly IAccountService _accountService;

        public DelegateCommand SendSecurityCodeCommand { get; private set; }

        public SendTwoFactorCodeViewModel(IHostDialogService dialog,
            ProxyTokenAuthControllerService proxyTokenAuthControllerService,
            IAccountService accountService)
        {
            this.dialog = dialog;
            _proxyTokenAuthControllerService = proxyTokenAuthControllerService;
            _accountService = accountService;
            _twoFactorAuthProviders = new List<string>();

            SendSecurityCodeCommand = new DelegateCommand(SendSecurityCodeAsync);
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
            });

            var promptResult = await dialog.ShowDialogAsync("");

            if (promptResult.Result == ButtonResult.OK)
            {
                var twoFactorVerificationCode = promptResult.Parameters.GetValue<string>("Value");

                if (!string.IsNullOrEmpty(twoFactorVerificationCode))
                {
                    _accountService.AuthenticateModel.TwoFactorVerificationCode = twoFactorVerificationCode;
                    _accountService.AuthenticateModel.RememberClient = true;
                    await SetBusyAsync(async () =>
                    {
                        await _accountService.LoginUserAsync();
                    });
                }
            }
        }
    }
}
