using Abp.Application.Services.Dto;
using Abp.UI.Inputs;
using AppFramework.ApiClient;
using AppFramework.Dto;
using AppFramework.DynamicEntityProperties;
using AppFramework.DynamicEntityProperties.Dto;
using System.Threading.Tasks;

namespace AppFramework.Application
{
    public class DynamicPropertyAppService : ProxyAppServiceBase, IDynamicPropertyAppService
    {
        public DynamicPropertyAppService(AbpApiClient apiClient) :
            base(apiClient)
        { }

        public async Task<PagedResultDto<DynamicPropertyDto>> GetListAsync(PagedAndSortedInputDto input)
        {
            var dynamicList = await ApiClient.GetAsync<ListResultDto<DynamicPropertyDto>>(GetEndpoint(nameof(GetAll)));
            return new PagedResultDto<DynamicPropertyDto>(dynamicList.Items.Count, dynamicList.Items);
        }

        public async Task<DynamicPropertyDto> Get(int id)
        {
            return await ApiClient.GetAsync<DynamicPropertyDto>(GetEndpoint(nameof(Get)), id);
        }

        public async Task<ListResultDto<DynamicPropertyDto>> GetAll()
        {
            return await ApiClient.GetAsync<ListResultDto<DynamicPropertyDto>>(GetEndpoint(nameof(GetAll)));
        }

        public async Task Add(DynamicPropertyDto dto)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(Add)), dto);
        }

        public async Task Update(DynamicPropertyDto dto)
        {
            await ApiClient.PutAsync(GetEndpoint(nameof(Update)), dto);
        }

        public async Task Delete(EntityDto input)
        {
            await ApiClient.DeleteAsync(GetEndpoint(nameof(Delete)), input);
        }

        public IInputType FindAllowedInputType(string name)
        {
            return ApiClient.GetAsync<IInputType>(GetEndpoint(nameof(FindAllowedInputType)), name).Result;
        }
    }
}