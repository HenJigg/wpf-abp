using Abp.Notifications;
using System; 
using System.Globalization; 
using System.Windows;
using System.Windows.Data;

namespace AppFramework.Converters
{
    public class StateToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var state = Enum.Parse<UserNotificationState>(value.ToString());
                if (state == UserNotificationState.Unread) return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
