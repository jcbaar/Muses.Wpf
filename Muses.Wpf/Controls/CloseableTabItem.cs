using System.Windows;
using System.Windows.Controls;

namespace Muses.Wpf.Controls
{
    public class CloseableTabItem : TabItem
    {
        public static DependencyProperty IsCloseableProperty = DependencyProperty.Register(nameof(IsCloseable), typeof(bool), typeof(CloseableTabItem), new PropertyMetadata(true));

        public bool IsCloseable
        {
            get => (bool)GetValue(IsCloseableProperty); 
            set => SetValue(IsCloseableProperty, value);
        }

        static CloseableTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CloseableTabItem), new FrameworkPropertyMetadata(typeof(CloseableTabItem)));
        }
    }
}
