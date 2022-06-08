using AppFramework.Common;
using System.Globalization; 
using System.Threading; 

namespace AppFramework.Localization
{
    public class LocaleCulture : ILocaleCulture
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            return Thread.CurrentThread.CurrentUICulture;
        }

        public void SetLocale(CultureInfo ci)
        {
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }
    }
}
