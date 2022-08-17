using System;
using System.Collections.Generic;
using System.Text;

namespace ThreatsManager.Utilities
{
    /// <summary>
    /// Interface implemented by objects that have been attached to the Undo/Redo Manager.
    /// </summary>
    public interface IUndoable
    {
        /// <summary>
        /// True if the Undo has been enabled for the object.
        /// </summary>
        bool IsUndoEnabled { get; set; }
    }
}
