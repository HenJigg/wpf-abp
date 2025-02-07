using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace AppFramework.Shared.Converters
{
    public class UrlToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var filePath = $"{SharedConsts.DocumentPath}/{value.ToString()}";

            if (!File.Exists(filePath)) return null;
             
            var profilePictureBytes = File.ReadAllBytes(filePath);

            return ImageSource.FromStream(() => new MemoryStream(profilePictureBytes));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
