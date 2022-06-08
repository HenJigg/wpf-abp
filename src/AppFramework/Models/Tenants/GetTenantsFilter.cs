using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.Models.Tenants
{
    public class GetTenantsFilter : PagedAndSortedFilter
    {
        private DateTime? subscriptionEndDateStart;
        private DateTime? subscriptionEndDateEnd;
        private DateTime? creationDateStart;
        private DateTime? creationDateEnd;
        private string filter;
        private int? editionId;

        public string Filter
        {
            get { return filter; }
            set { filter = value; RaisePropertyChanged(); }
        }

        public DateTime? SubscriptionEndDateStart
        {
            get { return subscriptionEndDateStart; }
            set { subscriptionEndDateStart = value; RaisePropertyChanged(); }
        }

        public DateTime? SubscriptionEndDateEnd
        {
            get { return subscriptionEndDateEnd; }
            set { subscriptionEndDateEnd = value; RaisePropertyChanged(); }
        }

        public DateTime? CreationDateStart
        {
            get { return creationDateStart; }
            set { creationDateStart = value; RaisePropertyChanged(); }
        }

        public DateTime? CreationDateEnd
        {
            get { return creationDateEnd; }
            set { creationDateEnd = value; RaisePropertyChanged(); }
        }

        public int? EditionId
        {
            get { return editionId; }
            set { editionId = value; RaisePropertyChanged(); }
        }

        public bool EditionIdSpecified { get; set; }
    }
}
