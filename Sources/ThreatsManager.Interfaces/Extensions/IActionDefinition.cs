using System;
using System.Drawing;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Interface that defines an action that extends the interface.
    /// </summary>
    public interface IActionDefinition
    {
        /// <summary>
        /// Identifier of the owner of the action.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Name of the action.
        /// </summary>
        /// <remarks>This is an internal, logical name.</remarks>
        string Name { get; }

        /// <summary>
        /// Label to be used to describe the action.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Icon of the action.
        /// </summary>
        /// <remarks>It must be a 32x32 image.</remarks>
        Bitmap Icon { get; }

        /// <summary>
        /// Small image of the action.
        /// </summary>
        /// <remarks>It must be a 16x16 image.</remarks>
        Bitmap SmallIcon { get; }

        /// <summary>
        /// Initial status for the action.
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Shortcut for the action.
        /// </summary>
        Shortcut Shortcut { get; }

        /// <summary>
        /// Tooltip for the action.
        /// </summary>
        string Tooltip { get; }

        /// <summary>
        /// Associated object.
        /// </summary>
        /// <remarks>It may be used to add other information to allow execution of the action.</remarks>
        object Tag { get; }
    }
}