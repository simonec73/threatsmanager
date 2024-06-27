using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Recording;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Quality.Annotations
{
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
    public class MeetingNotes : Annotation
    {
        public MeetingNotes()
        {
            Printable = false;
        }

        [JsonProperty("when")]
        private DateTime _when { get; set; }

        [property:NotRecorded]
        public DateTime When
        {
            get => _when;
            set
            {
                using (var scope = UndoRedoManager.OpenScope("Set When"))
                {
                    _when = value;
                    ModifiedOn = DateTime.Now;
                    ModifiedBy = UserName.GetDisplayName();
                    scope?.Complete();
                }
            }
        }

        [JsonProperty("participants")]
        private string _participants { get; set; }

        [property:NotRecorded]
        public string Participants
        {
            get => _participants;
            set
            {
                using (var scope = UndoRedoManager.OpenScope("Set Participants"))
                {
                    _participants = value;
                    ModifiedOn = DateTime.Now;
                    ModifiedBy = UserName.GetDisplayName();
                    scope?.Complete();
                }
            }
        }

        [JsonProperty("notes")]
        private string _notes { get; set; }

        [property: NotRecorded]
        public string Notes
        {
            get => _notes;
            set
            {
                using (var scope = UndoRedoManager.OpenScope("Set Notes"))
                {
                    _notes = value;
                    ModifiedOn = DateTime.Now;
                    ModifiedBy = UserName.GetDisplayName();
                    scope?.Complete();
                }
            }
        }

        [JsonProperty("references")]
        private string _references { get; set; }

        [property:NotRecorded]
        public IEnumerable<IUrl> References => _references?.SplitUrlDefinitions();

        public void SetReference(string label, string url)
        {
            InternalSetReference(_references.SetUrlDefinition(label, url));
        }

        public void SetReference(string oldLabel, string newLabel, string url)
        {
            InternalSetReference(_references.SetUrlDefinition(oldLabel, newLabel, url));
        }

        private void InternalSetReference(string value)
        {
            if (string.CompareOrdinal(value, _references) != 0)
            {
                using (var scope = UndoRedoManager.OpenScope("Set Reference"))
                {
                    _references = value;
                    ModifiedOn = DateTime.Now;
                    ModifiedBy = UserName.GetDisplayName();
                    scope?.Complete();
                }
            }
        }

        public void DeleteReference(string label)
        {
            if (_references.DeleteUrlDefinition(label, out var newUrlArray))
            {
                using (var scope = UndoRedoManager.OpenScope("Delete Reference"))
                {
                    _references = newUrlArray;
                    ModifiedOn = DateTime.Now;
                    ModifiedBy = UserName.GetDisplayName();
                    scope?.Complete();
                }
            }
        }
    }
}
