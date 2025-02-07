using CommunityToolkit.Mvvm.ComponentModel; 

namespace AppFramework.Admin.Models
{
    [INotifyPropertyChanged]
    public partial class EditionCreateModel 
    {
        [ObservableProperty]
        public string displayName;

        [ObservableProperty]
        public decimal? dailyPrice;

        [ObservableProperty]
        public decimal? weeklyPrice;

        [ObservableProperty]
        public decimal? monthlyPrice;

        [ObservableProperty]
        public decimal? annualPrice;

        [ObservableProperty]
        public int? trialDayCount;

        [ObservableProperty]
        public int? waitingDayAfterExpire;

        [ObservableProperty]
        public int? expiringEditionId;

        public int? Id { get; set; } 
    }
}