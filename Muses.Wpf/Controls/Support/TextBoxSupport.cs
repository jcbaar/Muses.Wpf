using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
            if (d is PasswordBox pb)
            {
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

        // Allows setting a masking option on a TextBox to allow only numeric input.
        // See: https://www.codeproject.com/Articles/34228/WPF-Maskable-TextBox-for-Numeric-Values
        //
        // Expanded with keyboard handling:
        //  - Enter triggers the binding on the Text property.
        //  - Up and PgUp increase the value by one.
        //  - Down and PgDown decrease the value by one.

        #region MinimumValue Property
        /// <summary>
        /// Dependency property for the MinimumValue of a <see cref="TextBox"/>.
        /// </summary>
        public static readonly DependencyProperty MinimumValueProperty = DependencyProperty.RegisterAttached("MinimumValue", typeof(double), typeof(TextBoxSupport), new FrameworkPropertyMetadata(double.NaN, MinimumValueChangedCallback));

        /// <summary>
        /// Gets the minimum value allowed in the <see cref="TextBox"/>.
        /// </summary>
        /// <param name="obj">The object the property is attached to.</param>
        /// <returns>The value of the property.</returns>
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static double GetMinimumValue(DependencyObject obj) => (double)obj.GetValue(MinimumValueProperty);

        /// <summary>
        /// Sets the minimum value allowed in the <see cref="TextBox"/>.
        /// </summary>
        /// <param name="obj">The object the property is attached to.</param>
        /// <param name="value">The value to set the property to.</param>
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static void SetMinimumValue(DependencyObject obj, double value) => obj.SetValue(MinimumValueProperty, value);

        /// <summary>
        /// Called when the value of the <see cref="MinimumValueProperty"/> was changed.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject"/> on which the <see cref="MinimumValueProperty"/> was changed.</param>
        /// <param name="e">The event arguments.</param>
        private static void MinimumValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox tb)
            {
                ValidateTextBox(tb);
            }
            else
            {
                throw new NotSupportedException($"{nameof(MinimumValueProperty)} is not supported for {d.GetType()}");
            }
        }
        #endregion

        #region MaximumValue Property
        /// <summary>
        /// Dependency property for the MaximumValue of a <see cref="TextBox"/>.
        /// </summary>
        public static readonly DependencyProperty MaximumValueProperty = DependencyProperty.RegisterAttached("MaximumValue", typeof(double), typeof(TextBoxSupport), new FrameworkPropertyMetadata(double.NaN, MaximumValueChangedCallback));

        /// <summary>
        /// Gets the maximum value allowed in the <see cref="TextBox"/>.
        /// </summary>
        /// <param name="obj">The object the property is attached to.</param>
        /// <returns>The value of the property.</returns>
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static double GetMaximumValue(DependencyObject obj) => (double)obj.GetValue(MaximumValueProperty);

        /// <summary>
        /// Sets the maximum value allowed in the <see cref="TextBox"/>.
        /// </summary>
        /// <param name="obj">The object the property is attached to.</param>
        /// <param name="value">The value to set the property to.</param>
        public static void SetMaximumValue(DependencyObject obj, double value) => obj.SetValue(MaximumValueProperty, value);

        /// <summary>
        /// Called when the value of the <see cref="MaximumValueProperty"/> was changed.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject"/> on which the <see cref="MaximumValueProperty"/> was changed.</param>
        /// <param name="e">The event arguments.</param>
        private static void MaximumValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox tb)
            {
                ValidateTextBox(tb);
            }
            else
            {
                throw new NotSupportedException($"{nameof(MaximumValueProperty)} is not supported for {d.GetType()}");
            }
        }
        #endregion

        #region Mask Property
        /// <summary>
        /// Dependency property for the Mask of a <see cref="TextBox"/>.
        /// </summary>
        public static readonly DependencyProperty MaskProperty = DependencyProperty.RegisterAttached("Mask", typeof(NumericMask), typeof(TextBoxSupport), new FrameworkPropertyMetadata(MaskChangedCallback));

        /// <summary>
        /// Gets the mask type allowed in the <see cref="TextBox"/>.
        /// </summary>
        /// <param name="obj">The object the property is attached to.</param>
        /// <returns>The value of the property.</returns>
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static NumericMask GetMask(DependencyObject obj) => (NumericMask)obj.GetValue(MaskProperty);

        /// <summary>
        /// Sets the maximum value allowed in the <see cref="TextBox"/>.
        /// </summary>
        /// <param name="obj">The object the property is attached to.</param>
        /// <param name="value">The value to set the property to.</param>
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static void SetMask(DependencyObject obj, NumericMask value) => obj.SetValue(MaskProperty, value);

        /// <summary>
        /// Called when the value of the <see cref="MaskProperty"/> was changed.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject"/> on which the <see cref="MaskProperty"/> was changed.</param>
        /// <param name="e">The event arguments.</param>
        private static void MaskChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is TextBox otb)
            {
                otb.PreviewTextInput -= TextBox_PreviewTextInput;
                otb.PreviewKeyDown -= TextBox_PreviewKeyDown;
                DataObject.RemovePastingHandler((e.OldValue as TextBox), TextBoxPastingEventHandler);
            }

            if (d is TextBox tb)
            {
                if ((NumericMask)e.NewValue != NumericMask.Any)
                {
                    tb.PreviewTextInput += TextBox_PreviewTextInput;
                    tb.PreviewKeyDown += TextBox_PreviewKeyDown;
                    DataObject.AddPastingHandler(tb, TextBoxPastingEventHandler);
                }

                ValidateTextBox(tb);
            }
            else
            {
                throw new NotSupportedException($"{nameof(MaximumValueProperty)} is not supported for {d.GetType()}");
            }
        }

        /// <summary>
        /// Helper that adds or removes a step from the current value of the <see cref="TextBox"/>
        /// </summary>
        /// <param name="tb">The <see cref="TextBox"/></param>
        /// <param name="removeStep">True to remove a step, false (default) to add a step.</param>
        private static void AddStep(TextBox tb, bool removeStep = false)
        {
            switch (GetMask(tb))
            {
                case NumericMask.Integer:
                    {
                        if (Int64.TryParse(tb.Text, out long number))
                        {
                            if (removeStep) number--; else number++;
                            number = (long)ValidateLimits(GetMinimumValue(tb), GetMaximumValue(tb), number);
                            tb.Text = number.ToString();
                        }
                        break;
                    }

                case NumericMask.Decimal:
                    {
                        if (Double.TryParse(tb.Text, out double dnumber))
                        {
                            if (removeStep) dnumber--; else dnumber++;
                            dnumber = (long)ValidateLimits(GetMinimumValue(tb), GetMaximumValue(tb), dnumber);
                            tb.Text = dnumber.ToString();
                        }
                        break;
                    }

            }
        }

        /// <summary>
        /// Called when a key was pressed.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private static void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DoUpdateSource(e.Source);
            }
            else if (sender is TextBox tb)
            {
                if (e.Key == Key.Up || e.Key == Key.PageUp)
                {
                    AddStep(tb);
                    DoUpdateSource(e.Source);
                }
                else if(e.Key == Key.Down || e.Key == Key.PageDown)
                {
                    AddStep(tb, true);
                    DoUpdateSource(e.Source);
                }
            }
        }

        /// <summary>
        /// Triggers the binding on the <see cref="TextBox.TextProperty"/> property for a
        /// given <see cref="TextBox"/>
        /// </summary>
        /// <param name="source">The source object for the property.</param>
        private static void DoUpdateSource(object source)
        {
            DependencyProperty property = TextBox.TextProperty;
            if (property == null)
            {
                return;
            }

            if (source is UIElement elt)
            {
                BindingExpression binding = BindingOperations.GetBindingExpression(elt, property);

                if (binding != null)
                {
                    binding.UpdateSource();
                }
            }
        }
        #endregion

        #region Private static helpers and event handlers.
        /// <summary>
        /// Validates the contents of the <see cref="TextBox"/>
        /// </summary>
        /// <param name="tb">The <see cref="TextBox"/> of which to validate the contents.</param>
        private static void ValidateTextBox(TextBox tb)
        {
            if (GetMask(tb) != NumericMask.Any)
            {
                tb.Text = ValidateValue(GetMask(tb), tb.Text, GetMinimumValue(tb), GetMaximumValue(tb));
            }
        }

        /// <summary>
        /// Called when text is pasted into a <see cref="TextBox"/>.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event parameters.</param>
        private static void TextBoxPastingEventHandler(object sender, DataObjectPastingEventArgs e)
        {
            if (sender is TextBox tb)
            {
                string clipboard = e.DataObject.GetData(typeof(string)) as string;
                clipboard = ValidateValue(GetMask(tb), clipboard, GetMinimumValue(tb), GetMaximumValue(tb));
                if (!string.IsNullOrEmpty(clipboard))
                {
                    tb.Text = clipboard;
                }
                e.CancelCommand();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Called when text was entered in a <see cref="TextBox"/>.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event parameters.</param>
        private static void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (sender is TextBox tb)
            {
                bool isValid = IsSymbolValid(GetMask(tb), e.Text);
                e.Handled = !isValid;
                if (isValid)
                {
                    int caret = tb.CaretIndex;
                    string text = tb.Text;
                    bool textInserted = false;
                    int selectionLength = 0;

                    if (tb.SelectionLength > 0)
                    {
                        text = text.Substring(0, tb.SelectionStart) + text.Substring(tb.SelectionStart + tb.SelectionLength);
                        caret = tb.SelectionStart;
                    }

                    if (e.Text == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                    {
                        while (true)
                        {
                            int ind = text.IndexOf(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
                            if (ind == -1)
                            {
                                break;
                            }

                            text = text.Substring(0, ind) + text.Substring(ind + 1);
                            if (caret > ind)
                            {
                                caret--;
                            }
                        }

                        if (caret == 0)
                        {
                            text = "0" + text;
                            caret++;
                        }
                        else
                        {
                            if (caret == 1 && string.Empty + text[0] == NumberFormatInfo.CurrentInfo.NegativeSign)
                            {
                                text = NumberFormatInfo.CurrentInfo.NegativeSign + "0" + text.Substring(1);
                                caret++;
                            }
                        }

                        if (caret == text.Length)
                        {
                            selectionLength = 1;
                            textInserted = true;
                            text = text + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "0";
                            caret++;
                        }
                    }
                    else if (e.Text == NumberFormatInfo.CurrentInfo.NegativeSign)
                    {
                        textInserted = true;
                        if (tb.Text.Contains(NumberFormatInfo.CurrentInfo.NegativeSign))
                        {
                            text = text.Replace(NumberFormatInfo.CurrentInfo.NegativeSign, string.Empty);
                            if (caret != 0)
                            {
                                caret--;
                            }
                        }
                        else
                        {
                            text = NumberFormatInfo.CurrentInfo.NegativeSign + tb.Text;
                            caret++;
                        }
                    }

                    if (!textInserted)
                    {
                        text = text.Substring(0, caret) + e.Text + ((caret < tb.Text.Length) ? text.Substring(caret) : string.Empty);
                        caret++;
                    }

                    try
                    {
                        double val = Convert.ToDouble(text);
                        double newVal = ValidateLimits(GetMinimumValue(tb), GetMaximumValue(tb), val);
                        if (val != newVal)
                        {
                            text = newVal.ToString();
                        }
                        else if (val == 0)
                        {
                            if (!text.Contains(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
                                text = "0";
                        }
                    }
                    catch(FormatException)
                    {
                        text = "0";
                    }

                    while (text.Length > 1 && text[0] == '0' && string.Empty + text[1] != NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                    {
                        text = text.Substring(1);
                        if (caret > 0)
                        {
                            caret--;
                        }
                    }

                    while (text.Length > 2 && string.Empty + text[0] == NumberFormatInfo.CurrentInfo.NegativeSign && text[1] == '0' && string.Empty + text[2] != NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                    {
                        text = NumberFormatInfo.CurrentInfo.NegativeSign + text.Substring(2);
                        if (caret > 1)
                        {
                            caret--;
                        }
                    }

                    if (caret > text.Length)
                    {
                        caret = text.Length;
                    }

                    tb.Text = text;
                    tb.CaretIndex = caret;
                    tb.SelectionStart = caret;
                    tb.SelectionLength = selectionLength;
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Helper that validates a value.
        /// </summary>
        /// <param name="mask">The masking type.</param>
        /// <param name="value">The value to validate.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The string representation of the validated value. Empty in case of a invalid input.</returns>
        private static string ValidateValue(NumericMask mask, string value, double min, double max)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            value = value.Trim();
            switch (mask)
            {
                case NumericMask.Integer:
                    if(Int64.TryParse(value, out long number))
                    {
                        return value;
                    }
                    return string.Empty;

                case NumericMask.Decimal:
                    if(Double.TryParse(value, out double dnumber))
                    {
                        return value;
                    }
                    return string.Empty;
            }
            return value;
        }

        /// <summary>
        /// Helper that validates a value.
        /// </summary>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="value">The value to validate.</param>
        /// <returns>The validated (clamped) value.</returns>
        private static double ValidateLimits(double min, double max, double value)
        {
            if (!min.Equals(double.NaN))
            {
                if (value < min)
                {
                    return min;
                }
            }

            if (!max.Equals(double.NaN))
            {
                if (value > max)
                {
                    return max;
                }
            }

            return value;
        }

        /// <summary>
        /// Validates if the input value does not contain illegal characters.
        /// </summary>
        /// <param name="mask">The masking type.</param>
        /// <param name="str">The input value.</param>
        /// <returns>True for OK, false for not OK.</returns>
        private static bool IsSymbolValid(NumericMask mask, string str)
        {
            switch (mask)
            {
                case NumericMask.Any:
                    return true;

                case NumericMask.Integer:
                    if (str == NumberFormatInfo.CurrentInfo.NegativeSign)
                        return true;
                    break;

                case NumericMask.Decimal:
                    if (str == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator ||
                        str == NumberFormatInfo.CurrentInfo.NegativeSign)
                        return true;
                    break;
            }

            if (mask.Equals(NumericMask.Integer) || mask.Equals(NumericMask.Decimal))
            {
                foreach (char ch in str)
                {
                    if (!Char.IsDigit(ch))
                        return false;
                }

                return true;
            }

            return false;
        }
        #endregion
    }
}
