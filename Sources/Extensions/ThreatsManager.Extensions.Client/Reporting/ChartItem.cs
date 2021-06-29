using System.Drawing;

namespace ThreatsManager.Extensions.Reporting
{
    /// <summary>
    /// Item to be shown in the Chart.
    /// </summary>
    public class ChartItem
    {
        /// <summary>
        /// Constructor for the Chart Item.
        /// </summary>
        /// <param name="label">Label to use for the Chart Item.</param>
        /// <param name="value">Value of the Chart Item.</param>
        /// <param name="color">[Optional] Color to use for the Chart Item.</param>
        public ChartItem(string label, float value, KnownColor? color = null)
        {
            Label = label;
            Value = value;
            Color = color;
        }

        /// <summary>
        /// Label to use for the Chart Item.
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// Value of the Chart Item.
        /// </summary>
        public float Value { get; private set; }

        /// <summary>
        /// Color to use for the Chart Item.
        /// </summary>
        /// <remarks>It is optional.</remarks>
        public KnownColor? Color { get; private set; }
    }
}