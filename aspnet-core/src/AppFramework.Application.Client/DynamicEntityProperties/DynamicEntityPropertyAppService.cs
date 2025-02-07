using Abp.Application.Services.Dto;
using AppFramework.ApiClient;
using AppFramework.DynamicEntityProperties;
using AppFramework.DynamicEntityProperties.Dto;
using System.Threading.Tasks;

namespace AppFramework.Application
{
    public class DynamicEntityPropertyAppService : ProxyAppServiceBase, IDynamicEntityPropertyAppService
    {
        public DynamicEntityPropertyAppService(AbpApiClient apiClient) : base(apiClient)
        {
        }

        public async Task Add(DynamicEntityPropertyDto dto)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(Add)), dto);
        }

        public async Task Delete(int id)
        {
            await ApiClient.DeleteAsync(GetEndpoint(nameof(Delete) + $"?id={id}"));
        }

        public async Task<DynamicEntityPropertyDto> Get(int id)
        {
            return await ApiClient.GetAsync<DynamicEntityPropertyDto>(GetEndpoint(nameof(Get)), id);
        }

        public async Task<ListResultDto<DynamicEntityPropertyDto>> GetAll()
        {
            return await ApiClient.GetAsync<ListResultDto<DynamicEntityPropertyDto>>(GetEndpoint(nameof(GetAll)));
        }

        public async Task<ListResultDto<GetAllEntitiesHasDynamicPropertyOutput>> GetAllEntitiesHasDynamicProperty()
        {
            return await ApiClient.GetAsync<ListResultDto<GetAllEntitiesHasDynamicPropertyOutput>>(GetEndpoint(nameof(GetAllEntitiesHasDynamicProperty)));
        }

        public async Task<ListResultDto<DynamicEntityPropertyDto>> GetAllPropertiesOfAnEntity(DynamicEntityPropertyGetAllInput input)
        {
            return await ApiClient.GetAsync<ListResultDto<DynamicEntityPropertyDto>>(GetEndpoint(nameof(GetAllPropertiesOfAnEntity)), input);
        }

        public async Task Update(DynamicEntityPropertyDto dto)
        {
            await ApiClient.PutAsync(GetEndpoint(nameof(Update)), dto);
        }
    }
}