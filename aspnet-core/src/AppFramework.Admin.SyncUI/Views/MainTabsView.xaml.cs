using AppFramework.Admin.ViewModels;
using AppFramework.Shared;
using AppFramework.Shared.Models;
using AppFramework.Shared.Services;
using AppFramework.Shared.Services.App; 
using DryIoc;
using Syncfusion.Windows.Shared;
using System.Windows;

namespace AppFramework.Admin.SyncUI
{
    public partial class MainTabsView : ChromelessWindow
    {
        private readonly IHostDialogService dialog;
        private readonly IAppStartService appStartService;

        public MainTabsView(IThemeService themeService,
            IHostDialogService dialog, IAppStartService appStartService)
        {
            InitializeComponent();
            this.dialog = dialog;
            this.appStartService=appStartService;
            themeService.SetCurrentTheme(this);
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
            BtnDoubleLeft.Click += BtnDoubleLeft_Click;
            treeViewItems.NodeExpanded += TreeViewItems_NodeExpanded;
            treeViewItems.SelectionChanged+=TreeViewItems_SelectionChanged;
        }

        private void TreeViewItems_SelectionChanged(object? sender, Syncfusion.UI.Xaml.TreeView.ItemSelectionChangedEventArgs e)
        {
            if (e != null && e.AddedItems != null)
            {
                if (e.AddedItems[0] is NavigationItem item)
                {
                    if (this.DataContext is MainTabsViewModel viewModel)
                        viewModel.Navigate(item);
                }
            }
        }

        private void TreeViewItems_NodeExpanded(object? sender, Syncfusion.UI.Xaml.TreeView.NodeExpandedCollapsedEventArgs e)
        {
            if (treeViewItems.ExpanderWidth == 0) CollapseMenu();
        }

        private void BtnDoubleLeft_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CollapseMenu();
        }

        private void CollapseMenu()
        {
            if (treeViewItems.ExpanderWidth > 0)
            {
                TitltStack.Visibility = Visibility.Collapsed;
                leftGridColumn.Width = new GridLength(50);
                TxtDoubleTitle.Text = "\ue74d";

                treeViewItems.ExpanderWidth = 0;
                treeViewItems.CollapseAll();
            }
            else
            {
                TitltStack.Visibility = Visibility.Visible;
                leftGridColumn.Width = new GridLength(220);
                TxtDoubleTitle.Text = "\ue74c";
                treeViewItems.ExpanderWidth = 15;
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
            SystemButtonsUpdate();
        }

        private void TabControlExt_OnCloseButtonClick(object sender, Syncfusion.Windows.Tools.Controls.CloseTabEventArgs e)
        {
            if (e.TargetTabItem != null)
            {
                if (this.DataContext is MainTabsViewModel viewModel)
                    viewModel.NavigationService.RemoveView(e.TargetTabItem.Content);
            }
        }

        private void TabControlExt_OnCloseAllTabs(object sender, Syncfusion.Windows.Tools.Controls.CloseTabEventArgs e)
        {
            if (e.ClosingTabItems != null)
            {
                if (this.DataContext is MainTabsViewModel viewModel)
                {
                    foreach (var item in e.ClosingTabItems)
                        viewModel.NavigationService.RemoveView(item);
                }
            }
        }

        private void TabControlExt_OnCloseOtherTabs(object sender, Syncfusion.Windows.Tools.Controls.CloseTabEventArgs e)
        {
            if (e.ClosingTabItems != null)
            {
                if (this.DataContext is MainTabsViewModel viewModel)
                {
                    foreach (var item in e.ClosingTabItems)
                    {
                        if (item != e.TargetTabItem.Content)
                            viewModel.NavigationService.RemoveView(item);
                    }
                }
            }
        }
    }
}