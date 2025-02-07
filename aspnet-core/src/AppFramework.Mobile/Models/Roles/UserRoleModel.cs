using Prism.Mvvm; 

namespace AppFramework.Shared.Models
{
    public class UserRoleModel : BindableBase
    {
        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; RaisePropertyChanged(); }
        }
         
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string RoleDisplayName { get; set; }

        public bool IsAssigned { get; set; }

        public bool InheritedFromOrganizationUnit { get; set; }
    }
}
