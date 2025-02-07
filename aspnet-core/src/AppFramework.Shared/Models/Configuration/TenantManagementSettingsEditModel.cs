using Prism.Mvvm;

namespace AppFramework.Shared.Models.Configuration
{
    public class TenantManagementSettingsEditModel : BindableBase
    {
        private bool allowSelfRegistration;
        private bool isNewRegisteredTenantActiveByDefault;
        private bool useCaptchaOnRegistration;
        private int? defaultEditionId;

        public bool AllowSelfRegistration
        {
            get { return allowSelfRegistration; }
            set { allowSelfRegistration = value; RaisePropertyChanged(); }
        }

        public bool IsNewRegisteredTenantActiveByDefault
        {
            get { return isNewRegisteredTenantActiveByDefault; }
            set { isNewRegisteredTenantActiveByDefault = value; RaisePropertyChanged(); }
        }

        public bool UseCaptchaOnRegistration
        {
            get { return useCaptchaOnRegistration; }
            set { useCaptchaOnRegistration = value; RaisePropertyChanged(); }
        }

        public int? DefaultEditionId
        {
            get { return defaultEditionId; }
            set { defaultEditionId = value; RaisePropertyChanged(); }
        }
    }
}