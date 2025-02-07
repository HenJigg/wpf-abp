using AppFramework.Shared;
using Prism.Services.Dialogs;
using System.Threading.Tasks;

namespace AppFramework.Admin.ViewModels.Shared
{
    public class SelectTenantViewModel : HostDialogViewModel
    {
        private string tenancyName = string.Empty;
        private bool isLoginForTenants;

        public string TenancyName
        {
            get { return tenancyName; }
            set { tenancyName = value; OnPropertyChanged(); }
        }

        public bool IsLoginForTenants
        {
            get { return isLoginForTenants; }
            set
            {
                isLoginForTenants = value;
                if (!value) TenancyName = string.Empty;
                OnPropertyChanged();
            }
        }

        public override async Task Save()
        {
            base.Save(IsLoginForTenants ? TenancyName : "");

            await Task.CompletedTask;
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        { }
    }
}
