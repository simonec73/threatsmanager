using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Extensions.Reporting
{
    public static class ReportingUtils
    {
        public static IEnumerable<ItemRow> GetItemRows(this IPropertiesContainer container, IPropertyType except = null)
        {
            IEnumerable<ItemRow> result = null;

            if (container is IThreatModelChild child && child.Model != null)
            {
                var model = child.Model;

                var schemas = GetSchemas(model, container.Properties)?.ToArray();

                if (schemas?.Any() ?? false)
                {
                    var list = new List<ItemRow>();

                    foreach (var schema in schemas)
                    {
                        var itemRows = container.Properties?
                            .Where(x => !(x.PropertyType?.DoNotPrint ?? false) &&
                                        x.PropertyType.SchemaId == schema.Id &&
                                        x.PropertyType != except)
                            .OrderBy(x => x.PropertyType?.Priority ?? 0)
                            .Select(x => ItemRow.Create(container, x))
                            .ToArray();

                        if (itemRows?.Any() ?? false)
                            list.AddRange(itemRows);
                    }

                    if (list.Any())
                        result = list;
                }
            }

            return result;
        }

        private static IEnumerable<IPropertySchema> GetSchemas([NotNull] IThreatModel model, IEnumerable<IProperty> properties)
        {
            IEnumerable<IPropertySchema> result = null;

            if (properties?.Any() ?? false)
            {
                List<IPropertySchema> list = new List<IPropertySchema>();
                foreach (var property in properties)
                {
                    var propertyType = model.GetPropertyType(property.PropertyTypeId);
                    if (propertyType != null)
                    {
                        var schema = model.GetSchema(propertyType.SchemaId);
                        if (schema != null && !list.Contains(schema))
                        {
                            list.Add(schema);
                        }
                    }
                }

                if (list.Count > 0)
                    result = list.Where(x => x.Visible)
                        .OrderByDescending(x => x.Priority)
                        .ThenByDescending(x => x.Name);
            }

            return result;
        }
    }
}
