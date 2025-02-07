using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Abp.Webhooks;

namespace AppFramework.WebHooks.Dto
{
    public class GetAllSendAttemptsOutput
    {
        public Guid Id { get; set; }

        /// <summary>
        /// <see cref="WebhookEvent"/> foreign id 
        /// </summary>
        public Guid WebhookEventId { get; set; }

        /// <summary>
        /// Webhook unique name <see cref="WebhookDefinition.Name"/>
        /// </summary>
        public string WebhookName { get; set; }

        /// <summary>
        /// Webhook data as JSON string.
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Webhook response content that webhook endpoint send back
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// Webhook response status code that webhook endpoint send back
        /// </summary>
        public HttpStatusCode? ResponseStatusCode { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
