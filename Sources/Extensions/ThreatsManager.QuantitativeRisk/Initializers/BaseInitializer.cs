using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.QuantitativeRisk.Facts;
using ThreatsManager.QuantitativeRisk.Schemas;

namespace ThreatsManager.QuantitativeRisk.Initializers
{
    public class BaseInitializer
    {
        protected void DoInitialize(IThreatModel model)
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

            FactsManager.RegisterThreatModel(model);
        }
    }
}
