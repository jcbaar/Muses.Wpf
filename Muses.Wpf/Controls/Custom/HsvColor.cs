using System;
using System.Windows.Media;

namespace Muses.Wpf.Controls.Custom
{
    /// <summary>
    /// Simple <see cref="Color"/> based class which is extended with support
    /// for HSV color model.
    /// </summary>
    public sealed class HsvColor
    {
        private double _hue;
        private double _saturation;
        private double _value;

        /// <summary>
        /// Constructor.Initializes an instance of the object.
        /// The color is initialized with <see cref="Colors.Black"/>
        /// </summary>
        public HsvColor()
        {
            Color = Colors.Black;
            RgbToHsv();
        }

        /// <summary>
        /// Constructor.Initializes an instance of the object.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to initialize with.</param>
        public HsvColor(Color color)
        {
            Color = color;
            RgbToHsv();
        }

        /// <summary>
        /// Constructor.Initializes an instance of the object.
        /// </summary>
        /// <param name="a">The Alpha component of the color (0-255)</param>
        /// <param name="r">The Red component of the color (0-255)</param>
        /// <param name="g">The Green component of the color (0-255)</param>
        /// <param name="b">The Blue component of the color (0-255)</param>
        public HsvColor(byte a, byte r, byte g, byte b)
        {
            Color = Color.FromArgb(a, r, g, b);
            RgbToHsv();
        }

        /// <summary>
        /// Constructor.Initializes an instance of the object.
        /// </summary>
        /// <param name="a">The Alpha component of the color (0-255)</param>
        /// <param name="h">The hue of the color (0.0-360.0)</param>
        /// <param name="s">The saturation of the color (0.0-1.0)</param>
        /// <param name="v">The value of the color (0.0-1.0)</param>
        public HsvColor(byte a, double h, double s, double v)
        {
            Alpha = a;
            Hue = h;
            Saturation = s;
            Value = v;

            HsvToRgb();
        }

        /// <summary>
        /// Gets or sets thew <see cref="Color"/>.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets the Alpha component of the color.
        /// </summary>
        public byte Alpha
        {
            get => Color.A;
            set => Color = Color.FromArgb(value, Color.R, Color.G, Color.B);
        }

        /// <summary>
        /// Gets or sets the Red component of the color.
        /// </summary>
        public byte Red
        {
            get => Color.R;
            set
            {
                Color = Color.FromArgb(Color.A, value, Color.G, Color.B);
                RgbToHsv();
            }
        }

        /// <summary>
        /// Gets or sets the Green component of the color.
        /// </summary>
        public byte Green
        {
            get => Color.G;
            set
            {
                Color = Color.FromArgb(Color.A, Color.R, value, Color.B);
                RgbToHsv();
            }
        }

        /// <summary>
        /// Gets or sets the Blue component of the color.
        /// </summary>
        public byte Blue
        {
            get => Color.B;
            set
            {
                Color = Color.FromArgb(Color.A, Color.R, Color.G, value);
                RgbToHsv();
            }
        }

        /// <summary>
        /// Gets or sets the Hue of the color.
        /// </summary>
        public double Hue
        {
            get => _hue;
            set
            {
                if(value != _hue)
                {
                    _hue = value;
                    HsvToRgb();
                }
            }
        }

        /// <summary>
        /// Gets or sets the Saturation of the color.
        /// </summary>
        public double Saturation
        {
            get => _saturation;
            set
            {
                if(value != _saturation)
                {
                    _saturation = value;
                    HsvToRgb();
                }
            }
        }

        /// <summary>
        /// Gets or sets the Value of the color.
        /// </summary>
        public double Value
        {
            get => _value;
            set
            {
                if(value != _value)
                {
                    _value = value;
                    HsvToRgb();
                }
            }
        }

        /// <summary>
        /// Helper to convert the RGB values to HSV.
        /// </summary>
        private void RgbToHsv()
        {
            double delta, min;
            double h = 0, s, v;

            min = Math.Min(Math.Min(Red, Green), Blue);
            v = Math.Max(Math.Max(Red, Green), Blue);
            delta = v - min;

            if (v == 0.0)
            {
                s = 0.0;
            }
            else
            {
                s = delta / v;
            }

            if (s == 0.0)
            {
                h = 0.0;
            }
            else
            {
                if (Red == v)
                {
                    h = (Green - Blue) / delta;
                }
                else if (Green == v)
                {
                    h = 2 + (Blue - Red) / delta;
                }
                else if (Blue == v)
                {
                    h = 4 + (Red - Green) / delta;
                }

                h *= 60.0;

                if (h < 0.0)
                {
                    h = h + 360.0;
                }
            }

            Hue = h;
            Saturation = s;
            Value = (v / 255.0);
        }

        /// <summary>
        /// Helper to convert the HSV values to RGB.
        /// </summary>
        private void HsvToRgb()
        {
            double r = 0, g = 0, b = 0;

            if (Saturation == 0)
            {
                r = Value;
                g = Value;
                b = Value;
            }
            else
            {
                int i;
                double f, p, q, t, hue = Hue;

                if (hue == 360.0)
                    hue = 0.0;
                else
                    hue = hue / 60.0;

                i = (int)Math.Truncate(hue);
                f = hue - i;

                p = Value * (1.0 - Saturation);
                q = Value * (1.0 - (Saturation * f));
                t = Value * (1.0 - (Saturation * (1.0 - f)));

                switch (i)
                {
                    case 0:
                        r = Value;
                        g = t;
                        b = p;
                        break;

                    case 1:
                        r = q;
                        g = Value;
                        b = p;
                        break;

                    case 2:
                        r = p;
                        g = Value;
                        b = t;
                        break;

                    case 3:
                        r = p;
                        g = q;
                        b = Value;
                        break;

                    case 4:
                        r = t;
                        g = p;
                        b = Value;
                        break;

                    default:
                        r = Value;
                        g = p;
                        b = q;
                        break;
                }
            }

            byte red = Convert.ToByte(r * 255.0),
                green = Convert.ToByte(g * 255.0),
                blue = Convert.ToByte(b * 255.0);

            if (Red != red ||
                Green != green ||
                Blue != blue)
            {
                Color = Color.FromArgb(Color.A, red, green, blue);
            }
        }
    }
}
