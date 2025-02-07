using Abp.Application.Services;
using AppFramework.Dto;
using AppFramework.Logging.Dto;

namespace AppFramework.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
