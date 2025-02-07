using System.Globalization;

namespace AppFramework.Shared
{ 
    public interface ILocaleCulture
    { 
        CultureInfo GetCurrentCultureInfo();
         
        void SetLocale(CultureInfo ci);
    }
}