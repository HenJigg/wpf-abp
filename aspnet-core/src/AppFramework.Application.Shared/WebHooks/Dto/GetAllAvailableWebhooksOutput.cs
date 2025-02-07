namespace AppFramework.WebHooks.Dto
{
    public class GetAllAvailableWebhooksOutput
    {
        /// <summary>
        /// Unique name of the webhook.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Display name of the webhook.
        /// Optional.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Description for the webhook.
        /// Optional.
        /// </summary>
        public string Description { get; set; }
    }
}
