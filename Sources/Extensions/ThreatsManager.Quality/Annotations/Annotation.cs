using System;
using Newtonsoft.Json;
using PostSharp.Patterns.Recording;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Quality.Annotations
{
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
    [Undoable]
    public class Annotation
    {
        public Annotation()
        {
            Printable = true;
            CreatedOn = DateTime.Now;
            CreatedBy = UserName.GetDisplayName();
        }

        [JsonProperty("text")]
        private string _text { get; set; }

        [property:NotRecorded]
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                ModifiedOn = DateTime.Now;
                ModifiedBy = UserName.GetDisplayName();
            }
        }

        [JsonProperty("printable")]
        [field:NotRecorded]
        public bool Printable { get; protected set; }
            
        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; private set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; private set; }

        [JsonProperty("modifiedOn")]
        public DateTime ModifiedOn { get; protected set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; protected set; }
    }
}