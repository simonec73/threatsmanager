using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.MsTmt.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.MsTmt.Schemas
{
    public class BaseFlowPropertySchemaManager
    {
        private readonly IThreatModel _model;

        public BaseFlowPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public IPropertySchema GetSchema()
        {
            IPropertySchema result;

            using (var scope = UndoRedoManager.OpenScope($"Get '{Resources.TmtFlowPropertySchema}' schema"))
            {
                result = _model.GetSchema(Resources.TmtFlowPropertySchema, Resources.DefaultNamespace) ?? _model.AddSchema(Resources.TmtFlowPropertySchema, Resources.DefaultNamespace);
                result.AppliesTo = Scope.DataFlow;
                result.Priority = 90;
                result.Visible = true;
                result.System = false;
                result.AutoApply = true;
                result.Description = Resources.FlowsPropertySchemaDescription;

                scope?.Complete();
            }

            return result;
        }
    }
}