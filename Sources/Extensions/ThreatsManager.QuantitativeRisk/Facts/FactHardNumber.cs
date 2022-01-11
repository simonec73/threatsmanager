using System;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Utilities;

namespace ThreatsManager.QuantitativeRisk.Facts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FactHardNumber : Fact
    {
        public FactHardNumber()
        {

        }

        public FactHardNumber([Required] string context, [Required] string source, [Required] string name, double value)
            : base(context, source, name)
        {
            _value = value;
        }

        private FactHardNumber([NotNull] FactProbRange pr) 
            : this(pr.Context, pr.Source, pr.Name, pr.MostProbable)
        {
            Id = pr.Id;
            Details = pr.Details;
            Tags = pr.Tags;
            ReferenceDate = pr.ReferenceDate;
            CreatedBy = pr.CreatedBy;
            CreatedOn = pr.CreatedOn;
            ModifiedBy = UserName.GetDisplayName();
            ModifiedOn = DateTime.Now;
            Obsolete = pr.Obsolete;
            ReplacedBy = pr.ReplacedBy;
        }

        public static implicit operator FactHardNumber(FactProbRange pr)
        {
            FactHardNumber result = null;

            if (pr != null)
                result = new FactHardNumber(pr);

            return result;
        }

        [JsonProperty("value")]
        private double _value;

        public double Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    ModifiedBy = UserName.GetDisplayName();
                    ModifiedOn = DateTime.Now;
                }
            }
        }
    }
}
