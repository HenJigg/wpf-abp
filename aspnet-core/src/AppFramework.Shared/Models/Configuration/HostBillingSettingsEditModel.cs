using Prism.Mvvm;

namespace AppFramework.Shared.Models.Configuration
{
    public class HostBillingSettingsEditModel : BindableBase
    {
        private string legalName;
        private string address;

        public string LegalName
        {
            get { return legalName; }
            set { legalName = value; RaisePropertyChanged(); }
        }

        public string Address
        {
            get { return address; }
            set { address = value; RaisePropertyChanged(); }
        }
    }
}