using CommunityToolkit.Mvvm.ComponentModel;
using System; 

namespace AppFramework.Admin.Models
{
    public partial class GetTenantsFilter : PagedAndSortedFilter
    {
        [ObservableProperty]
        private DateTime? subscriptionEndDateStart;

        [ObservableProperty]
        private DateTime? subscriptionEndDateEnd;

        [ObservableProperty]
        private DateTime? creationDateStart;

        [ObservableProperty]
        private DateTime? creationDateEnd;

        [ObservableProperty]
        public string filter;

        [ObservableProperty]
        private int? editionId;
          
        public bool EditionIdSpecified { get; set; }
    }
}
