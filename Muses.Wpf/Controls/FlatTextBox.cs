using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Muses.Wpf.Controls
{
    public class FlatTextBox : TextBox
    {
        public static DependencyProperty HintProperty = DependencyProperty.Register(nameof(Hint), typeof(String), typeof(FlatTextBox), new PropertyMetadata(String.Empty));

        public String Hint
        {
            get => (String)GetValue(HintProperty);
            set => SetValue(HintProperty, value);
        }

        public static DependencyProperty SelectAllOnFocusProperty = DependencyProperty.Register(nameof(SelectAllOnFocus), typeof(bool), typeof(FlatTextBox), new PropertyMetadata(true));

        public bool SelectAllOnFocus
        {
            get => (bool)GetValue(SelectAllOnFocusProperty);
            set
            {
                if (SelectAllOnFocus != value)
                {
                    SetValue(SelectAllOnFocusProperty, value);
                    SelectAllOnFocusChanged();
                }
            }
        }

        private MouseButtonEventHandler _mouseButtonHandler = new MouseButtonEventHandler(SelectivelyIgnoreMouseButton);
        private RoutedEventHandler _selectHandler = new RoutedEventHandler(SelectAllText);

        static FlatTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlatTextBox), new FrameworkPropertyMetadata(typeof(FlatTextBox)));
        }

        public FlatTextBox()
        {
            SelectAllOnFocusChanged();
        }

        private void SelectAllOnFocusChanged()
        {
            if(SelectAllOnFocus)
            {
                AddHandler(PreviewMouseLeftButtonDownEvent,_mouseButtonHandler, true);
                AddHandler(GotKeyboardFocusEvent,_selectHandler, true);
                AddHandler(MouseDoubleClickEvent,_selectHandler, true);
            }
            else
            {
                RemoveHandler(PreviewMouseLeftButtonDownEvent, _mouseButtonHandler);
                RemoveHandler(GotKeyboardFocusEvent, _selectHandler);
                RemoveHandler(MouseDoubleClickEvent, _selectHandler);
            }
        }
        private static void SelectivelyIgnoreMouseButton(object sender,
                                                     MouseButtonEventArgs e)
        {
            // Find the TextBox
            DependencyObject parent = e.OriginalSource as UIElement;
            while (parent != null && !(parent is TextBox))
                parent = VisualTreeHelper.GetParent(parent);

            if (parent != null)
            {
                var textBox = (TextBox)parent;
                if (!textBox.IsKeyboardFocusWithin)
                {
                    // If the text box is not yet focussed, give it the focus and
                    // stop further processing of this click event.
                    textBox.Focus();
                    e.Handled = true;
                }
            }
        }

        private static void SelectAllText(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null)
                textBox.SelectAll();
        }
    }
}
