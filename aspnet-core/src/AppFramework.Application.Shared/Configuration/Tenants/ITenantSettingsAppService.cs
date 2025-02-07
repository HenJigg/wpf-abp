using System.Threading.Tasks;
using Abp.Application.Services;
using AppFramework.Configuration.Tenants.Dto;

namespace AppFramework.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
