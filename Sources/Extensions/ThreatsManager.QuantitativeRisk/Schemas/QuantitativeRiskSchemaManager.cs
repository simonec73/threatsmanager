using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.VisualBasic;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.QuantitativeRisk.Engine;
using ThreatsManager.QuantitativeRisk.Facts;
using ThreatsManager.QuantitativeRisk.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.QuantitativeRisk.Schemas
{
    public class QuantitativeRiskSchemaManager
    {
        private readonly IThreatModel _model;

        public QuantitativeRiskSchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }
        
        public IPropertySchema GetSchema()
        {
            var result = _model.GetSchema(Resources.QuantitativeRiskSchemaName, Resources.DefaultNamespace) ??
                         _model.AddSchema(Resources.QuantitativeRiskSchemaName, Resources.DefaultNamespace);
            result.AppliesTo = Scope.ThreatModel;
            result.Priority = 20;
            result.Visible = false;
            result.System = true;
            result.NotExportable = true;
            result.AutoApply = false;
            result.RequiredExecutionMode = ExecutionMode.Expert;
            result.Description = Resources.ConfigurationPropertySchemaDescription;

            var currency = result.GetPropertyType("Currency") ?? result.AddPropertyType("Currency", PropertyValueType.SingleLineString);
            currency.Visible = false;
            currency.DoNotPrint = true;
            currency.Description = Resources.CurrencyProperty;

            var context = result.GetPropertyType("Context") ?? result.AddPropertyType("Context", PropertyValueType.SingleLineString);
            context.Visible = false;
            context.DoNotPrint = true;
            context.Description = Resources.FactContextProperty;

            var factProvider = result.GetPropertyType("FactProvider") ?? result.AddPropertyType("FactProvider", PropertyValueType.SingleLineString);
            factProvider.Visible = false;
            factProvider.DoNotPrint = true;
            factProvider.Description = Resources.FactProviderProperty;

            var fpParameters = result.GetPropertyType("FactProviderParams") ??
                               result.AddPropertyType("FactProviderParams", PropertyValueType.JsonSerializableObject);
            fpParameters.Visible = false;
            fpParameters.DoNotPrint = true;
            fpParameters.Description = Resources.FactProviderParamsProperty;

            var facts = result.GetPropertyType("Facts") ?? result.AddPropertyType("Facts", PropertyValueType.JsonSerializableObject);
            facts.Visible = false;
            facts.DoNotPrint = true;
            facts.Description = Resources.FactsProperty;

            var thresholds = result.GetPropertyType("Thresholds") ?? result.AddPropertyType("Thresholds", PropertyValueType.JsonSerializableObject);
            thresholds.Visible = false;
            thresholds.DoNotPrint = true;
            thresholds.Description = Resources.ThresholdsProperty;

            var lowerPercentile = result.GetPropertyType("LowerPercentile") ?? result.AddPropertyType("LowerPercentile", PropertyValueType.Integer);
            lowerPercentile.Visible = false;
            lowerPercentile.DoNotPrint = true;
            lowerPercentile.Description = Resources.LowerPercentileProperty;

            var upperPercentile = result.GetPropertyType("UpperPercentile") ?? result.AddPropertyType("UpperPercentile", PropertyValueType.Integer);
            upperPercentile.Visible = false;
            upperPercentile.DoNotPrint = true;
            upperPercentile.Description = Resources.UpperPercentileProperty;

            var referenceMeasure = result.GetPropertyType("ReferenceMeasure") ?? result.AddPropertyType("ReferenceMeasure", PropertyValueType.SingleLineString);
            referenceMeasure.Visible = false;
            referenceMeasure.DoNotPrint = true;
            referenceMeasure.Description = Resources.ReferenceMeasureProperty;

            var iterations = result.GetPropertyType("Iterations") ?? result.AddPropertyType("Iterations", PropertyValueType.Integer);
            upperPercentile.Visible = false;
            upperPercentile.DoNotPrint = true;
            upperPercentile.Description = Resources.IterationsProperty;

            return result;
        }

        public IPropertySchema GetEvaluationSchema()
        {
            var result = _model.GetSchema(Resources.EvaluationSchemaName, Resources.DefaultNamespace);
            if (result == null)
            {
                result = _model.AddSchema(Resources.EvaluationSchemaName, Resources.DefaultNamespace);
            }
            result.AppliesTo = Scope.ThreatEventScenario;
            result.AutoApply = false;
            result.Priority = 20;
            result.Visible = false;
            result.System = true;
            result.NotExportable = true;
            result.Description = Resources.EvaluationPropertySchemaDescription;

            var risk = result.GetPropertyType(Resources.RiskProperty);
            if (risk == null)
            {
                risk = result.AddPropertyType(Resources.RiskProperty, PropertyValueType.JsonSerializableObject);
            }

            return result;
        }

        public string Currency
        {
            get
            {
                string result = null;

                var propertyType = GetSchema()?.GetPropertyType("Currency");
                if (propertyType != null)
                {
                    result = (_model.GetProperty(propertyType) ?? _model.AddProperty(propertyType, NumberFormatInfo.CurrentInfo.CurrencySymbol))?.StringValue;
                }

                return result;
            }

            set
            {
                var propertyType = GetSchema()?.GetPropertyType("Currency");
                if (propertyType != null)
                {
                    var property = _model.GetProperty(propertyType) ?? _model.AddProperty(propertyType, null);
                    property.StringValue = value;
                }
            }
        }

        public string FactProviderId
        {
            get
            {
                string result = null;

                var propertyType = GetSchema()?.GetPropertyType("FactProvider");
                if (propertyType != null)
                {
                    result = _model.GetProperty(propertyType)?.StringValue;
                }

                return result;
            }

            set
            {
                var propertyType = GetSchema()?.GetPropertyType("FactProvider");
                if (propertyType != null)
                {
                    var property = _model.GetProperty(propertyType) ?? _model.AddProperty(propertyType, null);
                    property.StringValue = value;
                }
            }
        }

        public IFactProvider Provider
        {
            get
            {
                IFactProvider result = null;

                if (!string.IsNullOrWhiteSpace(FactProviderId))
                    result = ExtensionUtils.GetExtension<IFactProvider>(FactProviderId);

                return result;
            }
        }

        public IEnumerable<Parameter> GetFactProviderParameters()
        {
            IEnumerable<Parameter> result = null;

            var propertyType = GetSchema()?.GetPropertyType("FactProviderParams");
            if (propertyType != null)
            {
                var property = _model.GetProperty(propertyType);
                if (property is IPropertyJsonSerializableObject jsonObject)
                {
                    var parameters = jsonObject.Value as Parameters;
                    result = parameters?.Items.AsEnumerable();
                }
            }

            return result;
        }

        public void SetFactProviderParameters(IEnumerable<Parameter> parameters)
        {
            var propertyType = GetSchema()?.GetPropertyType("FactProviderParams");
            if (propertyType != null)
            {
                var property = _model.GetProperty(propertyType) ?? _model.AddProperty(propertyType, null);
                if (property is IPropertyJsonSerializableObject jsonObject)
                {
                    var container = new Parameters();
                    
                    var p = parameters?.ToArray();
                    if (p?.Any() ?? false)
                    {
                        container.Items = new List<Parameter>(p);
                    }

                    jsonObject.Value = container;
                }
            }
        }

        public int LowerPercentile
        {
            get
            {
                int result = 10;

                var propertyType = GetSchema()?.GetPropertyType("LowerPercentile");
                if (propertyType != null)
                {
                    var property = _model.GetProperty(propertyType) ?? _model.AddProperty(propertyType, "10");
                    if (property is IPropertyInteger propertyInteger)
                    {
                        result = propertyInteger.Value;
                    }
                }

                return result;
            }

            set
            {
                var propertyType = GetSchema()?.GetPropertyType("LowerPercentile");
                if (propertyType != null)
                {
                    var property = _model.GetProperty(propertyType) ?? _model.AddProperty(propertyType, null);
                    property.StringValue = value.ToString();
                }
            }
        }

        public int UpperPercentile
        {
            get
            {
                int result = 90;

                var propertyType = GetSchema()?.GetPropertyType("UpperPercentile");
                if (propertyType != null)
                {
                    var property = _model.GetProperty(propertyType) ?? _model.AddProperty(propertyType, "90");
                    if (property is IPropertyInteger propertyInteger)
                    {
                        result = propertyInteger.Value;
                    }
                }

                return result;
            }

            set
            {
                var propertyType = GetSchema()?.GetPropertyType("UpperPercentile");
                if (propertyType != null)
                {
                    var property = _model.GetProperty(propertyType) ?? _model.AddProperty(propertyType, null);
                    property.StringValue = value.ToString();
                }
            }
        }

        public string ReferenceMeasure
        {
            get
            {
                string result = null;

                var propertyType = GetSchema()?.GetPropertyType("ReferenceMeasure");
                if (propertyType != null)
                {
                    result = (_model.GetProperty(propertyType) ?? _model.AddProperty(propertyType, "Mode"))?.StringValue;
                }

                return result;
            }

            set
            {
                var propertyType = GetSchema()?.GetPropertyType("ReferenceMeasure");
                if (propertyType != null)
                {
                    var property = _model.GetProperty(propertyType) ?? _model.AddProperty(propertyType, null);
                    property.StringValue = value;
                }
            }
        }

        public int Iterations
        {
            get
            {
                int result = 100000;

                var propertyType = GetSchema()?.GetPropertyType("Iterations");
                if (propertyType != null)
                {
                    var property = _model.GetProperty(propertyType) ?? _model.AddProperty(propertyType, "100000");
                    if (property is IPropertyInteger propertyInteger)
                    {
                        result = propertyInteger.Value;
                    }
                }

                return result;
            }

            set
            {
                var propertyType = GetSchema()?.GetPropertyType("Iterations");
                if (propertyType != null)
                {
                    var property = _model.GetProperty(propertyType) ?? _model.AddProperty(propertyType, null);
                    property.StringValue = value.ToString();
                }
            }
        }

        public Risk GetRisk([NotNull] IThreatEventScenario scenario)
        {
            return (GetProperty(scenario, Resources.RiskProperty) as IPropertyJsonSerializableObject)?.Value as Risk;
        }

        public void SetRisk([NotNull] IThreatEventScenario scenario, Risk risk)
        {
            var property = GetProperty(scenario, Resources.RiskProperty) ?? AddProperty(scenario, Resources.RiskProperty);
            if (property is IPropertyJsonSerializableObject jsonSerializableObject)
            {
                jsonSerializableObject.Value = risk;
            }
        }

        public IDictionary<int, decimal> GetThresholds()
        {
            IDictionary<int, decimal> result = null;

            var propertyType = GetSchema()?.GetPropertyType("Thresholds");
            if (propertyType != null && _model.GetProperty(propertyType) is IPropertyJsonSerializableObject jsonObject && jsonObject.Value is Thresholds thresholds)
            {
                result = thresholds.Items?.ToDictionary(x => x.SeverityId, y => y.Value);
            }

            return result;
        }

        public void SetThresholds(IDictionary<int, decimal> thresholds)
        {
            var propertyType = GetSchema()?.GetPropertyType("Thresholds");

            if (propertyType != null)
            {
                if (thresholds?.Any() ?? false)
                {
                    var property = _model.GetProperty(propertyType) ?? _model.AddProperty(propertyType, null);
                    if (property is IPropertyJsonSerializableObject jsonObject)
                    {
                        jsonObject.Value = new Thresholds()
                        {
                            Items = new List<Threshold>(thresholds.Select(x => new Threshold(x.Key, x.Value)))
                        };
                    }
                }
                else
                {
                    var property = _model.GetProperty(propertyType);
                    if (property != null)
                        property.StringValue = null;
                }
            }
        }

        private IProperty GetProperty([NotNull] IThreatEventScenario scenario, [Required] string propertyName)
        {
            IProperty result = null;

            var schema = GetEvaluationSchema();           
            var propertyType = schema?.GetPropertyType(propertyName);
            if (propertyType != null)
            {
                result = scenario.GetProperty(propertyType);
            }

            return result;
        }

        private IProperty AddProperty([NotNull] IThreatEventScenario scenario, [Required] string propertyName)
        {
            IProperty result = null;

            var schema = GetEvaluationSchema();           
            var propertyType = schema?.GetPropertyType(propertyName);
            if (propertyType != null)
            {
                result = scenario.AddProperty(propertyType, null);
            }

            return result;
        }

        #region Facts management.
        public string Context
        {
            get
            {
                string result = null;

                var schema = GetSchema();
                var propertyType = schema?.GetPropertyType("Context");
                if (propertyType != null)
                {
                    result = _model.GetProperty(propertyType)?.StringValue;
                }

                return result;
            }

            set
            {
                var schema = GetSchema();           
                var propertyType = schema?.GetPropertyType("Context");
                if (propertyType != null)
                {
                    var property = _model.GetProperty(propertyType) ?? _model.AddProperty(propertyType, null);
                    if (property != null)
                        property.StringValue = value;
                }
            }
        }

        public IEnumerable<string> Contexts => GetFacts()?.Select(x => x.Context).Distinct().OrderBy(x => x);

        public IEnumerable<string> Tags
        {
            get
            {
                IEnumerable<string> result = null;

                var facts = GetFacts()?.ToArray();
                if (facts?.Any() ?? false)
                {
                    var list = new List<string>();

                    foreach (var fact in facts)
                    {
                        var tags = fact.Tags?.ToArray();
                        if (tags?.Any() ?? false)
                        {
                            foreach (var tag in tags)
                            {
                                if (!list.Contains(tag))
                                    list.Add(tag);
                            }
                        }
                    }

                    if (list.Any())
                        result = list.AsReadOnly();
                }

                return result;
            }
        }

        public IEnumerable<Fact> GetFacts()
        {
            IEnumerable<Fact> result = null;

            var schema = GetSchema();           
            var propertyType = schema?.GetPropertyType("Facts");
            if (propertyType != null && _model.GetProperty(propertyType) is IPropertyJsonSerializableObject jsonSerializableObject &&
                jsonSerializableObject.Value is FactContainer container)
            {
                result = container.Facts;
            }

            return result;
        }

        public bool AddFact([NotNull] Fact fact)
        {
            bool result = false;

            var schema = GetSchema();           
            var propertyType = schema?.GetPropertyType("Facts");
            if (propertyType != null)
            {
                var property = _model.GetProperty(propertyType) ?? _model.AddProperty(propertyType, null);
                if (property is IPropertyJsonSerializableObject jsonSerializableObject)
                {
                    if (jsonSerializableObject.Value is FactContainer container)
                    {
                        result = container.Add(fact);
                    }
                    else
                    {
                        container = new FactContainer();
                        result = container.Add(fact);
                        jsonSerializableObject.Value = container;
                    }
                }
            }

            return result;
        }

        public bool RemoveFact([NotNull] Fact fact)
        {
            bool result = false;

            var schema = GetSchema();           
            var propertyType = schema?.GetPropertyType("Facts");
            if (propertyType != null)
            {
                var property = _model.GetProperty(propertyType) ?? _model.AddProperty(propertyType, null);
                if (property is IPropertyJsonSerializableObject jsonSerializableObject)
                {
                    if (jsonSerializableObject.Value is FactContainer container)
                    {
                        result = container.Remove(fact);
                    }
                }
            }

            return result;
        }
        #endregion
    }
}