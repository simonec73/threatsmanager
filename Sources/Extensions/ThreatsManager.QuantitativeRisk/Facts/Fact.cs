using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Utilities;

namespace ThreatsManager.QuantitativeRisk.Facts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Fact
    {
        public Fact()
        {

        }

        public Fact([Required] string context, [Required] string source, [Required] string text)
        {
            Id = Guid.NewGuid();
            _context = context;
            _source = source;
            _text = text;
            CreatedBy = UserName.GetDisplayName();
            CreatedOn = DateTime.Now;
        }

        [JsonProperty("id")]
        public Guid Id { get; protected set; }

        [JsonProperty("context")] 
        private string _context;

        public string Context
        {
            get => _context;
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && string.CompareOrdinal(value, _context) != 0)
                {
                    _context = value;
                    ModifiedBy = UserName.GetDisplayName();
                    ModifiedOn = DateTime.Now;
                }
            }
        }

        [JsonProperty("source")] 
        private string _source;
        
        public string Source
        {
            get => _source;
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && string.CompareOrdinal(value, _source) != 0)
                {
                    _source = value;
                    ModifiedBy = UserName.GetDisplayName();
                    ModifiedOn = DateTime.Now;
                }
            }
        }

        [JsonProperty("text")]
        private string _text;

        public string Text
        {
            get => _text;
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && string.CompareOrdinal(value, _text) != 0)
                {
                    _text = value;
                    ModifiedBy = UserName.GetDisplayName();
                    ModifiedOn = DateTime.Now;
                }
            }
        }

        [JsonProperty("notes")]
        private string _notes;

        public string Notes
        {
            get => _notes;
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && string.CompareOrdinal(value, _notes) != 0)
                {
                    _notes = value;
                    ModifiedBy = UserName.GetDisplayName();
                    ModifiedOn = DateTime.Now;
                }
            }
        }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; protected set; }

        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; protected set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; protected set; }

        [JsonProperty("modifiedOn")]
        public DateTime ModifiedOn { get; protected set; }

        [JsonProperty("obsolete")]
        public bool Obsolete { get; protected set; }

        [JsonProperty("replacedBy")]
        public Guid ReplacedBy { get; protected set; }

        public void MarkObsolete(Fact newFact = null)
        {
            Obsolete = true;
            if (newFact != null)
            {
                ReplacedBy = newFact.Id;
            }
            ModifiedBy = UserName.GetDisplayName();
            ModifiedOn = DateTime.Now;
        }
    }
}
