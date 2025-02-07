using System; 
using System.Globalization; 
using System.Windows;
using System.Windows.Data;

namespace AppFramework.Converters
{
    public class UnreadMessageCountToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value!=null&&int.TryParse(value.ToString(), out int result))
            {
                if (result>0) return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
