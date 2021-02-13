using System.Collections.Generic;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.DevOps.Schemas
{
    [JsonObject(MemberSerialization.OptIn)]
    class Iterations
    {
        public Iterations()
        {

        }

        public Iterations([NotNull] IEnumerable<Iteration> iterations)
        {
            Items = new List<Iteration>(iterations);
        }

        [JsonProperty("iterations")]
        public List<Iteration> Items { get; set; }
    }
}
