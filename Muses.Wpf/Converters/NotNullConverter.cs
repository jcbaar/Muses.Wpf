using System;
using System.Globalization;
using System.Windows.Data;

namespace Muses.Wpf.Converters
{
    /// <summary>
    /// Converter class which returns a value of true when the object is not null.
    /// </summary>
    public class NotNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value != null);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("NotNullConverter can only be used OneWay.");
        }
    }
}
