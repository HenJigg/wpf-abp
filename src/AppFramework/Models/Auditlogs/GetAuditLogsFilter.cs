using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.Models.Auditlogs
{
    public class GetAuditLogsFilter : PagedAndSortedFilter
    {
        private DateTime startTime;
        private DateTime endTime;
        private string userName;
        private string serviceName;
        private string methodName;
        private string browserInfo;
        private bool? hasException;
        private int? minExecutionDuration;
        private int? maxExecutionDuration;

        public DateTime StartDate
        {
            get { return startTime; }
            set { startTime = value; RaisePropertyChanged(); }
        }

        public DateTime EndDate
        {
            get { return endTime; }
            set { endTime = value; RaisePropertyChanged(); }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; RaisePropertyChanged(); }
        }

        public string ServiceName
        {
            get { return serviceName; }
            set { serviceName = value; RaisePropertyChanged(); }
        }

        public string MethodName
        {
            get { return methodName; }
            set { methodName = value; RaisePropertyChanged(); }
        }

        public string BrowserInfo
        {
            get { return browserInfo; }
            set { browserInfo = value; RaisePropertyChanged(); }
        }

        public bool? HasException
        {
            get { return hasException; }
            set { hasException = value; RaisePropertyChanged(); }
        }

        public int? MinExecutionDuration
        {
            get { return minExecutionDuration; }
            set { minExecutionDuration = value; RaisePropertyChanged(); }
        }

        public int? MaxExecutionDuration
        {
            get { return maxExecutionDuration; }
            set { maxExecutionDuration = value; RaisePropertyChanged(); }
        }
    }
}
