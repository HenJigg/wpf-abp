using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppFramework.MultiTenancy.HostDashboard.Dto;

namespace AppFramework.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}