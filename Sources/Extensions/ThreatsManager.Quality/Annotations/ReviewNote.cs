using Newtonsoft.Json;
using PostSharp.Patterns.Recording;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Quality.Annotations
{
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
    public class ReviewNote : Annotation
    {
        public ReviewNote()
        {
            Printable = false;
        }
    }
}
