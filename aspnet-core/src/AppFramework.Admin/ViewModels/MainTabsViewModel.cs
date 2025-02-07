using AppFramework.Shared;
using AppFramework.Shared.Models;
using AppFramework.Admin.Services;
using AppFramework.Admin.Services.Notification;
using Prism.Commands;
using Prism.Regions;
using System.Threading.Tasks;
using AppFramework.ApiClient;

namespace AppFramework.Admin.ViewModels
{
    public class MainTabsViewModel : NavigationViewModel
    {
        public MainTabsViewModel(
            NotificationService notificationService,
            IRegionManager regionManager,
            NavigationService navigationService,
            IApplicationService appService,
            IApplicationContext applicationContext)
        {
            this.notificationService = notificationService;
            this.regionManager = regionManager;
            NavigationService = navigationService;
            this.appService = appService;
            this.applicationContext = applicationContext;
            SettingsCommand = new DelegateCommand(notificationService.Settings);
            NavigateCommand = new DelegateCommand<NavigationItem>(Navigate);
            SeeAllNotificationsCommand = new DelegateCommand(() =>
            {
                NotificationPanelIsOpen = false;
                notificationService.SeeAllNotificationsPage();
            });
            SetNotificationAsRead = new DelegateCommand(notificationService.SetNotificationAsRead);
            SetAllNotificationsAsReadCommand = new DelegateCommand(notificationService.SetAllNotificationsAsRead);
             
        }
         
        #region 字段/属性

        public NavigationService NavigationService { get; set; }
        public NotificationService notificationService { get; set; }
        public IApplicationService appService { get; init; }
        public DelegateCommand<NavigationItem> NavigateCommand { get; private set; }

        private bool notificationPanelIsOpen;
        private bool isShowFriendsPanel;
        private readonly IRegionManager regionManager;
        private readonly IApplicationContext applicationContext;

        public bool NotificationPanelIsOpen
        {
            get { return notificationPanelIsOpen; }
            set
            {
                notificationPanelIsOpen = value;
                if (value)
                {
                    AsyncRunner.Run(notificationService.GetNotifications());
                }
                OnPropertyChanged();
            }
        }

        private bool isShowUserPanel;

        public bool IsShowUserPanel
        {
            get { return isShowUserPanel; }
            set
            {
                if (value && !isShowUserPanel)
                {
                    regionManager.Regions[AppRegions.UserPanel].RequestNavigate(AppViews.UserPanel);
                }
                isShowUserPanel = value;
                OnPropertyChanged();
            }
        }

        public bool IsShowFriendsPanel
        {
            get { return isShowFriendsPanel; }
            set
            {
                if (value && !isShowFriendsPanel)
                {
                    regionManager.Regions[AppRegions.UserPanel].RequestNavigate(AppViews.Friends);
                }

                isShowUserPanel = isShowFriendsPanel = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(IsShowUserPanel));
            }
        }

        public DelegateCommand SeeAllNotificationsCommand { get; private set; }
        public DelegateCommand SettingsCommand { get; private set; }
        public DelegateCommand SetAllNotificationsAsReadCommand { get; private set; }
        public DelegateCommand SetNotificationAsRead { get; private set; }

        #endregion

        public void Navigate(NavigationItem item)
        {
            if (item==null) return;

            NavigationService.Navigate(item.PageViewName);
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext)
        {
            IsShowUserPanel=false;

            await appService.GetApplicationInfo();

            if (applicationContext.Configuration.Auth.GrantedPermissions.ContainsKey(AppPermissions.HostDashboard))
                NavigationService.Navigate(AppViews.Dashboard);
        }
    }
}