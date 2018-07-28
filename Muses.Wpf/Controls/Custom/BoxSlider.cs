using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;

namespace Muses.Wpf.Controls.Custom
{
    /// <summary>
    /// Simple slider class that has a thumb that can be moved horizontally and
    /// vertically.
    /// </summary>
    [TemplatePart(Name = PART_Canvas, Type = typeof(Canvas))]
    [TemplatePart(Name = PART_Thumb, Type = typeof(Thumb))]
    public class BoxSlider : Control
    {
        const string PART_Canvas = "PART_Canvas";
        const string PART_Thumb = "PART_Thumb";

        private Canvas _canvas;
        private Thumb _thumb;

        private double _horizontalRange;
        private double _verticalRange;

        static BoxSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BoxSlider), new FrameworkPropertyMetadata(typeof(BoxSlider)));
        }


        #region ThumbStyle dependency property
        /// <summary>
        /// The ThumbStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty ThumbStyleProperty = DependencyProperty.Register("ThumbStyle", typeof(Style), typeof(BoxSlider), new PropertyMetadata(default(Style)));

        /// <summary>
        /// Gets/sets the value of the ThumbStyle property.
        /// </summary>
        public Style ThumbStyle
        {
            get => (Style)GetValue(ThumbStyleProperty);
            set => SetValue(ThumbStyleProperty, value);
        }
        #endregion

        #region Horizontal properties
        #region MinimumValueHorizontal dependency property
        /// <summary>
        /// The MinimumValueHorizontal dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumValueHorizontalProperty = DependencyProperty.Register("MinimumValueHorizontal", typeof(double), typeof(BoxSlider), new PropertyMetadata(0.0, OnMinMaxChanged, CoerceHorizontalMin));

        /// <summary>
        /// Gets/sets the value of the MinimumValueHorizontal property.
        /// </summary>
        public double MinimumValueHorizontal
        {
            get => (double)GetValue(MinimumValueHorizontalProperty);
            set => SetValue(MinimumValueHorizontalProperty, value);
        }

        /// <summary>
        /// Value coercion for the horizontal minimum value.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject"/> to coerce.</param>
        /// <param name="value">The object value.</param>
        /// <returns>The coerced value.</returns>
        private static object CoerceHorizontalMin(DependencyObject d, object value)
        {
            if(d is BoxSlider slider)
            {
                double v = (double)value;
                if (v > slider.MaximumValueHorizontal) v = slider.MaximumValueHorizontal;
                return v;
            }
            return value;

        }
        #endregion

        #region MaximumValueHorizontal dependency property
        /// <summary>
        /// The MaximumValueHorizontal dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumValueHorizontalProperty = DependencyProperty.Register("MaximumValueHorizontal", typeof(double), typeof(BoxSlider), new PropertyMetadata(1.0, OnMinMaxChanged, CoerceHorizontalMax));

        /// <summary>
        /// Gets/sets the value of the MaximumValueHorizontal property.
        /// </summary>
        public double MaximumValueHorizontal
        {
            get => (double)GetValue(MaximumValueHorizontalProperty);
            set => SetValue(MaximumValueHorizontalProperty, value);
        }

        /// <summary>
        /// Value coercion for the horizontal maximum value.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject"/> to coerce.</param>
        /// <param name="value">The object value.</param>
        /// <returns>The coerced value.</returns>
        private static object CoerceHorizontalMax(DependencyObject d, object value)
        {
            if (d is BoxSlider slider)
            {
                double v = (double)value;
                if (v < slider.MinimumValueHorizontal) v = slider.MinimumValueHorizontal;
                return v;
            }
            return value;

        }
        #endregion

        #region ValueHorizontal dependency property
        /// <summary>
        /// The ValueHorizontal dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueHorizontalProperty = DependencyProperty.Register("ValueHorizontal", typeof(double), typeof(BoxSlider), new PropertyMetadata(0.0, OnPositionThumb, CoerceHorizontalValue));

        /// <summary>
        /// Gets/sets the value of the ValueHorizontal property.
        /// </summary>
        public double ValueHorizontal
        {
            get => (double)GetValue(ValueHorizontalProperty);
            set => SetValue(ValueHorizontalProperty, value);
        }

        /// <summary>
        /// Value coercion for the horizontal current value.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject"/> to coerce.</param>
        /// <param name="value">The object value.</param>
        /// <returns>The coerced value.</returns>
        private static object CoerceHorizontalValue(DependencyObject d, object value)
        {
            if (d is BoxSlider slider)
            {
                double v = (double)value;
                if (v > slider.MaximumValueHorizontal) v = slider.MaximumValueHorizontal;
                else if (v < slider.MinimumValueHorizontal) v = slider.MinimumValueHorizontal;
                return v;
            }
            return value;

        }
        #endregion

        #region SmallChangeHorizontal dependency property
        /// <summary>
        /// The SmallChangeHorizontal dependency property.
        /// </summary>
        public static readonly DependencyProperty SmallChangeHorizontalProperty = DependencyProperty.Register("SmallChangeHorizontal", typeof(double), typeof(BoxSlider), new PropertyMetadata(0.1));

        /// <summary>
        /// Gets/sets the value of the SmallChangeHorizontal property.
        /// </summary>
        public double SmallChangeHorizontal
        {
            get => (double)GetValue(SmallChangeHorizontalProperty);
            set => SetValue(SmallChangeHorizontalProperty, value);
        }
        #endregion

        #region LargeChangeHorizontal dependency property
        /// <summary>
        /// The LargeChangeHorizontal dependency property.
        /// </summary>
        public static readonly DependencyProperty LargeChangeHorizontalProperty = DependencyProperty.Register("LargeChangeHorizontal", typeof(double), typeof(BoxSlider), new PropertyMetadata(0.2));

        /// <summary>
        /// Gets/sets the value of the LargeChangeHorizontal property.
        /// </summary>
        public double LargeChangeHorizontal
        {
            get => (double)GetValue(LargeChangeHorizontalProperty);
            set => SetValue(LargeChangeHorizontalProperty, value);
        }
        #endregion
        #endregion

        #region Vertical properties
        #region MinimumValueVertical dependency property
        /// <summary>
        /// The MinimumValueVertical dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumValueVerticalProperty = DependencyProperty.Register("MinimumValueVertical", typeof(double), typeof(BoxSlider), new PropertyMetadata(0.0, OnMinMaxChanged, CoerceVerticalMin));

        /// <summary>
        /// Gets/sets the value of the MinimumValueVertical property.
        /// </summary>
        public double MinimumValueVertical
        {
            get => (double)GetValue(MinimumValueVerticalProperty);
            set => SetValue(MinimumValueVerticalProperty, value);
        }

        /// <summary>
        /// Value coercion for the vertical minimum value.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject"/> to coerce.</param>
        /// <param name="value">The object value.</param>
        /// <returns>The coerced value.</returns>
        private static object CoerceVerticalMin(DependencyObject d, object value)
        {
            if (d is BoxSlider slider)
            {
                double v = (double)value;
                if (v > slider.MaximumValueVertical) v = slider.MaximumValueVertical;
                return v;
            }
            return value;

        }
        #endregion

        #region MaximumValueVertical dependency property
        /// <summary>
        /// The MaximumValueVertical dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumValueVerticalProperty = DependencyProperty.Register("MaximumValueVertical", typeof(double), typeof(BoxSlider), new PropertyMetadata(1.0, OnMinMaxChanged, CoerceVerticalMax));

        /// <summary>
        /// Gets/sets the value of the MaximumValueVertical property.
        /// </summary>
        public double MaximumValueVertical
        {
            get => (double)GetValue(MaximumValueVerticalProperty);
            set => SetValue(MaximumValueVerticalProperty, value);
        }

        /// <summary>
        /// Value coercion for the horizontal maximum value.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject"/> to coerce.</param>
        /// <param name="value">The object value.</param>
        /// <returns>The coerced value.</returns>
        private static object CoerceVerticalMax(DependencyObject d, object value)
        {
            if (d is BoxSlider slider)
            {
                double v = (double)value;
                if (v < slider.MinimumValueVertical) v = slider.MinimumValueVertical;
                return v;
            }
            return value;

        }
        #endregion

        #region ValueVertical dependency property
        /// <summary>
        /// The ValueVertical dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueVerticalProperty = DependencyProperty.Register("ValueVertical", typeof(double), typeof(BoxSlider), new PropertyMetadata(0.0, OnPositionThumb, CoerceVerticalValue));

        /// <summary>
        /// Gets/sets the value of the ValueVertical property.
        /// </summary>
        public double ValueVertical
        {
            get => (double)GetValue(ValueVerticalProperty);
            set => SetValue(ValueVerticalProperty, value);
        }

        /// <summary>
        /// Value coercion for the vertical current value.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject"/> to coerce.</param>
        /// <param name="value">The object value.</param>
        /// <returns>The coerced value.</returns>
        private static object CoerceVerticalValue(DependencyObject d, object value)
        {
            if (d is BoxSlider slider)
            {
                double v = (double)value;
                if (v > slider.MaximumValueVertical) v = slider.MaximumValueVertical;
                else if (v < slider.MinimumValueVertical) v = slider.MinimumValueVertical;
                return v;
            }
            return value;
        }
        #endregion

        #region SmallChangeVertical dependency property
        /// <summary>
        /// The SmallChangeVertical dependency property.
        /// </summary>
        public static readonly DependencyProperty SmallChangeVerticalProperty = DependencyProperty.Register("SmallChangeVertical", typeof(double), typeof(BoxSlider), new PropertyMetadata(0.1));

        /// <summary>
        /// Gets/sets the value of the SmallChangeVertical property.
        /// </summary>
        public double SmallChangeVertical
        {
            get => (double)GetValue(SmallChangeVerticalProperty);
            set => SetValue(SmallChangeVerticalProperty, value);
        }
        #endregion

        #region LargeChangeVertical dependency property
        /// <summary>
        /// The LargeChangeVertical dependency property.
        /// </summary>
        public static readonly DependencyProperty LargeChangeVerticalProperty = DependencyProperty.Register("LargeChangeVertical", typeof(double), typeof(BoxSlider), new PropertyMetadata(0.2));

        /// <summary>
        /// Gets/sets the value of the LargeChangeVertical property.
        /// </summary>
        public double LargeChangeVertical
        {
            get => (double)GetValue(LargeChangeVerticalProperty);
            set => SetValue(LargeChangeVerticalProperty, value);
        }
        #endregion
        #endregion

        #region overrides and callbacks
        /// <summary>
        /// Called when the template was applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Remove any existing event callbacks.
            if (_thumb != null)
            {
                _thumb.DragDelta -= _thumb_DragDelta;
                _thumb.PreviewKeyDown -= _thumb_PreviewKeyDown;
            }
            if (_canvas != null) _canvas.MouseDown -= _canvas_MouseDown;

            // Get to the expected template parts.
            _canvas = GetTemplateChild(PART_Canvas) as Canvas;
            _thumb = GetTemplateChild(PART_Thumb) as Thumb;

            // Hook up the event handlers.
            if (_canvas != null && _thumb != null)
            {
                _canvas.MouseDown += _canvas_MouseDown;
                _thumb.DragDelta += _thumb_DragDelta;
                _thumb.PreviewKeyDown += _thumb_PreviewKeyDown;
            }


            _horizontalRange = MaximumValueHorizontal - MinimumValueHorizontal;
            _verticalRange = MaximumValueVertical - MinimumValueVertical;


            // When we have both the canvas and the thumb we need to know when the
            // control has loaded so that we can place the thumb at the initial position.
            if (_canvas != null && _thumb != null)
            {
                Loaded += BoxSlider_Loaded;
            }
        }

        private void _thumb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Up:
                    {
                        double step = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control ? LargeChangeVertical : SmallChangeVertical;
                        ValueVertical = Math.Min(ValueVertical + step, MaximumValueVertical);
                        e.Handled = true;
                        break;
                    }

                case Key.PageUp:
                    ValueVertical = Math.Min(ValueVertical + LargeChangeVertical, MaximumValueVertical);
                    e.Handled = true;
                    break;

                case Key.Down:
                    {
                        double step = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control ? LargeChangeVertical : SmallChangeVertical;
                        ValueVertical = Math.Max(ValueVertical - step, MinimumValueVertical);
                        e.Handled = true;
                        break;
                    }

                case Key.PageDown:
                    ValueVertical = Math.Max(ValueVertical - LargeChangeVertical, MinimumValueVertical);
                    e.Handled = true;
                    break;

                case Key.Right:
                    {
                        double step = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control ? LargeChangeHorizontal : SmallChangeHorizontal;
                        ValueHorizontal = Math.Min(ValueHorizontal + step, MaximumValueHorizontal);
                        e.Handled = true;
                        break;
                    }

                case Key.Left:
                    {
                        double step = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control ? LargeChangeHorizontal : SmallChangeHorizontal;
                        ValueHorizontal = Math.Max(ValueHorizontal - step, MinimumValueHorizontal);
                        e.Handled = true;
                        break;
                    }

                default:
                    break;
            }
        }

        /// <summary>
        /// Called when the object has loaded. It will position the thumb on the
        /// correct position.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void BoxSlider_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= BoxSlider_Loaded;
            PositionThumb();
        }

        /// <summary>
        /// Called when the user clicks the mouse inside the slider. It positions
        /// the thumb at the click and starts the drag.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event parameters.</param>
        private void _canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Move the thumb and start dragging it...
            var pos = e.GetPosition(_canvas);

            SetThumb(pos);

            Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() => _thumb.RaiseEvent(e)));
        }

        /// <summary>
        /// Called when the thumb is being dragged.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void _thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double hx = _thumb.ActualWidth / 2,
                hy = _thumb.ActualHeight / 2;

            var pos = new Point(Canvas.GetLeft(_thumb) + hx, Canvas.GetTop(_thumb) + hy);
            var posBox = _canvas.TransformToAncestor(this).Transform(new Point(0, 0));

            pos.X += e.HorizontalChange;
            pos.Y += e.VerticalChange;

            if (pos.X < posBox.X) pos.X = posBox.X;
            else if (pos.X > posBox.X + _canvas.ActualWidth) pos.X = (posBox.X + _canvas.ActualWidth);

            if (pos.Y < posBox.Y) pos.Y = posBox.Y;
            else if (pos.Y > posBox.Y + _canvas.ActualHeight) pos.Y = (posBox.Y + _canvas.ActualHeight);

            SetThumb(pos);
        }

        /// <summary>
        /// Called when the HorizontalValue or VerticalValue dependency properties change.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject"/> that was changed.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> arguments.</param>
        private static void OnPositionThumb(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BoxSlider slider)
            {
                if (slider._thumb != null && slider._thumb.IsMouseCaptured == false)
                {
                    slider.CoerceValue(ValueHorizontalProperty);
                    slider.CoerceValue(ValueVerticalProperty);

                    slider.PositionThumb();
                }
            }
        }

        /// <summary>
        /// Called when the minimum or maximum horizontal or vertical value dependency properties change.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject"/> that was changed.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> arguments.</param>
        private static void OnMinMaxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is BoxSlider slider)
            {
                slider.CoerceValue(MinimumValueHorizontalProperty);
                slider.CoerceValue(MinimumValueVerticalProperty);
                slider.CoerceValue(MaximumValueHorizontalProperty);
                slider.CoerceValue(MaximumValueVerticalProperty);

                slider._horizontalRange = slider.MaximumValueHorizontal - slider.MinimumValueHorizontal;
                slider._verticalRange = slider.MaximumValueVertical - slider.MinimumValueVertical;
            }
        }

        /// <summary>
        /// Update the thumb position if the size of the object changed.
        /// </summary>
        /// <param name="sizeInfo">Information about the new size.</param>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            PositionThumb();
        }
        #endregion

        #region Private helpers
        /// <summary>
        /// Move the thumb to the given position. Note that the position is expected to
        /// be the top-left corner of the thumb. Adjusts the horizontal and vertical value 
        /// properties after the move.
        /// </summary>
        /// <param name="pos">The position to move the thumb to.</param>
        private void SetThumb(Point pos, bool skipValueSet = false)
        {
            double horizFactor = _horizontalRange / _canvas.ActualWidth,
                vertFactor = _verticalRange / _canvas.ActualHeight,
                hx = _thumb.ActualWidth / 2,
                hy = _thumb.ActualHeight / 2;


            bool moved = false;
            if (Canvas.GetLeft(_thumb) != pos.X - hx)
            {
                moved = true;
                Canvas.SetLeft(_thumb, pos.X - hx);
                if (!skipValueSet)
                {
                    ValueHorizontal = MinimumValueHorizontal + (horizFactor * (pos.X < 0 ? 0 : (pos.X >= _canvas.ActualWidth ? _canvas.ActualWidth : pos.X)));
                }
            }
            if (Canvas.GetTop(_thumb) != pos.Y - hy)
            {
                moved = true;
                Canvas.SetTop(_thumb, pos.Y - hy);
                if (!skipValueSet)
                {
                    ValueVertical = MinimumValueVertical + (_verticalRange - (vertFactor * (pos.Y < 0 ? 0 : (pos.Y > _canvas.ActualHeight ? _canvas.ActualHeight : pos.Y))));
                }
            }

            // Moving the thumb over a background with an opacity mask can cause
            // some rendering artifacts. This solves that but it ain't pretty...
            if(moved) _canvas.InvalidateVisual();
        }

        /// <summary>
        /// Sets the thumb position from the horizontal and vertical value properties.
        /// </summary>
        private void PositionThumb()
        {
            if (_canvas != null && _thumb != null)
            {
                double horizFactor = _canvas.ActualWidth / _horizontalRange,
                    vertFactor = _canvas.ActualHeight / _verticalRange;

                var pos = new Point(horizFactor * Math.Abs(MinimumValueHorizontal - ValueHorizontal), _canvas.ActualHeight - (vertFactor * Math.Abs(MinimumValueVertical - ValueVertical)));

                SetThumb(pos, true);
            }
        }
        #endregion

    }
}
