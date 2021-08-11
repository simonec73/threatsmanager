using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.QuantitativeRisk.Engine;
using ThreatsManager.QuantitativeRisk.Properties;

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

                //result.AddPropertyType(Properties.Resources.SeriesProperty, PropertyValueType.Array);

            return result;
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
    }
}