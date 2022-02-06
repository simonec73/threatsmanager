using System;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Direction of the stage.
    /// </summary>
    [Flags]
    public enum StageDirection
    {
        /// <summary>
        /// Stage is to be used during the serialization of the Threat Model.
        /// </summary>
        Serialization = 0x1,
        /// <summary>
        /// Stage is to be used during the deserialization of the Threat Model.
        /// </summary>
        Deserialization = 0x2,
        /// <summary>
        /// The stage is to be used in both directions.
        /// </summary>
        Both = Serialization | Deserialization
    }
}
