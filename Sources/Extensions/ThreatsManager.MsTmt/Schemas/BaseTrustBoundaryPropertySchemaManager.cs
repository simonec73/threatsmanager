using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.MsTmt.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.MsTmt.Schemas
{
    public class BaseTrustBoundaryPropertySchemaManager
    {
        private readonly IThreatModel _model;

        public BaseTrustBoundaryPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public IPropertySchema GetSchema()
        {
            IPropertySchema result;

            using (var scope = UndoRedoManager.OpenScope($"Get '{Resources.TmtTrustBoundaryPropertySchema}' schema"))
            {
                result = _model.GetSchema(Resources.TmtTrustBoundaryPropertySchema, Resources.DefaultNamespace) ??
                         _model.AddSchema(Resources.TmtTrustBoundaryPropertySchema, Resources.DefaultNamespace);
                result.AppliesTo = Scope.TrustBoundary;
                result.Priority = 90;
                result.Visible = true;
                result.System = false;
                result.AutoApply = true;
                result.Description = Resources.TrustBoundaryPropertySchemaDescription;

                scope?.Complete();
            }

            return result;
        }
    }
}