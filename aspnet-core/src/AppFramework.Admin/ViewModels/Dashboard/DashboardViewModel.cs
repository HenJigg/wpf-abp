using AppFramework.Shared;
using AppFramework.Admin.Models; 
using AppFramework.MultiTenancy.HostDashboard;
using AppFramework.MultiTenancy.HostDashboard.Dto; 
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks; 

namespace AppFramework.Admin.ViewModels
{
    public class DashboardViewModel : NavigationViewModel
    {
        public DashboardViewModel(IHostDashboardAppService appService)
        {
            Title = Local.Localize("Dashboard");
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        public string TimeInterval
        {
            get { return timeInterval; }
            set { timeInterval = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 版本信息
        /// </summary>
        public ObservableCollection<DoughnutChartPopulations> EditionStatistics
        {
            get { return editionStatistics; }
            set { editionStatistics = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 收入统计
        /// </summary>
        public ObservableCollection<AreaSeriesChart3DModel> IncomeStatistics
        {
            get { return incomeStatistics; }
            set { incomeStatistics = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 统计面板
        /// </summary>
        public ObservableCollection<TopStatusItem> TopDashBoards
        {
            get { return topDashBoards; }
            set { topDashBoards = value; OnPropertyChanged(); }
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
            await appService.GetTopStatsData(new GetTopStatsInput()
            {
                StartDate = startDate,
                EndDate = endDate
            }).WebAsync(GetTopStatsDataSuccessed); 
        }

        /// <summary>
        /// 获取版本信息
        /// </summary>
        /// <returns></returns>
        private async Task GetEditionTenantStatistics()
        {
            await appService.GetEditionTenantStatistics(new GetEditionTenantStatisticsInput()
            {
                StartDate = startDate,
                EndDate = endDate
            }).WebAsync(GetEditionTenantStatisticsSuccessed); 
        }

        /// <summary>
        /// 获取收入统计信息
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="interval"></param>
        private async Task GetIncomeStatistics(ChartDateInterval interval)
        {
            await appService.GetIncomeStatistics(new GetIncomeStatisticsDataInput()
            {
                IncomeStatisticsDateInterval = interval,
                StartDate = startDate,
                EndDate = endDate
            }).WebAsync(GetIncomeStatisticsSuccessed); 
        }

        private async Task GetTopStatsDataSuccessed(TopStatsData topStatsData)
        {
            if (topStatsData != null)
            {
                TopDashBoards = new ObservableCollection<TopStatusItem>()
                {
                    new TopStatusItem()
                    {
                        Title=Local.Localize(AppLocalizationKeys.NewSubscriptionAmount),
                        Logo=$"/Assets/Images/{AppLocalizationKeys.NewSubscriptionAmount}.png",
                        Amount=topStatsData.NewSubscriptionAmount,
                        BackgroundGradientStart = "#f59083",
                        BackgroundGradientEnd = "#fae188",
                    },
                    new TopStatusItem()
                    {
                        Title=Local.Localize(AppLocalizationKeys.NewTenants),
                        Logo=$"/Assets/Images/{AppLocalizationKeys.NewTenants}.png",
                        Amount=topStatsData.NewTenantsCount,
                        BackgroundGradientStart = "#ff7272",
                        BackgroundGradientEnd = "#f650c5",
                    },
                    new TopStatusItem()
                    {
                        Title=Local.Localize(AppLocalizationKeys.DashboardSampleStatistics),
                        Logo=$"/Assets/Images/{AppLocalizationKeys.DashboardSampleStatistics}.png",
                        Amount=topStatsData.DashboardPlaceholder1,
                        BackgroundGradientStart = "#5e7cea",
                        BackgroundGradientEnd = "#1dcce3",
                    },
                    new TopStatusItem()
                    {
                        Title=Local.Localize(AppLocalizationKeys.DashboardSampleStatistics),
                        Logo="/Assets/Images/DashboardSampleStatistics2.png",
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

        #region INavigationAware 接口实现

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext)
        {
            await SetBusyAsync(async () =>
            {
                await GetWorkbenchSummary();
            });
        }

        #endregion 
    }
}