using Abp.IO.Extensions;
using Acr.UserDialogs;
using AppFramework.Shared.Models; 
using AppFramework.Shared.Services.Navigation;
using AppFramework.ApiClient;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Authorization.Users.Profile.Dto;
using AppFramework.Dto; 
using FFImageLoading;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks; 
using AppFramework.Shared.ViewModels;
using AppFramework.Shared.Views;

namespace AppFramework.Shared.Services
{
    public class ApplicationService : ViewModelBase, IApplicationService
    {
        private readonly IApplicationContext applicationContext;
        private readonly IDialogService dialogService;
        private readonly INavigationMenuService navigationItemService;
        private readonly INavigationService navigationService;
        private readonly IProfileAppService profileAppService;
        private readonly ProxyProfileControllerService profileControllerService;

        public ApplicationService(
            IApplicationContext applicationContext,
            IDialogService dialogService,
            INavigationMenuService navigationItemService,
            INavigationService navigationService,
            IProfileAppService profileAppService,
            ProxyProfileControllerService profileControllerService)
        {
            this.applicationContext = applicationContext;
            this.dialogService = dialogService;
            this.navigationItemService = navigationItemService;
            this.navigationService = navigationService;
            this.profileAppService = profileAppService;
            this.profileControllerService = profileControllerService;

            navigationItems = new ObservableCollection<NavigationItem>();
        }

        private byte[] photo;
        private string userNameAndSurname;
        private string applicationInfo;
        public string ApplicationName { get; set; } = "AppFramework";

        public byte[] Photo
        {
            get => photo;
            set
            {
                photo = value;
                RaisePropertyChanged();
            }
        }

        public string UserNameAndSurname
        {
            get => userNameAndSurname;
            set { userNameAndSurname = value; RaisePropertyChanged(); }
        }

        public string ApplicationInfo
        {
            get => applicationInfo;
            set { applicationInfo = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<NavigationItem> navigationItems;

        public ObservableCollection<NavigationItem> NavigationItems
        {
            get { return navigationItems; }
            set { navigationItems = value; RaisePropertyChanged(); }
        }

        private string emailAddress;

        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value; RaisePropertyChanged(); }
        }

        public async Task ShowMyProfile()
        {
            dialogService.ShowDialog(AppViews.MyProfile);

            await Task.CompletedTask;
        }

        protected async Task GetUserPhoto()
        {
            await WebRequest.Execute(async () =>
             await profileAppService.GetProfilePicture(),
                 async result => await GetProfilePictureByUserSuccessed(result));
        }

        private async Task GetProfilePictureByUserSuccessed(GetProfilePictureOutput output)
        {
            Photo = Convert.FromBase64String(output.ProfilePicture);
            await Task.CompletedTask;
        }

        private static async Task PickProfilePictureAsync(Func<MediaFile, Task> picturePicked)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
                return;

            try
            {
                using (var photo = await CrossMedia.Current.PickPhotoAsync())
                {
                    await picturePicked(photo);
                }
            }
            catch(Exception ex)
            {

            } 
        }

        private async Task TakeProfilePhotoAsync(Func<MediaFile, Task> photoTaken)
        {
            if (!CrossMedia.Current.IsCameraAvailable ||
                !CrossMedia.Current.IsTakePhotoSupported)
            {
                DialogHelper.Warn("NoCamera");
                return;
            }

            await SetBusyAsync(async () =>
            {
                using (var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    DefaultCamera = CameraDevice.Front,
                    PhotoSize = PhotoSize.MaxWidthHeight,
                    AllowCropping = true,
                    CompressionQuality = 80,
                    MaxWidthHeight = 700
                }))
                {
                    await photoTaken(photo);
                }
            });
        }

        /// <summary>
        /// Shows a crop view to crop the media file.
        /// Native image cropping feature is available only on UWP and IOS.
        /// For Android devices, custom cropping is implemented.
        /// </summary>
        private async Task CropImageIfNeedsAsync(MediaFile photo)
        {
            if (photo == null) return;

            await OnCropCompleted(File.ReadAllBytes(photo.Path), Path.GetFileName(photo.Path));
        }

        private async Task OnCropCompleted(byte[] croppedImageBytes, string fileName)
        {
            if (croppedImageBytes == null)
                return;

            var jpgStream = await ResizeImageAsync(croppedImageBytes);
            await SaveProfilePhoto(jpgStream.GetAllBytes(), fileName);
        }

        private static async Task<Stream> ResizeImageAsync(byte[] imageBytes, int width = 256, int height = 256)
        {
            var result = ImageService.Instance.LoadStream(token =>
            {
                var tcs = new TaskCompletionSource<Stream>();
                tcs.TrySetResult(new MemoryStream(imageBytes));
                return tcs.Task;
            }).DownSample(width, height);

            return await result.AsJPGStreamAsync();
        }

        private async Task SaveProfilePhoto(byte[] photoAsBytes, string fileName)
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(async () => await UpdateProfilePhoto(photoAsBytes, fileName), () =>
                {
                    Photo = photoAsBytes;
                    CloneProfilePicture(photoAsBytes);
                    return Task.CompletedTask;
                });
            });
        }

        private void CloneProfilePicture(byte[] photoAsBytes)
        {
            photoAsBytes = new byte[photoAsBytes.Length];
            photoAsBytes.CopyTo(photoAsBytes, 0);
        }

        private async Task UpdateProfilePhoto(byte[] photoAsBytes, string fileName)
        {
            var fileToken = Guid.NewGuid().ToString();

            using (Stream photoStream = new MemoryStream(photoAsBytes))
            {
                await profileControllerService.UploadProfilePicture(content =>
                {
                    content.AddFile("file", photoStream, fileName);
                    content.AddString(nameof(FileDto.FileToken), fileToken);
                    content.AddString(nameof(FileDto.FileName), fileName);
                }).ContinueWith(uploadResult =>
                {
                    if (uploadResult == null)
                        return;

                    profileAppService.UpdateProfilePicture(new UpdateProfilePictureInput
                    {
                        FileToken = fileToken
                    });
                });
            }
        }

        public void ChangeProfilePhoto()
        {
            UserDialogs.Instance.ActionSheet(new ActionSheetConfig
            {
                Title = Local.Localize("ChangeProfilePicture"),
                Options = new List<ActionSheetOption>  {
                    new ActionSheetOption(Local.Localize("PickFromGallery"),  async () => await PickProfilePictureAsync(CropImageIfNeedsAsync)),
                    new ActionSheetOption(Local.Localize("TakePhoto"),  async () => await TakeProfilePhotoAsync(CropImageIfNeedsAsync))
                }
            });
        }

        public async Task ShowProfilePhoto()
        {
            await navigationService.NavigateAsync(AppViews.ProfilePicture);
        }

        public async Task GetApplicationInfo()
        {
            RefreshAuthMenus();
            UserNameAndSurname = applicationContext.LoginInfo.User.Name;

            EmailAddress = applicationContext.LoginInfo.User.EmailAddress;

            ApplicationInfo = $"{ApplicationName}\n" +
                            $"v{applicationContext.LoginInfo.Application.Version} " +
                            $"[{applicationContext.LoginInfo.Application.ReleaseDate:yyyyMMdd}]";

            await GetUserPhoto();
        }

        public void RefreshAuthMenus()
        {
            var permissions = applicationContext.Configuration.Auth.GrantedPermissions;
            NavigationItems = navigationItemService.GetAuthMenus(permissions);
        }

        public void ExecuteUserAction(string key)
        { }
    }
}