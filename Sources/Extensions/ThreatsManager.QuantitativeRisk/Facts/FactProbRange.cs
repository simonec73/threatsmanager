using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PostSharp.Patterns.Contracts;
using ThreatsManager.QuantitativeRisk.Engine;
using ThreatsManager.Utilities;

namespace ThreatsManager.QuantitativeRisk.Facts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FactProbRange : FactRange
    {
        public FactProbRange()
        {

        }

        public FactProbRange([Required] string context, [Required] string source, [Required] string name, 
            double min, double max, double mostProbable, Confidence confidence)
            : base(context, source, name, min, max)
        {
            _mostProbable = mostProbable;
            _confidence = confidence;
        }

        private FactProbRange([NotNull] FactHardNumber hn) 
            : this(hn.Context, hn.Source, hn.Name, hn.Value, hn.Value, hn.Value, Confidence.Moderate)
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

        public static implicit operator FactProbRange(FactHardNumber hn)
        {
            FactProbRange result = null;

            if (hn != null)
                result = new FactProbRange(hn);

            return result;
        }

        [JsonProperty("mostProbable")]
        private double _mostProbable;

        public double MostProbable
        {
            get => _mostProbable;
            set
            {
                if (_mostProbable != value)
                {
                    _mostProbable = value;
                    ModifiedBy = UserName.GetDisplayName();
                    ModifiedOn = DateTime.Now;
                }
            }
        }

        [JsonProperty("confidence")]
        [JsonConverter(typeof(StringEnumConverter))]
        private Confidence _confidence;

        public Confidence Confidence
        {
            get => _confidence;
            set
            {
                if (_confidence != value)
                {
                    _confidence = value;
                    ModifiedBy = UserName.GetDisplayName();
                    ModifiedOn = DateTime.Now;
                }
            }
        }
    }
}
