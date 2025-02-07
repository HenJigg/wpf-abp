using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Webhooks;
using AppFramework.WebHooks.Dto;

namespace AppFramework.WebHooks
{
    public interface IWebhookSubscriptionAppService
    {
        /// <summary>
        /// Returns all subscriptions of tenant
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<GetAllSubscriptionsOutput>> GetAllSubscriptions();

        /// <summary>
        /// Returns subscription for given id. 
        /// </summary>
        /// <param name="subscriptionId">Unique identifier of <see cref="WebhookSubscriptionInfo"/></param>
        Task<WebhookSubscription> GetSubscription(string subscriptionId);

        Task AddSubscription(WebhookSubscription subscription);

        Task UpdateSubscription(WebhookSubscription subscription);

        Task ActivateWebhookSubscription(ActivateWebhookSubscriptionInput input);

        Task<bool> IsSubscribed(string webhookName);

        Task<ListResultDto<GetAllSubscriptionsOutput>> GetAllSubscriptionsIfFeaturesGranted(string webhookName);

        Task<ListResultDto<GetAllAvailableWebhooksOutput>> GetAllAvailableWebhooks();
    }
}
