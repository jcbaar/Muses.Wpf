using System;
using System.Windows;
using System.Windows.Controls;

// Based on: https://gist.github.com/anjdreas/90040fb174f71c5ab3ad
namespace Muses.Wpf.Controls.Support
{
    /// <summary>
    /// Class containing attached properties supporting spacing of the items
    /// contained by <see cref="StackPanel"/>.
    /// </summary>
    public class StackPanelSupport
    {
        #region Spacing property
        /// <summary>
        /// The Spacing attached property.
        /// </summary>
        public static readonly DependencyProperty SpacingProperty = DependencyProperty.RegisterAttached("Spacing", typeof(double), typeof(StackPanelSupport), new UIPropertyMetadata(0d, SpacingChangedCallback));

        /// <summary>
        /// Gets the value of the Spacing property.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject"/> the property is attached to.</param>
        /// <returns>The value of the attached property.</returns>
        public static double GetSpacing(DependencyObject d) => (double)d.GetValue(SpacingProperty);

        /// <summary>
        /// Sets the value of the Spacing property.
        /// </summary>
        /// <param name="d">The <see cref="UIElement"/> the property is attached to.</param>
        /// <param name="value">The value to set to the property.</param>
        public static void SetSpacing(DependencyObject d, double space) => d.SetValue(SpacingProperty, space);

        /// <summary>
        /// Called when the Horizontal value was changed.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject"/> on which the property was changed.</param>
        /// <param name="e">The event arguments.</param>
        private static void SpacingChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var space = (double)e.NewValue;
            if(!(d is StackPanel panel))
            {
                throw new NotSupportedException($"Spacing is not supported for {d.GetType()}");
            }
            SetMargin(d, panel.Orientation == Orientation.Horizontal ? new Thickness(0, 0, space, 0) : new Thickness(0, 0, 0, space));
        }
        #endregion

        #region MarginProperty
        /// <summary>
        /// The Margin attached property.
        /// </summary>
        public static readonly DependencyProperty MarginProperty = DependencyProperty.RegisterAttached("Margin", typeof(Thickness), typeof(StackPanelSupport), new UIPropertyMetadata(new Thickness(), MarginChangedCallback));

        /// <summary>
        /// Gets the value of the Margin property.
        /// </summary>
        /// <param name="element">The <see cref="DependencyObject"/> the property is attached to.</param>
        /// <returns>The value of the attached property.</returns>
        private static Thickness GetMargin(DependencyObject d) => (Thickness)d.GetValue(MarginProperty);

        /// <summary>
        /// Sets the value of the Margin property.
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/> the property is attached to.</param>
        /// <param name="value">The value to set to the property.</param>
        private static void SetMargin(DependencyObject d, Thickness value) => d.SetValue(MarginProperty, value);

        /// <summary>
        /// Called when the <see cref="MarginProperty"/> value changed.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject"/> that generated the event.</param>
        /// <param name="e">The event arguments.</param>
        private static void MarginChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // This may be paranoid...
            if(!(d is Panel panel))
            {
                throw new NotSupportedException($"Spacing is not supported for {d.GetType()}");
            }

            panel.Loaded -= OnPanelLoaded;
            panel.Loaded += OnPanelLoaded;

            if (panel.IsLoaded)
            {
                OnPanelLoaded(panel, null);
            }
        }

        /// <summary>
        /// Called after the <see cref="Panel"/> to which this event handler is attached is loaded.
        /// It will re-compute the margins for the Panel it's children.
        /// </summary>
        /// <param name="sender">The object that generated the event.</param>
        /// <param name="e">The event arguments.</param>
        private static void OnPanelLoaded(object sender, RoutedEventArgs e)
        {
            var panel = sender as StackPanel;
            for (var i = 0; i < panel.Children.Count-1; i++)
            {
                if (panel.Children[i] is FrameworkElement fe)
                {
                    fe.Margin = GetMargin(panel);
                }
            }
        }
        #endregion
    }
}
