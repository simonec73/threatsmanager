using System.Globalization;
using ThreatsManager.Interfaces.Extensions;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.QuantitativeRisk.Schemas;

namespace ThreatsManager.QuantitativeRisk.Initializers
{
    [Extension("49798631-1E7A-4C13-824B-969839A189E4", "Quantitative Risk Schema Initializer", 10, ExecutionMode.Pioneer)]
    public class QuantitativeRiskSchemaInitializer : IInitializer
    {
        public void Initialize([NotNull] IThreatModel model)
        {
            var schemaManager = new QuantitativeRiskSchemaManager(model);
            schemaManager.GetEvaluationSchema();
            var schema = schemaManager.GetSchema();
            var propertyType = schema.GetPropertyType("Currency");
            var property = model.GetProperty(propertyType);
            if (property != null)
            {
                if (string.IsNullOrWhiteSpace(property.StringValue))
                    property.StringValue = NumberFormatInfo.CurrentInfo.CurrencySymbol;
            }
            else
            {
                model.AddProperty(propertyType, NumberFormatInfo.CurrentInfo.CurrencySymbol);
            }
        }
    }
}