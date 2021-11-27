using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.QuantitativeRisk.Facts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FactContainer
    {
        [JsonProperty("facts")]
        public List<Fact> Facts { get; set; }

        public bool Add([NotNull] Fact fact)
        {
            bool result = false;

            var existing = Facts?.FirstOrDefault(x => x.Id == fact.Id);
            if (existing != null)
            {
                var index = Facts.IndexOf(existing);
                if (index >= 0)
                {
                    Facts[index] = fact;
                    result = true;
                }
            }
            else
            {
                if (Facts == null)
                    Facts = new List<Fact>();
                Facts.Add(fact);
                result = true;
            }

            return result;
        }

        public bool Remove([NotNull] Fact fact)
        {
            return Remove(fact.Id);
        }
        
        public bool Remove(Guid factId)
        {
            bool result = false;

            var existing = Facts?.FirstOrDefault(x => x.Id == factId);
            if (existing != null)
            {
                result = Facts.Remove(existing);
            }

            return result;
        }
    }
}
