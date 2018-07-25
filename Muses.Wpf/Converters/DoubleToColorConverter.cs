using System;
using System.Globalization;
using System.Windows.Data;

namespace Muses.Wpf.Converters
{
    public class DoubleToColorComponentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToByte(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value);
        }
    }
}
