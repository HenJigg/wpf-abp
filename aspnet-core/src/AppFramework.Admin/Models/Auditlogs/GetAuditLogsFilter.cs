using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace AppFramework.Admin.Models
{ 
    public partial class GetAuditLogsFilter : PagedAndSortedFilter
    {
        [ObservableProperty]
        public DateTime? startDate;

        [ObservableProperty]
        public DateTime? endDate;

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string serviceName;

        [ObservableProperty]
        private string methodName;

        [ObservableProperty]
        private string browserInfo;

        [ObservableProperty]
        private bool? hasException;

        [ObservableProperty]
        private int? minExecutionDuration;

        [ObservableProperty]
        private int? maxExecutionDuration; 
    }
}
