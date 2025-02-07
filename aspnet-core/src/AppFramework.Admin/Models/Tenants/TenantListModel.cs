using AppFramework.Shared.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace AppFramework.Admin.Models
{
    public partial class TenantListModel : EntityObject
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string tenancyName;

        [ObservableProperty]
        private string connectionString;

        [ObservableProperty]
        private bool isActive;

        [ObservableProperty]
        private DateTime? subscriptionEndDateUtc;

        [ObservableProperty]
        private bool isInTrialPeriod;

        [ObservableProperty]
        private string adminEmailAddress;

        [ObservableProperty]
        private bool sendActivationEmail;

        [ObservableProperty]
        private bool shouldChangePasswordOnNextLogin;
         
        public string EditionDisplayName { get; set; }
            
        public DateTime CreationTime { get; set; }

        public int? EditionId { get; set; } 
    }
}