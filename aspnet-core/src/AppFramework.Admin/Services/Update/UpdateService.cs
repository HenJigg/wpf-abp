using AppFramework.ApiClient;
using AppFramework.Shared;
using AppFramework.Shared.Services.App;
using AppFramework.Version;
using AppFramework.Version.Dtos;
using AutoUpdaterDotNET;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace AppFramework.Admin.Update
{
    public class UpdateService : IUpdateService
    {
        private readonly IAbpVersionsAppService appService;

        private readonly string CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public UpdateService(IAbpVersionsAppService appService)
        {
            this.appService = appService;
        }

        public async Task CheckVersion()
        {
            await WebRequest.Execute(() => appService.CheckVersion(new AppFramework.Version.Dtos.CheckVersionInput()
            {
                Version = CurrentVersion,
                ApplicationName = AppSharedConsts.AppName
            }), CheckVersionFinish);
        }

        private async Task CheckVersionFinish(UpdateFileOutput output)
        {
            if (output == null || !output.IsNewVersion) return;

            AutoUpdater.ShowSkipButton = false;
            AutoUpdater.ShowRemindLaterButton = false;
            AutoUpdater.LetUserSelectRemindLater = false;
            AutoUpdater.InstallationPath = Environment.CurrentDirectory;
            AutoUpdater.ReportErrors = true;
            AutoUpdater.ShowUpdateForm(new UpdateInfoEventArgs()
            {
                ChangelogURL = output.ChangelogURL,
                InstalledVersion = new System.Version(CurrentVersion),
                CurrentVersion = $"{AppSharedConsts.AppName} {output.Version}",
                DownloadURL = ApiUrlConfig.BaseUrl + output.DownloadURL,
                Mandatory = new Mandatory
                {
                    Value = output.IsForced,
                    MinimumVersion = output.MinimumVersion,
                },
                CheckSum = new CheckSum()
                {
                    Value = output.AlgorithmValue,
                    HashingAlgorithm = output.HashingAlgorithm,
                }
            });

            await Task.CompletedTask;
        }
    }
}
