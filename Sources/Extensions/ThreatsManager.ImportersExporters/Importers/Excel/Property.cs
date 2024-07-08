using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.ImportersExporters.Importers.Excel
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class Property
    {
        public Property()
        {
        }

        public Property([NotNull] RuleColumnSettings settings, string value)
        {
            Schema = settings.SchemaName;
            Namespace = settings.SchemaNamespace;
            Name = settings.PropertyName;
            Value = value;
        }

        [JsonProperty("schemaName")]
        public string Schema { get; private set; }
        [JsonProperty("schemaNs")]
        public string Namespace { get; private set; }
        [JsonProperty("propertyName")]
        public string Name { get; private set; }
        [JsonProperty("value")]
        public string Value { get; private set; }
        [JsonProperty("specifier")]
        public bool Specifier { get; private set; }
    }
}
