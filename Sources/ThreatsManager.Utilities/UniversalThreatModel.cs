using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Utilities
{
    /// <summary>
    /// Universal Threat Model.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class UniversalThreatModel
    {
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        private class Header
        {
            public const string CurrentVersion = "1.0";

            public Header()
            {
                Version = CurrentVersion;
            }

            public Header([Required] string header)
            {
                var bytes = Convert.FromBase64String(header);
                if (bytes?.Any() ?? false)
                {
                    var text = Encoding.UTF8.GetString(bytes);

                    using (var textReader = new StringReader(text))
                    using (var reader = new JsonTextReader(textReader))
                    {
                        var serializer = new JsonSerializer
                        {
                            MaxDepth = 128,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        var h = serializer.Deserialize<Header>(reader);
                        if (h != null)
                        {
                            Version = h.Version;
                        }
                    }

                }
            }

            [JsonProperty("ver")]
            public string Version { get; private set; }

            [JsonProperty("serialization")]
            private List<StageConfiguration> _serializationPipeline;

            [JsonProperty("deserialization")]
            private List<StageConfiguration> _deserializationPipeline;

            public IEnumerable<StageConfiguration> SerializationPipeline => _serializationPipeline?.AsEnumerable();

            public IEnumerable<StageConfiguration> DeserializationPipeline => _deserializationPipeline?.AsEnumerable();
        }

        private Header _header;
        private IThreatModel _model;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public UniversalThreatModel()
        {
        }

        /// <summary>
        /// Constructor starting from an existing model.
        /// </summary>
        /// <param name="model">Model to be used as a starting point.</param>
        public UniversalThreatModel([NotNull] IThreatModel model)
        {
            _model = model;
        }

        /// <summary>
        /// Threat Model 
        /// </summary>
        public IThreatModel Model => _model;

        [JsonProperty("inputPipeline")]
        private List<string> _deserializationPipeline { get; set; }

        public IEnumerable<IUniversalStage> DeserializationPipeline => GetStages(_deserializationPipeline)?.ToArray();

        [JsonProperty("outputPipeline")]
        private List<string> _serializationPipeline { get; set;}

        public IEnumerable<IUniversalStage> SerializationPipeline => GetStages(_serializationPipeline)?.ToArray();

        public static UniversalThreatModel Deserialize([Required] string serialized)
        {
            UniversalThreatModel result = null;

            var index = serialized.IndexOf('.');
            if (index > 0 && serialized.Length > index + 2)
            {
                var header = serialized.Substring(0, index);
                var payload = serialized.Substring(index + 1);

                //var serializedThreatModel = Apply(serialized, DeserializationPipeline);

            }


            return result;
        }

        public string Serialize()
        {
            return null;
        }

        private IEnumerable<IUniversalStage> GetStages(List<string> universalNames)
        {
            IEnumerable<IUniversalStage> result = null;

            var stages = ExtensionUtils.GetExtensions<IUniversalStage>()?.ToArray();
            if (stages?.Any() ?? false)
            { 
                var list = new List<IUniversalStage>();

                foreach (var name in universalNames)
                {
                    var stage = stages.FirstOrDefault(x => string.CompareOrdinal(name, x.GetExtensionUniversalId()) == 0);
                    if (stage != null)
                    {
                        list.Add(stage);
                    }
                }

                if (list.Any())
                {
                    result = list.AsEnumerable();
                }
            }

            return result;
        }

        private string Apply([Required] string text, IEnumerable<IUniversalStage> stages)
        {
            string result = text;

            if (stages?.Any() ?? false)
            {
                foreach (var stage in stages)
                {
                    result = stage.Execute(result);
                }
            }

            return result;
        }
    }
}
