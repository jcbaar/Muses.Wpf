using System;
using System.Windows.Markup;
using System.Windows.Media;

namespace Muses.Wpf.Controls
{
    /// <summary>
    /// Simple <see cref="MarkupExtension"/> which will return the icon
    /// <see cref="Geometry"/> of a <see cref="IconChromePack"/> based icon.
    /// </summary>
    [MarkupExtensionReturnType(typeof(IconChromeType))]
    public class IconChromeGeometryExtension : MarkupExtension
    {
        Geometry _geometry;

        /// <summary>
        /// Constructor. Initializes an instance of the object.
        /// </summary>
        public IconChromeGeometryExtension()
        { }

        /// <summary>
        /// Constructor. Initializes an instance of the object.
        /// </summary>
        /// <param name="kind">The <see cref="IconChromeType"/> of the icon.</param>
        public IconChromeGeometryExtension(IconChromeType kind)
        {
            Kind = kind;
            _geometry = null;
        }

        /// <summary>
        /// The <see cref="IconChromeType"/> of the icon.
        /// </summary>
        [ConstructorArgument("kind")]
        public IconChromeType Kind { get; set; } = IconChromeType.Empty;

        /// <summary>
        /// Called to provide a value for the markup.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/></param>
        /// <returns>The markup value.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_geometry == null)
            {
                _geometry = IconChromePack.GetIconGeometry(Kind);
            }
            return _geometry;
        }
    }
}
