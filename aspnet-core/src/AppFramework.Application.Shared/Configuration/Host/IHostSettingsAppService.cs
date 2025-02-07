using System.Threading.Tasks;
using Abp.Application.Services;
using AppFramework.Configuration.Host.Dto;

namespace AppFramework.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
