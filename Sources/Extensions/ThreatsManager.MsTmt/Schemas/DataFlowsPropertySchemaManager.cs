using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.MsTmt.Properties;

namespace ThreatsManager.MsTmt.Schemas
{
    public class DataFlowsPropertySchemaManager
    {
        private const string SchemaName = "MS TMT Flows Properties";
        private const string ThreatModelDataFlowId = "MSTMT_ThreatModel_DataFlowId";

        private readonly IThreatModel _model;

        public DataFlowsPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public IPropertySchema GetSchema()
        {
            var result = _model.GetSchema(SchemaName, Resources.DefaultNamespace);
            if (result == null)
            {
                result = _model.AddSchema(SchemaName, Resources.DefaultNamespace);
                result.AppliesTo = Scope.DataFlow;
                result.Priority = 30;
                result.Visible = true;
                result.System = true;
                result.AutoApply = false;
                result.Description = Resources.DataFlowsPropertySchemaDescription;
            }

            var id = result.GetPropertyType(ThreatModelDataFlowId);
            if (id == null)
            {
                id = result.AddPropertyType(ThreatModelDataFlowId, PropertyValueType.String);
                id.Visible = false;
                id.Description = Resources.ThreatModelDataFlowIdDescription;
            }

            return result;
        }

        public string GetMsTmtDataFlowId([NotNull] IDataFlow dataFlow)
        {
            var schema = GetSchema();
            var propertyType = schema.GetPropertyType(ThreatModelDataFlowId);
            var property = dataFlow.GetProperty(propertyType);

            return property?.StringValue;
        }

        public void SetMsTmtDataFlowId([NotNull] IDataFlow dataFlow, [Required] string value)
        {
            var schema = GetSchema();
            var propertyType = schema.GetPropertyType(ThreatModelDataFlowId);
            var property = dataFlow.GetProperty(propertyType);

            if (property == null)
            {
                property = dataFlow.AddProperty(propertyType, value);
            }
            else
            {
                property.StringValue = value;
            }

        }
    }
}