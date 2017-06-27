﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace Muses.Wpf.Controls.Support
{
    /// <summary>
    /// Class containing attached properties supporting <see cref="TextBox"/> and
    /// <see cref="PasswordBox"/> controls.
    /// </summary>
    public static class TextBoxSupport
    {
        #region Hint property handling
        public static readonly DependencyProperty HintProperty = DependencyProperty.RegisterAttached("Hint", typeof(String), typeof(TextBoxSupport), new PropertyMetadata(String.Empty, HintChanged));
        public static readonly DependencyProperty PasswordLengthProperty = DependencyProperty.RegisterAttached("PasswordLength", typeof(int), typeof(TextBoxSupport), new PropertyMetadata(0));

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
        public static string GetHint(UIElement element) => (String)element.GetValue(HintProperty);

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
        public static void SetHint(UIElement element, string value) => element.SetValue(HintProperty, value);


        [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
        public static int GetPasswordLength(UIElement element) => (int)element.GetValue(PasswordLengthProperty);
        private static void SetPasswordLength(UIElement element, int value) => element.SetValue(PasswordLengthProperty, value);

        /// <summary>
        /// Called when the <see cref="HintProperty"/> value was changed. This is used
        /// to attach or remove the <see cref="PasswordChanged(object, RoutedEventArgs)"/> event handler.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject"/> on which the <see cref="HintProperty"/> was changed.</param>
        /// <param name="e">The event arguments.</param>
        private static void HintChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // When this is a PasswordBox and the hint is set to a non-empty
            // string we need to update the PasswordLength property so that the
            // style can remove the hint when the length of the password is 0.
            if (d is PasswordBox)
            {
                var pb = d as PasswordBox;
                pb.PasswordChanged -= PasswordChanged;

                if (!String.IsNullOrEmpty((string)e.NewValue))
                {
                    pb.PasswordChanged += PasswordChanged;
                }
            }
        }

        /// <summary>
        /// Monitors changes in the password entry of a <see cref="PasswordBox"/> and sets the
        /// length of the changed password into the <see cref="PasswordLengthProperty"/> property.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox pb)
            {
                SetPasswordLength(pb, pb.Password.Length);
            }
        }
        #endregion

        #region SelectAllOnFocus property handling
        public static readonly DependencyProperty SelectAllOnFocusProperty = DependencyProperty.RegisterAttached("SelectAllOnFocus", typeof(bool), typeof(TextBoxSupport), new PropertyMetadata(false, SelectAllOnFocusChanged));

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
        public static bool GetSelectAllOnFocus(UIElement element) => (bool)element.GetValue(SelectAllOnFocusProperty);

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
        public static void SetSelectAllOnFocus(UIElement element, bool value) => element.SetValue(SelectAllOnFocusProperty, value);

        /// <summary>
        /// Called when the value of the <see cref="SelectAllOnFocusProperty"/> was changed.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject"/> on which the <see cref="SelectAllOnFocusProperty"/> was changed.</param>
        /// <param name="e">The event arguments.</param>
        private static void SelectAllOnFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox || d is PasswordBox)
            {
                var control = d as Control;
                if ((bool)e.NewValue)
                {
                    control.GotKeyboardFocus += GotKeyboardFocus;
                    control.GotMouseCapture += GotKeyboardFocus;
                }
                else
                {
                    control.GotKeyboardFocus -= GotKeyboardFocus;
                    control.GotMouseCapture -= GotKeyboardFocus;
                }
            }
        }

        /// <summary>
        /// Called when the control got keyboard focus.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private static void GotKeyboardFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox) ((TextBox)sender).SelectAll();
            else if (sender is PasswordBox) ((PasswordBox)sender).SelectAll();
        }
        #endregion
    }
}
