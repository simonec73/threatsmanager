using PostSharp.Patterns.Contracts;
using System.Drawing;

namespace ThreatsManager.Extensions.Diagrams
{
    /// <summary>
    /// Item to be shown in the Panel associated with the marker.
    /// </summary>
    public abstract class PanelItem
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name of the item.</param>
        public PanelItem([Required] string name)
        {
            Name = name;
        }

        /// <summary>
        /// Name of the item.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Tag associated with the panel item.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Get the icon for the panel item.
        /// </summary>
        /// <remarks>The returned icon is 16x16 bits.</remarks>
        public virtual Image Icon { get; }

        /// <summary>
        /// String to be shown for the object.
        /// </summary>
        /// <returns>Name of the panel item.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
