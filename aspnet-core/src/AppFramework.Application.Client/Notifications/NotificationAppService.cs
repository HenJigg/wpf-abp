using Abp.Application.Services.Dto;
using AppFramework.ApiClient;
using AppFramework.Notifications.Dto;
using System; 
using System.Threading.Tasks;

namespace AppFramework.Notifications
{
    public class NotificationAppService : ProxyAppServiceBase, INotificationAppService
    {
        public NotificationAppService(AbpApiClient apiClient) : base(apiClient)
        {
        }

        public async Task DeleteAllUserNotifications(DeleteAllUserNotificationsInput input)
        {
            await ApiClient.DeleteAsync(GetEndpoint(nameof(DeleteAllUserNotifications)), input);
        }

        public async Task DeleteNotification(EntityDto<Guid> input)
        {
            await ApiClient.DeleteAsync(GetEndpoint(nameof(DeleteNotification)), input);
        }

        public async Task<GetNotificationSettingsOutput> GetNotificationSettings()
        {
            return await ApiClient.GetAsync<GetNotificationSettingsOutput>(GetEndpoint(nameof(GetNotificationSettings)));
        }

        public async Task<GetNotificationsOutput> GetUserNotifications(GetUserNotificationsInput input)
        {
            return await ApiClient.GetAsync<GetNotificationsOutput>(GetEndpoint(nameof(GetUserNotifications)), input);
        }

        public async Task SetAllNotificationsAsRead()
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(SetAllNotificationsAsRead)));
        }

        public async Task<SetNotificationAsReadOutput> SetNotificationAsRead(EntityDto<Guid> input)
        {
            return await ApiClient.PostAsync<SetNotificationAsReadOutput>(GetEndpoint(nameof(SetNotificationAsRead)), input);
        }

        public async Task UpdateNotificationSettings(UpdateNotificationSettingsInput input)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(UpdateNotificationSettings)), input);
        }
    }
}
