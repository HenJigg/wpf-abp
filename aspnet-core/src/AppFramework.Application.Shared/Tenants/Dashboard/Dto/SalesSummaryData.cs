namespace AppFramework.Tenants.Dashboard.Dto
{
    public class SalesSummaryData
    {
        public string Period { get; set; }
        public int Sales { get; set; }
        public int Profit { get; set; }

        public SalesSummaryData(string period, int sales, int profit)
        {
            Period = period;
            Sales = sales;
            Profit = profit;
        }
    }
}