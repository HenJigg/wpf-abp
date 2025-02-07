using Acr.UserDialogs;
using AppFramework.Authorization.Accounts;
using AppFramework.Authorization.Accounts.Dto;
using AppFramework.Shared.Extensions;
using Prism.Commands;
using System.Threading.Tasks;

namespace AppFramework.Shared.ViewModels
{
    public class ForgotPasswordViewModel : DialogViewModel
    {
        public DelegateCommand SendForgotPasswordCommand { get; private set; }

        private readonly IAccountAppService _accountAppService;
        private bool _isForgotPasswordEnabled;

        public ForgotPasswordViewModel(IAccountAppService accountAppService)
        {
            _accountAppService = accountAppService;
            SendForgotPasswordCommand=new DelegateCommand(SendForgotPasswordAsync);
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
            await UserDialogs.Instance.AlertAsync(
                Local.Localize(LocalizationKeys.PasswordResetMailSentMessage), 
                Local.Localize(LocalizationKeys.MailSent), Local.Localize(LocalizationKeys.Ok));

            base.Save();
        }
    }
}