using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.MsTmt.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.MsTmt.Schemas
{
    public class BaseProcessPropertySchemaManager
    {
        private readonly IThreatModel _model;

        public BaseProcessPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public IPropertySchema GetSchema()
        {
            IPropertySchema result;

            using (var scope = UndoRedoManager.OpenScope($"Get '{Resources.TmtProcessPropertySchema}' schema"))
            {
                result = _model.GetSchema(Resources.TmtProcessPropertySchema, Resources.DefaultNamespace) ?? _model.AddSchema(Resources.TmtProcessPropertySchema, Resources.DefaultNamespace);
                result.AppliesTo = Scope.Process;
                result.Priority = 90;
                result.Visible = true;
                result.System = false;
                result.AutoApply = true;
                result.Description = Resources.EntityPropertySchemaDescription;

                scope?.Complete();
            }

            return result;
        }
    }
}