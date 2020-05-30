using System;
using System.Drawing;
using ThreatsManager.Interfaces.Extensions;

namespace ThreatsManager.Utilities
{
    /// <summary>
    /// Definition of an Action.
    /// </summary>
    /// <remarks>The class represents a typical implementation, which can be used as a reference.</remarks>
    public class ActionDefinition : IActionDefinition
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="id">Identifier of the owner of the action.</param>
        /// <param name="name">Name of the action.</param>
        /// <param name="label">Label to be used to describe the action.</param>
        /// <param name="icon">Icon of the action.</param>
        /// <param name="smallIcon">Small image of the action.</param>
        /// <param name="enabled">Inital status for the action.</param>
        /// <param name="shortcut">Shortcut for the action.</param>
        /// <param name="tooltip">Tooltip for the action.</param>
        public ActionDefinition(Guid id, string name, string label, 
            Bitmap icon, Bitmap smallIcon, bool enabled = true, 
            Shortcut shortcut = Shortcut.None, string tooltip = null)
        {
            Id = id;
            Label = label;
            Name = name;
            Icon = icon;
            SmallIcon = smallIcon;
            Enabled = enabled;
            Shortcut = shortcut;
            Tooltip = tooltip;
        }

        /// <summary>
        /// Identifier of the owner of the action.
        /// </summary>
        public Guid Id { get; }
 
        /// <summary>
        /// Name of the action.
        /// </summary>
        /// <remarks>This is an internal, logical name.</remarks>
        public string Name { get; }

        /// <summary>
        /// Label to be used to describe the action.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// Icon of the action.
        /// </summary>
        /// <remarks>It must be a 64x64 pixels image.</remarks>
        public Bitmap Icon { get; }

        /// <summary>
        /// Small image of the action.
        /// </summary>
        /// <remarks>It must be a 32x32 image.</remarks>
        public Bitmap SmallIcon { get; }

        /// <summary>
        /// Inital status for the action.
        /// </summary>
        public bool Enabled { get; }

        /// <summary>
        /// Shortcut for the action.
        /// </summary>
        public Shortcut Shortcut { get; }

        /// <summary>
        /// Tooltip for the action.
        /// </summary>
        public string Tooltip { get; }

        /// <summary>
        /// Associated object.
        /// </summary>
        /// <remarks>It may be used to add other information to allow execution of the action.</remarks>
        public object Tag { get; set; }
    }
}