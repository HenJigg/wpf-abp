using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Webhooks;

namespace AppFramework.WebHooks.Dto
{
    public class GetAllSubscriptionsOutput : EntityDto<Guid>
    {
        /// <summary>
        /// Subscription webhook endpoint
        /// </summary>
        public string WebhookUri { get; set; }

        /// <summary>
        /// Is subscription active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Subscribed webhook definitions unique names. <see cref="WebhookDefinition.Name"/>
        /// </summary>
        public List<string> Webhooks { get; set; }
    }
}
