using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.AutoThreatGeneration.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BooleanRuleNode : SelectionRuleNode
    {
        public BooleanRuleNode()
        {

        }

        public BooleanRuleNode([Required] string name, string schemaNamespace, string schemaName, bool value)
        {
            Name = name;
            Namespace = schemaNamespace;
            Schema = schemaName;
            Value = value;
        }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("schema")]
        public string Schema { get; set; }

        [JsonProperty("value")]
        public bool Value { get; set; }

        public override bool Evaluate([NotNull] IIdentity identity)
        {
            bool result = false;

            var scopedIdentity = GetScopedIdentity(identity);

            if (scopedIdentity != null)
            {
                if (Scope == Scope.AnyTrustBoundary)
                {
                    var crossedTrustBoundaries = GetCrossedTrustBoundaries(scopedIdentity)?.ToArray();
                    if (crossedTrustBoundaries?.Any() ?? false)
                    {
                        foreach (var tb in crossedTrustBoundaries)
                        {
                            result = InternalEvaluate(tb);

                            if (result)
                                break;
                        }
                    }
                }
                else
                {
                    result = InternalEvaluate(scopedIdentity);
                }
            }

            return result;
        }

        private bool InternalEvaluate([NotNull] IIdentity identity)
        {
            bool result = false;

            if (TryGetValue(identity, Namespace, Schema, Name, out var actualValue))
            {
                result = Value == actualValue;
            }

            return result;
        }

        private static bool TryGetValue([NotNull] IIdentity identity, 
            string schemaNs, string schemaName, [Required] string propertyName, out bool value)
        {
            bool result = false;
            value = false;

            if ((identity is IThreatModelChild child) && (child.Model is IThreatModel model) &&
                (identity is IPropertiesContainer container))
            {
                var schema = model.GetSchema(schemaName, schemaNs);
                var propertyType = schema?.GetPropertyType(propertyName);
                if (propertyType is IBoolPropertyType boolPropertyType &&
                    container.GetProperty(boolPropertyType) is IPropertyBool property)
                {
                    value = property.Value;
                    result = true;
                }
            } else if (identity is IThreatModel model2)
            {
                var schema = model2.GetSchema(schemaName, schemaNs);
                var propertyType = schema?.GetPropertyType(propertyName);
                if (propertyType is IBoolPropertyType boolPropertyType &&
                    model2.GetProperty(boolPropertyType) is IPropertyBool property)
                {
                    value = property.Value;
                    result = true;
                }
            }

            return result;
        }

        public override string ToString()
        {
            return $"{Scope.ToString()}:{Schema}.{Name} = {Value.ToString()}";
        }
    }
}
