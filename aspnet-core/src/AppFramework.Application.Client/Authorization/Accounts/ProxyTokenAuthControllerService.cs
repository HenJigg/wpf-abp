using AppFramework.ApiClient;
using System.Threading.Tasks;

namespace AppFramework.Authorization.Accounts
{
    public class ProxyTokenAuthControllerService : ProxyControllerBase
    {
        public ProxyTokenAuthControllerService(AbpApiClient apiClient) : base(apiClient)
        {
        }

        public async Task SendTwoFactorAuthCode(long userId, string provider)
        {
            await ApiClient
                .PostAsync("api/" + GetEndpoint(nameof(SendTwoFactorAuthCode)), new { UserId = userId, Provider = provider });
        }
    }
}