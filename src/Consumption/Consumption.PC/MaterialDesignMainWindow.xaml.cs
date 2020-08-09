using Dragablz;
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
    public partial class MaterialDesignMainWindow : Window
    {
        public MaterialDesignMainWindow()
        {
            InitializeComponent();
            //AddHandler(DragablzItem.DragStarted, new DragablzDragStartedEventHandler(ItemDragStarted), true);
            //AddHandler(DragablzItem.DragCompleted, new DragablzDragCompletedEventHandler(ItemDragCompleted), true);
        }

        private void ItemDragStarted(object sender, DragablzDragStartedEventArgs e)
        {

        }

        private void ItemDragCompleted(object sender, DragablzDragCompletedEventArgs e)
        {
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
