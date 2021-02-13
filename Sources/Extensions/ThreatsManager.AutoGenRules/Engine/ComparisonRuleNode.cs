using System;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ComparisonRuleNode : SelectionRuleNode
    {
        public ComparisonRuleNode()
        {

        }

        public ComparisonRuleNode([Required] string name, string schemaNamespace, string schemaName, 
            ComparisonOperator compOperator, string value)
        {
            Name = name;
            Namespace = schemaNamespace;
            Schema = schemaName;
            Operator = compOperator;
            Value = value;
        }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("schema")]
        public string Schema { get; set; }

        [JsonProperty("operator")]
        public ComparisonOperator Operator { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        public override bool Evaluate([NotNull] object context)
        {
            bool result = false;

            if (context is IIdentity identity)
            {
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
            }
            else
            {
                result = InternalEvaluate(context);
            }

            return result;
        }

        private bool InternalEvaluate([NotNull] object context)
        {
            bool result = false;

            if (TryGetValue(context,
                Namespace, Schema, Name, out var actualValue))
            {
                switch (Operator)
                {
                    case ComparisonOperator.Exact:
                        result = EvaluateExact(Value, actualValue);
                        break;
                    case ComparisonOperator.StartsWith:
                        result = EvaluateStartsWith(Value, actualValue);
                        break;
                    case ComparisonOperator.Contains:
                        result = EvaluateContains(Value, actualValue);
                        break;
                }
            }

            return result;
        }

        private static bool TryGetValue([NotNull] object context, 
            string schemaNs, string schemaName, [Required] string propertyName, out string value)
        {
            bool result = false;
            value = null;

            if (string.IsNullOrWhiteSpace(schemaNs) && string.IsNullOrWhiteSpace(schemaName))
            {
                if (context is IIdentity identity)
                {
                    switch (propertyName)
                    {
                        case "Name":
                            value = identity.Name;
                            result = true;
                            break;
                        case "Description":
                            value = identity.Description;
                            result = true;
                            break;
                    }
                }
                else if (string.CompareOrdinal(propertyName, "Name") == 0)
                {
                    value = context.ToString();
                    result = true;
                }
            }
            else
            {
                if ((context is IThreatModelChild child) && (child.Model is IThreatModel model) &&
                    (context is IPropertiesContainer container))
                {
                    var schema = model.GetSchema(schemaName, schemaNs);
                    var propertyType = schema?.GetPropertyType(propertyName);
                    if (propertyType != null)
                    {
                        value = container.GetProperty(propertyType)?.StringValue;
                        result = true;
                    }
                }
                else if (context is IThreatModel model2)
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

        private static bool EvaluateExact(string expectedValue, string actualValue)
        {
            return (string.Compare(expectedValue, actualValue, StringComparison.Ordinal) == 0);
        }

        private static bool EvaluateStartsWith(string value, string actualValue)
        {
            bool result;

            if (!string.IsNullOrEmpty(actualValue))
                result = actualValue.StartsWith(value);
            else
                result = string.IsNullOrEmpty(value);

            return result;
        }

        private static bool EvaluateContains(string value, string actualValue)
        {
            bool result;

            if (!string.IsNullOrEmpty(actualValue))
                result = actualValue.Contains(value);
            else
                result = string.IsNullOrEmpty(value);

            return result;
        }

        public override string ToString()
        {
            return $"{Scope.ToString()}:{Schema}.{Name} {Operator.ToString()} '{Value}'";
        }
    }
}
