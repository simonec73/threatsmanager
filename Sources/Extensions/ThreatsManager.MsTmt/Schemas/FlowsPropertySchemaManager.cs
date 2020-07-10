using System.Collections.Generic;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.MsTmt.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.MsTmt.Schemas
{
    public class FlowsPropertySchemaManager
    {
        private readonly IThreatModel _model;

        public FlowsPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public IPropertySchema GetSchema()
        {
            var result = _model.GetSchema(Resources.FlowsPropertySchema, Resources.DefaultNamespace);
            if (result == null)
            {
                result = _model.AddSchema(Resources.FlowsPropertySchema, Resources.DefaultNamespace);
                result.AppliesTo = Scope.DataFlow;
                result.Priority = 100;
                result.Visible = true;
                result.System = true;
                result.AutoApply = true;
                result.Description = Resources.TmtPropertySchemaDescription;
            }

            return result;
        }

        public string GetProperty([NotNull] IPropertiesContainer container, [Required] string propertyName)
        {
            string result = null;

            var schema = _model.GetSchema(Resources.FlowsPropertySchema, Resources.DefaultNamespace);
            var propertyType = schema?.GetPropertyType(propertyName);
            if (propertyType != null)
                result = container.GetProperty(propertyType)?.StringValue;

            return result;
        }

        public void SetProperty([NotNull] IPropertiesContainer container, [Required] string propertyName,
            [NotNull] IEnumerable<string> values, [Required] string value)
        {
            var schema = GetSchema();
            if (schema != null)
            {
                var propertyType = schema.GetPropertyType(propertyName);

                if (propertyType == null)
                {
                    propertyType = schema.AddPropertyType(propertyName, PropertyValueType.List);
                    if (propertyType is IListPropertyType listPropertyType)
                    {
                        listPropertyType.SetListProvider(new ListProvider());
                        listPropertyType.Context = values?.TagConcat();
                    }
                }

                var property = container.GetProperty(propertyType);

                if (property == null)
                {
                    container.AddProperty(propertyType, value);
                }
                else
                {
                    property.StringValue = value;
                }
            }
        }
    }
}