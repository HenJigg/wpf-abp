using CommunityToolkit.Mvvm.ComponentModel;
using Prism.Mvvm;

namespace AppFramework.Admin.Models
{
    [INotifyPropertyChanged]
    public partial class CreateOrganizationUnitModel 
    {
        [ObservableProperty]
        private string displayName;

        public long? ParentId { get; set; } 
    }
}