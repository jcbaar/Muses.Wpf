using System;
using System.Windows;
using System.Windows.Controls;

namespace Muses.Wpf.Controls.Support
{
    /// <summary>
    /// Class containing attached properties supporting <see cref="ComboBox"/> controls.
    /// </summary>
    public static class ComboBoxSupport
    {
        #region MaxDropDownItems property handling
        /// <summary>
        /// Dependency property for the maximum number of items visible in the drop-down of a <see cref="ComboBox"/>.
        /// </summary>
        public static readonly DependencyProperty MaxDropDownItemsProperty = DependencyProperty.RegisterAttached("MaxDropDownItems", typeof(int), typeof(ComboBoxSupport), new PropertyMetadata
        {
            PropertyChangedCallback = (d, e) =>
            {
                if (!(d is ComboBox))
                {
                    throw new NotSupportedException($"MaxDropDownItems is not supported for {d.GetType()}");
                }

                var box = (ComboBox)d;
                box.DropDownOpened += UpdateHeight;
                if (box.IsDropDownOpen)
                {
                    UpdateHeight(box, EventArgs.Empty);
                }
            }
        });

        /// <summary>
        /// Gets the maximum amount of items shown in the drop-down.
        /// </summary>
        /// <param name="element">The <see cref="ComboBox"/> to get the maximum number of items shown in the drop-down.</param>
        /// <returns>The maximum number of items shown in the drop-down.</returns>
        [AttachedPropertyBrowsableForType(typeof(ComboBox))]
        public static int GetMaxDropDownItems(DependencyObject element) => (int)element.GetValue(MaxDropDownItemsProperty);

        /// <summary>
        /// Sets the maximum amount of items shown in the drop-down.
        /// </summary>
        /// <param name="element">The <see cref="ComboBox"/> to set the maximum number of items shown in the drop-down.</param>
        [AttachedPropertyBrowsableForType(typeof(ComboBox))]
        public static void SetMaxDropDownItems(DependencyObject obj, int value) => obj.SetValue(MaxDropDownItemsProperty, value);

        /// <summary>
        /// Event handler which is called when the drop-down of the <see cref="ComboBox"/> is
        /// about to be shown.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event parameters.</param>
        private static void UpdateHeight(object sender, EventArgs e)
        {
            if (sender is ComboBox box)
            {
                box.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (box.ItemContainerGenerator.ContainerFromIndex(0) is UIElement container && container.RenderSize.Height > 0)
                    {
                        // This is somewhat nasty. The 3px extra room is necessary for the border and padding
                        // but should of course not be a magic constant number but rather computed is some way...
                        box.MaxDropDownHeight = (container.RenderSize.Height * GetMaxDropDownItems(box)) + 3;
                    }
                }));
            }
        }
        #endregion
    }
}
