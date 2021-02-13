using System.Collections.Generic;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.MsTmt.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.MsTmt.Schemas
{
    public class TmtPropertySchemaManager
    {
        private readonly IThreatModel _model;
        private readonly string _schemaName;
        private readonly Scope _scope;

        public TmtPropertySchemaManager([NotNull] IThreatModel model,
            [NotNull] string schemaName, Scope scope)
        {
            _model = model;
            _schemaName = schemaName;
            _scope = scope;
        }

        public IPropertySchema GetSchema()
        {
            var result = _model.GetSchema(_schemaName, Resources.DefaultNamespace) ?? _model.AddSchema(_schemaName, Resources.DefaultNamespace);
            result.AppliesTo = _scope;
            result.Priority = 100;
            result.Visible = true;
            result.System = false;
            result.AutoApply = false;
            result.Description = Resources.TmtPropertySchemaDescription;

            return result;
        }

        public string GetProperty([NotNull] IPropertiesContainer container, [Required] string propertyName)
        {
            string result = null;

            var schema = _model.GetSchema(_schemaName, Resources.DefaultNamespace);
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