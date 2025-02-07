using AppFramework.Shared.Services.App;
using AppFramework.Shared;
using System.Windows;
using AppFramework.Shared.Services;
using System.Windows.Controls;
using AppFramework.Admin.ViewModels;
using AppFramework.Admin.MaterialUI.Themes.Controls;

namespace AppFramework.Admin.MaterialUI.Views
{
    public partial class MainTabsView : Window
    {
        private readonly IHostDialogService dialog;
        private readonly IAppStartService appStartService;

        public MainTabsView(IHostDialogService dialog, IAppStartService appStartService)
        {
            InitializeComponent();

            this.dialog = dialog;
            this.appStartService=appStartService;

            HeaderBorder.MouseDown += (s, e) =>
            {
                if (e.ClickCount == 2) SetWindowState();
            };
            HeaderBorder.MouseMove += (s, e) =>
            {
                if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                    this.DragMove();
            };
            this.MouseDown += (s, e) => toggleShowPanel.IsChecked = false;

            BtnMin.Click += BtnMin_Click;
            BtnMax.Click += BtnMax_Click;
            BtnClose.Click += BtnClose_Click;

            toggleMenuButton.Click +=BtnDoubleLeft_Click; ;
        }

        private void BtnDoubleLeft_Click(object sender, RoutedEventArgs e)
        {
            CollapseMenu();
        }

        private void CollapseMenu()
        {
            if (StackHeader.Visibility== Visibility.Visible)
            {
                StackHeader.Visibility = Visibility.Collapsed;
                GridLeftMenu.Width = new GridLength(70); 
            }
            else
            {
                StackHeader.Visibility = Visibility.Visible;
                GridLeftMenu.Width = new GridLength(220); 
            }
        }

        private async void BtnClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (await dialog.Question(Local.Localize("AreYouSure")))
                appStartService.Exit();
        }

        private void BtnMax_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SetWindowState();
        }

        private void BtnMin_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            WindowState = ((base.WindowState != System.Windows.WindowState.Minimized) ?
               System.Windows.WindowState.Minimized : System.Windows.WindowState.Normal);

            this.Hide();
        }

        private void SetWindowState()
        {
            this.WindowState = ((base.WindowState != System.Windows.WindowState.Maximized) ? System.Windows.WindowState.Maximized : System.Windows.WindowState.Normal);
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource != null&&e.OriginalSource is TabCloseItem tabItem)
            { 
                if (this.DataContext is MainTabsViewModel viewModel)
                    viewModel.NavigationService.RemoveView(tabItem.Content);
            }
        } 
    }
}
