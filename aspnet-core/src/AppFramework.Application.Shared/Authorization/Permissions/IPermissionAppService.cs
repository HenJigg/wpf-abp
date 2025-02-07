using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AppFramework.Authorization.Permissions.Dto;
using System.Threading.Tasks;

namespace AppFramework.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        Task<ListResultDto<FlatPermissionWithLevelDto>> GetAllPermissions();
    }
}
