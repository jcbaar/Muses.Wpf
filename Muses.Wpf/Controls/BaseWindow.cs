using Muses.Wpf.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Muses.Wpf.Controls
{
    /// <summary>
    /// Base class for themed windows.
    /// </summary>
    [TemplatePart(Name = PART_TitlebarControls, Type = typeof(ContentPresenter))]
    public class BaseWindow : Window
    {
        #region Private fields
        const string PART_TitlebarControls = "PART_TitlebarControls";
        Point _startPosition;
        bool _isResizing = false;
        private ResizeGrip _grip;
        #endregion

        public static readonly DependencyProperty TitlebarControlsProperty = DependencyProperty.Register("TitlebarControls", typeof(ItemsControl), typeof(BaseWindow), new PropertyMetadata(null));

        internal ContentPresenter TitlebarControlsPresenter { get; set; }

        /// <summary>
        /// Gets/sets the <see cref="ContentPresenter"/> that hosts the title bar controls.
        /// </summary>
        public ItemsControl TitlebarControls
        {
            get { return (ItemsControl)GetValue(TitlebarControlsProperty); }
            set { SetValue(TitlebarControlsProperty, value); }
        }

        #region Static constructor.
        /// <summary>
        /// Static constructor.
        /// </summary>
        static BaseWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseWindow), new FrameworkPropertyMetadata(typeof(BaseWindow)));
        }
        #endregion

        #region FillTitleBar
        /// <summary>
        /// Dependency property for the <see cref="FillTitleBar"/> property.
        /// </summary>
        public static readonly DependencyProperty FillTitleBarProperty = DependencyProperty.Register("FillTitleBar", typeof(bool), typeof(BaseWindow), new PropertyMetadata(false, null));

        /// <summary>
        /// Gets or sets the property indicating whether or not the window title-bar
        /// is filled with the accent color.
        /// </summary>
        public bool FillTitleBar
        {
            get => (bool)GetValue(FillTitleBarProperty);
            set => SetValue(FillTitleBarProperty, value);
        }
        #endregion

        #region Protected overrides
        /// <summary>
        /// Called when the <see cref="BaseWindow"/> is initialized.
        /// </summary>
        /// <param name="e">The callback arguments.</param>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // Setup the system command bindings.
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, OnCloseWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, OnMaximizeWindow, OnCanResizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, OnMinimizeWindow, OnCanMinimizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, OnRestoreWindow, OnCanResizeWindow));
        }

        /// <summary>
        /// Called when the <see cref="BaseWindow"/> is closed.
        /// </summary>
        /// <param name="e">The callback arguments.</param>
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

        /// <summary>
        /// Called when the template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            TitlebarControlsPresenter = GetTemplateChild(PART_TitlebarControls) as ContentPresenter;

            if(TitlebarControls == null)
            {
                TitlebarControls = new ItemsControl();
            }

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
        #endregion

        #region Event handlers
        /// <summary>
        /// Called when the left mouse button was release inside the resize grip context.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event arguments.</param>
        private void _grip_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Only when we are resizing.
            if (_isResizing == true)
            {
                // Release the mouse.
                _isResizing = false;
                Mouse.Capture(null);
            }
        }

        /// <summary>
        /// Called when the mouse cursor was moved inside the resize grip context.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event arguments.</param>
        private void _grip_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            // Only when we are resizing.
            if (_isResizing)
            {
                // Resize the window respective to the mouse movement.
                Point currentPosition = Mouse.GetPosition(this);

                double newWidth = Width + (currentPosition.X - _startPosition.X);
                double newHeight = Height + (currentPosition.Y - _startPosition.Y);

                Width = newWidth > MinWidth ? newWidth : MinWidth;
                Height = newHeight > MinHeight ? newHeight : MinHeight;

                _startPosition = currentPosition;
            }
        }

        /// <summary>
        /// Called when the left mouse button was pressed inside the resize grip context.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event arguments.</param>
        private void _grip_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Capture the mouse and switch to resizing mode.
            if (Mouse.Capture(_grip))
            {
                _isResizing = true;
                _startPosition = Mouse.GetPosition(this);
            }
        }

        /// <summary>
        /// Called to see if the window can be resized.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnCanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        /// <summary>
        /// Called to see if the window can be minimized.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode != ResizeMode.NoResize;
        }

        /// <summary>
        /// Called to close the window.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnCloseWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        /// <summary>
        /// Called to maximize the window.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnMaximizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        /// <summary>
        /// Called to minimize the window.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnMinimizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        /// <summary>
        /// Called to restore the window.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnRestoreWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }
        #endregion
    }
}
