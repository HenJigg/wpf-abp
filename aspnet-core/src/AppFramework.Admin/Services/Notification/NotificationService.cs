using Abp.Notifications;
using AppFramework.Shared;
using AppFramework.Notifications;
using AppFramework.Notifications.Dto;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AppFramework.Admin.Services.Notification
{
    [INotifyPropertyChanged]
    public partial class NotificationService
    {
        public NotificationService(INotificationAppService appService,
           NavigationService navigationService)
        {
            this.appService = appService;
            this.navigationService = navigationService;
            items = new ObservableCollection<UserNotification>();
            input = new GetUserNotificationsInput()
            {
                MaxResultCount = 4,
                State = UserNotificationState.Unread
            };
        }

        private readonly INotificationAppService appService;
        private readonly NavigationService navigationService;
        private ObservableCollection<UserNotification> items;

        public ObservableCollection<UserNotification> Items
        {
            get { return items; }
            set { items = value; OnPropertyChanged(); }
        }

        public GetUserNotificationsInput input;

        private bool isunRead;

        public bool IsUnRead
        {
            get { return isunRead; }
            set { isunRead = value; OnPropertyChanged(); }
        }

        public async Task GetNotifications()
        {
            UpdateDisplayContent();

            await WebRequest.Execute(() => appService.GetUserNotifications(input),
                async output =>
                {
                    Items.Clear();
                    IsUnRead = output.UnreadCount > 0 ? true : false;

                    foreach (var item in output.Items)
                        Items.Add(item);

                    await Task.CompletedTask;
                });
        }

        public void Settings()
        { }

        public void SeeAllNotificationsPage()
        {
            navigationService.Navigate(AppViews.Notification);
        }

        public async void SetAllNotificationsAsRead()
        {
            await appService.SetAllNotificationsAsRead().WebAsync(GetNotifications);
        }

        public void SetNotificationAsRead()
        { }

        #region Display 

        private string title;
        private string setAllAsRead;
        private string seeAllNotifications;

        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(); }
        }

        public string SetAllAsRead
        {
            get { return setAllAsRead; }
            set { setAllAsRead = value; OnPropertyChanged(); }
        }

        public string SeeAllNotifications
        {
            get { return seeAllNotifications; }
            set { seeAllNotifications = value; OnPropertyChanged(); }
        }


        private void UpdateDisplayContent()
        {
            Title = Local.Localize("Notifications");
            SetAllAsRead = Local.Localize("SetAllAsRead");
            SeeAllNotifications = Local.Localize("SeeAllNotifications");
        }

        #endregion
    }
}
