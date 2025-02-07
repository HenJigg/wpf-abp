using AppFramework.Authorization.Accounts;
using AppFramework.Authorization.Accounts.Dto;
using AppFramework.Shared;
using AppFramework.Admin.ViewModels.Shared;
using Prism.Commands;
using Prism.Services.Dialogs;
using System.Threading.Tasks;

namespace AppFramework.Admin.ViewModels
{
    public class EmailActivationViewModel : HostDialogViewModel
    {
        public DelegateCommand SendEmailActivationCommand { get; private set; }

        private readonly IAccountAppService accountAppService;
        private bool _isEmailActivationEnabled;

        public EmailActivationViewModel(IAccountAppService accountAppService)
        {
            this.accountAppService = accountAppService;
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
                OnPropertyChanged();
            }
        }

        public bool IsEmailActivationEnabled
        {
            get => _isEmailActivationEnabled;
            set
            {
                _isEmailActivationEnabled = value;
                OnPropertyChanged();
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
                await accountAppService.SendEmailActivationLink(new SendEmailActivationLinkInput { EmailAddress = EmailAddress }).WebAsync(PasswordResetMailSentAsync);
            });
        }

        private async Task PasswordResetMailSentAsync()
        {
            DialogHelper.Info(Local.Localize(AppLocalizationKeys.ActivationMailSentMessage),
                Local.Localize(AppLocalizationKeys.MailSent));

            await Task.CompletedTask;
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        { }
    }
}
