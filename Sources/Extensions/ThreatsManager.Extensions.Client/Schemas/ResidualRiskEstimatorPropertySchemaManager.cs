using System.Collections.Generic;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Properties;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Schemas
{
    public class ResidualRiskEstimatorPropertySchemaManager
    {
        private const string SchemaName = "Residual Risk Estimator Configuration";

        private readonly IThreatModel _model;

        public ResidualRiskEstimatorPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public IPropertySchema GetSchema()
        {
            var result = _model.GetSchema(SchemaName, Resources.DefaultNamespace) ?? _model.AddSchema(SchemaName, Resources.DefaultNamespace);
            result.AppliesTo = Scope.ThreatModel;
            result.AutoApply = false;
            result.Priority = 100;
            result.Visible = false;
            result.System = true;
            result.NotExportable = true;
            result.Description = Resources.ResidualRiskEstimatorConfigurationPropertySchemaDescription;

            return result;
        }

        public IPropertyType GetSelectedEstimatorIdPropertyType()
        {
            IPropertyType result = null;

            var schema = GetSchema();
            if (schema != null)
            {
                result = schema.GetPropertyType("Selected Estimator") ?? schema.AddPropertyType("Selected Estimator", PropertyValueType.SingleLineString);
                result.Visible = false;
                result.DoNotPrint = true;
                result.Description = "Extension Id of the Selected Residual Risk Estimator";
            }

            return result;
        }

        public IPropertyType GetParametersPropertyType()
        {
            IPropertyType result = null;

            var schema = GetSchema();
            if (schema != null)
            {
                result = schema.GetPropertyType("Estimator Parameters") ?? schema.AddPropertyType("Estimator Parameters", PropertyValueType.JsonSerializableObject);
                result.Visible = false;
                result.DoNotPrint = true;
                result.Description = "Parameters of the Selected Residual Risk Estimator";
            }

            return result;
        }

        public IPropertyType GetInfinitePropertyType()
        {
            IPropertyType result = null;

            var schema = GetSchema();
            if (schema != null)
            {
                result = schema.GetPropertyType("Infinite Cap") ?? schema.AddPropertyType("Infinite Cap", PropertyValueType.Decimal);
                result.Visible = false;
                result.Description = "Infinite Cap for the selected Residual Risk Estimator";
            }

            return result;
        }

        public IResidualRiskEstimator SelectedEstimator
        {
            get
            {
                IResidualRiskEstimator result = null;

                var propertyType = GetSelectedEstimatorIdPropertyType();
                if (propertyType != null)
                {
                    var property = _model.GetProperty(propertyType);
                    if (property != null)
                        result = ExtensionUtils.GetExtension<IResidualRiskEstimator>(property.StringValue);
                }

                return result;
            }

            set
            {
                var propertyType = GetSelectedEstimatorIdPropertyType();
                if (propertyType != null)
                {
                    var property = _model.GetProperty(propertyType);

                    if (property == null)
                    {
                        if (value != null)
                        {
                            _model.AddProperty(propertyType, value.GetExtensionId());
                        }
                    }
                    else
                    {
                        property.StringValue = value?.GetExtensionId();
                    }
                }
            }
        }

        public IEnumerable<ResidualRiskEstimatorParameter> Parameters
        {
            get
            {
                IEnumerable<ResidualRiskEstimatorParameter> result = null;

                var propertyType = GetParametersPropertyType();
                if (propertyType != null)
                {
                    if (_model.GetProperty(propertyType) is IPropertyJsonSerializableObject jsonSerializableObject &&
                        jsonSerializableObject.Value is ResidualRiskEstimatorConfiguration config)
                    {
                        result = config.Parameters;
                    }
                }

                return result;
            }

            set
            {
                var propertyType = GetParametersPropertyType();
                if (propertyType != null)
                {
                    if (_model.GetProperty(propertyType) is IPropertyJsonSerializableObject property)
                    {
                        if (value == null)
                            property.Value = null;
                        else
                        {
                            property.Value = new ResidualRiskEstimatorConfiguration()
                            {
                                Parameters = new List<ResidualRiskEstimatorParameter>(value)
                            };
                        }
                    }
                    else
                    {
                        if (value != null)
                        {
                            if (_model.AddProperty(propertyType, null) is IPropertyJsonSerializableObject p)
                            {
                                p.Value = new ResidualRiskEstimatorConfiguration()
                                {
                                    Parameters = new List<ResidualRiskEstimatorParameter>(value)
                                };
                            }
                        }
                    }
                }
            }
        }

        public float Infinite
        {
            get
            {
                float result = -1f;

                var propertyType = GetInfinitePropertyType();
                if (propertyType != null)
                {
                    var property = _model.GetProperty(propertyType);
                    if (property is IPropertyDecimal decimalProperty)
                        result = (float) decimalProperty.Value;
                }

                return result;
            }

            set
            {
                var propertyType = GetInfinitePropertyType();
                if (propertyType != null)
                {
                    var property = _model.GetProperty(propertyType);

                    if (property == null)
                        property = _model.AddProperty(propertyType, null);

                    if (property is IPropertyDecimal decimalProperty)
                        decimalProperty.Value = (decimal) value;
                }
            }
        }
    }
}
