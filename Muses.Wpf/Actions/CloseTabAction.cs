using Muses.Wpf.Controls;
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Interactivity;

namespace Muses.Wpf.Actions
{
    public class CloseTabAction : TriggerAction<DependencyObject>
    {
        public static DependencyProperty CloseableTabControlProperty = DependencyProperty.Register(nameof(CloseableTabControl), typeof(CloseableTabControl), typeof(CloseTabAction), new PropertyMetadata(default(CloseableTabControl)));
        public static DependencyProperty CloseableTabItemProperty = DependencyProperty.Register(nameof(CloseableTabItem), typeof(CloseableTabItem), typeof(CloseTabAction), new PropertyMetadata(default(CloseableTabItem)));

        public CloseableTabControl CloseableTabControl
        {
            get => (CloseableTabControl)GetValue(CloseableTabControlProperty);
            set => SetValue(CloseableTabControlProperty, value);
        }

        public CloseableTabItem CloseableTabItem
        {
            get => (CloseableTabItem)GetValue(CloseableTabItemProperty);
            set => SetValue(CloseableTabItemProperty, value);
        }

        protected override void Invoke(object parameter)
        {
            if (CloseableTabControl == null || CloseableTabItem == null)
            {
                return;
            }

            var closeAction = new Action(() =>
            {
                // Make sure we can close this tab by sending the ClosingTabEvent to the
                // TabControl. When the Cancel property of the event arguments is set to true
                // after the event we do not close the tab.
                var args = new ClosingTabEventArgs(CloseableTabControl.ClosingTabItemEvent, (object)CloseableTabItem);
                CloseableTabControl.RaiseEvent(args);
                if(args.Cancel)
                {
                    // Select the tab they tried to close...
                    CloseableTabControl.SelectedItem = CloseableTabItem;
                    return;
                }

                if (CloseableTabControl.ItemsSource == null)
                {
                    // if the list is hard-coded (i.e. has no ItemsSource)
                    // then we remove the item from the collection
                    CloseableTabControl.Items.Remove(CloseableTabItem);
                }
                else
                {
                    // if ItemsSource is something we cannot work with, bail out
                    var collection = CloseableTabControl.ItemsSource as IList;
                    if (collection == null)
                    {
                        return;
                    }

                    // find the item and kill it (I mean, remove it)
                    var item2Remove = collection.OfType<object>().FirstOrDefault(item => CloseableTabItem == item || CloseableTabItem.DataContext == item);
                    if (item2Remove != null)
                    {
                        collection.Remove(item2Remove);
                    }
                }
            });
            Dispatcher.BeginInvoke(closeAction);
        }
    }
}
