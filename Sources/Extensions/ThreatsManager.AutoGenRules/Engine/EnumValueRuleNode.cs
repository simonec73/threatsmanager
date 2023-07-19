using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Recording;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
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
        [Reference]
        [field:NotRecorded]
        private List<string> _legacyValues { get; set; }

        [JsonProperty("items", ItemTypeNameHandling = TypeNameHandling.None, TypeNameHandling = TypeNameHandling.None)]
        [Child]
        private AdvisableCollection<RecordableString> _values { get; set; }

        [property:NotRecorded]
        public IEnumerable<string> Values
        {
            get { return _values?.Select(x => x.Value).AsEnumerable(); ; }
            set
            {
                if (_values?.Any() ?? false)
                {
                     foreach (var item in _values)
                        UndoRedoManager.Detach(item);

                     _values.Clear();
                }

                if (value?.Any() ?? false)
                {
                    if (_values == null)
                        _values = new AdvisableCollection<RecordableString>();
                    foreach (var v in value)
                    {
                        var r = new RecordableString(v);
                        UndoRedoManager.Attach(r, Model);
                        _values.Add(r);
                    }
                }
            }
        }

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

            if (TryGetValue(context, Namespace, Schema, Name, out var actualValue))
            {
                result = string.Compare(Value, actualValue, StringComparison.Ordinal) == 0;
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
                var model = context as IThreatModel ?? (context as IThreatModelChild)?.Model;
                switch (propertyName)
                {
                    case "Flow Type":
                        value = (context as IDataFlow)?.FlowType.GetEnumLabel();
                        result = true;
                        break;
                    case "Object Type":
                        if (context is IIdentity identity)
                        {
                            value = model?.GetIdentityTypeName(identity);
                            result = true;
                        }
                        else if (context is IThreatTypeMitigation)
                        {
                            value = "Threat Type Mitigation";
                            result = true;
                        }
                        else if (context is IThreatEventMitigation)
                        {
                            value = "Threat Event Mitigation";
                            result = true;
                        }
                        break;
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

        public override string ToString()
        {
            return $"{Scope.ToString()}:{Schema}.{Name} = '{Value}'";
        }

        [OnDeserialized]
        public void PostDeserialization(StreamingContext context)
        {
            if (_legacyValues?.Any() ?? false)
            {
                if (_values == null)
                    _values = new AdvisableCollection<RecordableString>();

                foreach (var item in _legacyValues)
                {
                    var r = new RecordableString(item);
                    UndoRedoManager.Attach(r, Model);
                    _values.Add(r);
                }

                _legacyValues.Clear();
            }
        }
    }
}
