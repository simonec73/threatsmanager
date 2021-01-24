using System.Collections.Generic;
using DevComponents.AdvTree;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Engine;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Utilities.WinForms.Rules
{
    internal class PropertyTypeItemContext : ButtonItemContext
    {
        public PropertyTypeItemContext([NotNull] IPropertyType propertyType, Scope scope) : base(scope)
        {
            PropertyType = propertyType;

            if (propertyType is IArrayPropertyType)
                ContextType = PropertyTypeItemContextType.EnumValue;
            else if (propertyType is IBoolPropertyType)
                ContextType = PropertyTypeItemContextType.Boolean;
            else if (propertyType is IDecimalPropertyType)
                ContextType = PropertyTypeItemContextType.Comparison;
            else if (propertyType is IIdentityReferencePropertyType)
                ContextType = PropertyTypeItemContextType.Comparison;
            else if (propertyType is IIntegerPropertyType)
                ContextType = PropertyTypeItemContextType.Comparison;
            else if (propertyType is IJsonSerializableObjectPropertyType)
                ContextType = PropertyTypeItemContextType.Comparison;
            else if (propertyType is IListMultiPropertyType)
                ContextType = PropertyTypeItemContextType.EnumValue;
            else if (propertyType is IListPropertyType)
                ContextType = PropertyTypeItemContextType.EnumValue;
            else if (propertyType is ISingleLineStringPropertyType)
                ContextType = PropertyTypeItemContextType.Comparison;
            else if (propertyType is IStringPropertyType)
                ContextType = PropertyTypeItemContextType.Comparison;
            else if (propertyType is ITokensPropertyType)
                ContextType = PropertyTypeItemContextType.Comparison;
        }

        public PropertyTypeItemContext([NotNull] IPropertyType propertyType,
            Scope scope,
            PropertyTypeItemContextType type) : this(propertyType, scope)
        {
            ContextType = type;
        }

        public IPropertyType PropertyType { get; private set; }
        
        public PropertyTypeItemContextType ContextType { get; private set; }

        public override SelectionRuleNode CreateNode([NotNull] Node node, params object[] parameters)
        {
            SelectionRuleNode result = null;

            if (parameters != null && parameters.Length >= 4 &&
                parameters[0] is PropertyTypeItemContextType contextType &&
                parameters[1] is string schemaNs &&
                parameters[2] is string schemaName)
            {
                switch (contextType)
                {
                    case PropertyTypeItemContextType.Boolean:
                        if (parameters[3] is bool boolValue)
                            result = new BooleanRuleNode(node.Text, schemaNs, schemaName, boolValue) { Scope = Scope };
                        break;
                    case PropertyTypeItemContextType.Comparison:
                        if (parameters[3] is ComparisonOperator op &&
                            parameters[4] is string textValue)
                            result = new ComparisonRuleNode(node.Text, schemaNs, schemaName, op, textValue) { Scope = Scope };
                        break;
                    case PropertyTypeItemContextType.EnumValue:
                        if (parameters[3] is IEnumerable<string> values &&
                            parameters[4] is string value)
                            result = new EnumValueRuleNode(node.Text, schemaNs, schemaName, values, value) { Scope = Scope };
                        break;
                }
            }

            return result;
        }
    }

    internal enum PropertyTypeItemContextType
    {
        Boolean,
        Comparison,
        EnumValue
    }
}