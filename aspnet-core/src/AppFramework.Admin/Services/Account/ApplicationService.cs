using AppFramework.ApiClient;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Authorization.Users.Profile.Dto;
using AppFramework.Shared.Models;
using AppFramework.Shared.Services.Permission;
using AppFramework.Admin.Services.Notification;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AppFramework.Shared;
using AppFramework.Shared.Services;

namespace AppFramework.Admin.Services.Account
{
    public class ApplicationService : ViewModelBase, IApplicationService
    {
        public ApplicationService(
            IHostDialogService dialog,
            IDialogService dialogService,
            IRegionManager regionManager,
            IAccountService accountService,
            INavigationMenuService navigationItemService,
            IProfileAppService profileAppService,
            IApplicationContext applicationContext,
            NavigationService navigationService,
            NotificationService notificationService)
        {
            this.dialog = dialog;
            this.accountService = accountService;
            this.applicationContext = applicationContext;
            this.navigationService = navigationService;
            this.notificationService = notificationService;
            this.dialogService = dialogService;
            this.navigationItemService = navigationItemService;
            this.regionManager = regionManager;
            this.profileAppService = profileAppService;

            navigationItems = new ObservableCollection<NavigationItem>();
        }

        #region 字段/属性

        private readonly IApplicationContext applicationContext;
        private readonly NavigationService navigationService;
        private readonly NotificationService notificationService;
        private readonly IDialogService dialogService;
        private readonly IHostDialogService dialog;
        private readonly INavigationMenuService navigationItemService;
        private readonly IRegionManager regionManager;
        private readonly IAccountService accountService;
        private readonly IProfileAppService profileAppService;

        private byte[] photo;
        private byte[] profilePictureBytes;
        private string userNameAndSurname;
        private string emailAddress;
        private string applicationInfo;

        private string applicationName;

        public string ApplicationName
        {
            get { return applicationName; }
            set { applicationName = value; OnPropertyChanged(); }
        }

        public byte[] Photo
        {
            get => photo;
            set
            {
                photo = value;
                OnPropertyChanged();
            }
        }

        public string UserNameAndSurname
        {
            get => userNameAndSurname;
            set { userNameAndSurname = value; OnPropertyChanged(); }
        }

        public string EmailAddress
        {
            get => emailAddress;
            set { emailAddress = value; OnPropertyChanged(); }
        }

        public string ApplicationInfo
        {
            get => applicationInfo;
            set { applicationInfo = value; OnPropertyChanged(); }
        }

        private ObservableCollection<NavigationItem> navigationItems;

        public ObservableCollection<NavigationItem> NavigationItems
        {
            get { return navigationItems; }
            set { navigationItems = value; OnPropertyChanged(); }
        }

        private ObservableCollection<PermissionItem> userMenuItems;

        public ObservableCollection<PermissionItem> UserMenuItems
        {
            get { return userMenuItems; }
            set { userMenuItems = value; OnPropertyChanged(); }
        }

        #endregion

        #region 用户方法

        public async Task ShowMyProfile()
        {
            dialogService.Show(AppViews.MyProfile);
            await Task.CompletedTask;
        }

        protected async Task GetUserPhoto()
        {
            await WebRequest.Execute(() => profileAppService.GetProfilePictureByUser(applicationContext.LoginInfo.User.Id),
                 GetProfilePictureByUserSuccessed);
        }

        private async Task GetProfilePictureByUserSuccessed(GetProfilePictureOutput output)
        {
            if (output != null)
                Photo = Convert.FromBase64String(output.ProfilePicture);
            await Task.CompletedTask;
        }

        public async Task ShowProfilePhoto()
        {
            if (profilePictureBytes == null) return;

            NavigationParameters param = new NavigationParameters();
            param.Add("Value", profilePictureBytes);
            regionManager.Regions[AppRegions.Main].RequestNavigate(AppViews.ProfilePicture, param);

            await Task.CompletedTask;
        }

        #endregion

        #region 用户菜单方法

        private int selectedIndex = -1;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; OnPropertyChanged(); }
        }

        public async Task GetApplicationInfo()
        {
            await GetUserPhoto();

            ApplicationName = Local.Localize("EmailActivation_Title");

            UserNameAndSurname = applicationContext.LoginInfo.User.Name;
            EmailAddress = applicationContext.LoginInfo.User.EmailAddress;

            RefreshAuthMenus();

            ApplicationInfo = $"{ApplicationName}\n" +
                            $"v{applicationContext.LoginInfo.Application.Version} " +
                            $"[{applicationContext.LoginInfo.Application.ReleaseDate:yyyyMMdd}]";
        }

        public void RefreshAuthMenus()
        {
            var permissions = applicationContext.Configuration.Auth.GrantedPermissions;
            NavigationItems = navigationItemService.GetAuthMenus(permissions);
            UserMenuItems = new ObservableCollection<PermissionItem>()
            {
               new PermissionItem("accounts",Local.Localize("ManageLinkedAccounts"), "",ManageLinkedAccounts),
               new PermissionItem("manageuser",Local.Localize("ManageUserDelegations"),"",ManageUserDelegations),
               new PermissionItem("password",Local.Localize("ChangePassword"),"",ChangePassword),
               new PermissionItem("loginattempts",Local.Localize("LoginAttempts"),"",LoginAttempts),
               new PermissionItem("picture",Local.Localize("ChangeProfilePicture"),"",ChangeProfilePicture),
               new PermissionItem("mysettings",Local.Localize("MySettings"),"",MySettings),
               new PermissionItem("download",Local.Localize("Download"),"",Download),
               new PermissionItem("logout",Local.Localize("Logout"),"",LogOut),
            };
        }

        public void ExecuteUserAction(string key)
        {
            var item = UserMenuItems.FirstOrDefault(t => t.Key.Equals(key));
            if (item != null) item.Action?.Invoke();
        }

        private async void LogOut()
        {
            if (await dialog.Question(Local.Localize("AreYouSure")))
            {
                regionManager.Regions[AppRegions.Main].RemoveAll();
                await accountService.LogoutAsync();
            }

            await ResetClickIndex();
        }

        private void ManageLinkedAccounts()
        {
            ShowPage(AppViews.ManageLinkedAccounts);
        }

        private void ManageUserDelegations()
        {
            ShowPage(AppViews.ManageUserDelegations);
        }

        private void ChangePassword()
        {
            ShowPage(AppViews.ChangePassword);
        }

        private void LoginAttempts()
        {
            navigationService.Navigate(AppViews.LoginAttempts);
        }

        private void MySettings()
        {
            ShowPage(AppViews.MyProfile);
        }

        private async void ShowPage(string pageName)
        {
            await dialog.ShowDialogAsync(pageName);
            await ResetClickIndex();
        }

        private async void Download()
        {
            await profileAppService.PrepareCollectedData().WebAsync(async () =>
            {
                await notificationService.GetNotifications();
                await ResetClickIndex();
                DialogHelper.Info(Local.Localize("Notifications"),
                   Local.Localize("GdprDataPreparedNotificationMessage"));
            });
        }

        private async void ChangeProfilePicture()
        {
            var dialogResult = await dialog.ShowDialogAsync(AppViews.ChangeAvatar);

            if (dialogResult.Result == ButtonResult.OK)
            {
                Photo = dialogResult.Parameters.GetValue<byte[]>("Value");

                await ResetClickIndex();
            }
        }

        private async Task ResetClickIndex()
        {
            SelectedIndex = -1;

            await Task.Delay(200);
        }

        #endregion
    }
}
