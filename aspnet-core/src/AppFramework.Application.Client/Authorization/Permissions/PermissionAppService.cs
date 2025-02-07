using Abp.Application.Services.Dto;
using AppFramework;
using AppFramework.ApiClient;
using AppFramework.Authorization.Permissions;
using AppFramework.Authorization.Permissions.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.Authorization.Permissions
{
    public class PermissionAppService : ProxyAppServiceBase, IPermissionAppService
    {
        public PermissionAppService(AbpApiClient apiClient) : base(apiClient)
        {
        }

        public async Task<ListResultDto<FlatPermissionWithLevelDto>> GetAllPermissions()
        {
            return await ApiClient.GetAsync<ListResultDto<FlatPermissionWithLevelDto>>(GetEndpoint(nameof(GetAllPermissions)));
        }
    }
}
