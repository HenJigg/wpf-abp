using Abp.Application.Services.Dto;
using AppFramework.ApiClient;
using AppFramework.Caching;
using AppFramework.Caching.Dto;
using System;
using System.Threading.Tasks;

namespace AppFramework.Application.Client
{
    public class CachingAppService : ProxyAppServiceBase, ICachingAppService
    {
        public CachingAppService(AbpApiClient apiClient) : base(apiClient)
        {
        }

        public Task ClearAllCaches()
        {
            throw new NotImplementedException();
        }

        public Task ClearCache(EntityDto<string> input)
        {
            throw new NotImplementedException();
        }

        public ListResultDto<CacheDto> GetAllCaches()
        {
            throw new NotImplementedException();
        }
    }
}