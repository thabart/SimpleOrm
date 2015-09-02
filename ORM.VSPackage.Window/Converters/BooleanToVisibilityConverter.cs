using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ORM.VSPackage.ImportWindowSqlServer.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isVisible = false;
            if (!bool.TryParse(value.ToString(), out isVisible))
            {
                return Visibility.Collapsed;
            }

            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
