using System;
using System.Globalization;
using System.Windows.Data;

namespace Muses.Wpf.Converters
{
    class DoubleToPercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return String.Format("{0:0}%", (double)value * 100);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
