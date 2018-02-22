using System;
using System.Windows;
using System.Windows.Controls;

// https://gist.github.com/anjdreas/90040fb174f71c5ab3ad
namespace Muses.Wpf.Controls.Support
{
    /// <summary>
    /// Class containing attached properties supporting <see cref="TextBox"/> and
    /// <see cref="PasswordBox"/> controls.
    /// </summary>
    public class SpacingSupport
    {
        #region Horizontal property
        public static readonly DependencyProperty HorizontalProperty = DependencyProperty.RegisterAttached("Horizontal", typeof(double), typeof(SpacingSupport), new UIPropertyMetadata(0d, HorizontalChangedCallback));

        public static double GetHorizontal(DependencyObject obj) => (double)obj.GetValue(HorizontalProperty);

        public static void SetHorizontal(DependencyObject obj, double space) => obj.SetValue(HorizontalProperty, space);

        private static void HorizontalChangedCallback(object sender, DependencyPropertyChangedEventArgs e)
        {
            var space = (double)e.NewValue;
            var obj = (DependencyObject)sender;

            SetMargin(obj, new Thickness(0, 0, space, 0));
            SetLastItemMargin(obj, new Thickness(0));
        }
        #endregion

        #region Vertical property
        public static readonly DependencyProperty VerticalProperty = DependencyProperty.RegisterAttached("Vertical", typeof(double), typeof(SpacingSupport), new UIPropertyMetadata(0d, VerticalChangedCallback));

        public static double GetVertical(DependencyObject obj) => (double)obj.GetValue(VerticalProperty);

        public static void SetVertical(DependencyObject obj, double value) => obj.SetValue(VerticalProperty, value);

        private static void VerticalChangedCallback(object sender, DependencyPropertyChangedEventArgs e)
        {
            var space = (double)e.NewValue;
            var obj = (DependencyObject)sender;

            SetMargin(obj, new Thickness(0, 0, 0, space));
            SetLastItemMargin(obj, new Thickness(0));
        }
        #endregion


        #region LastItemMargin property.
        public static readonly DependencyProperty LastItemMarginProperty = DependencyProperty.RegisterAttached("LastItemMargin", typeof(Thickness), typeof(SpacingSupport), new UIPropertyMetadata(new Thickness(), MarginChangedCallback));

        private static Thickness GetLastItemMargin(Panel obj) => (Thickness)obj.GetValue(LastItemMarginProperty);

        private static void SetLastItemMargin(DependencyObject obj, Thickness value) => obj.SetValue(LastItemMarginProperty, value);
        #endregion

        #region MarginProperty
        public static readonly DependencyProperty MarginProperty = DependencyProperty.RegisterAttached("Margin", typeof(Thickness), typeof(SpacingSupport), new UIPropertyMetadata(new Thickness(), MarginChangedCallback));

        private static Thickness GetMargin(DependencyObject obj) => (Thickness)obj.GetValue(MarginProperty);

        private static void SetMargin(DependencyObject obj, Thickness value) => obj.SetValue(MarginProperty, value);
        #endregion

        #region Property callback handling.
        private static void MarginChangedCallback(object sender, DependencyPropertyChangedEventArgs e)
        {
            var panel = sender as Panel;
            if (panel == null) return;

            panel.Loaded -= OnPanelLoaded;
            panel.Loaded += OnPanelLoaded;

            if (panel.IsLoaded)
            {
                OnPanelLoaded(panel, null);
            }
        }

        private static void OnPanelLoaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is Panel panel))
            {
                throw new NotSupportedException($"Spacing is not supported for {sender.GetType()}");
            }

            for (var i = 0; i < panel.Children.Count; i++)
            {
                UIElement child = panel.Children[i];
                var fe = child as FrameworkElement;
                if (fe == null) continue;

                bool isLastItem = i == panel.Children.Count - 1;
                fe.Margin = isLastItem ? GetLastItemMargin(panel) : GetMargin(panel);
            }
        }
        #endregion
    }
}
