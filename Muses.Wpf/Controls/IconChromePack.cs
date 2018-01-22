using System.Windows;

namespace Muses.Wpf.Controls
{
    /// <summary>
    /// Icon pack for the Window and control chrome icons.
    /// </summary>
    public class IconChromePack : PathIconPackBase<IconChromeType>
    {
        static IconChromePack()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconChromePack), new FrameworkPropertyMetadata(typeof(IconChromePack)));
        }

        /// <summary>
        /// Constructor. Sets up thew icon factory.
        /// </summary>
        public IconChromePack() : base(IconChromeFactory.Create) { }
    }
}
