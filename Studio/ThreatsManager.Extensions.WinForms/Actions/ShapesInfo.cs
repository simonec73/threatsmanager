using System.Collections.Generic;
using Newtonsoft.Json;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;

namespace ThreatsManager.Extensions.Actions
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ShapesInfo
    {
        [JsonProperty("shapes")]
        public List<IShape> Shapes;

        [JsonProperty("links")]
        public List<ILink> Links;
    }
}