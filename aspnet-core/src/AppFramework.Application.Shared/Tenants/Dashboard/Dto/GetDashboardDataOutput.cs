using System.Collections.Generic;

namespace AppFramework.Tenants.Dashboard.Dto
{
    public class GetDashboardDataOutput
    {
        public int TotalProfit { get; set; }

        public int NewFeedbacks { get; set; }

        public int NewOrders { get; set; }

        public int NewUsers { get; set; }

        public List<SalesSummaryData> SalesSummary { get; set; }

        public int TotalSales { get; set; }

        public int Revenue { get; set; }

        public int Expenses { get; set; }

        public int Growth { get; set; }

        public int TransactionPercent { get; set; }


        public int NewVisitPercent { get; set; }

        public int BouncePercent { get; set; }

        public int[] DailySales { get; set; }

        public int[] ProfitShares { get; set; }

    }
}