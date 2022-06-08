using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.Models.Auditlogs
{
    public class GetEntityChangeFilter : PagedAndSortedFilter
    {
        private DateTime startTime;
        private DateTime endTime;
        private string userName;
        private string entityTypeFullName;

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

        public string EntityTypeFullName
        {
            get { return entityTypeFullName; }
            set { entityTypeFullName = value; RaisePropertyChanged(); }
        }
    }
}
