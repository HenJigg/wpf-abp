using Consumption.PC.View;
using Consumption.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Consumption.PC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<string>(this, "UpdateBackground", UpdateBackground);
            Messenger.Default.Register<double>(this, "UpdateTrans", UpdateTrans);
            Messenger.Default.Register<double>(this, "UpdateGaussian", UpdateGaussian);
        }

        private void UpdateGaussian(double obj)
        {
            img_gaussian.Radius = obj;
            //保存用户配置...
        }

        private void UpdateTrans(double obj)
        {
            bd_trans.Opacity = obj / 100;
            //保存用户配置...
        }

        /// <summary>
        /// 更新用户背景颜色
        /// </summary>
        /// <param name="obj">图片名称</param>
        private void UpdateBackground(string obj)
        {
            string findurl = $"{AppDomain.CurrentDomain.BaseDirectory}Skin\\Kind\\{obj}";
            if (System.IO.File.Exists(findurl))
            {
                img.Source = new BitmapImage(new Uri(findurl, UriKind.RelativeOrAbsolute));
                //保存用户配置...
            }
        }
    }
}
