using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.AutoGenRules.Engine
{
    public static class RuleHelper
    {
        public static IEnumerable<IPropertySchema> Traverse(this IThreatModel model, SelectionRuleNode node)
        {
            List<IPropertySchema> list = new List<IPropertySchema>();

            if (node is BooleanRuleNode booleanRuleNode)
            {
                if (!string.IsNullOrWhiteSpace(booleanRuleNode.Namespace) &&
                    !string.IsNullOrWhiteSpace(booleanRuleNode.Schema))
                {
                    var schema = model.GetSchema(booleanRuleNode.Schema, booleanRuleNode.Namespace);
                    if (schema != null && !list.Contains(schema))
                        list.Add(schema);
                }
            }
            else if (node is ComparisonRuleNode comparisonRuleNode)
            {
                if (!string.IsNullOrWhiteSpace(comparisonRuleNode.Namespace) &&
                    !string.IsNullOrWhiteSpace(comparisonRuleNode.Schema))
                {
                    var schema = model.GetSchema(comparisonRuleNode.Schema, comparisonRuleNode.Namespace);
                    if (schema != null && !list.Contains(schema))
                        list.Add(schema);
                }
            }
            else if (node is EnumValueRuleNode enumValueRuleNode)
            {
                if (!string.IsNullOrWhiteSpace(enumValueRuleNode.Namespace) &&
                    !string.IsNullOrWhiteSpace(enumValueRuleNode.Schema))
                {
                    var schema = model.GetSchema(enumValueRuleNode.Schema, enumValueRuleNode.Namespace);
                    if (schema != null && !list.Contains(schema))
                        list.Add(schema);
                }
            }
            else if (node is NaryRuleNode naryRuleNode)
            {
                var children = naryRuleNode.Children?.ToArray();
                if (children?.Any() ?? false)
                {
                    foreach (var child in children)
                    {
                        var childItems = Traverse(model, child)?.ToArray();
                        if (childItems?.Any() ?? false)
                        {
                            foreach (var childItem in childItems)
                            {
                                if (childItem != null && !list.Contains(childItem))
                                    list.Add(childItem);
                            }
                        }
                    }
                }
            }
            else if (node is UnaryRuleNode unaryRuleNode)
            {
                var childItems = Traverse(model, unaryRuleNode.Child);
                if (childItems?.Any() ?? false)
                {
                    foreach (var childItem in childItems)
                    {
                        if (childItem != null && !list.Contains(childItem))
                            list.Add(childItem);
                    }
                }
            }

            return list.Any() ? list : null;
        }

        public static bool HasSchema(this SelectionRuleNode node,
            [Required] string schemaName, [Required] string schemaNamespace)
        {
            bool result = false;

            if (node is BooleanRuleNode booleanRuleNode)
            {
                if (string.CompareOrdinal(booleanRuleNode.Namespace, schemaNamespace) == 0 &&
                    string.CompareOrdinal(booleanRuleNode.Schema, schemaName) == 0)
                {
                    result = true;
                }
            }
            else if (node is ComparisonRuleNode comparisonRuleNode)
            {
                if (string.CompareOrdinal(comparisonRuleNode.Namespace, schemaNamespace) == 0 &&
                    string.CompareOrdinal(comparisonRuleNode.Schema, schemaName) == 0)
                {
                    result = true;
                }
            }
            else if (node is EnumValueRuleNode enumValueRuleNode)
            {
                if (string.CompareOrdinal(enumValueRuleNode.Namespace, schemaNamespace) == 0 &&
                    string.CompareOrdinal(enumValueRuleNode.Schema, schemaName) == 0)
                {
                    result = true;
                }
            }
            else if (node is NaryRuleNode naryRuleNode)
            {
                var children = naryRuleNode.Children?.ToArray();
                if (children?.Any() ?? false)
                {
                    foreach (var child in children)
                    {
                        result |= HasSchema(child, schemaName, schemaNamespace);
                    }
                }
            }
            else if (node is UnaryRuleNode unaryRuleNode)
            {
                result = HasSchema(unaryRuleNode.Child, schemaName, schemaNamespace);
            }

            return result;
        }

        public static bool UpdateSchema(this SelectionRuleNode node,
            [Required] string oldName, [Required] string oldNamespace, 
            [Required] string newName, [Required] string newNamespace)
        {
            bool result = false;

            if (node is BooleanRuleNode booleanRuleNode)
            {
                if (string.CompareOrdinal(booleanRuleNode.Namespace, oldNamespace) == 0 &&
                    string.CompareOrdinal(booleanRuleNode.Schema, oldName) == 0)
                {
                    booleanRuleNode.Namespace = newNamespace;
                    booleanRuleNode.Schema = newName;
                    result = true;
                }
            }
            else if (node is ComparisonRuleNode comparisonRuleNode)
            {
                if (string.CompareOrdinal(comparisonRuleNode.Namespace, oldNamespace) == 0 &&
                    string.CompareOrdinal(comparisonRuleNode.Schema, oldName) == 0)
                {
                    comparisonRuleNode.Namespace = newNamespace;
                    comparisonRuleNode.Schema = newName;
                    result = true;
                }
            }
            else if (node is EnumValueRuleNode enumValueRuleNode)
            {
                if (string.CompareOrdinal(enumValueRuleNode.Namespace, oldNamespace) == 0 &&
                    string.CompareOrdinal(enumValueRuleNode.Schema, oldName) == 0)
                {
                    enumValueRuleNode.Namespace = newNamespace;
                    enumValueRuleNode.Schema = newName;
                    result = true;
                }
            }
            else if (node is NaryRuleNode naryRuleNode)
            {
                var children = naryRuleNode.Children?.ToArray();
                if (children?.Any() ?? false)
                {
                    foreach (var child in children)
                    {
                        result |= UpdateSchema(child, oldName, oldNamespace, newName, newNamespace);
                    }
                }
            }
            else if (node is UnaryRuleNode unaryRuleNode)
            {
                result = UpdateSchema(unaryRuleNode.Child, oldName, oldNamespace, newName, newNamespace);
            }

            return result;
        }

        public static bool HasPropertyType(this SelectionRuleNode node,
            [Required] string schemaName, [Required] string schemaNamespace, [Required] string propertyName)
        {
            bool result = false;

            if (node is BooleanRuleNode booleanRuleNode)
            {
                if (string.CompareOrdinal(booleanRuleNode.Namespace, schemaNamespace) == 0 &&
                    string.CompareOrdinal(booleanRuleNode.Schema, schemaName) == 0 &&
                    string.CompareOrdinal(booleanRuleNode.Name, propertyName) == 0)
                {
                    result = true;
                }
            }
            else if (node is ComparisonRuleNode comparisonRuleNode)
            {
                if (string.CompareOrdinal(comparisonRuleNode.Namespace, schemaNamespace) == 0 &&
                    string.CompareOrdinal(comparisonRuleNode.Schema, schemaName) == 0 &&
                    string.CompareOrdinal(comparisonRuleNode.Name, propertyName) == 0)
                {
                    result = true;
                }
            }
            else if (node is EnumValueRuleNode enumValueRuleNode)
            {
                if (string.CompareOrdinal(enumValueRuleNode.Namespace, schemaNamespace) == 0 &&
                    string.CompareOrdinal(enumValueRuleNode.Schema, schemaName) == 0 &&
                    string.CompareOrdinal(enumValueRuleNode.Name, propertyName) == 0)
                {
                    result = true;
                }
            }
            else if (node is NaryRuleNode naryRuleNode)
            {
                var children = naryRuleNode.Children?.ToArray();
                if (children?.Any() ?? false)
                {
                    foreach (var child in children)
                    {
                        result |= HasPropertyType(child, schemaName, schemaNamespace, propertyName);
                    }
                }
            }
            else if (node is UnaryRuleNode unaryRuleNode)
            {
                result = HasPropertyType(unaryRuleNode.Child, schemaName, schemaNamespace, propertyName);
            }

            return result;
        }

        public static bool UpdatePropertyType(this SelectionRuleNode node,
            [Required] string schemaName, [Required] string schemaNamespace, 
            [Required] string oldPropertyTypeName, [Required] string newPropertyTypeName)
        {
            bool result = false;

            if (node is BooleanRuleNode booleanRuleNode)
            {
                if (string.CompareOrdinal(booleanRuleNode.Namespace, schemaNamespace) == 0 &&
                    string.CompareOrdinal(booleanRuleNode.Schema, schemaName) == 0 &&
                    string.CompareOrdinal(booleanRuleNode.Name, oldPropertyTypeName) == 0)
                {
                    booleanRuleNode.Name = newPropertyTypeName;
                    result = true;
                }
            }
            else if (node is ComparisonRuleNode comparisonRuleNode)
            {
                if (string.CompareOrdinal(comparisonRuleNode.Namespace, schemaNamespace) == 0 &&
                    string.CompareOrdinal(comparisonRuleNode.Schema, schemaName) == 0 &&
                    string.CompareOrdinal(comparisonRuleNode.Name, oldPropertyTypeName) == 0)
                {
                    comparisonRuleNode.Name = newPropertyTypeName;
                    result = true;
                }
            }
            else if (node is EnumValueRuleNode enumValueRuleNode)
            {
                if (string.CompareOrdinal(enumValueRuleNode.Namespace, schemaNamespace) == 0 &&
                    string.CompareOrdinal(enumValueRuleNode.Schema, schemaName) == 0 &&
                    string.CompareOrdinal(enumValueRuleNode.Name, oldPropertyTypeName) == 0)
                {
                    enumValueRuleNode.Name = newPropertyTypeName;
                    result = true;
                }
            }
            else if (node is NaryRuleNode naryRuleNode)
            {
                var children = naryRuleNode.Children?.ToArray();
                if (children?.Any() ?? false)
                {
                    foreach (var child in children)
                    {
                        result |= UpdatePropertyType(child, 
                            schemaName, schemaNamespace, oldPropertyTypeName, newPropertyTypeName);
                    }
                }
            }
            else if (node is UnaryRuleNode unaryRuleNode)
            {
                result = UpdatePropertyType(unaryRuleNode.Child, 
                    schemaName, schemaNamespace, oldPropertyTypeName, newPropertyTypeName);
            }

            return result;
        }
    }
}
