namespace AppFramework.Shared.Models
{
    public class DynamicPropertyModel : EntityObject
    {
        private string propertyName;
        private string displayName;
        private string inputType;
        private string permission;

        public string PropertyName
        {
            get { return propertyName; }
            set { propertyName = value; RaisePropertyChanged(); }
        }

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; RaisePropertyChanged(); }
        }

        public string InputType
        {
            get { return inputType; }
            set { inputType = value; RaisePropertyChanged(); }
        }

        public string Permission
        {
            get { return permission; }
            set { permission = value; RaisePropertyChanged(); }
        }

        public int? TenantId { get; set; }
    }
}