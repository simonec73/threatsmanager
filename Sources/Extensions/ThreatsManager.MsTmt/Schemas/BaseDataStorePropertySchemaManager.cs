using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.MsTmt.Properties;

namespace ThreatsManager.MsTmt.Schemas
{
    public class BaseDataStorePropertySchemaManager
    {
        private readonly IThreatModel _model;

        public BaseDataStorePropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public IPropertySchema GetSchema()
        {
            var result = _model.GetSchema(Resources.TmtDataStorePropertySchema, Resources.DefaultNamespace) ?? _model.AddSchema(Resources.TmtDataStorePropertySchema, Resources.DefaultNamespace);
            result.AppliesTo = Scope.DataStore;
            result.Priority = 90;
            result.Visible = true;
            result.System = false;
            result.AutoApply = true;
            result.Description = Resources.EntityPropertySchemaDescription;

            return result;
        }
    }
}