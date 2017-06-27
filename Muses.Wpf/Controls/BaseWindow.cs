using Muses.Wpf.Extensions;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Muses.Wpf.Controls
{
    public class BaseWindow : Window
    {
        Point _startPosition;
        bool _isResizing = false;
        private ResizeGrip _grip;

        public static readonly DependencyProperty FillTitleBarProperty = DependencyProperty.Register("FillTitleBar", typeof(bool), typeof(BaseWindow), new PropertyMetadata(false, null));
        public bool FillTitleBar
        {
            get { return (bool)GetValue(FillTitleBarProperty); }
            set { SetValue(FillTitleBarProperty, value); }
        }

        static BaseWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseWindow), new FrameworkPropertyMetadata(typeof(BaseWindow)));
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, OnCloseWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, OnMaximizeWindow, OnCanResizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, OnMinimizeWindow, OnCanMinimizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, OnRestoreWindow, OnCanResizeWindow));
        }

        protected override void OnClosed(EventArgs e)
        {
            // Make sure we unlink the events from the resize grip.
            if (_grip != null)
            {
                _grip.PreviewMouseLeftButtonDown += _grip_PreviewMouseLeftButtonDown;
                _grip.PreviewMouseMove += _grip_PreviewMouseMove;
                _grip.PreviewMouseLeftButtonUp += _grip_PreviewMouseLeftButtonUp;
            }
            base.OnClosed(e);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Find the window resize grip and hook up the necessary events.
            if (_grip == null)
            {
                _grip = this.FindChild<ResizeGrip>();
                if (_grip != null)
                {
                    _grip.PreviewMouseLeftButtonDown += _grip_PreviewMouseLeftButtonDown;
                    _grip.PreviewMouseMove += _grip_PreviewMouseMove;
                    _grip.PreviewMouseLeftButtonUp += _grip_PreviewMouseLeftButtonUp;
                }
            }
        }

        private void _grip_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isResizing == true)
            {
                _isResizing = false;
                Mouse.Capture(null);
            }
        }

        private void _grip_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_isResizing)
            {
                Point currentPosition = Mouse.GetPosition(this);

                double newWidth = Width + (currentPosition.X - _startPosition.X);
                double newHeight = Height + (currentPosition.Y - _startPosition.Y);

                Width = newWidth > MinWidth ? newWidth : MinWidth;
                Height = newHeight > MinHeight ? newHeight : MinHeight;

                _startPosition = currentPosition;
            }
        }

        private void _grip_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.Capture(_grip))
            {
                _isResizing = true;
                _startPosition = Mouse.GetPosition(this);
            }
        }

        private void OnCanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode != ResizeMode.NoResize;
        }

        private void OnCloseWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        private void OnMaximizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        private void OnMinimizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void OnRestoreWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }
    }
}
