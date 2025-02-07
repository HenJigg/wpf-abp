using Syncfusion.SfSkinManager;
using Syncfusion.Themes.FluentDark.WPF;
using Syncfusion.Themes.FluentLight.WPF;
using Syncfusion.Themes.MaterialDark.WPF;
using Syncfusion.Themes.MaterialDarkBlue.WPF;
using Syncfusion.Themes.MaterialLight.WPF;
using Syncfusion.Themes.MaterialLightBlue.WPF;
using Syncfusion.Themes.Office2019Black.WPF;
using Syncfusion.Themes.Office2019White.WPF;
using System.Windows;
using System.Windows.Media;

namespace AppFramework.Admin.SyncUI
{
    internal static class SkinManagerExtensions
    {
        public static void SetTheme(
            this DependencyObject dependencyObject,
            string themeName)
        {
            IThemeSetting theme = GetThemeSetting(themeName);
            var themeTypeName = theme.GetType().Name.Replace("ThemeSettings", "");
            SfSkinManager.RegisterThemeSettings(themeTypeName, theme);
            SfSkinManager.SetTheme(dependencyObject, new Theme(themeTypeName));
        }

        private static IThemeSetting GetThemeSetting(string themeName)
        {
            FontFamily fontFamily = new FontFamily("Microsoft YaHei");
            int bodyfontSize = 14;
            IThemeSetting theme = null;
            switch (themeName)
            {
                case "FluentLight":
                    theme = new FluentLightThemeSettings() { FontFamily = fontFamily, BodyFontSize = bodyfontSize, };
                    break;

                case "MaterialLight":
                    theme = new MaterialLightThemeSettings() { FontFamily = fontFamily, BodyFontSize = bodyfontSize, };
                    break;

                case "MaterialLightBlue":
                    theme = new MaterialLightBlueThemeSettings() { FontFamily = fontFamily, BodyFontSize = bodyfontSize, };
                    break;

                case "Office2019White":
                    theme = new Office2019WhiteThemeSettings() { FontFamily = fontFamily, BodyFontSize = bodyfontSize, };
                    break;

                case "FluentDark":
                    theme = new FluentDarkThemeSettings() { FontFamily = fontFamily, BodyFontSize = bodyfontSize, };
                    break;

                case "MaterialDark":
                    theme = new MaterialDarkThemeSettings() { FontFamily = fontFamily, BodyFontSize = bodyfontSize, };
                    break;

                case "MaterialDarkBlue":
                    theme = new MaterialDarkBlueThemeSettings() { FontFamily = fontFamily, BodyFontSize = bodyfontSize, };
                    break;

                case "Office2019Black":
                    theme = new Office2019BlackThemeSettings() { FontFamily = fontFamily, BodyFontSize = bodyfontSize, };
                    break;
            }
            return theme;
        }
    }
}