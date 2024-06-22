using System;
using DevComponents.DotNetBar;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Properties;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Schemas
{
    public class DiagramPropertySchemaManager
    {
        private const string LinksSchemaName = "Diagram - Links";
        private const string DiagramSchemaName = "Diagram";

        private readonly IThreatModel _model;

        public DiagramPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public IPropertySchema GetLinksSchema()
        {
            IPropertySchema result;

            using (var scope = UndoRedoManager.OpenScope($"Get '{LinksSchemaName}' schema"))
            {
                result = _model.GetSchema(LinksSchemaName, Properties.Resources.DefaultNamespace) ?? _model.AddSchema(LinksSchemaName, Properties.Resources.DefaultNamespace);
                result.AppliesTo = Scope.Link;
                result.Priority = 15;
                result.Visible = false;
                result.System = true;
                result.AutoApply = false;
                result.NotExportable = true;
                result.Description = Properties.Resources.LinksPropertySchemaDescription;

                var points = result.GetPropertyType("Points") ?? result.AddPropertyType("Points", PropertyValueType.Array);
                points.Visible = false;
                points.DoNotPrint = true;
                points.Description = Resources.PropertyPoints;

                scope?.Complete();
            }

            return result;
        }

        public IPropertyType GetLinksPropertyType()
        {
            IPropertyType result = null;

            using (var scope = UndoRedoManager.OpenScope("Get Links property type"))
            {
                var schema = GetLinksSchema();

                if (schema != null)
                {
                    result = schema.GetPropertyType("Points") ?? schema.AddPropertyType("Points", PropertyValueType.Array);
                    result.Visible = false;
                    result.DoNotPrint = true;
                    result.Description = Resources.PropertyPoints;
                    scope?.Complete();
                }
            }

            return result;
        }

        public IPropertyType GetTextLocationPropertyType()
        {
            IPropertyType result = null;

            using (var scope = UndoRedoManager.OpenScope("Get TextLocation property type"))
            {
                var schema = GetLinksSchema();

                if (schema != null)
                {
                    result = schema.GetPropertyType("TextLocation") ??
                             schema.AddPropertyType("TextLocation", PropertyValueType.Boolean);
                    result.Visible = false;
                    result.DoNotPrint = true;
                    result.Description = Resources.PropertyTextLocation;
                    scope?.Complete();
                }
            }

            return result;
        }
 
        public IPropertySchema GetSchema()
        {
            IPropertySchema result;

            using (var scope = UndoRedoManager.OpenScope($"Get '{DiagramSchemaName}' schema"))
            {
                result = _model.GetSchema(DiagramSchemaName, Resources.DefaultNamespace) ?? _model.AddSchema(DiagramSchemaName, Resources.DefaultNamespace);
                result.AppliesTo = Scope.Diagram;
                result.Priority = 15;
                result.Visible = false;
                result.System = true;
                result.AutoApply = false;
                result.Description = Resources.DiagramPropertySchemaDescription;
                scope?.Complete();
            }

            return result;
        }

        public IPropertyType GetDpiFactorPropertyType()
        {
            IPropertyType result = null;

            using (var scope = UndoRedoManager.OpenScope("Get DpiFactor property type"))
            {
                var schema = GetSchema();
                if (schema != null)
                {
                    result = schema.GetPropertyType("DpiFactor") ?? schema.AddPropertyType("DpiFactor", PropertyValueType.Decimal);
                    result.Visible = false;
                    result.DoNotPrint = true;
                    result.Description = Resources.PropertyDpiFactor;
                    scope?.Complete();
                }
            }

            return result;
        }

        public float GetDpiFactor([NotNull] IDiagram diagram)
        {
            float result = 0f;

            var propertyType = GetDpiFactorPropertyType();
            if (propertyType != null)
            {
                if (diagram.GetProperty(propertyType) is IPropertyDecimal property)
                {
                    result = Convert.ToSingle(property.Value);
                }
            }

            return result;
        }

        public void SetDpiFactor([NotNull] IDiagram diagram)
        {
            using (var scope = UndoRedoManager.OpenScope("Set DpiFactor"))
            {
                var propertyType = GetDpiFactorPropertyType();
                if (propertyType != null)
                {
                    if ((diagram.GetProperty(propertyType) ??
                         diagram.AddProperty(propertyType, null)) is IPropertyDecimal property)
                    {
                        property.Value = 1;
                        scope?.Complete();
                    }
                }
            }
        }
    }
}