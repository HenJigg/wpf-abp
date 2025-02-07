namespace AppFramework.Admin.Models
{
    public class EditionListModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public decimal? DailyPrice { get; set; }

        public decimal? WeeklyPrice { get; set; }

        public decimal? MonthlyPrice { get; set; }

        public decimal? AnnualPrice { get; set; }

        public int? WaitingDayAfterExpire { get; set; }

        public int? TrialDayCount { get; set; }

        public string ExpiringEditionDisplayName { get; set; }
    }
}