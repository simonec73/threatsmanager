using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.MsTmt.Properties;

namespace ThreatsManager.MsTmt.Schemas
{
    public class BaseExternalInteractorPropertySchemaManager
    {
        private readonly IThreatModel _model;

        public BaseExternalInteractorPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public IPropertySchema GetSchema()
        {
            var result = _model.GetSchema(Resources.TmtExternalInteractorPropertySchema, Resources.DefaultNamespace) ??
                         _model.AddSchema(Resources.TmtExternalInteractorPropertySchema, Resources.DefaultNamespace);
            result.AppliesTo = Scope.ExternalInteractor;
            result.Priority = 90;
            result.Visible = true;
            result.System = false;
            result.AutoApply = true;
            result.Description = Resources.EntityPropertySchemaDescription;

            return result;
        }
    }
}