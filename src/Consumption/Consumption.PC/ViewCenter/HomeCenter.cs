/*
*
* 文件名    ：HomeCenter                             
* 程序说明  : 系统首页 
* 更新时间  : 2020-07-19 14:17
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

namespace Consumption.PC.ViewCenter
{
    using Consumption.PC.View;
    using System;
    using Consumption.Core.Entity;
    using GalaSoft.MvvmLight;
    using System.Collections.ObjectModel;
    using LiveCharts;
    using LiveCharts.Wpf;

    /// <summary>
    /// 系统首页
    /// </summary>
    public class HomeCenter : BaseCenter<HomeView, HomeViewModel>
    {

    }

    /// <summary>
    /// 首页模块
    /// </summary>
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel()
        {
            Bills = new ObservableCollection<Bill>();
            for (int i = 0; i < 13; i++)
            {
                Bills.Add(new Bill()
                {
                    Remark = "公益活动",
                    Name = "参与社区活动",
                    CreateDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    Amount = 3000
                });
            }

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "收入",
                    Values = new ChartValues<double> { 5674, 7842, 4648, 8574 ,7973 },
                },
                new LineSeries
                {
                    Title = "支出",
                    Values = new ChartValues<double> { 7346, 5757, 9213, 11435 ,16708 },
                }
            };
            Labels = new[] { "2020-01", "2020-02", "2020-03", "2020-04", "2020-05" };
            YFormatter = value => value.ToString("C");
        }

        public string SelectPageTitle { get; } = "首页";

        private ObservableCollection<Bill> bills;

        public ObservableCollection<Bill> Bills
        {
            get { return bills; }
            set { bills = value; RaisePropertyChanged(); }
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
    }
}
