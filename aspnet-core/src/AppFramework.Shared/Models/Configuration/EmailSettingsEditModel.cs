using Abp.Auditing;
using Prism.Mvvm;

namespace AppFramework.Shared.Models.Configuration
{
    public class EmailSettingsEditModel : BindableBase
    {
        private string defaultFromAddress;
        private string defaultFromDisplayName;
        private string smtpHost;
        private int smtpPort;
        private string smtpUserName;
        private string smtpPassword;
        private string smtpDomain;
        private bool smtpEnableSsl;
        private bool smtpUseDefaultCredentials;

        public string DefaultFromAddress
        {
            get { return defaultFromAddress; }
            set { defaultFromAddress = value; RaisePropertyChanged(); }
        }

        public string DefaultFromDisplayName
        {
            get { return defaultFromDisplayName; }
            set { defaultFromDisplayName = value; RaisePropertyChanged(); }
        }

        public string SmtpHost
        {
            get { return smtpHost; }
            set { smtpHost = value; RaisePropertyChanged(); }
        }

        public int SmtpPort
        {
            get { return smtpPort; }
            set { smtpPort = value; RaisePropertyChanged(); }
        }

        public string SmtpUserName
        {
            get { return smtpUserName; }
            set { smtpUserName = value; RaisePropertyChanged(); }
        }

        [DisableAuditing]
        public string SmtpPassword
        {
            get { return smtpPassword; }
            set { smtpPassword = value; RaisePropertyChanged(); }
        }

        public string SmtpDomain
        {
            get { return smtpDomain; }
            set { smtpDomain = value; RaisePropertyChanged(); }
        }

        public bool SmtpEnableSsl
        {
            get { return smtpEnableSsl; }
            set { smtpEnableSsl = value; RaisePropertyChanged(); }
        }

        public bool SmtpUseDefaultCredentials
        {
            get { return smtpUseDefaultCredentials; }
            set { smtpUseDefaultCredentials = value; RaisePropertyChanged(); }
        }
    }
}