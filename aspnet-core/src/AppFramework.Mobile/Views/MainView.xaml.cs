using AppFramework.Shared.ViewModels;
using Prism.Regions.Xaml;
using Xamarin.Forms;
using AppFramework.Shared.Core;
using AppFramework.Shared.Models;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.Helpers;
using AppFramework.Shared.Services.Messenger;
using AppFramework.Shared.Services.Navigation;

namespace AppFramework.Shared.Views
{
    public partial class MainView : ContentView
    {
        private readonly IMessenger messenger;

        public MainView(IMessenger messenger)
        {
            InitializeComponent();
            SfTreeView.SelectionChanged += SfTreeView_SelectionChanged;
            this.messenger = messenger;
            //注销退出时, 移除所注册的区域
            this.messenger.Sub(AppMessengerKeys.RemoveAllRegion, RemoveAll);

            //设置屏幕安全区域  说明:左、上、右、下
            var safeArea = new SafeArea(false, true, false, false);
            SafeAreaEffect.SetSafeArea(navigationDrawer, safeArea);
        }

        private void SfTreeView_SelectionChanged(object sender, Syncfusion.XForms.TreeView.ItemSelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var navigationPage = e.AddedItems[0] as NavigationItem;
                if (navigationPage != null)
                {
                    //如果存在子项, 则返回
                    if (navigationPage.Items != null && navigationPage.Items.Count > 0)
                        return;

                    //导航选中菜单的内容页
                    (this.BindingContext as MainViewModel).Navigate(navigationPage);
                    navigationDrawer.IsOpen = !navigationDrawer.IsOpen;
                }
            }
        }

        private void btnSettingsClick(object sender, System.EventArgs e)
        {
            //展示用户主题设置
            viewSetting.Show();
        }

        private void btnHeaderClick(object sender, System.EventArgs e)
        {
            //隐藏左侧菜单
            navigationDrawer.IsOpen = !navigationDrawer.IsOpen;
        }

        /// <summary>
        /// 移除所有注册区域
        /// </summary>
        private void RemoveAll()
        {
            RegionManager.GetRegionManager(DisplayView).Regions.Remove(AppRegions.Main);
        }
    }
}