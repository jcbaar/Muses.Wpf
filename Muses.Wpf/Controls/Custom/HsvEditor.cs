using System.Windows;
using System.Windows.Controls;

namespace Muses.Wpf.Controls.Custom
{
    /// <summary>
    /// Control class for editing HSV color value.
    /// </summary>
    public class HsvEditor : Control
    {
        static HsvEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HsvEditor), new FrameworkPropertyMetadata(typeof(HsvEditor)));
        }

        #region Saturation dependency property
        /// <summary>
        /// The Saturation dependency property.
        /// </summary>
        public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register("Saturation", typeof(double), typeof(HsvEditor), new PropertyMetadata(0.0));

        /// <summary>
        /// Gets/sets the value of the Saturation property.
        /// </summary>
        public double Saturation
        {
            get => (double)GetValue(SaturationProperty);
            set => SetValue(SaturationProperty, value);
        }
        #endregion

        #region Value dependency property
        /// <summary>
        /// The Value dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(HsvEditor), new PropertyMetadata(0.0));

        /// <summary>
        /// Gets/sets the value of the Value property.
        /// </summary>
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        #endregion

        #region Hue dependency property
        /// <summary>
        /// The Hue dependency property.
        /// </summary>
        public static readonly DependencyProperty HueProperty = DependencyProperty.Register("Hue", typeof(double), typeof(HsvEditor), new PropertyMetadata(0.0));

        /// <summary>
        /// Gets/sets the value of the Hue property.
        /// </summary>
        public double Hue
        {
            get => (double)GetValue(HueProperty);
            set => SetValue(HueProperty, value);
        }
        #endregion
    }
}
