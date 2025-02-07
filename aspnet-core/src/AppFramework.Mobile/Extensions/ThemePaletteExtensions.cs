using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AppFramework.Shared
{
    [Preserve(AllMembers = true)]
    public static class ThemePalette
    {
        public static void ApplyDarkTheme(this ResourceDictionary resources)
        {
            if (resources != null)
            {
                var mergedDictionaries = resources.MergedDictionaries;

                var lightTheme = mergedDictionaries.OfType<LightTheme>().FirstOrDefault();
                if (lightTheme != null)
                {
                    mergedDictionaries.Remove(lightTheme);
                }

                mergedDictionaries.Add(new DarkTheme());
                AppSettings.Instance.IsDarkTheme = true;
            }
        }

        public static void ApplyLightTheme(this ResourceDictionary resources)
        {
            if (resources != null)
            {
                var mergedDictionaries = resources.MergedDictionaries;

                var darkTheme = mergedDictionaries.OfType<DarkTheme>().FirstOrDefault();
                if (darkTheme != null)
                {
                    mergedDictionaries.Remove(darkTheme);
                }

                mergedDictionaries.Add(new LightTheme());
                AppSettings.Instance.IsDarkTheme = false;
            }
        }

        public static void ApplyColorSet(int index)
        {
            switch (index)
            {
                case 0:
                    ApplyColorSet1();
                    break;

                case 1:
                    ApplyColorSet2();
                    break;

                case 2:
                    ApplyColorSet3();
                    break;

                case 3:
                    ApplyColorSet4();
                    break;

                case 4:
                    ApplyColorSet5();
                    break;
            }
        }

        public static void ApplyColorSet1()
        {
            App.Current.Resources["PrimaryColor"] = Color.FromHex("#f54e5e");
            App.Current.Resources["PrimaryDarkColor"] = Color.FromHex("#d0424f");
            App.Current.Resources["PrimaryDarkenColor"] = Color.FromHex("#ab3641");
            App.Current.Resources["PrimaryLighterColor"] = Color.FromHex("#edcacd");
            App.Current.Resources["PrimaryLight"] = Color.FromHex("#ffe8f4");
            App.Current.Resources["PrimaryGradient"] = Color.FromHex("e83f94");
        }

        public static void ApplyColorSet2()
        {
            if (AppSettings.Instance.IsDarkTheme)
            {
                App.Current.Resources["PrimaryColor"] = Color.FromHex("#42A1FF");
                App.Current.Resources["PrimaryDarkColor"] = Color.FromHex("#0F88FF");
                App.Current.Resources["PrimaryDarkenColor"] = Color.FromHex("#006EDB");
                App.Current.Resources["PrimaryLighterColor"] = Color.FromHex("#75BAFF");
                App.Current.Resources["PrimaryLight"] = Color.FromHex("#A8D4FF");
                App.Current.Resources["PrimaryGradient"] = Color.FromHex("#0080FF");
            }
            else
            {
                App.Current.Resources["PrimaryColor"] = Color.FromHex("#2f72e4");
                App.Current.Resources["PrimaryDarkColor"] = Color.FromHex("#1a5ac6");
                App.Current.Resources["PrimaryDarkenColor"] = Color.FromHex("#174fb0");
                App.Current.Resources["PrimaryLighterColor"] = Color.FromHex("#73a0ed");
                App.Current.Resources["PrimaryLight"] = Color.FromHex("#cdddf9");
                App.Current.Resources["PrimaryGradient"] = Color.FromHex("#00acff");
            }
        }

        public static void ApplyColorSet3()
        {
            if (AppSettings.Instance.IsDarkTheme)
            {
                App.Current.Resources["PrimaryColor"] = Color.FromHex("#D88AFF");
                App.Current.Resources["PrimaryDarkColor"] = Color.FromHex("#9E63BC");
                App.Current.Resources["PrimaryDarkenColor"] = Color.FromHex("#804A9B");
                App.Current.Resources["PrimaryLighterColor"] = Color.FromHex("#D49FEE");
                App.Current.Resources["PrimaryLight"] = Color.FromHex("#D4B6E3");
                App.Current.Resources["PrimaryGradient"] = Color.FromHex("#6C58FF");
            }
            else
            {
                App.Current.Resources["PrimaryColor"] = Color.FromHex("#5d4cf7");
                App.Current.Resources["PrimaryDarkColor"] = Color.FromHex("#4b3ae1");
                App.Current.Resources["PrimaryDarkenColor"] = Color.FromHex("#3829ba");
                App.Current.Resources["PrimaryLighterColor"] = Color.FromHex("#b5aefb");
                App.Current.Resources["PrimaryLight"] = Color.FromHex("#eae8fe");
                App.Current.Resources["PrimaryGradient"] = Color.FromHex("#aa9cfc");
            }
        }

        public static void ApplyColorSet4()
        {
            if (AppSettings.Instance.IsDarkTheme)
            {
                App.Current.Resources["PrimaryColor"] = Color.FromHex("#17B0A8");
                App.Current.Resources["PrimaryDarkColor"] = Color.FromHex("#11837D");
                App.Current.Resources["PrimaryDarkenColor"] = Color.FromHex("#0B5652");
                App.Current.Resources["PrimaryLighterColor"] = Color.FromHex("#8AF0EA");
                App.Current.Resources["PrimaryLight"] = Color.FromHex("#CDF9F6");
                App.Current.Resources["PrimaryGradient"] = Color.FromHex("#A5FEB2");
            }
            else
            {
                App.Current.Resources["PrimaryColor"] = Color.FromHex("#06846a");
                App.Current.Resources["PrimaryDarkColor"] = Color.FromHex("#056c56");
                App.Current.Resources["PrimaryDarkenColor"] = Color.FromHex("#045343");
                App.Current.Resources["PrimaryLighterColor"] = Color.FromHex("#98f0de");
                App.Current.Resources["PrimaryLight"] = Color.FromHex("#ebf9f7");
                App.Current.Resources["PrimaryGradient"] = Color.FromHex("#0ed342");
            }
        }

        public static void ApplyColorSet5()
        {
            if (AppSettings.Instance.IsDarkTheme)
            {
                App.Current.Resources["PrimaryColor"] = Color.FromHex("#FF668C");
                App.Current.Resources["PrimaryDarkColor"] = Color.FromHex("#C83A62");
                App.Current.Resources["PrimaryDarkenColor"] = Color.FromHex("#882742 ");
                App.Current.Resources["PrimaryLighterColor"] = Color.FromHex("#FF9FBA");
                App.Current.Resources["PrimaryLight"] = Color.FromHex("#FAC7D5");
                App.Current.Resources["PrimaryGradient"] = Color.FromHex("#FFBF9F");
            }
            else
            {
                App.Current.Resources["PrimaryColor"] = Color.FromHex("#d54008");
                App.Current.Resources["PrimaryDarkColor"] = Color.FromHex("#a43106");
                App.Current.Resources["PrimaryDarkenColor"] = Color.FromHex("#862805");
                App.Current.Resources["PrimaryLighterColor"] = Color.FromHex("#fa9e7c");
                App.Current.Resources["PrimaryLight"] = Color.FromHex("#fee7de");
                App.Current.Resources["PrimaryGradient"] = Color.FromHex("#ff6374");
            }
        }
    }
}