using AppFramework.Localization.Resources;
using System.Reflection;
using System.Resources;
using System.Globalization; 
using System.Threading;

namespace AppFramework.Shared
{
    public class LocaleCulture : ILocaleCulture
    {
        private const string ResourceId = "AppFramework.Admin.SyncUI.Localization.Resources.LocalTranslation";

        public CultureInfo GetCurrentCultureInfo()
        {
            return Thread.CurrentThread.CurrentUICulture;
        }

        public string GetString(string key)
        {
            var resourceManager = new ResourceManager(ResourceId, typeof(LocaleCulture).GetTypeInfo().Assembly);
            return resourceManager.GetString(key, GetCurrentCultureInfo());
        }

        public void SetLocale(CultureInfo ci)
        {
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            LocalTranslation.Culture = ci;
        }
    }
}
