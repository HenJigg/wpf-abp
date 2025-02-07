using AppFramework.ApiClient;
using AppFramework.Tenants.Dashboard;
using AppFramework.Tenants.Dashboard.Dto;

namespace AppFramework.Application
{
    public class TenantDashboardAppService : ProxyAppServiceBase, ITenantDashboardAppService
    {
        public TenantDashboardAppService(AbpApiClient apiClient) : base(apiClient)
        {
        }

        public GetDailySalesOutput GetDailySales()
        {
            return ApiClient.GetAsync<GetDailySalesOutput>(GetEndpoint(nameof(GetDailySales))).Result;
        }

        public GetDashboardDataOutput GetDashboardData(GetDashboardDataInput input)
        {
            return ApiClient.GetAsync<GetDashboardDataOutput>(GetEndpoint(nameof(GetDashboardData)), input).Result;
        }

        public GetGeneralStatsOutput GetGeneralStats()
        {
            return ApiClient.GetAsync<GetGeneralStatsOutput>(GetEndpoint(nameof(GetGeneralStats))).Result;
        }

        public GetMemberActivityOutput GetMemberActivity()
        {
            return ApiClient.GetAsync<GetMemberActivityOutput>(GetEndpoint(nameof(GetMemberActivity))).Result;
        }

        public GetProfitShareOutput GetProfitShare()
        {
            return ApiClient.GetAsync<GetProfitShareOutput>(GetEndpoint(nameof(GetProfitShare))).Result;
        }

        public GetRegionalStatsOutput GetRegionalStats()
        {
            return ApiClient.GetAsync<GetRegionalStatsOutput>(GetEndpoint(nameof(GetRegionalStats))).Result;
        }

        public GetSalesSummaryOutput GetSalesSummary(GetSalesSummaryInput input)
        {
            return ApiClient.GetAsync<GetSalesSummaryOutput>(GetEndpoint(nameof(GetSalesSummary)), input).Result;
        }

        public GetTopStatsOutput GetTopStats()
        {
            return ApiClient.GetAsync<GetTopStatsOutput>(GetEndpoint(nameof(GetTopStats))).Result;
        }
    }
}