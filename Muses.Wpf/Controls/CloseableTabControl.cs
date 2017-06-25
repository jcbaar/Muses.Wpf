using System.Windows;
using System.Windows.Controls;

namespace Muses.Wpf.Controls
{
    public class CloseableTabControl : TabControl
    {
        public static readonly RoutedEvent ClosingTabItemEvent = EventManager.RegisterRoutedEvent("ClosingTabItem", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CloseableTabControl));
        public static readonly RoutedEvent CloseTabItemEvent = EventManager.RegisterRoutedEvent("CloseTabItem", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CloseableTabControl));

        public event RoutedEventHandler ClosingTabItem
        {
            add { AddHandler(ClosingTabItemEvent, value); }
            remove { RemoveHandler(ClosingTabItemEvent, value); }
        }

        public event RoutedEventHandler CloseTabItem
        {
            add { AddHandler(CloseTabItemEvent, value); }
            remove { RemoveHandler(CloseTabItemEvent, value); }
        }

        static CloseableTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CloseableTabControl), new FrameworkPropertyMetadata(typeof(CloseableTabControl)));
        }

    }
}
