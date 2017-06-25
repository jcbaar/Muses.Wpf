using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Muses.Wpf.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Looks for a child control within a parent by name.
        /// NOTE: This uses recursion!
        /// </summary>
        /// <param name="parent">The parent UIElement to search.</param>
        /// <param name="name">The name of the child element to look for.</param>
        public static UIElement FindChild(this UIElement parent, string name)
        {
            if (parent == null || string.IsNullOrEmpty(name)) return null;
            if (parent is FrameworkElement && (parent as FrameworkElement).Name == name) return parent;

            UIElement result = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i) as UIElement;
                result = FindChild(child, name);
                if (result != null) break;
            }
            return result;
        }

        /// <summary>
        /// Looks for a child control within a parent by type.
        /// NOTE: This uses recursion!
        /// </summary>
        /// <typeparam name="T">The type of child to look for.</typeparam>
        /// <param name="parent">The parent UIElement to search.</param>
        public static T FindChild<T>(this UIElement parent)
            where T : UIElement
        {
            if (parent == null) return null;
            if (parent is T) return parent as T;

            UIElement foundChild = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i) as UIElement;
                foundChild = FindChild<T>(child);
                if (foundChild != null) break;
            }
            return foundChild as T;
        }

        public static T InvokeIfNecessary<T>(this DispatcherObject dispatcherObject, Func<T> func)
        {
            if (dispatcherObject == null)
            {
                throw new ArgumentNullException(nameof(dispatcherObject));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            if (dispatcherObject.Dispatcher.CheckAccess())
            {
                return func();
            }
            else
            {
                return (T)dispatcherObject.Dispatcher.Invoke(func);
            }
        }

        public static void InvokeIfNecessary(this DispatcherObject dispatcherObject, Action invokeAction)
        {
            if (dispatcherObject == null)
            {
                throw new ArgumentNullException(nameof(dispatcherObject));
            }
            if (invokeAction == null)
            {
                throw new ArgumentNullException(nameof(invokeAction));
            }
            if (dispatcherObject.Dispatcher.CheckAccess())
            {
                invokeAction();
            }
            else
            {
                dispatcherObject.Dispatcher.Invoke(invokeAction);
            }
        }
    }
}
