using System;
using System.Globalization;
using System.Windows.Data;

namespace Muses.Wpf.Converters
{
    /// <summary>
    /// Converter class which returns a inverted boolean value.
    /// </summary>
    public class InvertedBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Boolean)
            {
                return !(bool)value;
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("BooleanToVisibilityHiddenConverter can only be used OneWay.");
        }
    }
}
