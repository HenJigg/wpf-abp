using Abp.Application.Services.Dto;
using AppFramework.ApiClient;
using AppFramework.Version.Dtos;
using System;
using System.Threading.Tasks;

namespace AppFramework.Version
{
    public class AbpVersionsAppService : ProxyAppServiceBase, IAbpVersionsAppService
    {
        public AbpVersionsAppService(AbpApiClient apiClient) : base(apiClient)
        {
        }

        public async Task CreateOrEdit(CreateOrEditAbpVersionDto input)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(CreateOrEdit)), input);
        }

        public async Task Delete(EntityDto input)
        {
            await ApiClient.DeleteAsync(GetEndpoint(nameof(Delete)), input);
        }

        public async Task<AbpVersionDto> GetAbpVersionForEdit(EntityDto input)
        {
            return await ApiClient.PostAsync<AbpVersionDto>(GetEndpoint(nameof(GetAbpVersionForEdit)), input);
        }

        public async Task<AbpVersionDto> GetAbpVersionForView(int id)
        {
            return await ApiClient.GetAsync<AbpVersionDto>(GetEndpoint(nameof(GetAbpVersionForView)), id);
        }

        public async Task<PagedResultDto<AbpVersionDto>> GetAll(GetAllAbpVersionsInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<AbpVersionDto>>(GetEndpoint(nameof(GetAll)), input);
        }

        public async Task<UpdateFileOutput> CheckVersion(CheckVersionInput input)
        {
            return await ApiClient.PostAnonymousAsync<UpdateFileOutput>(GetEndpoint(nameof(CheckVersion)), input);
        }
    }
}
