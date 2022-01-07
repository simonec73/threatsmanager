using System;
using System.ComponentModel;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Utilities;

namespace ThreatsManager.QuantitativeRisk.Facts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FactRange : Fact
    {
        public FactRange()
        {

        }

        public FactRange([Required] string context, [Required] string source, [Required] string name, double min, double max)
            : base(context, source, name)
        {
            _min = min;
            _max = max;
        }

        private FactRange([NotNull] FactHardNumber hn) 
            : this(hn.Context, hn.Source, hn.Name, hn.Value, hn.Value)
        {
            Id = hn.Id;
            Details = hn.Details;
            Tags = hn.Tags;
            ReferenceDate = hn.ReferenceDate;
            CreatedBy = hn.CreatedBy;
            CreatedOn = hn.CreatedOn;
            ModifiedBy = UserName.GetDisplayName();
            ModifiedOn = DateTime.Now;
            Obsolete = hn.Obsolete;
            ReplacedBy = hn.ReplacedBy;
        }

        public static implicit operator FactRange(FactHardNumber hn)
        {
            FactRange result = null;

            if (hn != null)
                result = new FactRange(hn);

            return result;
        }

        [JsonProperty("min")]
        private double _min;

        public double Min
        {
            get => _min;
            set
            {
                if (_min != value)
                {
                    _min = value;
                    ModifiedBy = UserName.GetDisplayName();
                    ModifiedOn = DateTime.Now;
                }
            }
        }

        [JsonProperty("max")]
        private double _max;

        public double Max
        {
            get => _max;
            set
            {
                if (_max != value)
                {
                    _max = value;
                    ModifiedBy = UserName.GetDisplayName();
                    ModifiedOn = DateTime.Now;
                }
            }
        }
    }
}
