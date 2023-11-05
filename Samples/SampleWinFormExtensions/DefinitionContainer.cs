using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Recording;
using ThreatsManager.Utilities;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.SampleWinFormExtensions
{
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
    public class DefinitionContainer
    {
        // Note: PostSharp provides also an AdvisableDictionary. It does not work with Undo/Redo!
        //  You must use AdvisableCollection.
        //  Analogously, standard classes like List<> would not work.

        [JsonProperty("definitions")]
        [Child]
        private AdvisableCollection<Definition> _definitions { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Definitions => _definitions?
            .OrderBy(x => x.Name)
            .Select(x => new KeyValuePair<string, string>(x.Name, x.Value));

        public string GetDefinition([Required] string name)
        {
            return _definitions?.FirstOrDefault(x => string.CompareOrdinal(name, x.Name) == 0)?.Value;
        }

        public bool SetDefinition([Required] string name, [Required] string value)
        {
            bool result = false;

            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(value))
            {
                using (var scope = UndoRedoManager.OpenScope("Set Definition"))
                {
                    if (_definitions == null)
                        _definitions = new AdvisableCollection<Definition>();

                    var definition = _definitions.FirstOrDefault(x => string.CompareOrdinal(name, x.Name) == 0);
                    if (definition != null)
                    {
                        definition.Value = value;
                    }
                    else
                    {
                        _definitions.Add(new Definition(name, value));
                    }
                    scope?.Complete();
                }
                result = true;
            }

            return result;
        }

        public bool RemoveDefinition([Required] string name)
        {
            bool result = false;

            if (!string.IsNullOrWhiteSpace(name))
            {
                using (var scope = UndoRedoManager.OpenScope("Remove Definition"))
                {
                    var definition = _definitions?.FirstOrDefault(x => string.CompareOrdinal(name, x.Name) == 0);
                    if (definition != null)
                    {
                        _definitions.Remove(definition);
                        scope?.Complete();
                        result = true;
                    }
                }
            }

            return result;
        }
    }
}
