using Muses.Wpf.Controls.Custom;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Muses.Wpf.Converters
{
    /// <summary>
    /// Converts HSV values to a <see cref="Color"/> and back.
    /// </summary>
    class HsvToColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 3 &&
                values[0] is double hue &&
                values[1] is double saturation &&
                values[2] is double value)
            {
                return new HsvColor(255, hue, saturation, value).Color;
            }
            return Colors.Black;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var result = new object[3];
            if(value is Color color)
            {
                var hsvColor = new HsvColor(color);
                result[0] = hsvColor.Hue;
                result[1] = hsvColor.Saturation;
                result[2] = hsvColor.Value;
            }
            return result;
        }
    }
}
