using AppFramework.MultiTenancy.HostDashboard;
using AppFramework.MultiTenancy.HostDashboard.Dto;
using Prism.Regions.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AppFramework.Shared.Models;
using AppFramework.Shared.Extensions;

namespace AppFramework.Shared.ViewModels
{
    public class DashboardViewModel : RegionViewModel
    {
        public DashboardViewModel(IHostDashboardAppService appService)
        {
            this.appService = appService;
            EditionStatistics = new ObservableCollection<DoughnutChartPopulations>();
            IncomeStatistics = new ObservableCollection<AreaSeriesChart3DModel>();
        }

        #region 字段/属性

        private DateTime startDate = DateTime.Now.AddDays(-30);
        private DateTime endDate = DateTime.Now;

        private bool isDaily = true, isWeekly, isMonthly;
        private string timeInterval;
        private readonly IHostDashboardAppService appService;

        private ObservableCollection<TopStatusItem> topDashBoards;
        private ObservableCollection<DoughnutChartPopulations> editionStatistics;
        private ObservableCollection<AreaSeriesChart3DModel> incomeStatistics;

        public bool IsDaily
        {
            get { return isDaily; }
            set
            {
                isDaily = value;
                if (value)
                    AsyncRunner.Run(GetIncomeStatistics(ChartDateInterval.Daily));
                RaisePropertyChanged();
            }
        }

        public bool IsWeekly
        {
            get { return isWeekly; }
            set
            {
                isWeekly = value;
                if (value)
                    AsyncRunner.Run(GetIncomeStatistics(ChartDateInterval.Weekly));
                RaisePropertyChanged();
            }
        }

        public bool IsMonthly
        {
            get { return isMonthly; }
            set
            {
                isMonthly = value;
                if (value)
                    AsyncRunner.Run(GetIncomeStatistics(ChartDateInterval.Monthly));
                RaisePropertyChanged();
            }
        }

        public string TimeInterval
        {
            get { return timeInterval; }
            set { timeInterval = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 版本信息
        /// </summary>
        public ObservableCollection<DoughnutChartPopulations> EditionStatistics
        {
            get { return editionStatistics; }
            set { editionStatistics = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 收入统计
        /// </summary>
        public ObservableCollection<AreaSeriesChart3DModel> IncomeStatistics
        {
            get { return incomeStatistics; }
            set { incomeStatistics = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 统计面板
        /// </summary>
        public ObservableCollection<TopStatusItem> TopDashBoards
        {
            get { return topDashBoards; }
            set { topDashBoards = value; RaisePropertyChanged(); }
        }

        #endregion 字段/属性

        #region 统计方法

        /// <summary>
        /// 获取统计面板数据
        /// </summary>
        /// <returns></returns>
        private async Task GetWorkbenchSummary()
        {
            IsDaily = true;
            TimeInterval = $"{startDate.ToString("yyyy-MM-dd")}~{endDate.ToString("yyyy-MM-dd")}";

            await GetTopStatsData();
            await GetEditionTenantStatistics();
        }

        /// <summary>
        /// 获取最近一个月的统计数据 (新租户、新订阅金额、样例1、样例2)
        /// </summary>
        /// <returns></returns>
        private async Task GetTopStatsData()
        {
            await WebRequest.Execute(() => appService.GetTopStatsData(new GetTopStatsInput()
            {
                StartDate = startDate,
                EndDate = endDate
            }), result => GetTopStatsDataSuccessed(result));
        }

        /// <summary>
        /// 获取版本信息
        /// </summary>
        /// <returns></returns>
        private async Task GetEditionTenantStatistics()
        {
            await WebRequest.Execute(() =>
                appService.GetEditionTenantStatistics(new GetEditionTenantStatisticsInput()
                {
                    StartDate = startDate,
                    EndDate = endDate
                }), result => GetEditionTenantStatisticsSuccessed(result));
        }

        /// <summary>
        /// 获取收入统计信息
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="interval"></param>
        private async Task GetIncomeStatistics(ChartDateInterval interval)
        {
            await WebRequest.Execute(() => appService.GetIncomeStatistics(new GetIncomeStatisticsDataInput()
            {
                IncomeStatisticsDateInterval = interval,
                StartDate = startDate,
                EndDate = endDate
            }), result => GetIncomeStatisticsSuccessed(result));
        }

        private async Task GetTopStatsDataSuccessed(TopStatsData topStatsData)
        {
            if (topStatsData != null)
            {
                TopDashBoards = new ObservableCollection<TopStatusItem>()
                {
                    new TopStatusItem()
                    {
                        Title=Local.Localize(LocalizationKeys.NewSubscriptionAmount),
                        Logo=$"{LocalizationKeys.NewSubscriptionAmount}.png",
                        Amount=topStatsData.NewSubscriptionAmount,
                        BackgroundGradientStart = "#f59083",
                        BackgroundGradientEnd = "#fae188",
                    },
                    new TopStatusItem()
                    {
                        Title=Local.Localize(LocalizationKeys.NewTenants),
                        Logo=$"{LocalizationKeys.NewTenants}.png",
                        Amount=topStatsData.NewTenantsCount,
                        BackgroundGradientStart = "#ff7272",
                        BackgroundGradientEnd = "#f650c5",
                    },
                    new TopStatusItem()
                    {
                        Title=Local.Localize(LocalizationKeys.DashboardSampleStatistics),
                        Logo=$"{LocalizationKeys.DashboardSampleStatistics}.png",
                        Amount=topStatsData.DashboardPlaceholder1,
                        BackgroundGradientStart = "#5e7cea",
                        BackgroundGradientEnd = "#1dcce3",
                    },
                    new TopStatusItem()
                    {
                        Title=Local.Localize(LocalizationKeys.DashboardSampleStatistics),
                        Logo="DashboardSampleStatistics2.png",
                        Amount=topStatsData.DashboardPlaceholder2,
                        BackgroundGradientStart = "#255ea6",
                        BackgroundGradientEnd = "#b350d1",
                    },
                };
            }

            await Task.CompletedTask;
        }

        private async Task GetIncomeStatisticsSuccessed(GetIncomeStatisticsDataOutput output)
        {
            IncomeStatistics.Clear();
            foreach (var item in output?.IncomeStatistics)
            {
                IncomeStatistics.Add(new AreaSeriesChart3DModel()
                {
                    Date = item.Date,
                    Amount = item.Amount
                });
            }

            await Task.CompletedTask;
        }

        private async Task GetEditionTenantStatisticsSuccessed(GetEditionTenantStatisticsOutput output)
        {
            EditionStatistics.Clear();
            foreach (var edition in output?.EditionStatistics)
            {
                EditionStatistics.Add(new DoughnutChartPopulations()
                {
                    Category = edition.Label,
                    Expenditure = edition.Value,
                });
            }

            await Task.CompletedTask;
        }

        #endregion

        #region IRegionAware 接口实现

        public override bool IsNavigationTarget(INavigationContext navigationContext)
        {
            return false;
        }

        public override async void OnNavigatedTo(INavigationContext navigationContext)
        {
            await SetBusyAsync(async () =>
            {
                await GetWorkbenchSummary();
            });
        }

        #endregion
    }
}