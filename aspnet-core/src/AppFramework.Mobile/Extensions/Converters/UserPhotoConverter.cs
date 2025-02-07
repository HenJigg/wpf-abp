using AppFramework.Authorization.Users.Dto;
using System; 
using System.Globalization; 
using Xamarin.Forms;

namespace AppFramework.Shared.Converters
{
    public class UserPhotoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var user = value as UserListDto;

                if (user != null && user.Name.Length > 0)
                    return user.Name[0].ToString().ToUpper();
            }
            return "N";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
