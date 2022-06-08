using Syncfusion.Licensing;

namespace AppFramework
{
    /// <summary>
    /// 应用程序配置文件
    /// </summary> 
    public class AppSettings
    {
        private bool isDarkTheme = true;
        private string themeName = "Material";

        static AppSettings()
        {
            Instance = new AppSettings();
        }

        public static AppSettings Instance { get; }

        public bool IsDarkTheme
        {
            get => isDarkTheme;
            set
            {
                isDarkTheme = value;
            }
        }

        public string ThemeName
        {
            get => themeName;
            set { themeName = value; }
        }

        public static void OnInitialized()
        {
            Syncfusion.SfSkinManager.SfSkinManager.ApplyStylesOnApplication = true;
            //Syncfusion LicenseKey
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("");
        }
    }
}