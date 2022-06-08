using AppFramework.Authorization.Accounts;
using AppFramework.Authorization.Accounts.Dto;
using AppFramework.Common;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class EmailActivationViewModel : HostDialogViewModel
    {
        public DelegateCommand SendEmailActivationCommand { get; private set; }

        private readonly IAccountAppService _accountAppService;
        private readonly IDialogService dialog;
        private bool _isEmailActivationEnabled;

        public EmailActivationViewModel(IAccountAppService accountAppService,
            IDialogService dialog)
        {
            _accountAppService = accountAppService;
            this.dialog = dialog;
            SendEmailActivationCommand = new DelegateCommand(SendEmailActivationAsync);
        }

        private string _emailAddress;

        public string EmailAddress
        {
            get => _emailAddress;
            set
            {
                _emailAddress = value;
                SetEmailActivationButtonEnabled();
                RaisePropertyChanged();
            }
        }

        public bool IsEmailActivationEnabled
        {
            get => _isEmailActivationEnabled;
            set
            {
                _isEmailActivationEnabled = value;
                RaisePropertyChanged();
            }
        }

        public void SetEmailActivationButtonEnabled()
        {
            IsEmailActivationEnabled = !string.IsNullOrWhiteSpace(EmailAddress);
        }

        private async void SendEmailActivationAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(
                    async () =>
                    await _accountAppService.SendEmailActivationLink(new SendEmailActivationLinkInput { EmailAddress = EmailAddress }),
                    PasswordResetMailSentAsync
                );
            });
        }

        private async Task PasswordResetMailSentAsync()
        {  
            dialog.ShowMessage(Local.Localize(AppLocalizationKeys.ActivationMailSentMessage),
                Local.Localize(AppLocalizationKeys.MailSent));

            await Task.CompletedTask;
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        { 
        }
    }
}
