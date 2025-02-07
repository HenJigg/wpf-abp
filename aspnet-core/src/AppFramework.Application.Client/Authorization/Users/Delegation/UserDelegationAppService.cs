using Abp.Application.Services.Dto;
using AppFramework.ApiClient;
using AppFramework.Authorization.Users.Delegation.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppFramework.Authorization.Users.Delegation
{
    public class UserDelegationAppService : ProxyAppServiceBase, IUserDelegationAppService
    {
        public UserDelegationAppService(AbpApiClient apiClient) : base(apiClient)
        {
        }

        public async Task DelegateNewUser(CreateUserDelegationDto input)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(DelegateNewUser)), input);
        }

        public async Task<List<UserDelegationDto>> GetActiveUserDelegations()
        {
            return await ApiClient.GetAsync<List<UserDelegationDto>>(GetEndpoint(nameof(GetActiveUserDelegations)));
        }

        public async Task<PagedResultDto<UserDelegationDto>> GetDelegatedUsers(GetUserDelegationsInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<UserDelegationDto>>(GetEndpoint(nameof(GetDelegatedUsers)), input);
        }

        public async Task RemoveDelegation(EntityDto<long> input)
        {
            await ApiClient.DeleteAsync(GetEndpoint(nameof(RemoveDelegation)), input);
        }
    }
}
