using Muses.Wpf.Extensions;
using Muses.Wpf.Themes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;

namespace Muses.Wpf.Controls
{
    /// <summary>
    /// Base class for themed windows.
    /// </summary>
    [TemplatePart(Name = PART_TitlebarControls, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = PART_Statusbar, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = PART_StatusbarBackground, Type = typeof(Grid))]
    public class BaseWindow : Window
    {
        #region Private fields
        const string PART_TitlebarControls = "PART_TitlebarControls";
        const string PART_Statusbar = "PART_Statusbar";
        const string PART_StatusbarBackground = "PART_StatusbarBackground";
        Point _startPosition;
        bool _isResizing = false;
        ResizeGrip _grip;
        HwndSource _source;
        #endregion

        #region StatusBar dependency property
        internal ContentPresenter StatusbarPresenter { get; set; }

        /// <summary>
        /// The StatusBar dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusBarProperty = DependencyProperty.Register("StatusBar", typeof(StatusBar), typeof(BaseWindow), new PropertyMetadata(null));

        /// <summary>
        /// Gets/sets the value of the StatusBar property.
        /// </summary>
        public StatusBar Statusbar
        {
            get => (StatusBar)GetValue(StatusBarProperty);
            set => SetValue(StatusBarProperty, value);
        }
        #endregion

        #region TitlebarControls dependency property.
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
        #endregion

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

            if(_source != null)
            {
                _source.RemoveHook(WndProc);
            }

            base.OnClosed(e);
        }

        /// <summary>
        /// Called when the template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            StatusbarPresenter = GetTemplateChild(PART_Statusbar) as ContentPresenter;

            if(Statusbar == null)
            {
                Statusbar = new StatusBar();
            }
            Statusbar.Focusable = false;

            TitlebarControlsPresenter = GetTemplateChild(PART_TitlebarControls) as ContentPresenter;

            if(TitlebarControls == null)
            {
                TitlebarControls = new ItemsControl();
            }
            TitlebarControls.Focusable = false;

            // Make sure the background of the whole status bar including
            // the resize grip is updated properly...
            if (GetTemplateChild(PART_StatusbarBackground) is Grid status)
            {
                Binding backBinding = new Binding
                {
                    Source = Statusbar,
                    Path = new PropertyPath("Background"),
                    Mode = BindingMode.OneWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
                BindingOperations.SetBinding(status, StatusBar.BackgroundProperty, backBinding);
            }

            if (_source == null)
            {
                IntPtr hwnd = new WindowInteropHelper(this).EnsureHandle();
                _source = HwndSource.FromHwnd(hwnd);
                _source.AddHook(WndProc);
            }

            // Find the window resize grip and hook up the necessary events.
            if (_grip == null)
            {
                _grip = this.FindChild<ResizeGrip>();
                if (_grip != null)
                {
                    // Make sure the grip foreground color stays up to date
                    // with the status bar foreground color.
                    var foreBinding = new Binding
                    {
                        Source = Statusbar,
                        Path = new PropertyPath("Foreground"),
                        Mode = BindingMode.OneWay,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    };
                    BindingOperations.SetBinding(_grip, StatusBar.ForegroundProperty, foreBinding);

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

        private const int WM_DWMCOLORIZATIONCOLORCHANGED = 0x320;
        private const int WM_DWMCOMPOSITIONCHANGED = 0x31A;
        private const int WM_THEMECHANGED = 0x31E;

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_DWMCOLORIZATIONCOLORCHANGED:
                case WM_DWMCOMPOSITIONCHANGED:
                case WM_THEMECHANGED:
                    // Re-evaluate the accent color if we are setup to use the
                    // system accent color.
                    if (ThemeHelper.UseSystemAccentColor)
                    {
                        ThemeHelper.SetSystemAccentColor();
                    }
                    return IntPtr.Zero;

                default:
                    return IntPtr.Zero;
            }
        }
        #endregion
    }
}
