using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.AdvTree;
using DevComponents.DotNetBar;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Engine;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities.WinForms.Properties;

namespace ThreatsManager.Utilities.WinForms.Rules
{
    public partial class RuleEditor : UserControl, IRuleEditor
    {
        private Node _nodeSelectedByContextMenu;
        private IThreatModel _model;

        public RuleEditor()
        {
            InitializeComponent();

            _colName.Width.Absolute = (int) (200 * Dpi.Factor.Width);
            _colNamespace.Width.Absolute = (int) (125 * Dpi.Factor.Width);
            _colSchema.Width.Absolute = (int)(125 * Dpi.Factor.Width);
            _colOperator.Width.Absolute = (int)(100 * Dpi.Factor.Width);
            _colValue.Width.Absolute = (int)(150 * Dpi.Factor.Width);

            _tooltipErrorProvider.Caption = Resources.TooltipCaptionErrorDecisionTree;
            _validator.CustomErrorProvider = _tooltipErrorProvider;

            _booleanAnd.Tag = new NaryItemContext(typeof(AndRuleNode));
            _booleanOr.Tag = new NaryItemContext(typeof(OrRuleNode));
            _booleanNot.Tag = new NaryItemContext(typeof(NotRuleNode));
            _booleanTrue.Tag = new TruismItemContext();
        }

        #region Public members.
        public void Initialize([NotNull] IThreatModel model)
        {
            _model = model;

            this.AddComparisonRule("Name");
            this.AddComparisonRule("Name", Scope.Source);
            this.AddComparisonRule("Name", Scope.Target);
            this.AddComparisonRule("Description");
            this.AddComparisonRule("Description", Scope.Source);
            this.AddComparisonRule("Description", Scope.Target);
            this.AddEnumValueRule("Flow Type", EnumExtensions.GetEnumLabels<FlowType>());
            this.AddCrossTrustBoundaryRule("Crosses Trust Boundary");
            this.AddEnumValueRule("Object Type",
                new[] { "External Interactor", "Process", "Data Store", "Flow", "Threat Model" });
            this.AddEnumValueRule("Object Type",
                new[] { "External Interactor", "Process", "Data Store" },
                Scope.Source);
            this.AddEnumValueRule("Object Type",
                new[] { "External Interactor", "Process", "Data Store" },
                Scope.Target);
            this.AddExternalInteractorTemplateRule(model);
            this.AddExternalInteractorTemplateRule(model, Scope.Source);
            this.AddExternalInteractorTemplateRule(model, Scope.Target);
            this.AddProcessTemplateRule(model);
            this.AddProcessTemplateRule(model, Scope.Source);
            this.AddProcessTemplateRule(model, Scope.Target);
            this.AddDataStoreTemplateRule(model);
            this.AddDataStoreTemplateRule(model, Scope.Source);
            this.AddDataStoreTemplateRule(model, Scope.Target);
            this.AddFlowTemplateRule(model);
            this.AddTrustBoundaryTemplateRule(model);
            this.AddIncomingRule();
            this.AddIncomingRule(Scope.Source);
            this.AddIncomingRule(Scope.Target);
            this.AddOutgoingRule();
            this.AddOutgoingRule(Scope.Source);
            this.AddOutgoingRule(Scope.Target);

            var schemas = model.Schemas?
                .Where(x => x.Visible && (x.AppliesTo.HasFlag(Interfaces.Scope.ExternalInteractor) ||
                                          x.AppliesTo.HasFlag(Interfaces.Scope.Process) ||
                                          x.AppliesTo.HasFlag(Interfaces.Scope.DataStore) ||
                                          x.AppliesTo.HasFlag(Interfaces.Scope.DataFlow) ||
                                          x.AppliesTo.HasFlag(Interfaces.Scope.TrustBoundary) ||
                                          x.AppliesTo.HasFlag(Interfaces.Scope.ThreatModel))).ToArray();
            if (schemas?.Any() ?? false)
            {
                foreach (var schema in schemas)
                {
                    var flowSchema = schema.AppliesTo.HasFlag(Interfaces.Scope.DataFlow);
                    var entitySchema = schema.AppliesTo.HasFlag(Interfaces.Scope.ExternalInteractor) ||
                                       schema.AppliesTo.HasFlag(Interfaces.Scope.Process) ||
                                       schema.AppliesTo.HasFlag(Interfaces.Scope.DataStore);
                    var trustBoundarySchema = schema.AppliesTo.HasFlag(Interfaces.Scope.TrustBoundary);
                    var threatModelSchema = schema.AppliesTo.HasFlag(Interfaces.Scope.ThreatModel);
                    var properties = schema.PropertyTypes?.Where(x => x.Visible).ToArray();
                    if (properties?.Any() ?? false)
                    {
                        foreach (var property in properties)
                        {
                            if (flowSchema || entitySchema || threatModelSchema)
                            {
                                this.AddPropertyRule(property, schema);
                                if (entitySchema)
                                {
                                    this.AddPropertyRule(property, schema, Scope.Source);
                                    this.AddPropertyRule(property, schema, Scope.Target);
                                }
                            }

                            if (trustBoundarySchema)
                            {
                                this.AddPropertyRule(property, schema, Scope.AnyTrustBoundary);
                            }
                        }
                    }
                }
            }
        }

        public SelectionRule Rule
        {
            get
            {
                var result = new SelectionRule();

                RuleEditorHelper.TraverseTree(_decisionTree.DisplayRootNode, result, null);

                return result;
            }

            set
            {
                Clear();

                if (value?.Root != null)
                {
                    AppendNode(value.Root, null);
                    _decisionTree.ExpandAll();
                }
            }
        }
        #endregion

        #region Private member functions: Initialization.
        public void AddButton([NotNull] ButtonItem button, Scope scope)
        {
            switch (scope)
            {
                case Scope.Source:
                    _panelPropertiesSource.SubItems.Add(button);
                    break;
                case Scope.Target:
                    _panelPropertiesTarget.SubItems.Add(button);
                    break;
                case Scope.AnyTrustBoundary:
                    _panelPropertiesAnyTrustBoundary.SubItems.Add(button);
                    break;
                default:
                    _panelProperties.SubItems.Add(button);
                    break;
            }
        }

        private void AppendNode(SelectionRuleNode ruleNode, Node parent)
        {
            if (ruleNode != null)
            {
                Node node = RuleEditorHelper.CreateNode(ruleNode, _model);

                if (parent != null)
                    parent.Nodes.Add(node);
                else
                {
                    _decisionTree.Nodes.Add(node);
                    _decisionTree.DisplayRootNode = node;
                }

                if (ruleNode is NaryRuleNode nary)
                {
                    if (nary.Children.Count > 0)
                    {
                        foreach (SelectionRuleNode item in nary.Children)
                        {
                            AppendNode(item, node);
                        }
                    }
                }
                else if (ruleNode is UnaryRuleNode unary && unary.Child != null)
                {
                    AppendNode(unary.Child, node);
                }
            }
        }
        #endregion

        #region Private member functions: Cleaning the Decision Tree.
        private void Clear()
        {
            RuleEditorHelper.ClearNode(_decisionTree.DisplayRootNode);
            _decisionTree.DisplayRootNode = null;
            _decisionTree.Nodes.Clear();
        }
        #endregion

        #region Private member functions: Drag & Drop management.
        private void _decisionTree_NodeDragFeedback(object sender, TreeDragFeedbackEventArgs e)
        {
            // Get mouse position relative to tree control
            Point mousePos = _decisionTree.PointToClient(Control.MousePosition);

            // Get node mouse is over
            Node mouseOverNode = _decisionTree.GetNodeAt(mousePos.Y);

            if (mouseOverNode == null && _decisionTree.Nodes.Count > 0)
                e.AllowDrop = false;
            else if (mouseOverNode != null)
            {
                if (e.ParentNode?.Tag is NaryItemContext naryItemContext)
                {
                    if (naryItemContext.BooleanType == typeof(NotRuleNode) &&
                        e.ParentNode.HasChildNodes)
                    {
                        e.AllowDrop = false;
                    }
                }
                else
                {
                    e.AllowDrop = false;
                }
            }
        }

        private void _decisionTree_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(Node)) is Node)
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                ButtonItem item = (ButtonItem)e.Data.GetData(typeof(ButtonItem));
                if (item == null)
                    e.Effect = DragDropEffects.None;
                else
                {
                    var node = RuleEditorHelper.CreateNode(item, _model);
                    e.Data.SetData(typeof(Node), node);
                    e.Effect = DragDropEffects.Copy;
                    _validator.ClearFailedValidations();
                }
            }
        }

        private void _decisionTree_BeforeNodeDrop(object sender, TreeDragDropEventArgs e)
        {
            if (e.NewParentNode != null)
            {
                if (e.NewParentNode.Tag is NaryItemContext context)
                {
                    if (context.BooleanType == null)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        if (typeof(UnaryRuleNode).IsAssignableFrom(context.BooleanType) &&
                            e.NewParentNode.HasChildNodes)
                            e.Cancel = true;
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                if (_decisionTree.Nodes.Count > 0)
                    e.Cancel = true;
                else
                    _decisionTree.DisplayRootNode = e.Node;
            }
        }

        private void _decisionTree_AfterNodeDrop(object sender, TreeDragDropEventArgs e)
        {
            CompleteNode(e.Node);
        }

        private void CompleteNode([NotNull] Node node)
        {
            foreach (Cell cell in node.Cells)
            {
                if (cell.HostedControl != null)
                {
                    var control = cell.HostedControl;
                    cell.HostedControl = control;

                }
            }

            var children = node.Nodes.OfType<Node>().ToArray();
            if (children.Any())
            {
                foreach (var child in children)
                {
                    CompleteNode(child);
                }
            }
        }
        #endregion

        #region Private member functions: Context menu for the Decision Tree.
        private void _contextDecisionTree_Opening(object sender, CancelEventArgs e)
        {
            Point clientPos = _decisionTree.PointToClient(MousePosition);
            _nodeSelectedByContextMenu = _decisionTree.GetNodeAt(clientPos.X, clientPos.Y);

            if (_nodeSelectedByContextMenu == null)
            {
                _contextDeleteNode.Enabled = false;
                _contextChangeNodeType.Enabled = false;
            }
            else
            {
                _contextDeleteNode.Enabled = true;
                if (_nodeSelectedByContextMenu.Tag is NaryItemContext context)
                {
                    _contextChangeNodeType.Enabled =
                        (context.BooleanType == typeof(AndRuleNode)) ||
                        (context.BooleanType == typeof(OrRuleNode));
                }
                else
                {
                    _contextChangeNodeType.Enabled = false;
                }
            }
        }

        private void _contextClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, Resources.AskClearDecisionTree,
                Resources.CaptionConfirmChanges, MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Clear();
                _nodeSelectedByContextMenu = null;
            }
        }

        private void _contextChangeNodeType_Click(object sender, EventArgs e)
        {
            if (_nodeSelectedByContextMenu.Tag is NaryItemContext context)
            {
                if (context.BooleanType == typeof(AndRuleNode))
                {
                    _nodeSelectedByContextMenu.Name = Resources.LabelOr;
                    _nodeSelectedByContextMenu.Text = Resources.LabelOr;
                    context.BooleanType = typeof(OrRuleNode);
                    _nodeSelectedByContextMenu.Image = Resources.logic_or;
                }
                else if (context.BooleanType == typeof(OrRuleNode))
                {
                    _nodeSelectedByContextMenu.Name = Resources.LabelAnd;
                    _nodeSelectedByContextMenu.Text = Resources.LabelAnd;
                    context.BooleanType = typeof(AndRuleNode);
                    _nodeSelectedByContextMenu.Image = Resources.logic_and;
                }
            }
        }

        private void _contextDeleteNode_Click(object sender, EventArgs e)
        {
            if (_nodeSelectedByContextMenu != null)
            {
                if (MessageBox.Show(this,
                    string.Format(Resources.AskDeleteNode, _nodeSelectedByContextMenu.Text),
                    Resources.CaptionConfirmChanges, MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (_nodeSelectedByContextMenu.Parent != null)
                        _nodeSelectedByContextMenu.Parent.Nodes.Remove(_nodeSelectedByContextMenu);
                    else
                    {
                        _decisionTree.Nodes.Remove(_nodeSelectedByContextMenu);
                        _decisionTree.DisplayRootNode = null;
                    }
                    _nodeSelectedByContextMenu = null;
                }
            }
        }
        #endregion

        #region Private member functions: Validation.
        public bool ValidateRule()
        {
            return _validator.Validate();
        }

        private void _validator_CustomValidatorValidateValue(object sender, DevComponents.DotNetBar.Validator.ValidateValueEventArgs e)
        {
            e.IsValid = true;
            _decisionTreeValidator.ErrorMessage = Resources.ErrorGenericInvalidTree;

            if (e.ControlToValidate == _decisionTree)
            {
                if (_decisionTree.DisplayRootNode != null)
                {
                    e.IsValid = Validate(_decisionTree.DisplayRootNode);
                }
            }
        }

        private bool Validate(Node node)
        {
            bool result = true;

            if (node != null)
            {
                if (string.IsNullOrWhiteSpace(node.Text))
                {
                    _decisionTreeValidator.ErrorMessage = Resources.ErrorNodeWithoutText;
                    result = false;
                }
                else
                {
                    if (node.Tag is NaryItemContext naryItemContext)
                    {
                        if ((naryItemContext.BooleanType == typeof(AndRuleNode)) || 
                            (naryItemContext.BooleanType == typeof(OrRuleNode)))
                        {
                            if (node.Nodes.Count <= 1)
                            {
                                _decisionTreeValidator.ErrorMessage =
                                    string.Format(Resources.ErrorMissingChildren, node.Text);
                                result = false;
                            }
                            else
                            {
                                foreach (Node child in node.Nodes)
                                {
                                    if (!Validate(child))
                                    {
                                        result = false;
                                        break;
                                    }
                                }
                            }
                        }
                        else if (naryItemContext.BooleanType == typeof(NotRuleNode))
                        {
                            if (node.Nodes.Count != 1)
                            {
                                _decisionTreeValidator.ErrorMessage =
                                    string.Format(Resources.ErrorMissingChild, node.Text);
                                result = false;
                            }
                            else
                            {
                                result = Validate(node.Nodes[0]);
                            }
                        }
                        else
                        {
                            _decisionTreeValidator.ErrorMessage =
                                string.Format(Resources.ErrorUnknownNodeType, node.Text);
                            result = false;
                        }
                    }
                    else if (!node.IsValidRule())
                    {
                        _decisionTreeValidator.ErrorMessage =
                            string.Format(Resources.ErrorInvalidNode, node.Text);
                        result = false;
                    }
                }
            }

            return result;
        }
        #endregion

        private void _panel_ExpandChange(object sender, EventArgs e)
        {
            if (sender is SideBarPanelItem panel)
            {
                panel.ForeColor = panel.Expanded ? Color.White : Color.Black;
            }
        }
    }
}
