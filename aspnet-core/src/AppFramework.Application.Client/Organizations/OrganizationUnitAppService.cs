using Abp.Application.Services.Dto;
using AppFramework.ApiClient;
using AppFramework.Organizations;
using AppFramework.Organizations.Dto;
using System.Threading.Tasks;

namespace AppFramework.Application.Organizations
{
    public class OrganizationUnitAppService : ProxyAppServiceBase, IOrganizationUnitAppService
    {
        public OrganizationUnitAppService(AbpApiClient apiClient) : base(apiClient)
        {
        }

        public async Task AddRolesToOrganizationUnit(RolesToOrganizationUnitInput input)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(AddRolesToOrganizationUnit)), input);
        }

        public async Task AddUsersToOrganizationUnit(UsersToOrganizationUnitInput input)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(AddUsersToOrganizationUnit)), input);
        }

        public async Task<OrganizationUnitDto> CreateOrganizationUnit(CreateOrganizationUnitInput input)
        {
            return await ApiClient.PostAsync<OrganizationUnitDto>(GetEndpoint(nameof(CreateOrganizationUnit)), input);
        }

        public async Task DeleteOrganizationUnit(EntityDto<long> input)
        {
            await ApiClient.DeleteAsync(GetEndpoint(nameof(DeleteOrganizationUnit)), input);
        }

        public async Task<PagedResultDto<NameValueDto>> FindRoles(FindOrganizationUnitRolesInput input)
        {
            return await ApiClient.PostAsync<PagedResultDto<NameValueDto>>(GetEndpoint(nameof(FindRoles)), input);
        }

        public async Task<PagedResultDto<NameValueDto>> FindUsers(FindOrganizationUnitUsersInput input)
        {
            return await ApiClient.PostAsync<PagedResultDto<NameValueDto>>(GetEndpoint(nameof(FindUsers)), input);
        }

        public async Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnits()
        {
            return await ApiClient.GetAsync<ListResultDto<OrganizationUnitDto>>(GetEndpoint(nameof(GetOrganizationUnits)));
        }

        public async Task<PagedResultDto<OrganizationUnitUserListDto>> GetOrganizationUnitUsers(GetOrganizationUnitUsersInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<OrganizationUnitUserListDto>>(GetEndpoint(nameof(GetOrganizationUnitUsers)), input);
        }

        public async Task<PagedResultDto<OrganizationUnitRoleListDto>> GetOrganizationUnitRoles(GetOrganizationUnitRolesInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<OrganizationUnitRoleListDto>>(GetEndpoint(nameof(GetOrganizationUnitRoles)), input);
        }

        public async Task<OrganizationUnitDto> MoveOrganizationUnit(MoveOrganizationUnitInput input)
        {
            return await ApiClient.PostAsync<OrganizationUnitDto>(GetEndpoint(nameof(MoveOrganizationUnit)), input);
        }

        public async Task RemoveRoleFromOrganizationUnit(RoleToOrganizationUnitInput input)
        {
            await ApiClient.DeleteAsync(GetEndpoint(nameof(RemoveRoleFromOrganizationUnit)), input);
        }

        public async Task RemoveUserFromOrganizationUnit(UserToOrganizationUnitInput input)
        {
            await ApiClient.DeleteAsync(GetEndpoint(nameof(RemoveUserFromOrganizationUnit)), input);
        }

        public async Task<OrganizationUnitDto> UpdateOrganizationUnit(UpdateOrganizationUnitInput input)
        {
            return await ApiClient.PutAsync<OrganizationUnitDto>(GetEndpoint(nameof(UpdateOrganizationUnit)), input);
        }
    }
}