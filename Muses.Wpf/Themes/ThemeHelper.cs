using System;
using System.Threading;
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
            if (correctionFactor < -1.0 || correctionFactor > 1.0) throw new ArgumentOutOfRangeException(nameof(correctionFactor), "Value must be between -1.0 and 1.0");

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

        /// <summary>
        /// Initiate a color update.
        /// </summary>
        public static void BeginColorUpdate()
        {
            Interlocked.Increment(ref _count);
        }

        /// <summary>
        /// End an apply color updates. The update will be applied when <see cref="EndColorUpdate"/> has
        /// been called as many times as <seealso cref="BeginColorUpdate"/>.
        /// </summary>
        public static void EndColorUpdate()
        {
            if (Interlocked.Decrement(ref _count) == 0)
            {
                // Re-apply the current theme which will make use
                // of the updated color table.
                var dict = new ResourceDictionary()
                {
                    Source = Application.Current.Resources.MergedDictionaries[0].Source
                };

                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }
        }

        /// <summary>
        /// Set the value of a theme color.
        /// </summary>
        /// <param name="colorName">The name of the theme color to set.</param>
        /// <param name="newValue">The color to set.</param>
        public static void UpdateColor(string colorName, Color newValue)
        {
            // We only update the table after BeginColorUpdate() has been
            // called at least one time.
            if (Interlocked.CompareExchange(ref _count, 0, 0) > 0)
            {
                Application.Current.Resources[colorName] = newValue;
            }
            else
            {
                throw new InvalidOperationException("UpdateColor() should only be called between BeginColorUpdate() and EndColorUpdate()");
            }
        }

        /// <summary>
        /// Set the theme accent color. This will actually set the following colors.
        /// <list type="bullet">
        ///     <listheader> 
        ///         <term>Color name</term>
        ///         <description>The name of the color.</description>  
        ///     </listheader>  
        ///     <item>  
        ///         <term>AccentColor</term>  
        ///         <description>The actual accent color.</description>  
        ///     </item>  
        ///     <item>  
        ///         <term>AccentLightColor</term>  
        ///         <description>The accent color (.5 brighter accent color).</description>  
        ///     </item>  
        ///     <item>  
        ///         <term>AccentDarkColor</term>  
        ///         <description>The accent color (.3 darker accent color).</description>  
        ///     </item>  
        ///     <item>  
        ///         <term>AccentHoverColor</term>  
        ///         <description>The accent color (.2 brighter accent color).</description>  
        ///     </item>  
        ///     <item>  
        ///         <term>ControlHoveredColor</term>  
        ///         <description>The color when a control is hovered. (40% transparent accent color). This
        ///         will only be set when the <paramref name="accentHoverColor"/> is set to true./></description>  
        ///     </item>  
        /// </list>
        /// </summary>
        /// <param name="color">The color to set as accent color.</param>
        /// <param name="accentHoverColor">True to also set the ControlHoverColor. False to skip setting
        /// this color (default).</param>
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
