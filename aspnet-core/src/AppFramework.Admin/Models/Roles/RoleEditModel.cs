using CommunityToolkit.Mvvm.ComponentModel; 

namespace AppFramework.Admin.Models
{
    [INotifyPropertyChanged]
    public partial class RoleEditModel 
    {
        [ObservableProperty]
        private string displayName;

        [ObservableProperty]
        private bool isDefault;

        public long? Id { get; set; } 
    }
}