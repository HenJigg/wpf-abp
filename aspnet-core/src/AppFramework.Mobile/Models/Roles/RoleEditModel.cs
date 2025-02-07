using Prism.Mvvm;

namespace AppFramework.Shared.Models
{
    public class RoleEditModel : BindableBase
    {
        private string displayName;
        private bool isDefault;

        public long? Id { get; set; }

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; RaisePropertyChanged(); }
        }

        public bool IsDefault
        {
            get { return isDefault; }
            set { isDefault = value; RaisePropertyChanged(); }
        }
    }
}