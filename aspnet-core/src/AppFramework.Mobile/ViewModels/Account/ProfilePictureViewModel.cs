using AppFramework.Shared.Services; 
using Prism.Commands; 

namespace AppFramework.Shared.ViewModels
{
    public class ProfilePictureViewModel : NavigationViewModel
    {
        public IApplicationService appService { get; set; }

        public DelegateCommand ChangeProfilePictureCommand { get; private set; }

        public ProfilePictureViewModel(IApplicationService appService)
        {
            ChangeProfilePictureCommand = new DelegateCommand(ChangeProfilePicture);
            this.appService = appService;
        }

        private void ChangeProfilePicture()
        {
            appService.ChangeProfilePhoto();
        }
    }
}