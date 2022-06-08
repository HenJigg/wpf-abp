using AppFramework.Authorization.Users.Profile;
using AppFramework.Authorization.Users.Profile.Dto;
using AppFramework.Common; 
using Prism.Commands;
using Prism.Services.Dialogs; 
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class ChangePasswordViewModel : HostDialogViewModel
    {
        public DelegateCommand SendChangePasswordCommand { get; private set; }

        private readonly IProfileAppService _profileAppService;
        private readonly IDialogService dialog;
        private bool _isChangePasswordEnabled;

        public ChangePasswordViewModel(IProfileAppService profileAppService,
            IDialogService dialog)
        {
            _profileAppService = profileAppService;
            this.dialog = dialog;
            SendChangePasswordCommand = new DelegateCommand(SendChangePasswordAsync);
        }

        private string _currentPassword;

        public string CurrentPassword
        {
            get => _currentPassword;
            set
            {
                _currentPassword = value;
                SetChangePasswordButtonEnabled();
                RaisePropertyChanged();
            }
        }

        private string _newPassword;

        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                SetChangePasswordButtonEnabled();
                RaisePropertyChanged();
            }
        }

        private string _newPasswordRepeat;

        public string NewPasswordRepeat
        {
            get => _newPasswordRepeat;
            set
            {
                _newPasswordRepeat = value;
                SetChangePasswordButtonEnabled();
                RaisePropertyChanged();
            }
        }

        public bool IsChangePasswordEnabled
        {
            get => _isChangePasswordEnabled;
            set
            {
                _isChangePasswordEnabled = value;
                RaisePropertyChanged();
            }
        }

        public void SetChangePasswordButtonEnabled()
        {
            IsChangePasswordEnabled = !string.IsNullOrWhiteSpace(CurrentPassword)
                                      && !string.IsNullOrWhiteSpace(NewPassword)
                                      && !string.IsNullOrWhiteSpace(NewPasswordRepeat);
        }

        private async void SendChangePasswordAsync()
        {
            if (NewPassword != NewPasswordRepeat)
            {
                dialog.ShowMessage("", Local.Localize(AppLocalizationKeys.PasswordsDontMatch));
            }
            else
            {
                await SetBusyAsync(async () =>
                {
                    await WebRequest.Execute(
                        async () =>
                            await _profileAppService.ChangePassword(new ChangePasswordInput
                            {
                                CurrentPassword = CurrentPassword,
                                NewPassword = NewPassword
                            }),
                        PasswordChangedAsync
                    );
                });
            }
        }

        private async Task PasswordChangedAsync()
        {
            dialog.ShowMessage(Local.Localize(AppLocalizationKeys.YourPasswordHasChangedSuccessfully),
                Local.Localize(AppLocalizationKeys.ChangePassword));

            await Task.CompletedTask;
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}
