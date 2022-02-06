using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
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
            Model = model;
        }

        /// <summary>
        /// Threat Model 
        /// </summary>
        public IThreatModel Model { get; }

        [JsonProperty("inputPipeline")]
        private List<string> _deserializationPipeline { get; set; }

        public IEnumerable<IUniversalStage> DeserializationPipeline => GetStages(_deserializationPipeline)?.ToArray();

        [JsonProperty("outputPipeline")]
        private List<string> _serializationPipeline { get; set;}

        public IEnumerable<IUniversalStage> SerializationPipeline => GetStages(_serializationPipeline)?.ToArray();

        public static UniversalThreatModel Deserialize(string serialized)
        {
            UniversalThreatModel result = null;

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
    }
}
