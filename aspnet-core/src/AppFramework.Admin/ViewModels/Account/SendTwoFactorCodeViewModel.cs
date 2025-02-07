using AppFramework.ApiClient.Models;
using AppFramework.Authorization.Accounts;
using AppFramework.Admin.Services;
using Prism.Commands;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Linq;
using AppFramework.Shared;
using AppFramework.Shared.Services;

namespace AppFramework.Admin.ViewModels
{
    public class SendTwoFactorCodeViewModel : HostDialogViewModel
    {
        private readonly IHostDialogService dialog;
        private readonly ProxyTokenAuthControllerService proxyTokenAuthControllerService;
        private readonly IAccountService accountService;

        public DelegateCommand SendSecurityCodeCommand { get; private set; }

        public SendTwoFactorCodeViewModel(IHostDialogService dialog,
            ProxyTokenAuthControllerService proxyTokenAuthControllerService,
            IAccountService accountService)
        {
            this.dialog = dialog;
            this.proxyTokenAuthControllerService = proxyTokenAuthControllerService;
            this.accountService = accountService;
            twoFactorAuthProviders = new List<string>();

            SendSecurityCodeCommand = new DelegateCommand(SendSecurityCodeAsync);
        }

        private List<string> twoFactorAuthProviders;

        public List<string> TwoFactorAuthProviders
        {
            get => twoFactorAuthProviders;
            set
            {
                twoFactorAuthProviders = value;
                OnPropertyChanged();
            }
        }

        private string selectedProvider;

        public string SelectedProvider
        {
            get => selectedProvider;
            set
            {
                selectedProvider = value;
                OnPropertyChanged();
            }
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            accountService.AuthenticateResultModel = parameters
              .GetValue<AbpAuthenticateResultModel>("Value");

            TwoFactorAuthProviders = accountService
                .AuthenticateResultModel.TwoFactorAuthProviders.ToList();
            SelectedProvider = TwoFactorAuthProviders.FirstOrDefault()??"";
        }

        private async void SendSecurityCodeAsync()
        {
            await SetBusyAsync(async () =>
            {
                await proxyTokenAuthControllerService.SendTwoFactorAuthCode(
                    accountService.AuthenticateResultModel.UserId,
                    selectedProvider
                );
            });

            var promptResult = await dialog.ShowDialogAsync("");

            if (promptResult.Result == ButtonResult.OK)
            {
                var twoFactorVerificationCode = promptResult.Parameters.GetValue<string>("Value");

                if (!string.IsNullOrEmpty(twoFactorVerificationCode))
                {
                    accountService.AuthenticateModel.TwoFactorVerificationCode = twoFactorVerificationCode;
                    accountService.AuthenticateModel.RememberClient = true;
                    await SetBusyAsync(async () =>
                    {
                        await accountService.LoginUserAsync();
                    });
                }
            }
        }
    }
}
