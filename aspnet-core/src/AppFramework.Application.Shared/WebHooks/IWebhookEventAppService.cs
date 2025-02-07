using System.Threading.Tasks;
using Abp.Webhooks;

namespace AppFramework.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
