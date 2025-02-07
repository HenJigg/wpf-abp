using System;

namespace AppFramework.Shared.Models
{
    public class TenantListModel : EntityObject
    {
        private string name;
        private string tenancyName;
        private string connectionString;
        private bool isActive;
        private DateTime? subscriptionEndDateUtc;
        private bool isInTrialPeriod;
        private string adminEmailAddress;
        private bool sendActivationEmail;
        private bool shouldChangePasswordOnNextLogin;

        public string AdminEmailAddress
        {
            get { return adminEmailAddress; }
            set { adminEmailAddress = value; RaisePropertyChanged(); }
        }

        public bool ShouldChangePasswordOnNextLogin
        {
            get { return shouldChangePasswordOnNextLogin; }
            set { shouldChangePasswordOnNextLogin = value; RaisePropertyChanged(); }
        }

        public bool SendActivationEmail
        {
            get { return sendActivationEmail; }
            set { sendActivationEmail = value; RaisePropertyChanged(); }
        }

        public string TenancyName
        {
            get { return tenancyName; }
            set { tenancyName = value; RaisePropertyChanged(); }
        }

        public string Name
        {
            get { return name; }
            set { name = value; RaisePropertyChanged(); }
        }

        public string EditionDisplayName { get; set; }

        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; RaisePropertyChanged(); }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; RaisePropertyChanged(); }
        }

        public DateTime? SubscriptionEndDateUtc
        {
            get { return subscriptionEndDateUtc; }
            set { subscriptionEndDateUtc = value; RaisePropertyChanged(); }
        }

        public DateTime CreationTime { get; set; }

        public int? EditionId { get; set; }

        public bool IsInTrialPeriod
        {
            get { return isInTrialPeriod; }
            set { isInTrialPeriod = value; RaisePropertyChanged(); }
        }
    }
}