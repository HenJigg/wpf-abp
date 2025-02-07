using Abp.Application.Services.Dto;
using AppFramework.ApiClient;
using AppFramework.DynamicEntityProperties;
using AppFramework.DynamicEntityProperties.Dto;
using System.Threading.Tasks;

namespace AppFramework.Application
{
    public class DynamicPropertyValueAppService : ProxyAppServiceBase, IDynamicPropertyValueAppService
    {
        public DynamicPropertyValueAppService(AbpApiClient apiClient) : base(apiClient)
        {
        }

        public async Task Add(DynamicPropertyValueDto dto)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(Add)), dto);
        }

        public async Task Delete(int id)
        {
            await ApiClient.DeleteAsync(GetEndpoint(nameof(Delete) + $"?id={id}"));
        }

        public async Task<DynamicPropertyValueDto> Get(int id)
        {
            return await ApiClient.GetAsync<DynamicPropertyValueDto>(GetEndpoint(nameof(Get)), id);
        }

        public async Task<ListResultDto<DynamicPropertyValueDto>> GetAllValuesOfDynamicProperty(EntityDto input)
        {
            return await ApiClient.GetAsync<ListResultDto<DynamicPropertyValueDto>>(GetEndpoint(nameof(GetAllValuesOfDynamicProperty)), input);
        }

        public async Task Update(DynamicPropertyValueDto dto)
        {
            await ApiClient.PutAsync(GetEndpoint(nameof(Update)), dto);
        }
    }
}