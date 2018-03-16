using System;
using System.Windows;
using System.Windows.Controls;

namespace Muses.Wpf.Controls
{
    public class CloseableTabControl : TabControl
    {
        static CloseableTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CloseableTabControl), new FrameworkPropertyMetadata(typeof(CloseableTabControl)));
        }

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


        #region TabTransition dependency property
        /// <summary>
        /// The TabTransition dependency property.
        /// </summary>
        public static readonly DependencyProperty TabTransitionProperty = DependencyProperty.Register("TabTransition", typeof(TransitionType), typeof(CloseableTabControl), new PropertyMetadata(TransitionType.None));

        /// <summary>
        /// Gets/sets the value of the TabTransition property.
        /// </summary>
        public TransitionType TabTransition
        {
            get => (TransitionType)GetValue(TabTransitionProperty);
            set => SetValue(TabTransitionProperty, value);
        }
        #endregion

        #region TabTransitionDuration dependency property
        /// <summary>
        /// The TabTransitionDuration dependency property.
        /// </summary>
        public static readonly DependencyProperty TabTransitionDurationProperty = DependencyProperty.Register("TabTransitionDuration", typeof(TimeSpan), typeof(CloseableTabControl), new PropertyMetadata(new TimeSpan(0,0,0,0,200))); 

        /// <summary>
        /// Gets/sets the value of the TabTransitionDuration property.
        /// </summary>
        public TimeSpan TabTransitionDuration
        {
            get => (TimeSpan)GetValue(TabTransitionDurationProperty);
            set => SetValue(TabTransitionDurationProperty, value);
        }
        #endregion

    }
}
