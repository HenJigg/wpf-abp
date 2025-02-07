using AppFramework.Shared.Services;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace AppFramework.Admin.SyncUI
{
    public class ThemeService : BindableBase, IThemeService
    {
        public ThemeService()
        {
            ThemeItems = new ObservableCollection<ThemeItem>()
            {
                new ThemeItem(){  DisplayName="Fluent", LightName="FluentLight",DarkName="FluentDark"},
                new ThemeItem(){  DisplayName="Material",LightName="MaterialLight",DarkName="MaterialDark"},
                new ThemeItem(){  DisplayName="MaterialBlue",LightName="MaterialLightBlue",DarkName="MaterialDarkBlue"},
                new ThemeItem(){  DisplayName="Office2019",LightName="Office2019White",DarkName="Office2019Black"},
            };
        }

        private string themeName = "Material";
        private bool isDarkTheme = false;
        private ObservableCollection<ThemeItem> themeItems;

        public bool IsDarkTheme
        {
            get { return isDarkTheme; }
            set
            {
                isDarkTheme = value;
                SetThemeMode();
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ThemeItem> ThemeItems
        {
            get { return themeItems; }
            set { themeItems = value; RaisePropertyChanged(); }
        }

        public void SetTheme(string displayName)
        {
            themeName = displayName;
            SetThemeInternal();
        }

        public void SetThemeMode()
        {
            SetThemeInternal();
        }

        private void SetThemeInternal()
        {
            var item = ThemeItems.FirstOrDefault(t => t.DisplayName.Equals(themeName));
            var skinName = IsDarkTheme ? item.DarkName : item.LightName;

            if (System.Windows.Application.Current.MainWindow != null)
                System.Windows.Application.Current.MainWindow.SetTheme(skinName);
        }

        public void SetCurrentTheme(DependencyObject dependency)
        {
            var item = ThemeItems.FirstOrDefault(t => t.DisplayName.Equals(themeName));
            var skinName = IsDarkTheme ? item.DarkName : item.LightName;
            dependency.SetTheme(skinName);
        }

    }
}