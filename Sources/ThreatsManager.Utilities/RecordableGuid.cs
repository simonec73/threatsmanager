using Newtonsoft.Json;
using PostSharp.Patterns.Recording;
using System;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Utilities
{
    /// <summary>
    /// String wrapper to support Undo/Redo of lists of Guids.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [Recordable]
    [Undoable]
    public class RecordableGuid
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public RecordableGuid()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">Initial value.</param>
        public RecordableGuid(Guid value)
        {
            Value = value;
        }

        /// <summary>
        /// Value of the string.
        /// </summary>
        [JsonProperty("value")]
        public Guid Value { get; set; }

        /// <summary>
        /// Value of the string,
        /// </summary>
        /// <returns>String value.</returns>
        public override string ToString()
        {
            return Value.ToString("N");
        }
    }
}
