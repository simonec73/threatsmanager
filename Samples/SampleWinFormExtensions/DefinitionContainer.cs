using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace SampleWinFormExtensions
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DefinitionContainer
    {
        [JsonProperty("definitions")]
        private Dictionary<string, string> _definitions { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Definitions => _definitions?.OrderBy(x => x.Key);

        public string GetDefinition(string name)
        {
            string result = null;

            string value = null;
            if (_definitions?.TryGetValue(name, out value) ?? false)
                result = value;

            return result;
        }

        public bool SetDefinition(string name, string value)
        {
            bool result = false;

            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(value))
            {
                if (_definitions == null)
                    _definitions = new Dictionary<string, string>();

                _definitions[name] = value;
                result = true;
            }

            return result;
        }

        public bool RemoveDefinition(string name)
        {
            bool result = false;

            if (_definitions?.ContainsKey(name) ?? false)
            {
                _definitions.Remove(name);
                result = true;
            }

            return result;
        }
    }
}
