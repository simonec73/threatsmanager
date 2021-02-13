using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Panels.PropertySchemaList;
using ThreatsManager.Extensions.Properties;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Schemas
{
    public class EffortPropertySchemaManager
    {
        private const string SchemaName = "Effort";

        private readonly IThreatModel _model;

        public EffortPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public bool IsEffortSupported => _model.Schemas?
            .Any(x => string.CompareOrdinal(x.Name, SchemaName) == 0 &&
                      string.CompareOrdinal(x.Namespace,
                          Properties.Resources.DefaultNamespace) == 0) ?? false;

        public bool AddSupport()
        {
            var result = false;

            if (!IsEffortSupported)
            {
                var schema = GetSchema();
                _model.ApplySchema(schema.Id);
                result = true;
            }

            return result;
        }

        public bool RemoveSupport()
        {
            var result = false;

            if (IsEffortSupported)
            {
                var schema = GetSchema();
                result = _model.RemoveSchema(schema.Id, true);
            }

            return result;
        }

        public IPropertySchema GetSchema()
        {
            var result = _model.GetSchema(SchemaName, Properties.Resources.DefaultNamespace) ?? _model.AddSchema(SchemaName, Properties.Resources.DefaultNamespace);
            result.AppliesTo = Scope.Mitigation | Scope.ThreatEventMitigation;
            result.AutoApply = true;
            result.Priority = 50;
            result.Visible = true;
            result.System = true;
            result.NotExportable = true;
            result.Description = Properties.Resources.EffortPropertySchemaDescription;

            var effort = result.GetPropertyType("Effort");
            if (effort == null)
            {
                effort = result.AddPropertyType("Effort", PropertyValueType.List);
                if (effort is IListPropertyType listPropertyType)
                {
                    listPropertyType.SetListProvider(new ListProvider());
                    listPropertyType.Context = EnumExtensions.GetEnumLabels<Effort>().TagConcat();
                }
            }
            effort.Visible = true;
            effort.Description = Resources.PropertyEffort;

            return result;
        }

        public IPropertyType GetPropertyType()
        {
            var schema = GetSchema();

            return schema?.GetPropertyType("Effort");
        }
    }
}