using AppFramework.Authorization.Accounts;
using AppFramework.Authorization.Accounts.Dto;
using AppFramework.Shared;
using AppFramework.Admin.ViewModels.Shared;
using Prism.Commands;
using Prism.Services.Dialogs;
using System.Threading.Tasks;

namespace AppFramework.Admin.ViewModels
{
    public class ForgotPasswordViewModel : HostDialogViewModel
    {
        public DelegateCommand SendForgotPasswordCommand { get; private set; }

        private readonly IAccountAppService accountAppService;
        private bool _isForgotPasswordEnabled;

        public ForgotPasswordViewModel(IAccountAppService accountAppService)
        {
            this.accountAppService = accountAppService;
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
                OnPropertyChanged();
            }
        }

        public bool IsForgotPasswordEnabled
        {
            get => _isForgotPasswordEnabled;
            set
            {
                _isForgotPasswordEnabled = value;
                OnPropertyChanged();
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
                await accountAppService.SendPasswordResetCode(new SendPasswordResetCodeInput { EmailAddress = EmailAddress }).WebAsync(PasswordResetMailSentAsync);
            });
        }

        private async Task PasswordResetMailSentAsync()
        {
            DialogHelper.Info(Local.Localize(AppLocalizationKeys.PasswordResetMailSentMessage),
               Local.Localize(AppLocalizationKeys.MailSent));

            await base.Save(); 
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}
