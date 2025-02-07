using Prism.Mvvm;

namespace AppFramework.Shared.Models
{
    public class EditionCreateModel : BindableBase
    {
        private string displayName;
        private decimal? deilyPrice;
        private decimal? weeklyPrice;
        private decimal? monthlyPrice;
        private decimal? annualPrice;
        private int? trialDayCount;
        private int? waitingDayAfterExpire;
        private int? expiringEditionId;

        public int? Id { get; set; }

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; RaisePropertyChanged(); }
        }

        public decimal? DailyPrice
        {
            get { return deilyPrice; }
            set { deilyPrice = value; RaisePropertyChanged(); }
        }

        public decimal? WeeklyPrice
        {
            get { return weeklyPrice; }
            set { weeklyPrice = value; RaisePropertyChanged(); }
        }

        public decimal? MonthlyPrice
        {
            get { return monthlyPrice; }
            set { monthlyPrice = value; RaisePropertyChanged(); }
        }

        public decimal? AnnualPrice
        {
            get { return annualPrice; }
            set { annualPrice = value; RaisePropertyChanged(); }
        }

        public int? TrialDayCount
        {
            get { return trialDayCount; }
            set { trialDayCount = value; RaisePropertyChanged(); }
        }

        public int? WaitingDayAfterExpire
        {
            get { return waitingDayAfterExpire; }
            set { waitingDayAfterExpire = value; RaisePropertyChanged(); }
        }

        public int? ExpiringEditionId
        {
            get { return expiringEditionId; }
            set { expiringEditionId = value; RaisePropertyChanged(); }
        }
    }
}