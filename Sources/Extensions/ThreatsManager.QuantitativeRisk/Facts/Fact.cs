using System;
using System.Collections.Generic;
using System.Linq;
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

        public Fact([Required] string context, [Required] string source, [Required] string name)
        {
            Id = Guid.NewGuid();
            _context = context;
            _source = source;
            _name = name;
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

        [JsonProperty("name")]
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && string.CompareOrdinal(value, _name) != 0)
                {
                    _name = value;
                    ModifiedBy = UserName.GetDisplayName();
                    ModifiedOn = DateTime.Now;
                }
            }
        }

        [JsonProperty("details")]
        private string _details;

        public string Details
        {
            get => _details;
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && string.CompareOrdinal(value, _details) != 0)
                {
                    _details = value;
                    ModifiedBy = UserName.GetDisplayName();
                    ModifiedOn = DateTime.Now;
                }
            }
        }

        [JsonProperty("tags")]
        private List<string> _tags;

        public IEnumerable<string> Tags
        {
            get => _tags?.ToArray();
            set
            {
                bool different = true;

                if ((value?.Count() ?? 0) == (_tags?.Count ?? 0))
                {
                    different = false;
                    if (value?.Any() ?? false)
                    {
                        foreach (var item in value)
                        {
                            if (!_tags.Contains(item))
                            {
                                different = true;
                                break;
                            }
                        }
                    }
                }

                if (different)
                {
                    _tags = new List<string>(value);
                    ModifiedBy = UserName.GetDisplayName();
                    ModifiedOn = DateTime.Now;
                }
            }
        }

        [JsonProperty("refDate")]
        public DateTime ReferenceDate { get; set; }

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
