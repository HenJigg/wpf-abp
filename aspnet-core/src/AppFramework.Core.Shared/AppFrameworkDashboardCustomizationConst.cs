namespace AppFramework
{
    public class AppFrameworkDashboardCustomizationConsts
    {
        /// <summary>
        /// Main page name your user will see if they dont change default page's name.
        /// </summary>
        public const string DefaultPageName = "Default Page";

        //Must use underscore instead of dot in widget and filter ids
        //(these data are also used as ids in the input in html pages. Please provide appropriate values.)
        public class Widgets
        {
            public class Tenant
            {
                public const string GeneralStats = "Widgets_Tenant_GeneralStats";
                public const string DailySales = "Widgets_Tenant_DailySales";
                public const string ProfitShare = "Widgets_Tenant_ProfitShare";
                public const string MemberActivity = "Widgets_Tenant_MemberActivity";
                public const string RegionalStats = "Widgets_Tenant_RegionalStats";
                public const string SalesSummary = "Widgets_Tenant_SalesSummary";
                public const string TopStats = "Widgets_Tenant_TopStats";
            }

            public class Host
            {
                public const string TopStats = "Widgets_Host_TopStats";
                public const string IncomeStatistics = "Widgets_Host_IncomeStatistics";
                public const string EditionStatistics = "Widgets_Host_EditionStatistics";
                public const string SubscriptionExpiringTenants = "Widgets_Host_SubscriptionExpiringTenants";
                public const string RecentTenants = "Widgets_Host_RecentTenants";
            }
        }

        public class Filters
        {
            public const string FilterDateRangePicker = "Filters_DateRangePicker";
        }

        public class DashboardNames
        {
            public const string DefaultTenantDashboard = "TenantDashboard";

            public const string DefaultHostDashboard = "HostDashboard";
        }

        public class Applications
        {
            public const string Mvc = "Mvc";
            public const string Angular = "Angular";
        }
    }
}
