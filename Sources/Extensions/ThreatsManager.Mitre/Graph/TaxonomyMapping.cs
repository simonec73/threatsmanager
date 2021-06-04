using Newtonsoft.Json;

namespace ThreatsManager.Mitre.Graph
{
    [JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public class TaxonomyMapping
    {
        public TaxonomyMapping()
        {

        }

        public TaxonomyMapping(string taxonomy, string id, string name, bool exactFit)
        {
            Taxonomy = taxonomy;
            EntryId = id;
            EntryName = name;
            ExactFit = exactFit;
        }

        [JsonProperty("taxonomy")]
        public string Taxonomy { get; private set; }

        [JsonProperty("id")]
        public string EntryId { get; private set; }

        [JsonProperty("name")]
        public string EntryName { get; private set; }

        [JsonProperty("exact")]
        public bool ExactFit { get; private set; }
    }
}