using System.Collections.ObjectModel;
using System.Windows;

namespace AppFramework.Shared.Services
{
    public interface IThemeService
    {
        ObservableCollection<ThemeItem> ThemeItems { get; set; }

        void SetTheme(string themeName);

        void SetThemeMode();

        void SetCurrentTheme(DependencyObject dependency);
    }
}