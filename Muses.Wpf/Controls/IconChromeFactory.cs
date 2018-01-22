using System.Collections.Generic;

namespace Muses.Wpf.Controls
{
    /// <summary>
    /// Factory for the window and control chrome icons.
    /// </summary>
    internal static class IconChromeFactory
    {
        /// <summary>
        /// Factory method for creating the window and control chrome icons.
        /// </summary>
        /// <returns>The factory method.</returns>
        internal static IDictionary<IconChromeType, string> Create() => new Dictionary<IconChromeType, string>
        {
            {IconChromeType.Empty, "" },

            { IconChromeType.Flag, "M0,0 L13,0 L5,7 L8,13 L7,13 L0,0 z" },
            { IconChromeType.Warning, "M 38,22.1667C 41.1666,22.1667 57,52.25 55.4166,53.8333C 53.8333,55.4167 22.1667,55.4167 20.5833,53.8333C 19,52.25 34.8333,22.1667 38,22.1667 Z M 38,45.9167C 36.6883,45.9167 35.625,46.98 35.625,48.2917C 35.625,49.6034 36.6883,50.6667 38,50.6667C 39.3116,50.6667 40.375,49.6034 40.375,48.2917C 40.375,46.98 39.3116,45.9167 38,45.9167 Z M 35.625,31.6667L 36.4166,44.3333L 39.5833,44.3333L 40.375,31.6667L 35.625,31.6667 Z" },
            { IconChromeType.Magnify, "M9.5,3A6.5,6.5 0 0,1 16,9.5C16,11.11 15.41,12.59 14.44,13.73L14.71,14H15.5L20.5,19L19,20.5L14,15.5V14.71L13.73,14.44C12.59,15.41 11.11,16 9.5,16A6.5,6.5 0 0,1 3,9.5A6.5,6.5 0 0,1 9.5,3M9.5,5C7,5 5,7 5,9.5C5,12 7,14 9.5,14C12,14 14,12 14,9.5C14,7 12,5 9.5,5Z" },
            { IconChromeType.ResizeGrip, "M1,10 L3,10 M5,10 L7,10 M9,10 L11,10 M2,9 L2,11 M6,9 L6,11 M10,9 L10,11 M5,6 L7,6 M9,6 L11,6 M6,5 L6,7 M10,5 L10,7 M9,2 L11,2 M10,1 L10,3" },
            { IconChromeType.MinimizeButton, "M0,6 L8,6 Z" },
            { IconChromeType.RestoreButton, "M2,0 L8,0 L8,6 M0,3 L6,3 M0,2 L6,2 L6,8 L0,8 Z" },
            { IconChromeType.MaximizeButton, "M0,1 L9,1 L9,2 L0,2 M9,2 L9,8 L0,8 L0,1 Z" },
            { IconChromeType.CloseButton, "M0,0 L8,8 M8,0 L0,8 Z" },
            { IconChromeType.ComboDownArrow, "M1,3 L4,7 L7,3 L1,3 z" },
            { IconChromeType.CheckMark, "M0,2 L0,4.8 L2.5,7.4 L7.1,2.8 L7.1,0 L2.5,4.6 z" },
            { IconChromeType.CheckMarkIndeterminate, "M 0 0 L 0 8 L 8 8 L 8 0" },
            { IconChromeType.SubMenuArrow, "M0,0 L0,8 L4,4 z" },
            { IconChromeType.ToolbarOverflowArrow,"M0,0 L6,0 L6,1 L0,1 Z M0,3 L6,3 L3,6 Z" },
            { IconChromeType.ToolbarThumb, "M 4 4 L 4 8 L 8 8 L 8 4 z" },
            { IconChromeType.ScrollBarLeftArrow, "M 4 0 L 4 8 L 0 4 Z" },
            { IconChromeType.ScrollBarRightArrow, "M 0 0 L 4 4 L 0 8 Z" },
            { IconChromeType.ScrollBarUpArrow, "M 0 4 L 8 4 L 4 0 Z" },
            { IconChromeType.ScrollBarDownArrow, "M 0 0 L 4 4 L 8 0 Z" },
            { IconChromeType.CommandCut, "M19,3L13,9L15,11L22,4V3M12,12.5A0.5,0.5 0 0,1 11.5,12A0.5,0.5 0 0,1 12,11.5A0.5,0.5 0 0,1 12.5,12A0.5,0.5 0 0,1 12,12.5M6,20A2,2 0 0,1 4,18C4,16.89 4.9,16 6,16A2,2 0 0,1 8,18C8,19.11 7.1,20 6,20M6,8A2,2 0 0,1 4,6C4,4.89 4.9,4 6,4A2,2 0 0,1 8,6C8,7.11 7.1,8 6,8M9.64,7.64C9.87,7.14 10,6.59 10,6A4,4 0 0,0 6,2A4,4 0 0,0 2,6A4,4 0 0,0 6,10C6.59,10 7.14,9.87 7.64,9.64L10,12L7.64,14.36C7.14,14.13 6.59,14 6,14A4,4 0 0,0 2,18A4,4 0 0,0 6,22A4,4 0 0,0 10,18C10,17.41 9.87,16.86 9.64,16.36L12,14L19,21H22V20L9.64,7.64Z" },
            { IconChromeType.CommandCopy, "M19,21H8V7H19M19,5H8A2,2 0 0,0 6,7V21A2,2 0 0,0 8,23H19A2,2 0 0,0 21,21V7A2,2 0 0,0 19,5M16,1H4A2,2 0 0,0 2,3V17H4V3H16V1Z" },
            { IconChromeType.CommandPaste, "M19,20H5V4H7V7H17V4H19M12,2A1,1 0 0,1 13,3A1,1 0 0,1 12,4A1,1 0 0,1 11,3A1,1 0 0,1 12,2M19,2H14.82C14.4,0.84 13.3,0 12,0C10.7,0 9.6,0.84 9.18,2H5A2,2 0 0,0 3,4V20A2,2 0 0,0 5,22H19A2,2 0 0,0 21,20V4A2,2 0 0,0 19,2Z" }
         };
    }
}
