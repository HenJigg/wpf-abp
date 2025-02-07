using AppFramework.Shared;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AppFramework.Converters
{
    public class InverseBoolToYesNoStrConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && bool.TryParse(value.ToString(), out bool result))
            {
                if (!result)
                    return Local.Localize("Yes");
                else
                    return Local.Localize("No");
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
