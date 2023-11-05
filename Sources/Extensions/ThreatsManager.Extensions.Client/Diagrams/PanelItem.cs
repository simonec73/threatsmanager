using PostSharp.Patterns.Contracts;
using System;
using System.Drawing;

namespace ThreatsManager.Extensions.Diagrams
{
    /// <summary>
    /// Item to be shown in the Panel associated with the marker.
    /// </summary>
    public abstract class PanelItem
    {
        private Color _backColor = Color.White;
        private Color _textColor = Color.Black;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name of the item.</param>
        public PanelItem([Required] string name)
        {
            Name = name;
        }

        public event Action<PanelItem> Removed;
        public event Action<PanelItem> Updated; 

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
        /// Action to be performed when the used clicks the panel item.
        /// </summary>
        public virtual ClickAction ActionOnClick { get; } = ClickAction.ShowObject;

        /// <summary>
        /// Text to be shown when <see cref="ActionOnClick"/> is <see cref="ClickAction.ShowTooltip"/>.
        /// </summary>
        public virtual string TooltipText { get; } = string.Empty;

        /// <summary>
        /// Available actions from the Tooltip, when <see cref="ActionOnClick"/> is <see cref="ClickAction.ShowTooltip"/>.
        /// </summary>
        public virtual TooltipAction TooltipAction { get; } = TooltipAction.None;

        /// <summary>
        /// Action to be executed when <see cref="ActionOnClick"/> is <see cref="ClickAction.CallMethod"/>.
        /// </summary>
        /// <param name="target">Object that is linked with the panel item.</param>
        public virtual void ExecuteAction(object target) { }

        /// <summary>
        /// Action to be executed when the user click "Edit" from the Tooltip.
        /// </summary>
        /// <param name="target">Object that is linked with the panel item.</param>
        public virtual void EditAction(object target) 
        {
            Updated?.Invoke(this);
        }

        /// <summary>
        /// Action to be executed when the user click "Remove" from the Tooltip.
        /// </summary>
        /// <param name="target">Object that is linked with the panel item.</param>
        public virtual void RemoveAction(object target) 
        {
            Removed?.Invoke(this);
        }

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
