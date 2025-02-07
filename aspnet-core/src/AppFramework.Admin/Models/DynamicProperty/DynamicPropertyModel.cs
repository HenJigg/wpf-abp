using AppFramework.Shared.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AppFramework.Admin.Models
{
    public partial class DynamicPropertyModel : EntityObject
    {
        [ObservableProperty]
        private string propertyName;

        [ObservableProperty]
        private string displayName;

        [ObservableProperty]
        private string inputType;

        [ObservableProperty]
        private string permission;
         
        public int? TenantId { get; set; }
    }
}