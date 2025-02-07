using System;
using System.Globalization; 
using System.Windows.Data;

namespace AppFramework.Converters
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currentTime = DateTime.Now;
            var dateTime = (DateTime)value;

            if (dateTime.Day == currentTime.Day)
                return dateTime.ToLongTimeString();
            else
                return dateTime.ToString("F");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
