using Newtonsoft.Json;
using PostSharp.Patterns.Recording;

namespace ThreatsManager.Utilities
{
    /// <summary>
    /// String wrapper to support Undo/Redo of lists of strings.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [Recordable]
    public class RecordableString
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public RecordableString()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">Initial value.</param>
        public RecordableString(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Value of the string.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary>
        /// Value of the string,
        /// </summary>
        /// <returns>String value.</returns>
        public override string ToString()
        {
            return Value;
        }
    }
}
