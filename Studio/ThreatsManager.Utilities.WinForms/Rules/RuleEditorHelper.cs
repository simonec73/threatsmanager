using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.AdvTree;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Engine;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities.WinForms.Properties;

namespace ThreatsManager.Utilities.WinForms.Rules
{
    internal static class RuleEditorHelper
    {
        public static void AddComparisonRule(this IRuleEditor ruleEditor, 
            [NotNull] string propertyName, Scope scope = Scope.Object)
        {
            ButtonItem item = new ButtonItem()
            {
                ButtonStyle = eButtonStyle.ImageAndText,
                Image = GetImage(scope),
                ImagePosition = eImagePosition.Left,
                Text = propertyName,
                Tooltip = "Search for objects with a specific text",
                Tag = new ComparisonItemContext(scope)
            };

            ruleEditor.AddButton(item, scope);
        }
    
        public static void AddIncomingRule(this IRuleEditor ruleEditor, Scope scope = Scope.Object)
        {
            ButtonItem item = new ButtonItem()
            {
                ButtonStyle = eButtonStyle.ImageAndText,
                Image = GetImage(scope),
                ImagePosition = eImagePosition.Left,
                Text = "Incoming Flows",
                Tooltip = "One or more incoming flows from a specific entity type.\nEvaluates to False if the object is a Flow",
                Tag = new FlowsItemContext(true, scope)
            };

            ruleEditor.AddButton(item, scope);
        }

        public static void AddOutgoingRule(this IRuleEditor ruleEditor, Scope scope = Scope.Object)
        {
            ButtonItem item = new ButtonItem()
            {
                ButtonStyle = eButtonStyle.ImageAndText,
                Image = GetImage(scope),
                ImagePosition = eImagePosition.Left,
                Text = "Outgoing Flows",
                Tooltip = "One or more outgoing flows to a specific entity type.\nEvaluates to false if the object is a flow.",
                Tag = new FlowsItemContext(false, scope)
            };

            ruleEditor.AddButton(item, scope);
        }

        public static void AddCrossTrustBoundaryRule(this IRuleEditor ruleEditor, 
            [NotNull] string propertyName, Scope scope = Scope.Object)
        {
            ButtonItem item = new ButtonItem()
            {
                ButtonStyle = eButtonStyle.ImageAndText,
                Image = GetImage(scope),
                ImagePosition = eImagePosition.Left,
                Text = propertyName,
                Tooltip = propertyName,
                Tag = new CrossTrustBoundaryItemContext()
            };

            ruleEditor.AddButton(item, scope);
        }

        public static void AddEnumValueRule(this IRuleEditor ruleEditor, [NotNull] string propertyName,
            [NotNull] IEnumerable<string> names, Scope scope = Scope.Object)
        {
            ButtonItem item = new ButtonItem()
            {
                ButtonStyle = eButtonStyle.ImageAndText,
                Image = GetImage(scope),
                ImagePosition = eImagePosition.Left,
                Text = propertyName,
                Tooltip = "Select a value among the available ones.",
                Tag = new EnumValueItemContext(names, scope)
            };

            ruleEditor.AddButton(item, scope);
        }

        public static void  AddPropertyRule(this IRuleEditor ruleEditor, [NotNull] IPropertyType propertyType, 
            [NotNull] IPropertySchema schema, Scope scope = Scope.Object)
        {
            ButtonItem item = new ButtonItem()
            {
                ButtonStyle = eButtonStyle.ImageAndText,
                Image = GetImage(scope),
                ImagePosition = eImagePosition.Left,
                Text = propertyType.Name,
                Tooltip = $"Name = {propertyType.Name}\nSchema = {schema.Name}\nNamespace = {schema.Namespace}",
                Tag = new PropertyTypeItemContext(propertyType, scope)
            };

            ruleEditor.AddButton(item, scope);
        }

        public static void AddExternalInteractorTemplateRule(this IRuleEditor ruleEditor, [NotNull] IThreatModel model, Scope scope = Scope.Object)
        {
            ButtonItem item = new ButtonItem()
            {
                ButtonStyle = eButtonStyle.ImageAndText,
                Image = GetImage(scope),
                ImagePosition = eImagePosition.Left,
                Text = "External Interactor Template",
                Tooltip = "Object derived from a specific External Interactor Template.\nEvaluates to false if the object is a Flow,\na Trust Boundary or is not derived from an External Interactor Template.",
                Tag = new ExternalInteractorTemplateItemContext(model, scope)
            };

            ruleEditor.AddButton(item, scope);
        }

        public static void AddProcessTemplateRule(this IRuleEditor ruleEditor, [NotNull] IThreatModel model, Scope scope = Scope.Object)
        {
            ButtonItem item = new ButtonItem()
            {
                ButtonStyle = eButtonStyle.ImageAndText,
                Image = GetImage(scope),
                ImagePosition = eImagePosition.Left,
                Text = "Process Template",
                Tooltip = "Object derived from a specific Process Template.\nEvaluates to false if the object is a Flow,\na Trust Boundary or is not derived from a Process Template.",
                Tag = new ProcessTemplateItemContext(model, scope)
            };

            ruleEditor.AddButton(item, scope);
        }

        public static void AddDataStoreTemplateRule(this IRuleEditor ruleEditor, [NotNull] IThreatModel model, Scope scope = Scope.Object)
        {
            ButtonItem item = new ButtonItem()
            {
                ButtonStyle = eButtonStyle.ImageAndText,
                Image = GetImage(scope),
                ImagePosition = eImagePosition.Left,
                Text = "Data Store Template",
                Tooltip = "Object derived from a specific Data Store Template.\nEvaluates to false if the object is a Flow,\na Trust Boundary or is not derived from a Data Store Template.",
                Tag = new DataStoreTemplateItemContext(model, scope)
            };

            ruleEditor.AddButton(item, scope);
        }

        public static void AddFlowTemplateRule(this IRuleEditor ruleEditor, [NotNull] IThreatModel model)
        {
            ButtonItem item = new ButtonItem()
            {
                ButtonStyle = eButtonStyle.ImageAndText,
                Image = GetImage(Scope.Object),
                ImagePosition = eImagePosition.Left,
                Text = "Flow Template",
                Tooltip = "Flow derived from a specific Flow Template.\nEvaluates to false if the object is a not a Flow\nor is not derived from a Flow Template.",
                Tag = new FlowTemplateItemContext(model, Scope.Object)
            };

            ruleEditor.AddButton(item, Scope.Object);
        }

        public static void AddTrustBoundaryTemplateRule(this IRuleEditor ruleEditor, [NotNull] IThreatModel model)
        {
            ButtonItem item = new ButtonItem()
            {
                ButtonStyle = eButtonStyle.ImageAndText,
                Image = GetImage(Scope.AnyTrustBoundary),
                ImagePosition = eImagePosition.Left,
                Text = "Trust Boundary Template",
                Tooltip = "Flow crossing a Trust Boundary derived from a specific Template.\nEvaluates to false if the object is not a flow.",
                Tag = new TrustBoundaryTemplateItemContext(model, Scope.AnyTrustBoundary)
            };

            ruleEditor.AddButton(item, Scope.AnyTrustBoundary);
        }

        public static Image GetImage(Scope scope)
        {
            Image result;

            switch (scope)
            {
                case Scope.Source:
                    result = Resources.arrow_from;
                    break;
                case Scope.Target:
                    result = Resources.arrow_to;
                    break;
                case Scope.AnyTrustBoundary:
                    result = Icons.Resources.trust_boundary;
                    break;
                default:
                    result = Resources.tag;
                    break;
            }

            return result;
        }

        public static Node CreateNode([NotNull] SelectionRuleNode ruleNode, [NotNull] IThreatModel model)
        {
            var result = new Node(ruleNode.Name)
            {
                Image = GetImage(ruleNode.Scope),
            };

            if (ruleNode is NaryRuleNode nary)
            {
                if (nary is AndRuleNode)
                    result.Image = Resources.logic_and;
                else if (nary is OrRuleNode)
                    result.Image = Resources.logic_or;

                result.Tag = new NaryItemContext(ruleNode.GetType());
            }
            else if (ruleNode is UnaryRuleNode unary)
            {
                if (unary is NotRuleNode)
                    result.Image = Resources.logic_not;

                result.Tag = new NaryItemContext(ruleNode.GetType());
            }
            else if (ruleNode is CrossTrustBoundaryRuleNode crossRuleNode)
            {
                UpdateNode(result, crossRuleNode);
                result.Tag = new CrossTrustBoundaryItemContext();
            }
            else if (ruleNode is ComparisonRuleNode comparisonRuleNode)
            {
                UpdateNode(result, comparisonRuleNode);
                var propertyType = GetPropertyType(comparisonRuleNode.Namespace,
                    comparisonRuleNode.Schema, comparisonRuleNode.Name, model);
                if (propertyType == null)
                    result.Tag = new ComparisonItemContext(comparisonRuleNode.Scope);
                else
                    result.Tag = new PropertyTypeItemContext(propertyType,
                        comparisonRuleNode.Scope, PropertyTypeItemContextType.Comparison);
            }
            else if (ruleNode is EnumValueRuleNode enumValueRuleNode)
            {
                UpdateNode(result, enumValueRuleNode);
                var propertyType = GetPropertyType(enumValueRuleNode.Namespace,
                    enumValueRuleNode.Schema, enumValueRuleNode.Name, model);
                if (propertyType == null)
                    result.Tag = new EnumValueItemContext(enumValueRuleNode.Values, enumValueRuleNode.Scope);
                else
                    result.Tag = new PropertyTypeItemContext(propertyType,
                        enumValueRuleNode.Scope, PropertyTypeItemContextType.EnumValue);
            }
            else if (ruleNode is BooleanRuleNode booleanRuleNode)
            {
                UpdateNode(result, booleanRuleNode);
                var propertyType = GetPropertyType(booleanRuleNode.Namespace,
                    booleanRuleNode.Schema, booleanRuleNode.Name, model);
                if (propertyType == null)
                    result.Tag = new BooleanItemContext(booleanRuleNode.Scope);
                else
                    result.Tag = new PropertyTypeItemContext(propertyType,
                        booleanRuleNode.Scope, PropertyTypeItemContextType.Boolean);
            }
            else if (ruleNode is HasIncomingRuleNode hasIncomingRuleNode)
            {
                var flowsItemContext = new FlowsItemContext(true, hasIncomingRuleNode.Scope);
                UpdateNode(result, flowsItemContext.Values, hasIncomingRuleNode);
                result.Tag = flowsItemContext;
            }
            else if (ruleNode is HasOutgoingRuleNode hasOutgoingRuleNode)
            {
                var flowsItemContext = new FlowsItemContext(false, hasOutgoingRuleNode.Scope);
                UpdateNode(result, flowsItemContext.Values, hasOutgoingRuleNode);
                result.Tag = flowsItemContext;
            }
            else if (ruleNode is TruismRuleNode)
            {
                result.Image = Resources.ok;
                result.Tag = new TruismItemContext();
            }
            else if (ruleNode is ExternalInteractorTemplateRuleNode externalInteractorTemplateRuleNode)
            {
                var externalInteractorTemplateItemContext = new ExternalInteractorTemplateItemContext(model, externalInteractorTemplateRuleNode.Scope);
                UpdateNode(result, model.EntityTemplates?.Where(x => x.EntityType == EntityType.ExternalInteractor), externalInteractorTemplateRuleNode);
                result.Tag = externalInteractorTemplateItemContext;
            }
            else if (ruleNode is ProcessTemplateRuleNode processTemplateRuleNode)
            {
                var processTemplateContext = new ProcessTemplateItemContext(model, processTemplateRuleNode.Scope);
                UpdateNode(result, model.EntityTemplates?.Where(x => x.EntityType == EntityType.Process), processTemplateRuleNode);
                result.Tag = processTemplateContext;
            }
            else if (ruleNode is DataStoreTemplateRuleNode dataStoreTemplateRuleNode)
            {
                var dataStoreTemplateItemContext = new DataStoreTemplateItemContext(model, dataStoreTemplateRuleNode.Scope);
                UpdateNode(result, model.EntityTemplates?.Where(x => x.EntityType == EntityType.DataStore), dataStoreTemplateRuleNode);
                result.Tag = dataStoreTemplateItemContext;
            }
            else if (ruleNode is EntityTemplateRuleNode entityTemplateRuleNode)
            {
                var entityTemplate = model.GetEntityTemplate(entityTemplateRuleNode.EntityTemplate);
                switch (entityTemplate.EntityType)
                {
                    case EntityType.ExternalInteractor:
                        var eiTemplateItemContext = new ExternalInteractorTemplateItemContext(model, entityTemplateRuleNode.Scope);
                        UpdateNode(result, model.EntityTemplates?.Where(x => x.EntityType == EntityType.ExternalInteractor), entityTemplateRuleNode);
                        result.Tag = eiTemplateItemContext;
                        break;
                    case EntityType.Process:
                        var pTemplateContext = new ProcessTemplateItemContext(model, entityTemplateRuleNode.Scope);
                        UpdateNode(result, model.EntityTemplates?.Where(x => x.EntityType == EntityType.Process), entityTemplateRuleNode);
                        result.Tag = pTemplateContext;
                        break;
                    case EntityType.DataStore:
                        var dsTemplateItemContext = new DataStoreTemplateItemContext(model, entityTemplateRuleNode.Scope);
                        UpdateNode(result, model.EntityTemplates?.Where(x => x.EntityType == EntityType.DataStore), entityTemplateRuleNode);
                        result.Tag = dsTemplateItemContext;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (ruleNode is TrustBoundaryTemplateRuleNode trustBoundaryTemplateRuleNode)
            {
                var trustBoundaryContext = new TrustBoundaryTemplateItemContext(model, trustBoundaryTemplateRuleNode.Scope);
                UpdateNode(result, model.TrustBoundaryTemplates, trustBoundaryTemplateRuleNode);
                result.Tag = trustBoundaryContext;
            }
            else if (ruleNode is FlowTemplateRuleNode flowTemplateRuleNode)
            {
                var flowTemplateContext = new FlowTemplateItemContext(model, flowTemplateRuleNode.Scope);
                UpdateNode(result, model.FlowTemplates, flowTemplateRuleNode);
                result.Tag = flowTemplateContext;
            }

            return result;
        }

        private static IPropertyType GetPropertyType(string schemaNs, string schemaName, string name, [NotNull] IThreatModel model)
        {
            IPropertyType result = null;

            if (!String.IsNullOrWhiteSpace(schemaNs) &&
                !String.IsNullOrWhiteSpace(schemaName) &&
                !String.IsNullOrWhiteSpace(name))
            {
                var schema = model.GetSchema(schemaName, schemaNs);
                if (schema != null)
                {
                    result = schema.GetPropertyType(name);
                }
            }

            return result;
        }

        #region Recover Rule definition.
        public static void TraverseTree(Node currentNode,
            [NotNull] SelectionRule rule, SelectionRuleNode parentRuleNode)
        {
            SelectionRuleNode ruleNode = null;

            if (currentNode != null)
            {
                ruleNode = CreateRuleNode(currentNode);
            }

            if (parentRuleNode == null)
            {
                rule.Root = ruleNode;
            }
            else
            {
                if (parentRuleNode is NaryRuleNode nary)
                {
                    if (ruleNode != null)
                        nary.Children.Add(ruleNode);
                }
                else
                {
                    if (parentRuleNode is UnaryRuleNode unary)
                    {
                        unary.Child = ruleNode;
                    }
                }
            }

            if (currentNode?.HasChildNodes ?? false)
            {
                foreach (Node child in currentNode.Nodes)
                {
                    TraverseTree(child, rule, ruleNode);
                }
            }
        }

        public static bool IsValidRule(this Node node)
        {
            return CreateRuleNode(node) != null;
        }

        private static SelectionRuleNode CreateRuleNode(Node node)
        {
            SelectionRuleNode result = null;

            if (node != null)
            {
                var schemaNs = node.GetSchemaNamespace();
                var schema = node.GetSchemaName();
                var switchButton = node.GetSwitchButton();
                //var comboBox = node.GetComboBox();
                var textValue = node.GetValue();
                var values = node.GetComboBoxValues();
                var value = node.GetComboBoxValue();
                ComparisonOperator? comparisonOperator = node.GetComparisonOperator();

                if (node.Tag is BooleanItemContext booleanItemContext)
                {
                    if (switchButton != null)
                    {
                        result = booleanItemContext.CreateNode(node, switchButton.Value);
                    }
                }
                else if (node.Tag is ComparisonItemContext comparisonItemContext)
                {
                    if (comparisonOperator.HasValue)
                    {
                        result = comparisonItemContext.CreateNode(node, comparisonOperator.Value, textValue);
                    }
                }
                else if (node.Tag is CrossTrustBoundaryItemContext crossTrustBoundaryItemContext)
                {
                    if (switchButton != null)
                    {
                        result = crossTrustBoundaryItemContext.CreateNode(node, switchButton.Value);
                    }
                }
                else if (node.Tag is EnumValueItemContext enumValueItemContext)
                {
                    result = enumValueItemContext.CreateNode(node, values, value);
                }
                else if (node.Tag is FlowsItemContext flowsItemContext)
                {
                    result = flowsItemContext.CreateNode(node, value);
                }
                else if (node.Tag is PropertyTypeItemContext propertyTypeItemContext)
                {
                    switch (propertyTypeItemContext.ContextType)
                    {
                        case PropertyTypeItemContextType.Boolean:
                            if (switchButton != null)
                            {
                                result = propertyTypeItemContext.CreateNode(node, 
                                    propertyTypeItemContext.ContextType,
                                    schemaNs, schema, switchButton.Value);
                            }
                            break;
                        case PropertyTypeItemContextType.Comparison:
                            if (comparisonOperator.HasValue)
                            {
                                result = propertyTypeItemContext.CreateNode(node,
                                    propertyTypeItemContext.ContextType,
                                    schemaNs, schema, comparisonOperator.Value, textValue);
                            }
                            break;
                        case PropertyTypeItemContextType.EnumValue:
                            result = propertyTypeItemContext.CreateNode(node,
                                propertyTypeItemContext.ContextType,
                                schemaNs, schema, values, value);
                            break;
                    }
                }
                else if (node.Tag is ExternalInteractorTemplateItemContext externalInteractorTemplateItemContext)
                {
                    result = externalInteractorTemplateItemContext.CreateNode(node, value);
                }
                else if (node.Tag is ProcessTemplateItemContext processTemplateItemContext)
                {
                    result = processTemplateItemContext.CreateNode(node, value);
                }
                else if (node.Tag is DataStoreTemplateItemContext dataStoreTemplateItemContext)
                {
                    result = dataStoreTemplateItemContext.CreateNode(node, value);
                }
                else if (node.Tag is TrustBoundaryTemplateItemContext trustBoundaryTemplateItemContext)
                {
                    result = trustBoundaryTemplateItemContext.CreateNode(node, value);
                }
                else if (node.Tag is ButtonItemContext context)
                {
                    result = context.CreateNode(node);
                }
            }

            return result;
        }

        private static void UpdateNode([NotNull] Node node, [NotNull] ComparisonRuleNode ruleNode)
        {
            UpdateNode(node, ruleNode.Name, ruleNode.Namespace, ruleNode.Schema,
                ruleNode.Operator, ruleNode.Value);
        }

        private static void UpdateNode([NotNull] Node node, [NotNull] EnumValueRuleNode ruleNode)
        {
            UpdateNode(node, ruleNode.Name, ruleNode.Namespace, ruleNode.Schema,
                ruleNode.Values, ruleNode.Value);
        }

        private static void UpdateNode([NotNull] Node node, [NotNull] CrossTrustBoundaryRuleNode ruleNode)
        {
            UpdateNode(node, ruleNode.Name, null, null, ruleNode.Value);
        }

        private static void UpdateNode([NotNull] Node node, [NotNull] BooleanRuleNode ruleNode)
        {
            UpdateNode(node, ruleNode.Name, ruleNode.Namespace, ruleNode.Schema, ruleNode.Value);
        }

        private static void UpdateNode([NotNull] Node node, [NotNull] IEnumerable<string> values, [NotNull] HasIncomingRuleNode ruleNode)
        {
            UpdateNode(node, ruleNode.Name, null, null, values, ruleNode.EntityType.GetEnumLabel());
        }

        private static void UpdateNode([NotNull] Node node, 
            [NotNull] IEnumerable<string> values, [NotNull] HasOutgoingRuleNode ruleNode)
        {
            UpdateNode(node, ruleNode.Name, null, null, values, ruleNode.EntityType.GetEnumLabel());
        }

        private static void UpdateNode([NotNull] Node node, 
            IEnumerable<IEntityTemplate> templates, [NotNull] EntityTemplateRuleNode ruleNode)
        {
            var values = templates?.Select(x => x.Name).ToArray();
            var selected = templates?.FirstOrDefault(x => x.Id == ruleNode.EntityTemplate)?.Name;
            if ((values?.Any() ?? false) && !string.IsNullOrWhiteSpace(selected))
                UpdateNode(node, ruleNode.Name, null, null, values, selected);
        }

        private static void UpdateNode([NotNull] Node node,
            IEnumerable<ITrustBoundaryTemplate> templates, [NotNull] TrustBoundaryTemplateRuleNode ruleNode)
        {
            var values = templates?.Select(x => x.Name).ToArray();
            var selected = templates?.FirstOrDefault(x => x.Id == ruleNode.TrustBoundaryTemplate)?.Name;
            if ((values?.Any() ?? false) && !string.IsNullOrWhiteSpace(selected))
                UpdateNode(node, ruleNode.Name, null, null, values, selected, "crosses");
        }

        private static void UpdateNode([NotNull] Node node,
            IEnumerable<IFlowTemplate> templates, [NotNull] FlowTemplateRuleNode ruleNode)
        {
            var values = templates?.Select(x => x.Name).ToArray();
            var selected = templates?.FirstOrDefault(x => x.Id == ruleNode.FlowTemplate)?.Name;
            if ((values?.Any() ?? false) && !string.IsNullOrWhiteSpace(selected))
                UpdateNode(node, ruleNode.Name, null, null, values, selected);
        }

        public static void UpdateNode([NotNull] Node node, [Required] string name,
            string schemaNs, string schema, ComparisonOperator op, string value)
        {
            ComboBox editor = new ComboBox
            {
                Name = String.Concat("_comboBox", name.Replace(" ", "")),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            // ReSharper disable once CoVariantArrayConversion
            editor.Items.AddRange(Enum.GetNames(typeof(ComparisonOperator)));
            editor.SelectedItem = op.ToString();
            editor.Tag = node;

            node.Cells.Add(new Cell { Name = Resources.LabelSchemaNamespace, Text = schemaNs });
            node.Cells.Add(new Cell { Name = Resources.LabelSchemaName, Text = schema });
            node.Cells.Add(new Cell { Name = Resources.LabelOperator, HostedControl = editor });
            node.Cells.Add(new Cell { Name = Resources.LabelValue, Text = value });
        }

        public static void UpdateNode([NotNull] Node node, [Required] string name,
            string schemaNs, string schema, IEnumerable<string> names, string value, string verb = "is")
        {
            var values = names?.ToArray();
            if (values?.Any() ?? false)
            {
                ComboBox combo = new ComboBox
                {
                    Name = String.Concat("_comboBox", name.Replace(" ", "")),
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                // ReSharper disable once CoVariantArrayConversion
                combo.Items.AddRange(values);
                combo.SelectedItem = value;

                node.Cells.Add(new Cell { Name = Resources.LabelSchemaNamespace, Text = schemaNs });
                node.Cells.Add(new Cell { Name = Resources.LabelSchemaName, Text = schema });
                var cell = new Cell { Text = verb, Editable = false };
                cell.StyleNormal = new ElementStyle(Color.Black, Color.FromArgb(255, 240, 240, 240))
                {
                    TextAlignment = eStyleTextAlignment.Center
                };
                node.Cells.Add(cell);
                node.Cells.Add(new Cell { Name = Resources.LabelControl, HostedControl = combo });
            }
        }

        public static void UpdateNode([NotNull] Node node, [Required] string name,
            string schemaNs, string schema, bool value)
        {
            SwitchButton switchButton = new SwitchButton
            {
                Name = String.Concat("_switch", name.Replace(" ", "")),
                Value = value
            };

            node.Cells.Add(new Cell { Name = Resources.LabelSchemaNamespace, Text = schemaNs });
            node.Cells.Add(new Cell { Name = Resources.LabelSchemaName, Text = schema });
            var cell = new Cell { Text = "is", Editable = false };
            cell.StyleNormal = new ElementStyle(Color.Black, Color.FromArgb(255, 240, 240, 240))
            {
                TextAlignment = eStyleTextAlignment.Center
            };
            node.Cells.Add(cell);
            node.Cells.Add(new Cell { Name = Resources.LabelControl, HostedControl = switchButton });
        }

        private static string GetSchemaNamespace(this Node node)
        {
            return node?.Cells[Resources.LabelSchemaNamespace]?.Text;
        }

        private static string GetSchemaName(this Node node)
        {
            return node?.Cells[Resources.LabelSchemaName]?.Text;
        }

        private static string GetValue(this Node node)
        {
            return node?.Cells[Resources.LabelValue]?.Text;
        }

        private static Control GetControl(this Node node)
        {
            return node?.Cells[Resources.LabelControl]?.HostedControl;
        }

        private static ComboBox GetComboBox(this Node node)
        {
            return GetControl(node) as ComboBox;
        }

        private static string GetComboBoxValue(this Node node)
        {
            return node?.GetComboBox()?.SelectedItem as string;
        }

        private static IEnumerable<string> GetComboBoxValues(this Node node)
        {
            return node?.GetComboBox()?.Items.OfType<string>();
        }

        private static SwitchButton GetSwitchButton(this Node node)
        {
            return GetControl(node) as SwitchButton;
        }

        private static ComparisonOperator? GetComparisonOperator(this Node node)
        {
            ComparisonOperator? result = null;

            if (node?.Cells[Resources.LabelOperator]?.HostedControl is ComboBox comboBox &&
                Enum.TryParse<ComparisonOperator>((string) comboBox.SelectedItem, out var op))
            {
                result = op;
            }

            return result;
        }
        #endregion

        #region Cleaning the Decision Tree.
        public static void ClearNode(Node node)
        {
            if (node != null)
            {
                if (node.HasChildNodes)
                {
                    foreach (Node child in node.Nodes)
                        ClearNode(child);
                }

                if (node.Cells.Count > 0)
                {
                    foreach (Cell cell in node.Cells)
                    {
                        cell.HostedControl?.Dispose();
                    }
                }
            }
        }
        #endregion

        #region Drag & Drop management.
        public static Node CreateNode([NotNull] ButtonItem item, [NotNull] IThreatModel model)
        {
            Node node = new Node(item.Text)
            {
                Image = item.Image,
                Tag = item.Tag
            };

            if (item.Tag is ButtonItemContext context)
            {
                switch (context.Scope)
                {
                    case Scope.Object:
                        if (!(context is NaryItemContext))
                            node.Tooltip = "Applies to the Object.\nIf the property applies to a specific category of Objects,\nit evaluates to False for every other Object.";
                        break;
                    case Scope.Source:
                        node.Tooltip =
                            "If the Object is a Flow, it applies to the Source,\notherwise it evaluates to False.";
                        break;
                    case Scope.Target:
                        node.Tooltip =
                            "If the Object is a Flow, it applies to the Target,\notherwise it evaluates to False.";
                        break;
                    case Scope.AnyTrustBoundary:
                        node.Tooltip =
                            "If the Object is a Flow, it evaluates to True\nif at least a crossed Trust Boundary satisfies the condition,\notherwise it evaluates to False.";
                        break;
                }
            }

            if (item.Tag is PropertyTypeItemContext ptContext)
            {
                if (ptContext.PropertyType is IListPropertyType listPropertyType &&
                    listPropertyType.Values?.Select(x => x.Id) is IEnumerable<string> list)
                {
                    var schema = model.GetSchema(listPropertyType.SchemaId);
                    if (schema != null)
                    {
                        UpdateNode(node, listPropertyType.Name,
                            schema.Namespace, schema.Name, list, null);
                    }
                }
                else if (ptContext.PropertyType is IBoolPropertyType boolPropertyType)
                {
                    var schema = model.GetSchema(boolPropertyType.SchemaId);
                    if (schema != null)
                    {
                        UpdateNode(node, boolPropertyType.Name,
                            schema.Namespace, schema.Name, false);
                    }
                }
                else if (ptContext.PropertyType is IPropertyType propertyType)
                {
                    var schema = model.GetSchema(propertyType.SchemaId);
                    if (schema != null)
                    {
                        UpdateNode(node, propertyType.Name, schema.Namespace, schema.Name,
                            ComparisonOperator.Exact, null);
                    }
                }
            }
            else if (item.Tag is FlowsItemContext fContext)
            {
                UpdateNode(node, item.Text, null, null, fContext.Values, null, 
                    fContext.Incoming ? "from" : "to");
            }
            else if (item.Tag is TrustBoundaryTemplateItemContext tbContext)
            {
                node.Text = "Flow";
                UpdateNode(node, item.Text, null, null, tbContext.Values, null, "crosses");
            }
            else if (item.Tag is ListItemContext lContext)
            {
                UpdateNode(node, item.Text, null, null, lContext.Values, null);
            }
            else if (item.Tag is NaryItemContext)
            {

            }
            else if (item.Tag is TruismItemContext)
            {

            }
            else if (item.Tag is CrossTrustBoundaryItemContext)
            {
                UpdateNode(node, item.Text, null, null, true);
            }
            else if (item.Tag is ButtonItemContext)
            {
                UpdateNode(node, item.Text, null, null,
                    ComparisonOperator.Exact, null);
            }

            return node;
        }
        #endregion
    }
}