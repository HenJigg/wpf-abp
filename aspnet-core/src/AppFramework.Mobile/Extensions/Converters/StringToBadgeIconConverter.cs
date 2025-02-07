using Syncfusion.XForms.BadgeView;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace AppFramework.Shared.Converters
{
    public class StringToBadgeIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (bool.TryParse(value.ToString(), out bool result))
            {
                return result ? BadgeIcon.Dot : BadgeIcon.None;
            }

            return BadgeIcon.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
