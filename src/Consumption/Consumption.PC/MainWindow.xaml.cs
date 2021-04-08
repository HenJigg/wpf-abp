using System.Windows;

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
        }

        private void btnMax(object sender, RoutedEventArgs e)
        {
            this.UnregisterName("");
            this.SetWindowSize();
        }

        private void SetWindowSize()
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;
        }

        private void btnClose(object sender, RoutedEventArgs e)
        {
            //Question?
            App.Current.Shutdown();
        }

        private void btnMin(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
