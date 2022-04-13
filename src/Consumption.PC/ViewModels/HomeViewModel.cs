using LiveCharts;
using LiveCharts.Wpf;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumption.PC.ViewModels
{
    public class HomeViewModel : BindableBase
    {
        public HomeViewModel()
        {
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "收入",
                    Values = new ChartValues<double> { 5674, 7842, 54648, 28574 ,17973,45000,23000,17829 },
                },
                new LineSeries
                {
                    Title = "支出",
                    Values = new ChartValues<double> { 7346, 15757, 9213, 11435 ,16708,20000,6000,7821,8897 },
                },
                new LineSeries
                {
                    Title = "贷款",
                    Values = new ChartValues<double> { 1200,2341, 13242, 8900, 4351 ,3400,12000,4300,6400 },
                }
            };
            Labels = new[] { "2020-01", "2020-02", "2020-03", "2020-04", "2020-05", "2020-06", "2020-07", "2020-08", "2020-09" };
            YFormatter = value => value.ToString("C");
        }
        public string SelectPageTitle { get; } = "首页";
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
    }
}
