using System;
using System.Windows;
using System.Windows.Controls;

// Based on: https://gist.github.com/anjdreas/90040fb174f71c5ab3ad
namespace Muses.Wpf.Controls.Support
{
    /// <summary>
    /// Class containing attached properties supporting spacing of the items
    /// contained by <see cref="Panel"/> or derived objects.
    /// </summary>
    public class SpacingSupport
    {
        #region Horizontal property
        /// <summary>
        /// The Horizontal attached property.
        /// </summary>
        public static readonly DependencyProperty HorizontalProperty = DependencyProperty.RegisterAttached("Horizontal", typeof(double), typeof(SpacingSupport), new UIPropertyMetadata(0d, HorizontalChangedCallback));

        /// <summary>
        /// Gets the value of the Horizontal property.
        /// </summary>
        /// <param name="element">The <see cref="DependencyObject"/> the property is attached to.</param>
        /// <returns>The value of the attached property.</returns>
        public static double GetHorizontal(DependencyObject element) => (double)element.GetValue(HorizontalProperty);

        /// <summary>
        /// Sets the value of the Horizontal property.
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/> the property is attached to.</param>
        /// <param name="value">The value to set to the property.</param>
        public static void SetHorizontal(DependencyObject element, double space) => element.SetValue(HorizontalProperty, space);

        /// <summary>
        /// Called when the Horizontal value was changed.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject"/> on which the property was changed.</param>
        /// <param name="e">The event arguments.</param>
        private static void HorizontalChangedCallback(object sender, DependencyPropertyChangedEventArgs e)
        {
            var space = (double)e.NewValue;
            var obj = (DependencyObject)sender;

            SetMargin(obj, new Thickness(0, 0, space, 0));
        }
        #endregion

        #region Vertical property
        /// <summary>
        /// The Vertical attached property.
        /// </summary>
        public static readonly DependencyProperty VerticalProperty = DependencyProperty.RegisterAttached("Vertical", typeof(double), typeof(SpacingSupport), new UIPropertyMetadata(0d, VerticalChangedCallback));

        /// <summary>
        /// Gets the value of the Vertical property.
        /// </summary>
        /// <param name="element">The <see cref="DependencyObject"/> the property is attached to.</param>
        /// <returns>The value of the attached property.</returns>
        public static double GetVertical(DependencyObject obj) => (double)obj.GetValue(VerticalProperty);

        /// <summary>
        /// Sets the value of the Vertical property.
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/> the property is attached to.</param>
        /// <param name="value">The value to set to the property.</param>
        public static void SetVertical(DependencyObject obj, double value) => obj.SetValue(VerticalProperty, value);

        /// <summary>
        /// Called when the Vertical value was changed.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject"/> on which the property was changed.</param>
        /// <param name="e">The event arguments.</param>
        private static void VerticalChangedCallback(object sender, DependencyPropertyChangedEventArgs e)
        {
            var space = (double)e.NewValue;
            var obj = (DependencyObject)sender;

            SetMargin(obj, new Thickness(0, 0, 0, space));
        }
        #endregion

        #region MarginProperty
        /// <summary>
        /// The Margin attached property.
        /// </summary>
        public static readonly DependencyProperty MarginProperty = DependencyProperty.RegisterAttached("Margin", typeof(Thickness), typeof(SpacingSupport), new UIPropertyMetadata(new Thickness(), MarginChangedCallback));

        /// <summary>
        /// Gets the value of the Margin property.
        /// </summary>
        /// <param name="element">The <see cref="DependencyObject"/> the property is attached to.</param>
        /// <returns>The value of the attached property.</returns>
        private static Thickness GetMargin(DependencyObject obj) => (Thickness)obj.GetValue(MarginProperty);

        /// <summary>
        /// Sets the value of the Margin property.
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/> the property is attached to.</param>
        /// <param name="value">The value to set to the property.</param>
        private static void SetMargin(DependencyObject obj, Thickness value) => obj.SetValue(MarginProperty, value);

        /// <summary>
        /// Called when the <see cref="MarginProperty"/> value changed.
        /// </summary>
        /// <param name="sender">The <see cref="DependencyObject"/> that generated the event.</param>
        /// <param name="e">The event arguments.</param>
        private static void MarginChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if(!(sender is Panel panel))
            {
                throw new NotSupportedException($"Spacing is not supported for {sender.GetType()}");
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
            var panel = sender as Panel;
            for (var i = 0; i < panel.Children.Count-1; i++)
            {
                UIElement child = panel.Children[i];
                var fe = child as FrameworkElement;
                if (fe == null) continue;

                bool isLastItem = i == panel.Children.Count - 1;
                fe.Margin = GetMargin(panel);
            }
        }
        #endregion
    }
}
