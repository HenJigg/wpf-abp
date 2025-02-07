using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO; 
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace AppFramework.Converters
{
    public class ByteToImageConverter : IValueConverter
    { 
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is byte[] photoBytes)
            {
                BitmapImage bmp;
                try
                {
                    bmp = new BitmapImage();
                    bmp.BeginInit();
                    bmp.StreamSource = new MemoryStream(photoBytes);
                    bmp.EndInit(); 
                    return bmp;
                }
                catch { }
            }
            return $"/Assets/Images/user.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
