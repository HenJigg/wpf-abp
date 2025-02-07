using Abp.Application.Services.Dto;
using AppFramework.ApiClient;
using AppFramework.Authorization.Roles;
using AppFramework.Authorization.Roles.Dto;
using System.Threading.Tasks;

namespace AppFramework.Application
{
    public class RoleAppService : ProxyAppServiceBase, IRoleAppService
    {
        public RoleAppService(AbpApiClient apiClient) : base(apiClient)
        {
        }

        public async Task CreateOrUpdateRole(CreateOrUpdateRoleInput input)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(CreateOrUpdateRole)), input);
        }
         
        public async Task DeleteRole(EntityDto input)
        {
            await ApiClient.DeleteAsync(GetEndpoint(nameof(DeleteRole)), input);
        }

        public async Task<GetRoleForEditOutput> GetRoleForEdit(NullableIdDto input)
        {
            return await ApiClient.GetAsync<GetRoleForEditOutput>(GetEndpoint(nameof(GetRoleForEdit)), input);
        }

        public async Task<ListResultDto<RoleListDto>> GetRoles(GetRolesInput input)
        {
            return await ApiClient.PostAsync<ListResultDto<RoleListDto>>(GetEndpoint(nameof(GetRoles)), input);
        } 
    }
}