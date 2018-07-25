using Muses.Wpf.Controls.Custom;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Muses.Wpf.Converters
{
    class HueToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double hue = (double)value;
            return new HsvColor(255, hue, 1.0, 1.0).Color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Color color)
            {
                return new HsvColor(color).Hue;
            }
            return 0.0;
        }
    }
}
