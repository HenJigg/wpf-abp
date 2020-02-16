using GalaSoft.MvvmLight;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace NoteMoblie.ViewModel
{
    public class UserDetailChartViewModel : ViewModelBase
    {
        public UserDetailChartViewModel()
        {
            FilterBy = new ObservableCollection<string>();
            FilterBy.Add("按月");
            FilterBy.Add("按年");

            FilterResultOne = new ObservableCollection<string>();
            FilterResultTwo = new ObservableCollection<string>();
            SetFilterResult();
            InitChart();
        }

        private int index = 0;

        public int Index
        {
            get { return index; }
            set
            {
                if (index == value) return;
                index = value;
                SetFilterResult();
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<string> FilterBy { get; set; }

        public ObservableCollection<string> FilterResultOne { get; set; }
        public ObservableCollection<string> FilterResultTwo { get; set; }

        private void SetFilterResult()
        {
            FilterResultTwo.Clear();
            if (Index == 0)
            {
                FilterResultOne.Clear();
                int year = DateTime.Now.Year;
                int month = DateTime.Now.Month;

                for (int i = year - 5; i <= year; i++)
                    FilterResultOne.Add($"{i}年");

                for (int i = 1; i <= month; i++)
                    FilterResultTwo.Add($"{i}月");
            }
            else
            {
                int year = DateTime.Now.Year;
                for (int i = year - 5; i <= year; i++)
                    FilterResultTwo.Add($"{i}年");
            }
        }

        #region Chart

        private LineChart staLineChart;
        public LineChart StaLineChart
        {
            get { return staLineChart; }
            set { staLineChart = value; RaisePropertyChanged(); }
        }

        private PieChart staChart;
        public PieChart StaChart
        {
            get { return staChart; }
            set { staChart = value; RaisePropertyChanged(); }
        }

        private async void InitChart()
        {
            StaLineChart = new LineChart();

            List<ChartEntry> entries = new List<ChartEntry>();

            for (int i = 1; i < 32; i++)
            {
                int value = new Random().Next(10, 99);
                entries.Add(new ChartEntry(value)
                {
                    Label = i.ToString(),
                    Color = SKColor.Parse("#59E6B5"),
                    ValueLabel = value.ToString()
                });
                await Task.Delay(10);
            }

            StaLineChart.Entries = entries;
            StaLineChart.LineMode = LineMode.Spline;
            StaLineChart.LineSize = 4;
            StaLineChart.PointMode = PointMode.Circle;
            StaLineChart.PointSize = 12;
            StaLineChart.LabelOrientation = Orientation.Vertical;
            StaLineChart.AnimationDuration = new TimeSpan(2);

            StaChart = new PieChart();
            List<ChartEntry> pie_entries = new List<ChartEntry>();

            pie_entries.Add(new ChartEntry(20) { ValueLabel = "UWP", Color = SKColor.Parse("#FF7000") });
            pie_entries.Add(new ChartEntry(30) { ValueLabel = "IOS", Color = SKColor.Parse("#FFC100") });
            pie_entries.Add(new ChartEntry(40) { ValueLabel = "WP", Color = SKColor.Parse("#59E6B5") });
            pie_entries.Add(new ChartEntry(10) { ValueLabel = "AD", Color = SKColor.Parse("#3DD0FB") });
            StaChart.Entries = pie_entries;
            StaChart.LabelTextSize = 48;



        }

        #endregion

    }

    public class TestModel
    {
        public string Name { get; set; }

        public int Value { get; set; }

    }

}
