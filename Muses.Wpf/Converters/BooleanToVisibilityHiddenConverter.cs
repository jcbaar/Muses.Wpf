using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Muses.Wpf.Converters
{
    /// <summary>
    /// Converter class which returns a settable <see cref="Visibility"/> value when
    /// the value is false. By default <see cref="Visibility.Hidden"/> is returned for
    /// a false value.
    /// </summary>
    public class BooleanToVisibilityHiddenConverter : IValueConverter
    {
        public Visibility FalseValue { get; set; } = Visibility.Hidden;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Boolean)
            {
                return (bool)value == false ? FalseValue : Visibility.Visible;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("BooleanToVisibilityHiddenConverter can only be used OneWay.");
        }
    }
}
