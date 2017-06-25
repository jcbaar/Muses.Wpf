using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Muses.Wpf.Themes
{
    public static class ThemeHelper
    {
        public static int _count = 0;

        /// <summary>
        /// Creates color with corrected brightness.
        /// </summary>
        /// <param name="color">Color to correct.</param>
        /// <param name="correctionFactor">The brightness correction factor. Must be between -1 and 1. 
        /// Negative values produce darker colors.</param>
        /// <returns>
        /// Corrected <see cref="Color"/> structure.
        /// </returns>
        public static Color ChangeColorBrightness(Color color, float correctionFactor)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);
        }

        public static void BeginColorUpdate()
        {
            Interlocked.Increment(ref _count);
        }

        public static void EndColorUpdate()
        {
            if (Interlocked.Decrement(ref _count) == 0)
            {
                var dict = new ResourceDictionary()
                {
                    Source = Application.Current.Resources.MergedDictionaries[0].Source
                };

                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }
        }

        public static void UpdateColor(string colorName, Color newValue)
        {
            if (Interlocked.CompareExchange(ref _count, 0, 0) > 0)
            {
                Application.Current.Resources[colorName] = newValue;
            }
        }

        public static void SetAccentColor(Color color, bool accentHoverColor = true)
        {
            BeginColorUpdate();
            UpdateColor("AccentColor", color);
            UpdateColor("AccentLightColor", ChangeColorBrightness(color, 0.5F));
            UpdateColor("AccentDarkColor", ChangeColorBrightness(color, -0.3F));
            UpdateColor("AccentHoverColor", ChangeColorBrightness(color, 0.2F));
            if (accentHoverColor)
            {
                color.A = 100;
                UpdateColor("ControlHoveredColor", ChangeColorBrightness(color, 0.5F));
            }

            // Re-apply style. Seems to work but if it is the correct way...
            EndColorUpdate();
        }
    }
}
