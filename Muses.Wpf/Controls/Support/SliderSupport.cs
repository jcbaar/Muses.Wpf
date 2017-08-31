using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Muses.Wpf.Controls.Support
{
    /// <summary>
    /// Class containing attached properties supporting <see cref="Slider"/> controls.
    /// </summary>
    public static class SliderSupport
    {
        #region Thumb color
        /// <summary>
        /// Dependency property for the color of a <see cref="Slider"/> thumb.
        /// </summary>
        public static readonly DependencyProperty ThumbBrushProperty = DependencyProperty.RegisterAttached("ThumbBrush", typeof(Brush), typeof(SliderSupport), new PropertyMetadata(Brushes.White));

        /// <summary>
        /// Gets the color brush used for the color of a <see cref="Slider"/> thumb.
        /// </summary>
        /// <param name="element">The <see cref="Slider"/> to get the thumb brush of.</param>
        /// <returns>The thumb brush.</returns>
        [AttachedPropertyBrowsableForType(typeof(Slider))]
        public static Brush GetThumbBrush(UIElement element) => (Brush)element.GetValue(ThumbBrushProperty);

        /// <summary>
        /// Sets the color brush used for the color of a <see cref="Slider"/> thumb.
        /// </summary>
        /// <param name="element">The <see cref="Slider"/> to get the thumb color brush of.</param>
        /// <param name="value">The thumb color brush set.</param>
        [AttachedPropertyBrowsableForType(typeof(Slider))]
        public static void SetThumbBrush(UIElement element, Brush value) => element.SetValue(ThumbBrushProperty, value);
        #endregion

        #region Mouse-over thumb color.
        /// <summary>
        /// Dependency property for the mouse-over thumb color brush of a <see cref="Slider"/>.
        /// </summary>
        public static readonly DependencyProperty ThumbMouseOverBrushProperty = DependencyProperty.RegisterAttached("ThumbMouseOverBrush", typeof(Brush), typeof(SliderSupport), new PropertyMetadata(Brushes.Gray));

        /// <summary>
        /// Gets the mouse-over color brush used for the color of a <see cref="Slider"/> thumb.
        /// </summary>
        /// <param name="element">The <see cref="Slider"/> to get the mouse-over thumb brush of.</param>
        /// <returns>The mouse-over thumb brush.</returns>
        [AttachedPropertyBrowsableForType(typeof(Slider))]
        public static Brush GetThumbMouseOverBrush(UIElement element) => (Brush)element.GetValue(ThumbMouseOverBrushProperty);

        /// <summary>
        /// Sets the mouse-over color brush used for the color of a <see cref="Slider"/> thumb.
        /// </summary>
        /// <param name="element">The <see cref="Slider"/> to get the mouse-over thumb color brush of.</param>
        /// <param name="value">The mouse-over thumb color brush set.</param>
        [AttachedPropertyBrowsableForType(typeof(Slider))]
        public static void SetThumbMouseOverBrush(UIElement element, Brush value) => element.SetValue(ThumbMouseOverBrushProperty, value);
        #endregion

        #region Fill color
        /// <summary>
        /// Dependency property for the fill color brush of a <see cref="Slider"/>.
        /// </summary>
        public static readonly DependencyProperty FillBrushProperty = DependencyProperty.RegisterAttached("FillBrush", typeof(Brush), typeof(SliderSupport), new PropertyMetadata(Brushes.Blue));

        /// <summary>
        /// Gets the fill color brush used for the color of a <see cref="Slider"/>.
        /// </summary>
        /// <param name="element">The <see cref="Slider"/> to get the fill brush of.</param>
        /// <returns>The fill brush.</returns>
        [AttachedPropertyBrowsableForType(typeof(Slider))]
        public static Brush GetFillBrush(UIElement element) => (Brush)element.GetValue(FillBrushProperty);

        /// <summary>
        /// Sets the fill color brush used for the color of a <see cref="Slider"/>.
        /// </summary>
        /// <param name="element">The <see cref="Slider"/> to get the fill color brush of.</param>
        /// <param name="value">The fill color brush set.</param>
        [AttachedPropertyBrowsableForType(typeof(Slider))]
        public static void SetFillBrush(UIElement element, Brush value) => element.SetValue(FillBrushProperty, value);
        #endregion
    }
}
