using System;
using System.Net;
using Abp.Webhooks;

namespace AppFramework.WebHooks.Dto
{
    public class GetAllSendAttemptsOfWebhookEventOutput
    {
        /// <summary>
        /// <see cref="WebhookSendAttempt"/> unique id 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// <see cref="WebhookSubscriptionInfo.WebhookUri"/>
        /// </summary>
        public string WebhookUri { get; set; }

        /// <summary>
        /// <see cref="WebhookSubscription"/> foreign id 
        /// </summary>
        public Guid WebhookSubscriptionId { get; set; }

        /// <summary>
        /// Webhook response content that webhook endpoint send back
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// Webhook response status code that webhook endpoint send back
        /// </summary>
        public HttpStatusCode? ResponseStatusCode { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }
    }
}
