using PostSharp.Patterns.Contracts;
using System.ComponentModel;
using System.Drawing;
using System.Xml.Linq;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Diagrams
{
    /// <summary>
    /// Definition of an item to be shown in a custom Palette.
    /// </summary>
    public abstract class PaletteItem
    {
        private Color _backColor = Color.White;
        private Color _textColor = Color.Black;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name of the item.</param>
        public PaletteItem([Required] string name)
        {
            Name = name;
        }

        /// <summary>
        /// Name of the item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Background color.
        /// </summary>
        /// <remarks>Lime green is reserved for transparency. If selected, it will be overridden with the default color, which is white.</remarks>
        public Color BackColor
        {
            get => _backColor;

            set
            {
                if (value == null || value == Color.LimeGreen)
                    _backColor = Color.White;
                else
                    _backColor = value;
            }
        }

        /// <summary>
        /// Text color.
        /// </summary>
        /// <remarks>Lime green is reserved for transparency. If selected, it will be overridden with the default color, which is white.</remarks>
        public Color TextColor
        {
            get => _textColor;

            set
            {
                if (value == null || value == Color.LimeGreen)
                    _textColor = Color.Black;
                else
                    _textColor = value;
            }
        }

        /// <summary>
        /// Width of the palette item.
        /// </summary>
        public int Width { get; set; } = 500;

        /// <summary>
        /// Tag associated with the palette item.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Apply the item to a target.
        /// </summary>
        /// <param name="target">Target that should receive the item.</param>
        public virtual void Apply(object target)
        {

        }
    }
}
