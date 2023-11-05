using System;

namespace ThreatsManager.Interfaces.ObjectModel.Properties
{
    /// <summary>
    /// Interface implemented by Json-serializable objects that must be aware of the containing Threat Model.
    /// </summary>
    public interface IThreatModelAware
    {
        /// <summary>
        /// Identifier of the Model.
        /// </summary>
        Guid ModelId { get; set; }
    }
}
