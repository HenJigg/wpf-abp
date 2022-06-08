using Syncfusion.UI.Xaml.Charts;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace AppFramework.Views
{ 
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();

            var colorModel = new ChartColorModel();
            var customBrushes = new List<Brush>();
            customBrushes.Add(new SolidColorBrush(Color.FromArgb(0xFF, 0x47, 0xBA, 0x9F)));
            customBrushes.Add(new SolidColorBrush(Color.FromArgb(0xFF, 0xE5, 0x88, 0x70)));
            customBrushes.Add(new SolidColorBrush(Color.FromArgb(0xFF, 0x96, 0x86, 0xC9)));
            customBrushes.Add(new SolidColorBrush(Color.FromArgb(0xFF, 0xE5, 0x65, 0x90)));

            colorModel.CustomBrushes = customBrushes;
            doughnutSeries1.ColorModel = colorModel;
            doughnutSeries1.Palette = ChartColorPalette.Custom;
        }
    }
}