using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Muses.Wpf.Controls.Support
{
    /// <summary>
    /// Class containing attached properties supporting <see cref="TextBox"/> and
    /// <see cref="PasswordBox"/> controls.
    /// </summary>
    public static class TextBoxSupport
    {
        #region Hint property handling
        /// <summary>
        /// Dependency property for the Hint text of a <see cref="TextBox"/> or a <see cref="PasswordBox"/>.
        /// </summary>
        public static readonly DependencyProperty HintProperty = DependencyProperty.RegisterAttached("Hint", typeof(String), typeof(TextBoxSupport), new PropertyMetadata(String.Empty, HintChanged));

        /// <summary>
        /// Dependency property for the length of the password in a <see cref="PasswordBox"/>. 
        /// </summary>
        public static readonly DependencyProperty PasswordLengthProperty = DependencyProperty.RegisterAttached("PasswordLength", typeof(int), typeof(TextBoxSupport), new PropertyMetadata(0));

        /// <summary>
        /// Gets the text used as Hint text of a <see cref="TextBox"/> or a <see cref="PasswordBox"/>
        /// </summary>
        /// <param name="element">The <see cref="TextBox"/> or <see cref="PasswordBox"/> to get the Hint text of.</param>
        /// <returns>The Hint text.</returns>
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
        public static string GetHint(UIElement element) => (String)element.GetValue(HintProperty);

        /// <summary>
        /// Sets the text used as Hint text of a <see cref="TextBox"/> or a <see cref="PasswordBox"/>
        /// </summary>
        /// <param name="element">The <see cref="TextBox"/> or <see cref="PasswordBox"/> to get the Hint text of.</param>
        /// <param name="value">The hint text to set.</param>
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
        public static void SetHint(UIElement element, string value) => element.SetValue(HintProperty, value);

        /// <summary>
        /// Gets the length of the password typed into a <see cref="PasswordBox"/>
        /// </summary>
        /// <param name="element">The <see cref="PasswordBox"/> to get the length of the typed password of.</param>
        /// <returns>The length of the typed password.</returns>
        [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
        public static int GetPasswordLength(UIElement element) => (int)element.GetValue(PasswordLengthProperty);

        /// <summary>
        /// Sets the length of the password typed into a <see cref="PasswordBox"/>. This is used by
        /// <seealso cref="PasswordChanged(object, RoutedEventArgs)"/> event callback to set the
        /// value. It is not accessible from outside this class.
        /// </summary>
        /// <param name="element">The <see cref="PasswordBox"/> to set the length of the typed password of.</param>
        /// <param name="value">The length to set.</param>
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
            else if(!(d is TextBox))
            {
                throw new NotSupportedException($"{nameof(HintProperty)} is not supported for {d.GetType()}");
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
        /// <summary>
        /// Dependency property for the SelectAllOnFocus flag of a <see cref="TextBox"/> or a <see cref="PasswordBox"/>.
        /// </summary>
        public static readonly DependencyProperty SelectAllOnFocusProperty = DependencyProperty.RegisterAttached("SelectAllOnFocus", typeof(bool), typeof(TextBoxSupport), new PropertyMetadata(false, SelectAllOnFocusChanged));

        /// <summary>
        /// Gets the flag indicating whether or not the text should be selected when the control
        /// get the keyboard focus.
        /// </summary>
        /// <param name="element">The <see cref="TextBox"/> or <see cref="PasswordBox"/> to get the
        /// flag from.</param>
        /// <returns>The flag for SelectAllOnFocus.</returns>
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
        public static bool GetSelectAllOnFocus(UIElement element) => (bool)element.GetValue(SelectAllOnFocusProperty);

        /// <summary>
        /// Sets the flag indicating whether or not the text should be selected when the control
        /// get the keyboard focus.
        /// </summary>
        /// <param name="element">The <see cref="TextBox"/> or <see cref="PasswordBox"/> to set the
        /// flag for.</param>
        /// <param name="value">The value for the SelectAllOnFocus flag.</param>
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
            // We only support TextBox and PasswordBox
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
            else
            {
                throw new NotSupportedException($"{nameof(SelectAllOnFocusProperty)} is not supported for {d.GetType()}");
            }
        }

        /// <summary>
        /// Called when the control got keyboard focus. Used to select the
        /// text of a TextBox or PasswordBox.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private static void GotKeyboardFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox) ((TextBox)sender).SelectAll();
            else if (sender is PasswordBox) ((PasswordBox)sender).SelectAll();
        }
        #endregion

        #region ShowCapsLockWarning property handling
        /// <summary>
        /// Dependency property for the CapsLockActive flag of a <see cref="PasswordBox"/>.
        /// </summary>
        public static readonly DependencyProperty CapsLockActiveProperty = DependencyProperty.RegisterAttached("CapsLockActive", typeof(bool), typeof(TextBoxSupport));

        /// <summary>
        /// Gets the flag indicating whether or not the caps-lock key is active. Note that this property
        /// can only be true when the <see cref="PasswordBox"/> has keyboard focus.
        /// </summary>
        /// <param name="element">The <see cref="PasswordBox"/> to get the
        /// flag from.</param>
        /// <returns>The flag for CapsLockActive.</returns>
        public static bool GetCapsLockActive(UIElement element) => (bool)element.GetValue(CapsLockActiveProperty);

        /// <summary>
        /// Sets the flag indicating whether or not the caps-lock key is active.
        /// </summary>
        /// <param name="element">The <see cref="PasswordBox"/> to get the
        /// flag from.</param>
        /// <returns>The flag for CapsLockActive.</returns>
        internal static void SetCapsLockActive(UIElement element, bool value) => element.SetValue(CapsLockActiveProperty, value);

        /// <summary>
        /// Dependency property for the ShowCapsLockWarning flag of a <see cref="PasswordBox"/>.
        /// </summary>
        public static readonly DependencyProperty ShowCapsLockWarningProperty = DependencyProperty.RegisterAttached("ShowCapsLockWarning", typeof(bool), typeof(TextBoxSupport), new PropertyMetadata(false, ShowCapsLockWarningChanged));

        /// <summary>
        /// Gets the flag indicating whether or not a warning should be displayed when
        /// caps-lock is activated.
        /// </summary>
        /// <param name="element">The <see cref="PasswordBox"/> to get the
        /// flag from.</param>
        /// <returns>The flag for ShowCapsLockWarning.</returns>
        [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
        public static bool GetShowCapsLockWarning(UIElement element) => (bool)element.GetValue(ShowCapsLockWarningProperty);

        /// <summary>
        /// Sets the flag indicating whether or not a warning should be displayed when
        /// caps-lock is activated.
        /// </summary>
        /// <param name="element">The <see cref="PasswordBox"/> to set the
        /// flag for.</param>
        /// <param name="value">The value for the ShowCapsLockWarning flag.</param>
        [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
        public static void SetShowCapsLockWarning(UIElement element, bool value) => element.SetValue(ShowCapsLockWarningProperty, value);

        /// <summary>
        /// Called when the value of the <see cref="ShowCapsLockWarningProperty"/> was changed.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject"/> on which the <see cref="ShowCapsLockWarningProperty"/> was changed.</param>
        /// <param name="e">The event arguments.</param>
        private static void ShowCapsLockWarningChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // We only support PasswordBox
            if (d is PasswordBox)
            {
                var control = d as Control;
                if ((bool)e.NewValue)
                {
                    control.GotKeyboardFocus += GotKeyboardFocus_CapsLock;
                    control.GotMouseCapture += GotKeyboardFocus_CapsLock;
                    control.LostKeyboardFocus += GotKeyboardFocus_CapsLock;
                    control.LostMouseCapture += GotKeyboardFocus_CapsLock;
                    control.PreviewKeyDown += GotKeyboardFocus_CapsLock;
                }
                else
                {
                    control.GotKeyboardFocus -= GotKeyboardFocus_CapsLock;
                    control.GotMouseCapture -= GotKeyboardFocus_CapsLock;
                    control.LostKeyboardFocus -= GotKeyboardFocus_CapsLock;
                    control.LostMouseCapture -= GotKeyboardFocus_CapsLock;
                    control.PreviewKeyDown -= GotKeyboardFocus_CapsLock;
                }
            }
            else
            {
                throw new NotSupportedException($"{nameof(ShowCapsLockWarningProperty)} is not supported for {d.GetType()}");
            }
        }

        /// <summary>
        /// Called when the status of the <see cref="CapsLockActiveProperty"/> needs to be updated.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private static void GotKeyboardFocus_CapsLock(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox pb)
            {
                // Set the flag to true when the CapsLock key is active _and_
                // the control has keyboard focus.
                SetCapsLockActive(pb, Console.CapsLock && pb.IsKeyboardFocused);
            }
        }
        #endregion
    }
}
