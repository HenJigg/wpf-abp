using AppFramework.Shared; 
using Syncfusion.Windows.Tools.Controls;
using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace AppFramework.Admin.SyncUI.Converters
{
    public class MenuTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is TabItemExt tabItem)
            { 
                var ctor = tabItem.Content as UserControl;
                if (ctor != null && ctor.DataContext is NavigationViewModel viewModel)
                {
                    tabItem.Header = viewModel.Title;
                }
                return tabItem.Header;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
