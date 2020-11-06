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
    using LiveCharts;
    using LiveCharts.Wpf;
    using Microsoft.Toolkit.Mvvm.ComponentModel;
    using Consumption.ViewModel.Interfaces;

    /// <summary>
    /// 系统首页
    /// </summary>
    public class HomeCenter : ComponentCenter<HomeView>,IHomeCenter
    {
        public HomeCenter(IHomeViewModel viewModel) : base(viewModel) { }
    }

    /// <summary>
    /// 首页模块
    /// </summary>
    public class HomeViewModel : ObservableObject, IHomeViewModel
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
