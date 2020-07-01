using Consumption.ViewModel;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Consumption.PC.View
{
    /// <summary>
    /// HomeView.xaml 的交互逻辑
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
            SeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "日用",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(8) },
                    LabelPoint = point =>
                    {
                       return  string.Format("{1:P}",point.Y,point.Participation);
                    },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "生活",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(6) },
                     LabelPoint = point =>
                    {
                       return  string.Format("{1:P}",point.Y,point.Participation);
                    },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "服装",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(10) },
                     LabelPoint = point =>
                    {
                       return  string.Format("{1:P}",point.Y,point.Participation);
                    },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "通讯",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(4) },
                     LabelPoint = point =>
                    {
                       return  string.Format("{1:P}",point.Y,point.Participation);
                    },
                    DataLabels = true
                }
            };
            SeriesCollection1 = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "花呗",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(8) },
                    LabelPoint = point =>
                    {
                       return  string.Format("{1:P}",point.Y,point.Participation);
                    },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "银行",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(11) },
                     LabelPoint = point =>
                    {
                       return  string.Format("{1:P}",point.Y,point.Participation);
                    },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "房贷",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(5) },
                     LabelPoint = point =>
                    {
                       return  string.Format("{1:P}",point.Y,point.Participation);
                    },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "车贷",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(4) },
                     LabelPoint = point =>
                    {
                       return  string.Format("{1:P}",point.Y,point.Participation);
                    },
                    DataLabels = true
                }
            };
            SeriesCollection2 = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "借款",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(8) },
                    LabelPoint = point =>
                    {
                       return  string.Format("{1:P}",point.Y,point.Participation);
                    },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "出行",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(6) },
                     LabelPoint = point =>
                    {
                       return  string.Format("{1:P}",point.Y,point.Participation);
                    },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "公益",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(10) },
                     LabelPoint = point =>
                    {
                       return  string.Format("{1:P}",point.Y,point.Participation);
                    },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "往来",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(4) },
                     LabelPoint = point =>
                    {
                       return  string.Format("{1:P}",point.Y,point.Participation);
                    },
                    DataLabels = true
                }
            };
            SeriesCollection3 = new SeriesCollection
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
            gridBill.DataContext = new HomeViewModel();
            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public SeriesCollection SeriesCollection1 { get; set; }
        public SeriesCollection SeriesCollection2 { get; set; }
        public SeriesCollection SeriesCollection3 { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
    }
}
