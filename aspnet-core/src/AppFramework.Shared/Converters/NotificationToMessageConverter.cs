using Abp.Notifications;
using AppFramework.Shared;
using Newtonsoft.Json;
using System; 
using System.Globalization; 
using System.Text; 
using System.Windows.Data;

namespace AppFramework.Converters
{
    public class NotificationMessage
    {
        public string SourceName { get; set; }

        public string Name { get; set; }
    }

    public class NotificationToMessageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                StringBuilder sb = new StringBuilder();

                var utf = value as UserNotification;
                if (utf != null)
                {
                    foreach (var item in utf.Notification.Data.Properties)
                    {
                        if (item.Key.Equals("Message"))
                        {
                            var msg = item.Value.ToString();
                            try
                            {
                                var val = JsonConvert.DeserializeObject<NotificationMessage>(msg);
                                if (val != null) sb.Append(Local.Localize(val.Name));
                            }
                            catch
                            {
                                sb.Append(msg);
                            }
                        }
                    }
                }
                return sb.ToString();
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
