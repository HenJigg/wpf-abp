using Abp.Application.Services.Dto;
using AppFramework.ApiClient;
using AppFramework.MultiTenancy.Dto;
using System.Threading.Tasks;

namespace AppFramework.MultiTenancy
{
    public class ProxyTenantAppService : ProxyAppServiceBase, ITenantAppService
    {
        public ProxyTenantAppService(AbpApiClient apiClient) :
           base(apiClient)
        {
        }

        public async Task<GetTenantFeaturesEditOutput> GetTenantFeaturesForEdit(EntityDto input)
        {
            return await ApiClient.GetAsync<GetTenantFeaturesEditOutput>(GetEndpoint(nameof(GetTenantFeaturesForEdit)), input);
        }

        public async Task UpdateTenantFeatures(UpdateTenantFeaturesInput input)
        {
            await ApiClient.PutAsync(GetEndpoint(nameof(UpdateTenantFeatures)), input);
        }

        public async Task ResetTenantSpecificFeatures(EntityDto input)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(ResetTenantSpecificFeatures)), input);
        }

        public async Task UnlockTenantAdmin(EntityDto input)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(UnlockTenantAdmin)), input);
        }

        public async Task<PagedResultDto<TenantListDto>> GetTenants(GetTenantsInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<TenantListDto>>(GetEndpoint(nameof(GetTenants)), input);
        }

        public async Task CreateTenant(CreateTenantInput input)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(CreateTenant)), input);
        }

        public async Task<TenantEditDto> GetTenantForEdit(EntityDto input)
        {
            return await ApiClient.GetAsync<TenantEditDto>(GetEndpoint(nameof(GetTenantForEdit)), input);
        }

        public async Task UpdateTenant(TenantEditDto input)
        {
            await ApiClient.PutAsync(GetEndpoint(nameof(UpdateTenant)), input);
        }

        public async Task DeleteTenant(EntityDto input)
        {
            await ApiClient.DeleteAsync(GetEndpoint(nameof(DeleteTenant)), input);
        }
    }
}