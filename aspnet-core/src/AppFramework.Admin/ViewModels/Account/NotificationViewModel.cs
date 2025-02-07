using Abp.Notifications;
using AppFramework.Shared;
using AppFramework.Notifications;
using AppFramework.Notifications.Dto;
using AppFramework.Admin.Services.Notification;
using AppFramework.Admin.ViewModels.Shared;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Threading.Tasks;
using AppFramework.Shared.Services;

namespace AppFramework.Admin.ViewModels
{
    public class NotificationViewModel : NavigationCurdViewModel
    {
        private readonly INotificationAppService appService;
        private readonly NotificationService notificationService;
        public GetUserNotificationsInput input;

        public DelegateCommand<UserNotification> SetNotificationAsReadCommand { get; private set; }
        public DelegateCommand SetAllNotificationsAsReadCommand { get; private set; }
        public DelegateCommand<UserNotification> DeleteNotificationCommand { get; private set; }
        public DelegateCommand DeleteAllUserNotificationsCommand { get; private set; }

        public DateTime? StartDate
        {
            get { return input.StartDate; }
            set { input.StartDate = value; OnPropertyChanged(); }
        }

        public DateTime? EndDate
        {
            get { return input.EndDate; }
            set { input.EndDate = value; OnPropertyChanged(); }
        }

        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                input.State = value == 0 ? null : UserNotificationState.Unread;
                OnPropertyChanged();
            }
        }

        public NotificationViewModel(INotificationAppService appService,
            NotificationService notificationService)
        {
            Title = Local.Localize("Notifications");
            this.appService = appService;
            this.notificationService = notificationService;
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            var day = DateTime.Now.Day;

            input = new GetUserNotificationsInput()
            {
                StartDate = new DateTime(year, month, day - 7, 0, 0, 0),
                EndDate = new DateTime(year, month, day, 23, 59, 59),
                MaxResultCount = 10,
            };
            dataPager.OnPageIndexChangedEventhandler += DataPager_OnPageIndexChangedEventhandler;

            DeleteAllUserNotificationsCommand = new DelegateCommand(DeleteAllUserNotifications);
            DeleteNotificationCommand = new DelegateCommand<UserNotification>(DeleteNotification);
            SetNotificationAsReadCommand = new DelegateCommand<UserNotification>(SetNotificationAsRead);
            SetAllNotificationsAsReadCommand = new DelegateCommand(notificationService.SetAllNotificationsAsRead);
        }

        private async void DeleteAllUserNotifications()
        {
            if (await dialog.Question(Local.Localize("DeleteListedNotificationsWarningMessage")))
            {
                await appService.DeleteAllUserNotifications(new DeleteAllUserNotificationsInput()
                {
                    StartDate = input.StartDate,
                    EndDate = input.EndDate,
                    State = input.State
                }).WebAsync(async () => await OnNavigatedToAsync());
            }
        }

        private async void SetNotificationAsRead(UserNotification obj)
        {
            await appService.SetNotificationAsRead(new Abp.Application.Services.Dto.EntityDto<Guid>() { Id = obj.Id })
                            .WebAsync(async () => await OnNavigatedToAsync());
        }

        private async void DeleteNotification(UserNotification obj)
        {
            if (await dialog.Question(Local.Localize("NotificationDeleteWarningMessage")))
            {
                await appService.DeleteNotification(new Abp.Application.Services.Dto.EntityDto<Guid>() { Id = obj.Id })
                                .WebAsync(async () => await OnNavigatedToAsync());
            }
        }

        private async void DataPager_OnPageIndexChangedEventhandler(object sender, PageIndexChangedEventArgs e)
        {
            input.StartDate = StartDate.GetFirstDate();
            input.EndDate = EndDate.GetLastDate();
            input.SkipCount = e.SkipCount;
            input.MaxResultCount = e.PageSize;

            await SetBusyAsync(async () =>
            {
                await GetUserNotifications(input);
            });
        }

        private async Task GetUserNotifications(GetUserNotificationsInput filter)
        {
            await appService.GetUserNotifications(filter).WebAsync(dataPager.SetList); 
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            dataPager.PageIndex = 0;
            await Task.CompletedTask;
        }
    }
}
