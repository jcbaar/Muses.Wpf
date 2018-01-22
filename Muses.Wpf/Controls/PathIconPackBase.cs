using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Muses.Wpf.Controls
{
    public abstract class PathIconPackBase : Control
    {
        internal abstract void UpdateData();
    }

    /// <summary>
    /// Base class for creating an <see cref="PathGeometry"/> based icon control for icon packs.
    /// This code has been borrowed from the Material Design in XAML Toolkit (http://materialdesigninxaml.net/)
    /// </summary>
    /// <typeparam name="TKind">The type of the icon pack.</typeparam>
    public abstract class PathIconPackBase<TKind> : PathIconPackBase
    {
        static Lazy<IDictionary<TKind, string>> _dataIndex;

        /// <param name="dataIndexFactory">
        /// Inheritors should provide a factory for setting up the path data index (per icon kind).
        /// The factory will only be utilized once, across all closed instances (first instantiation wins).
        /// </param>
        protected PathIconPackBase(Func<IDictionary<TKind, string>> dataIndexFactory)
        {
            if (dataIndexFactory == null) throw new ArgumentNullException(nameof(dataIndexFactory));

            if (_dataIndex == null)
                _dataIndex = new Lazy<IDictionary<TKind, string>>(dataIndexFactory);
        }

        /// <summary>
        /// Gets the Geometry string of the given <typeparamref name="TKind"/> icon type.
        /// </summary>
        /// <param name="kind">The type of icon to get the geometry string of.</param>
        /// <returns>The icon geometry string or an empty string if not found.</returns>
        public static string GetIconGeometryString(TKind kind)
        {
            if (_dataIndex.Value != null && _dataIndex.Value.TryGetValue(kind, out string value))
            {
                return value;
            }
            return String.Empty;
        }

        /// <summary>
        /// Gets the <see cref="Geometry"/> of the given <typeparamref name="TKind"/> icon type.
        /// </summary>
        /// <param name="kind">The type of icon to get the <see cref="Geometry"/> of.</param>
        /// <returns>The icon <see cref="Geometry"/> or an empty string if not found.</returns>
        public static Geometry GetIconGeometry(TKind kind) => Geometry.Parse(GetIconGeometryString(kind));

        /// <summary>
        /// <see cref="DependencyProperty"/> of the <see cref="Fill"/> property.
        /// </summary>
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(nameof(Fill), typeof(Brush), typeof(PathIconPackBase<TKind>), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> for the fill.
        /// </summary>
        public Brush Fill
        {
            get => (Brush)GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }

        /// <summary>
        /// <see cref="DependencyProperty"/> of the <see cref="Stroke"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(PathIconPackBase<TKind>), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> for the stroke.
        /// </summary>
        public Brush Stroke
        {
            get => (Brush)GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }

        /// <summary>
        /// <see cref="DependencyProperty"/> of the <see cref="Stretch"/> property.
        /// </summary>
        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(PathIconPackBase<TKind>), new PropertyMetadata(default(Stretch)));

        /// <summary>
        /// Gets or sets the <see cref="Stretch"/> value.
        /// </summary>
        public Stretch Stretch
        {
            get => (Stretch)GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }

        /// <summary>
        /// <see cref="DependencyProperty"/> of the <see cref="StrokeThickness"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(PathIconPackBase<TKind>), new PropertyMetadata(1.0));

        /// <summary>
        /// Gets or sets the <see cref="StrokeThickness"/> value.
        /// </summary>
        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        /// <summary>
        /// <see cref="DependencyProperty"/> of the icon <see cref="Kind"/> property.
        /// </summary>
        public static readonly DependencyProperty KindProperty = DependencyProperty.Register(nameof(Kind), typeof(TKind), typeof(PathIconPackBase<TKind>), new PropertyMetadata(default(TKind), KindPropertyChangedCallback));

        /// <summary>
        /// Gets or sets the icon <see cref="Kind"/> value.
        /// </summary>
        public TKind Kind
        {
            get => (TKind)GetValue(KindProperty);
            set => SetValue(KindProperty, value);
        }

        /// <summary>
        /// Callback which is called when the <see cref="Kind"/> property changes.
        /// </summary>
        /// <param name="dependencyObject">The <see cref="DependencyObject"/> that owns the property.</param>
        /// <param name="dependencyPropertyChangedEventArgs">The event arguments.</param>
        static void KindPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ((PathIconPackBase)dependencyObject).UpdateData();
        }

        /// <summary>
        /// <see cref="DependencyPropertyKey"/> of the icon <see cref="Data"/> property.
        /// </summary>
        static readonly DependencyPropertyKey DataPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Data), typeof(string), typeof(PathIconPackBase<TKind>), new PropertyMetadata(""));

        /// <summary>
        /// <see cref="DependencyProperty"/> of the icon <see cref="Data"/> property.
        /// </summary>
        public static readonly DependencyProperty DataProperty = DataPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the icon path data for the current <see cref="Kind"/>.
        /// </summary>
        [TypeConverter(typeof(GeometryConverter))]
        public string Data
        {
            get => (string)GetValue(DataProperty); 
            private set => SetValue(DataPropertyKey, value); 
        }

        /// <summary>
        /// Called when the template was applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateData();
        }

        /// <summary>
        /// Called to update the <see cref="Data"/> property.
        /// </summary>
        internal override void UpdateData()
        {
            Data = GetIconGeometryString(Kind);
        }
    }
}
