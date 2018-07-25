using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Muses.Wpf.Controls.Custom
{
    /// <summary>
    /// A simple RGB/HSV color picket control.
    /// </summary>
    [TemplatePart(Name = "PART_TransparencyBack", Type = typeof(Border))]
    [TemplatePart(Name = "PART_SelectedColor", Type = typeof(Border))]

    [TemplatePart(Name = "PART_AlphaSlider", Type = typeof(Slider))]
    [TemplatePart(Name = "PART_HueSlider", Type = typeof(Slider))]
    [TemplatePart(Name = "PART_SaturationSlider", Type = typeof(Slider))]
    [TemplatePart(Name = "PART_BrightnessSlider", Type = typeof(Slider))]
    [TemplatePart(Name = "PART_RedSlider", Type = typeof(Slider))]
    [TemplatePart(Name = "PART_GreenSlider", Type = typeof(Slider))]
    [TemplatePart(Name = "PART_BlueSlider", Type = typeof(Slider))]
    public class ColorEditor : Control
    {
        private bool _fromHsv = false,
            _fromRgb = false,
            _originalSet = false;

        private Slider _hueSlider,
            _saturationSlider,
            _brightnessSlider,
            _redSlider,
            _greenSlider,
            _blueSlider,
            _alphaSlider;

        static ColorEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorEditor), new FrameworkPropertyMetadata(typeof(ColorEditor)));
        }

        public ColorEditor()
        {
            SelectedColor = Colors.Black;            
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _hueSlider = GetTemplateChild("PART_HueSlider") as Slider;
            _saturationSlider = GetTemplateChild("PART_SaturationSlider") as Slider;
            _brightnessSlider = GetTemplateChild("PART_BrightnessSlider") as Slider;
            _redSlider = GetTemplateChild("PART_RedSlider") as Slider;
            _greenSlider = GetTemplateChild("PART_GreenSlider") as Slider;
            _blueSlider = GetTemplateChild("PART_BlueSlider") as Slider;
            _alphaSlider = GetTemplateChild("PART_AlphaSlider") as Slider;

            var hsvColor = new HsvColor(SelectedColor);

            _fromRgb = true;
            Hue = hsvColor.Hue;
            Saturation = hsvColor.Saturation;
            Value = hsvColor.Value;
            _fromRgb = false;

            _fromHsv = true;
            Red = hsvColor.Red;
            Green = hsvColor.Green;
            Blue = hsvColor.Blue;
            Alpha = hsvColor.Alpha;
            _fromHsv = false;

            if(_originalSet == false)
            {
                OriginalColor = hsvColor.Color;
                _originalSet = true;
            }

            SetSliders(this);
        }

        #region ARGB

        #region Alpha dependency property
        /// <summary>
        /// The Alpha dependency property.
        /// </summary>
        public static readonly DependencyProperty AlphaProperty = DependencyProperty.Register("Alpha", typeof(byte), typeof(ColorEditor), new PropertyMetadata(Byte.MaxValue, ArgbChanged));

        /// <summary>
        /// Gets/sets the value of the Alpha property.
        /// </summary>
        public byte Alpha
        {
            get => (byte)GetValue(AlphaProperty);
            set => SetValue(AlphaProperty, value);
        }
        #endregion

        #region Red dependency property
        /// <summary>
        /// The Red dependency property.
        /// </summary>
        public static readonly DependencyProperty RedProperty = DependencyProperty.Register("Red", typeof(byte), typeof(ColorEditor), new PropertyMetadata(Byte.MinValue, ArgbChanged));

        /// <summary>
        /// Gets/sets the value of the Red property.
        /// </summary>
        public byte Red
        {
            get => (byte)GetValue(RedProperty);
            set => SetValue(RedProperty, value);
        }
        #endregion

        #region Green dependency property
        /// <summary>
        /// The Green dependency property.
        /// </summary>
        public static readonly DependencyProperty GreenProperty = DependencyProperty.Register("Green", typeof(byte), typeof(ColorEditor), new PropertyMetadata(Byte.MinValue, ArgbChanged));

        /// <summary>
        /// Gets/sets the value of the Green property.
        /// </summary>
        public byte Green
        {
            get => (byte)GetValue(GreenProperty);
            set => SetValue(GreenProperty, value);
        }
        #endregion

        #region Blue dependency property
        /// <summary>
        /// The Blue dependency property.
        /// </summary>
        public static readonly DependencyProperty BlueProperty = DependencyProperty.Register("Blue", typeof(byte), typeof(ColorEditor), new PropertyMetadata(Byte.MinValue, ArgbChanged));
            
        /// <summary>
        /// Gets/sets the value of the Blue property.
        /// </summary>
        public byte Blue
        {
            get => (byte)GetValue(BlueProperty);
            set => SetValue(BlueProperty, value);
        }
        #endregion

        private static void ArgbChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is ColorEditor picker)
            {
                if (picker.RgbToHsv())
                {
                    picker.SelectedColor = Color.FromArgb(picker.Alpha, (byte)picker.Red, (byte)picker.Green, (byte)picker.Blue);
                }
            }
        }
        #endregion
        
        #region HSV
        
        #region Hue dependency property
        /// <summary>
        /// The Hue dependency property.
        /// </summary>
        public static readonly DependencyProperty HueProperty = DependencyProperty.Register("Hue", typeof(double), typeof(ColorEditor), new PropertyMetadata(0.0, HsvChanged));
        
        /// <summary>
        /// Gets/sets the value of the Hue property.
        /// </summary>
        public double Hue
        {
            get => (double)GetValue(HueProperty);
            set => SetValue(HueProperty, value);
        }
        #endregion

        #region Saturation dependency property
        /// <summary>
        /// The Saturation dependency property.
        /// </summary>
        public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register("Saturation", typeof(double), typeof(ColorEditor), new PropertyMetadata(0.0, HsvChanged));

        /// <summary>
        /// Gets/sets the value of the Saturation property.
        /// </summary>
        public double Saturation
        {
            get => (double)GetValue(SaturationProperty);
            set => SetValue(SaturationProperty, value);
        }
        #endregion

        #region Value dependency property
        /// <summary>
        /// The Value dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(ColorEditor), new PropertyMetadata(0.0, HsvChanged));

        /// <summary>
        /// Gets/sets the value of the Value property.
        /// </summary>
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        #endregion

        private static void HsvChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorEditor picker)
            {
                if (picker.HsvToRgb())
                {
                    picker.SelectedColor = Color.FromArgb(picker.Alpha, (byte)picker.Red, (byte)picker.Green, (byte)picker.Blue);
                }
            }
        }

        #endregion

        #region SelectedColor dependency property
        /// <summary>
        /// The SelectedColor dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorEditor), new PropertyMetadata(Colors.Black, OnSelectedColorChanged));

        /// <summary>
        /// Gets/sets the value of the SelectedColor property.
        /// </summary>
        public Color SelectedColor
        {
            get => (Color)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        private static void OnSelectedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is ColorEditor picker)
            {
                SetSliders(picker);
            }
        }

        #endregion


        #region OriginalColor dependency property
        /// <summary>
        /// The OriginalColor dependency property.
        /// </summary>
        public static readonly DependencyProperty OriginalColorProperty = DependencyProperty.Register("OriginalColor", typeof(Color), typeof(ColorEditor), new PropertyMetadata(Colors.Black));

        /// <summary>
        /// Gets/sets the value of the OriginalColor property.
        /// </summary>
        public Color OriginalColor
        {
            get => (Color)GetValue(OriginalColorProperty);
            private set => SetValue(OriginalColorProperty, value);
        }
        #endregion


        private static void SetSliders(ColorEditor picker)
        {
            if (picker._hueSlider != null)
            {
                HsvColor c1 = new HsvColor(255, 255, 0, 0)
                {
                    Saturation = picker.Saturation,
                    Value = picker.Value
                },
                c2 = new HsvColor(255, 255, 255, 0)
                {
                    Saturation = picker.Saturation,
                    Value = picker.Value
                },
                c3 = new HsvColor(255, 0, 255, 0)
                {
                    Saturation = picker.Saturation,
                    Value = picker.Value
                },
                c4 = new HsvColor(255, 0, 255, 255)
                {
                    Saturation = picker.Saturation,
                    Value = picker.Value
                },
                c5 = new HsvColor(255, 0, 0, 255)
                {
                    Saturation = picker.Saturation,
                    Value = picker.Value
                },
                c6 = new HsvColor(255, 255, 0, 255)
                {
                    Saturation = picker.Saturation,
                    Value = picker.Value
                };

                picker._hueSlider.Background = new LinearGradientBrush(
                    new GradientStopCollection
                    {
                          new GradientStop(c1.Color, 0.000),
                          new GradientStop(c2.Color, 0.167),
                          new GradientStop(c3.Color, 0.333),
                          new GradientStop(c4.Color, 0.500),
                          new GradientStop(c5.Color, 0.667),
                          new GradientStop(c6.Color, 0.834),
                          new GradientStop(c1.Color, 1.00)
                    },
                    new Point(0, 0),
                    picker._hueSlider.Orientation == Orientation.Vertical ? new Point(0, 1) : new Point(1, 0));
            }
            if (picker._alphaSlider != null)
            {
                HsvColor c1 = new HsvColor(picker.SelectedColor)
                {
                    Saturation = 1.0,
                    Alpha = 255
                },
                c2 = new HsvColor(picker.SelectedColor)
                {
                    Saturation = 1.0,
                    Alpha = 0
                };

                if (picker._alphaSlider.Orientation == Orientation.Vertical)
                {
                    picker._alphaSlider.Background = new LinearGradientBrush(c1.Color, c2.Color, new Point(0, 0), new Point(0, 1));
                }
                else
                {
                    picker._alphaSlider.Background = new LinearGradientBrush(c1.Color, c2.Color, new Point(1, 0), new Point(0, 0));
                }
            }
            if (picker._saturationSlider != null)
            {
                HsvColor c1 = new HsvColor(picker.SelectedColor)
                {
                    Saturation = 0.0,
                    Alpha = 255
                },
                c2 = new HsvColor(picker.SelectedColor)
                {
                    Saturation = 1.0,
                    Alpha = 255
                };

                picker._saturationSlider.Background = new LinearGradientBrush(c1.Color, c2.Color, new Point(0, 0), new Point(1, 0));
            }
            if (picker._brightnessSlider != null)
            {
                HsvColor c1 = new HsvColor(picker.SelectedColor)
                {
                    Value = 0.0,
                    Alpha = 255
                },
                c2 = new HsvColor(picker.SelectedColor)
                {
                    Value = 1.0,
                    Alpha = 255
                };

                picker._brightnessSlider.Background = new LinearGradientBrush(c1.Color, c2.Color, new Point(0, 0), new Point(1, 0));
            }
            if (picker._redSlider != null)
            {
                HsvColor c1 = new HsvColor(picker.SelectedColor)
                {
                    Red = 0,
                    Alpha = 255
                },
                c2 = new HsvColor(picker.SelectedColor)
                {
                    Red = 255,
                    Alpha = 255
                };

                picker._redSlider.Background = new LinearGradientBrush(c1.Color, c2.Color, new Point(0, 0), new Point(1, 0));
            }
            if (picker._greenSlider != null)
            {
                HsvColor c1 = new HsvColor(picker.SelectedColor)
                {
                    Green = 0,
                    Alpha = 255
                },
                c2 = new HsvColor(picker.SelectedColor)
                {
                    Green = 255,
                    Alpha = 255
                };

                picker._greenSlider.Background = new LinearGradientBrush(c1.Color, c2.Color, new Point(0, 0), new Point(1, 0));
            }
            if (picker._blueSlider != null)
            {
                HsvColor c1 = new HsvColor(picker.SelectedColor)
                {
                    Blue = 0,
                    Alpha = 255
                },
                c2 = new HsvColor(picker.SelectedColor)
                {
                    Blue = 255,
                    Alpha = 255
                };

                picker._blueSlider.Background = new LinearGradientBrush(c1.Color, c2.Color, new Point(0, 0), new Point(1, 0));
            }
        }

        #region Helpers
        private bool RgbToHsv()
        {
            // Prevent circular dependencies from wreaking havoc...
            if (_fromRgb) return false;

            HsvColor color = new HsvColor(Alpha, Red, Green, Blue);

            try
            {
                _fromRgb = true;
                if(!AreSimular(color.Hue, Hue)) Hue = color.Hue;
                if(!AreSimular(color.Saturation, Saturation)) Saturation = color.Saturation;
                if(!AreSimular(color.Value, Value)) Value = color.Value;
            }
            finally
            {
                _fromRgb = false;
            }
            return true;
        }

        private bool HsvToRgb()
        {
            // Prevent circular dependencies from wreaking havoc...
            if (_fromHsv) return false;

            HsvColor color = new HsvColor
            {
                Hue = Hue,
                Saturation = Saturation,
                Value = Value
            };

            try
            {
                _fromHsv = true;

                if(color.Red != Red) Red = color.Red;
                if(color.Green != Green) Green = color.Green;
                if(color.Blue != Blue) Blue = color.Blue;
            }
            finally
            {
                _fromHsv = false;
            }
            return true;
        }

        private static bool AreSimular(double left, double right, double precission = 0.001) => Math.Abs(left - right) < precission;
        #endregion
    }
}
