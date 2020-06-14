using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.AutoThreatGeneration.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    public class EnumValueRuleNode : SelectionRuleNode
    {
        public EnumValueRuleNode()
        {

        }

        public EnumValueRuleNode([Required] string name, string schemaNamespace, string schemaName, 
            [NotNull] IEnumerable<string> values, string value)
        {
            Name = name;
            Namespace = schemaNamespace;
            Schema = schemaName;
            Values = values;
            Value = value;
        }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("schema")]
        public string Schema { get; set; }

        [JsonProperty("values")]
        private List<string> _values;

        public IEnumerable<string> Values
        {
            get { return _values; }
            set
            {
                if (value?.Any() ?? false)
                {
                    _values = new List<string>(value);
                }
                else
                {
                    _values = null;
                }
            }
        }

        [JsonProperty("value")]
        public string Value { get; set; }

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
                result = string.Compare(Value, actualValue, StringComparison.Ordinal) == 0;
            }

            return result;
        }

        private static bool TryGetValue([NotNull] IIdentity identity, 
            string schemaNs, string schemaName, [Required] string propertyName, out string value)
        {
            bool result = false;
            value = null;

            if (string.IsNullOrWhiteSpace(schemaNs) && string.IsNullOrWhiteSpace(schemaName))
            {
                var model = identity as IThreatModel ?? (identity as IThreatModelChild)?.Model;
                switch (propertyName)
                {
                    case "Flow Type":
                        value = (identity as IDataFlow)?.FlowType.GetEnumLabel();
                        result = true;
                        break;
                    case "Object Type":
                        value = model?.GetIdentityTypeName(identity);
                        result = true;
                        break;
                }
            }
            else
            {
                if ((identity is IThreatModelChild child) && (child.Model is IThreatModel model) &&
                    (identity is IPropertiesContainer container))
                {
                    var schema = model.GetSchema(schemaName, schemaNs);
                    var propertyType = schema?.GetPropertyType(propertyName);
                    if (propertyType != null)
                    {
                        value = container.GetProperty(propertyType)?.StringValue;
                        result = true;
                    }
                }
                else if (identity is IThreatModel model2)
                {
                    var schema = model2.GetSchema(schemaName, schemaNs);
                    var propertyType = schema?.GetPropertyType(propertyName);
                    if (propertyType != null)
                    {
                        value = model2.GetProperty(propertyType)?.StringValue;
                        result = true;
                    }
                }
            }

            return result;
        }

        public override string ToString()
        {
            return $"{Scope.ToString()}:{Schema}.{Name} = '{Value}'";
        }
    }
}
