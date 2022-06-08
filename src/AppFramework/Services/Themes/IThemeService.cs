using AppFramework.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace AppFramework.Services
{
    public interface IThemeService
    {
        ObservableCollection<ThemeItem> ThemeItems { get; set; }

        string GetCurrentName();

        void SetTheme(string themeName);

        void SetThemeMode();

        void SetCurrentTheme(DependencyObject dependency);
    }
}