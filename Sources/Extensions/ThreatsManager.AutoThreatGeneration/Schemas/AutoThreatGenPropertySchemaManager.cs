using System.Runtime.Remoting.Messaging;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoThreatGeneration.Properties;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.AutoThreatGeneration.Schemas
{
    public class AutoThreatGenPropertySchemaManager
    {
        private const string SchemaName = "Automatic Threat Event Generation Extension Configuration";

        private readonly IThreatModel _model;

        public AutoThreatGenPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public IPropertySchema GetSchema()
        {
            var result = _model.GetSchema(SchemaName, Properties.Resources.DefaultNamespace);
            if (result == null)
            {
                result = _model.AddSchema(SchemaName, Properties.Resources.DefaultNamespace);
                result.AppliesTo = Scope.ThreatType;
                result.AutoApply = true;
                result.Priority = 10;
                result.Visible = false;
                result.System = true;
                result.Description = Resources.AutoThreatGenPropertySchemaDescription;
            }

            return result;
        }

        public IPropertyType GetPropertyType()
        {
            IPropertyType result = null;

            var schema = GetSchema();
            if (schema != null)
            {
                result = schema.GetPropertyType("AutoGenRule");
                if (result == null)
                {
                    result = schema.AddPropertyType("AutoGenRule", PropertyValueType.JsonSerializableObject);
                    result.Visible = false;
                    result.Description = Resources.PropertyAutoGenRule;
                }
            }

            return result;
        }
    }
}