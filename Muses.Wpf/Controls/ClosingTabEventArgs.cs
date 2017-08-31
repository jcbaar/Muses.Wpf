using System.Windows;

namespace Muses.Wpf.Controls
{
    public class ClosingTabEventArgs : RoutedEventArgs
    {
        public ClosingTabEventArgs(CloseableTabItem item) { TabItem = item; }
        public ClosingTabEventArgs(RoutedEvent e, CloseableTabItem item) : base(e) { TabItem = item; }
        public ClosingTabEventArgs(RoutedEvent e, object source, CloseableTabItem item) : base(e, source) { TabItem = item; }

        public CloseableTabItem TabItem { get; private set; }
        public bool Cancel { get; set; }
    }
}
