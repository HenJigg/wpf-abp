using CommunityToolkit.Mvvm.ComponentModel;
using System; 

namespace AppFramework.Admin.Models
{
    public partial class GetEntityChangeFilter : PagedAndSortedFilter
    {
        [ObservableProperty]
        public DateTime startDate;

        [ObservableProperty]
        public DateTime endDate;

        [ObservableProperty]
        public string userName;

        [ObservableProperty]
        public string entityTypeFullName; 
    }
}
