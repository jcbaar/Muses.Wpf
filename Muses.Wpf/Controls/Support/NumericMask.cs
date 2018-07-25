namespace Muses.Wpf.Controls.Support
{
    /// <summary>
    /// Types for numeric masking in a <see cref="TextBox"/>
    /// </summary>
    public enum NumericMask
    {
        /// <summary>
        /// No masking. Allow all characters.
        /// </summary>
        Any,

        /// <summary>
        /// Only integer numbers.
        /// </summary>
        Integer,

        /// <summary>
        /// Floating point numbers.
        /// </summary>
        Decimal
    }
}
