using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.MsTmt.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.MsTmt.Schemas
{
    public class ThreatsPropertySchemaManager
    {
        private const string SchemaName = "MS TMT Threats Properties";

        private readonly IThreatModel _model;

        public ThreatsPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public IPropertySchema GetSchema()
        {
            IPropertySchema result;

            using (var scope = UndoRedoManager.OpenScope($"Get '{SchemaName}' schema"))
            {
                result = _model.GetSchema(SchemaName, Resources.DefaultNamespace) ?? _model.AddSchema(SchemaName, Resources.DefaultNamespace);
                result.AppliesTo = Scope.ThreatType | Scope.ThreatEvent;
                result.Priority = 30;
                result.Visible = true;
                result.System = false;
                result.AutoApply = false;
                result.Description = Resources.ThreatsPropertySchemaDescription;
                scope?.Complete();
            }

            return result;
        }
    }
}