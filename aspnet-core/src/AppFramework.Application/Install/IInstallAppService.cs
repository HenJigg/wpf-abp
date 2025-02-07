using System.Threading.Tasks;
using Abp.Application.Services;
using AppFramework.Install.Dto;

namespace AppFramework.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}