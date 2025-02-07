using AppFramework.Shared;
using HandyControl.Data;
using HandyControl.Tools;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows;
using System;

namespace AppFramework.Admin.HandyUI
{
    internal class VisualViewModel : NavigationViewModel
    {
        public VisualViewModel()
        {
            Title = Local.Localize("VisualSettings");
        }

        private bool isDarkTheme;

        public bool IsDarkTheme
        {
            get { return isDarkTheme; }
            set
            {
                isDarkTheme = value;
                UpdateSkin();
                OnPropertyChanged();
            }
        }

        void UpdateSkin()
        {
            SkinType skinType = IsDarkTheme ? SkinType.Dark : SkinType.Default;

            var skins0 = App.Current.Resources.MergedDictionaries[0];
            skins0.MergedDictionaries.Clear(); 
            skins0.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source=new Uri($"pack://application:,,,/HandyControl;component/Themes/Skin{skinType}.xaml", UriKind.RelativeOrAbsolute)
            });
            skins0.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source=new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml", UriKind.RelativeOrAbsolute)
            });
        
            App.Current.MainWindow?.OnApplyTemplate();
        }
    }
}
