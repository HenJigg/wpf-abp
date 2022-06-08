using AppFramework.Authorization.Accounts;
using AppFramework.Authorization.Accounts.Dto;
using AppFramework.Common;
using Prism.Commands;
using Prism.Services.Dialogs; 
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class ForgotPasswordViewModel : HostDialogViewModel
    {
        public DelegateCommand SendForgotPasswordCommand { get; private set; }

        private readonly IAccountAppService _accountAppService;
        private readonly IDialogService dialog;
        private bool _isForgotPasswordEnabled;

        public ForgotPasswordViewModel(IAccountAppService accountAppService,
            IDialogService dialog)
        {
            _accountAppService = accountAppService;
            this.dialog = dialog;
            SendForgotPasswordCommand = new DelegateCommand(SendForgotPasswordAsync);
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

        public bool IsForgotPasswordEnabled
        {
            get => _isForgotPasswordEnabled;
            set
            {
                _isForgotPasswordEnabled = value;
                RaisePropertyChanged();
            }
        }

        public void SetEmailActivationButtonEnabled()
        {
            IsForgotPasswordEnabled = !string.IsNullOrWhiteSpace(EmailAddress);
        }

        private async void SendForgotPasswordAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(
                    async () =>
                    await _accountAppService.SendPasswordResetCode(new SendPasswordResetCodeInput { EmailAddress = EmailAddress }),
                    PasswordResetMailSentAsync
                );
            });
        }

        private async Task PasswordResetMailSentAsync()
        {
            dialog.ShowMessage(
               Local.Localize(AppLocalizationKeys.PasswordResetMailSentMessage),
               Local.Localize(AppLocalizationKeys.MailSent));

            base.Save();

            await Task.CompletedTask;
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}
