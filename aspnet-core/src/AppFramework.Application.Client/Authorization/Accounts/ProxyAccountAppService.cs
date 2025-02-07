using AppFramework.ApiClient;
using AppFramework.Authorization.Accounts.Dto;
using System.Threading.Tasks;

namespace AppFramework.Authorization.Accounts
{
    public class ProxyAccountAppService : ProxyAppServiceBase, IAccountAppService
    {
        public ProxyAccountAppService(AbpApiClient apiClient) : base(apiClient)
        {
        }

        public async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
        {
            return await ApiClient.PostAnonymousAsync<IsTenantAvailableOutput>(GetEndpoint(nameof(IsTenantAvailable)), input);
        }

        public async Task<int?> ResolveTenantId(ResolveTenantIdInput input)
        {
            return await ApiClient.PostAnonymousAsync<int?>(GetEndpoint(nameof(ResolveTenantId)), input);
        }

        public async Task<RegisterOutput> Register(RegisterInput input)
        {
            return await ApiClient.PostAnonymousAsync<RegisterOutput>(GetEndpoint(nameof(Register)), input);
        }

        public async Task SendPasswordResetCode(SendPasswordResetCodeInput input)
        {
            await ApiClient.PostAnonymousAsync(GetEndpoint(nameof(SendPasswordResetCode)), input);
        }

        public async Task<ResetPasswordOutput> ResetPassword(ResetPasswordInput input)
        {
            return await ApiClient.PostAnonymousAsync<ResetPasswordOutput>(GetEndpoint(nameof(ResetPassword)), input);
        }

        public async Task SendEmailActivationLink(SendEmailActivationLinkInput input)
        {
            await ApiClient.PostAnonymousAsync(GetEndpoint(nameof(SendEmailActivationLink)), input);
        }

        public async Task ActivateEmail(ActivateEmailInput input)
        {
            await ApiClient.PostAnonymousAsync(GetEndpoint(nameof(ActivateEmail)), input);
        }

        public async Task<ImpersonateOutput> ImpersonateUser(ImpersonateUserInput input)
        {
            return await ApiClient.PostAsync<ImpersonateOutput>(GetEndpoint(nameof(ImpersonateUser)), input);
        }

        public async Task<ImpersonateOutput> ImpersonateTenant(ImpersonateTenantInput input)
        {
            return await ApiClient.PostAsync<ImpersonateOutput>(GetEndpoint(nameof(ImpersonateTenant)), input);
        }

        public async Task<ImpersonateOutput> BackToImpersonator()
        {
            return await ApiClient.PostAnonymousAsync<ImpersonateOutput>(GetEndpoint(nameof(BackToImpersonator)));
        }

        public async Task<SwitchToLinkedAccountOutput> SwitchToLinkedAccount(SwitchToLinkedAccountInput input)
        {
            return await ApiClient.PostAnonymousAsync<SwitchToLinkedAccountOutput>(GetEndpoint(nameof(SwitchToLinkedAccount)));
        }

        public async Task<ImpersonateOutput> DelegatedImpersonate(DelegatedImpersonateInput input)
        {
            return await ApiClient.PostAsync<ImpersonateOutput>(GetEndpoint(nameof(DelegatedImpersonate)), input);
        }
    }
}