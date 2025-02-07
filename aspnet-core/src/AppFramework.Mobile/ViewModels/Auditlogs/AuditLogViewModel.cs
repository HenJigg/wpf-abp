using AppFramework.Auditing;
using AppFramework.Auditing.Dto;
using Prism.Commands;
using System;
using System.Threading.Tasks;

namespace AppFramework.Shared.ViewModels
{
    public class AuditLogViewModel : NavigationMasterViewModel
    {
        private readonly IAuditLogAppService appService;
        public GetAuditLogsInput input;
        private int CurrentPage;
        private int TotalCount;

        private AuditLogListDto auditLog;

        public AuditLogListDto AuditLog
        {
            get { return auditLog; }
            set { auditLog = value; RaisePropertyChanged(); }
        }

        public DelegateCommand<AuditLogListDto> DetailsCommand { get; private set; }

        public AuditLogViewModel(IAuditLogAppService appService)
        {
            input = new GetAuditLogsInput()
            {
                StartDate = DateTime.Now.AddDays(-30),
                EndDate = DateTime.Now,
                MaxResultCount = AppConsts.DefaultPageSize,
            };
            this.appService = appService;

            DetailsCommand=new DelegateCommand<AuditLogListDto>(ShowDetails);
        }

        private void ShowDetails(AuditLogListDto auditLog)
        {
            AuditLog=auditLog;
            messenger.Send("AuditLogDetailsViewIsVisible", true);
        }

        public override async Task RefreshAsync()
        {
            CurrentPage = 0;
            input.SkipCount = 0;

            await GetAuditLogAsync();
        }

        public override async void LoadMore()
        {
            if (IsBusy || dataPager.GridModelList?.Count == TotalCount) return;

            input.SkipCount = AppConsts.DefaultPageSize * ++CurrentPage;

            await GetAuditLogAsync(true);
        }

        private async Task GetAuditLogAsync(bool isAppend = false)
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => appService.GetAuditLogs(input),
                           async result =>
                           {
                               if (!isAppend)
                                   await dataPager.SetList(result);
                               else
                               {
                                   foreach (var item in result.Items)
                                       dataPager.GridModelList.Add(item);
                               }
                           });
            });
        }
    }
}