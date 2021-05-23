using Consumption.Shared.Common.Events;
using Dragablz;
using Prism.Events;
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
using System.Windows.Shapes;

namespace Consumption.PC
{
    /// <summary>
    /// MaterialDesignMainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IEventAggregator aggregator)
        {
            InitializeComponent();
            aggregator.GetEvent<StringMessageEvent>().Subscribe(Execute);
        }

        void Execute(string arg)
        {
            switch (arg)
            {
                case "Min": this.WindowState = WindowState.Minimized; break;
                case "Max":
                    if (WindowState == WindowState.Normal)
                        this.WindowState = WindowState.Maximized;
                    else
                        this.WindowState = WindowState.Normal;
                    break;
                case "Exit": Environment.Exit(0); break;
            }
        }

        private void btnGithub(object sender, RoutedEventArgs e)
        {
            Link.OpenInBrowser("https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples");
        }

        private void btnBilibili(object sender, RoutedEventArgs e)
        {
            Link.OpenInBrowser("https://space.bilibili.com/32497462");
        }

        private void btnQQ(object sender, RoutedEventArgs e)
        {
            Link.OpenInBrowser(" http://qm.qq.com/cgi-bin/qm/qr?k=KpcFszjNfY2g-o0q1eEMIoYWbzjSMO2-&authKey=lg1kMENlcHkLO2gejRLvXmGq9Xy6GGb0X1h%2B9QDMhbNxvLLLugAsDQUIuzPJZhDy&group_code=874752819");
        }
    }
}
