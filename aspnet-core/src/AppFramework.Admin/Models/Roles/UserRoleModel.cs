using CommunityToolkit.Mvvm.ComponentModel;
using Prism.Mvvm; 

namespace AppFramework.Admin.Models
{
    [INotifyPropertyChanged]
    public partial class UserRoleModel
    {
        [ObservableProperty]
        private bool isChecked;
         
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string RoleDisplayName { get; set; }

        public bool IsAssigned { get; set; }

        public bool InheritedFromOrganizationUnit { get; set; }
    }
}
