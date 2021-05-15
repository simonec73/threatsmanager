using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Northwoods.Go;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Dialogs;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.WinForms.Dialogs;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    public partial class ModelPanel
    {
        private Dictionary<string, List<ICommandsBarDefinition>> _commandsBarContextAwareActions;

        public event Action<string, bool> ChangeCustomActionStatus;

        public string TabLabel => "Diagram";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                var result = new List<ICommandsBarDefinition>();

                if (_executionMode != ExecutionMode.Business && _executionMode != ExecutionMode.Management)
                {
                    var addList = new List<IActionDefinition>()
                    {
                        new ActionDefinition(Id, "CreateExtInteractor", "New External Interactor",
                            Resources.external_big_new,
                            Resources.external_new,
                            true, Shortcut.CtrlShiftE),
                        new ActionDefinition(Id, "CreateProcess", "New Process",
                            Resources.process_big_new,
                            Resources.process_new,
                            true, Shortcut.CtrlShiftP),
                        new ActionDefinition(Id, "CreateDataStore", "New Data Store",
                            Resources.storage_big_new,
                            Resources.storage_new,
                            true, Shortcut.CtrlShiftS),
                        new ActionDefinition(Id, "CreateTrustBoundary", "New Trust Boundary",
                            Resources.trust_boundary_big_new,
                            Resources.trust_boundary_new,
                            true, Shortcut.CtrlShiftB),
                        new ActionDefinition(Id, "CreateThreatType", "New Threat Type",
                            Resources.threat_type_big_new,
                            Resources.threat_type_new,
                            true, Shortcut.CtrlShiftT)
                    };
                    if (_commandsBarContextAwareActions?.Any(x => string.CompareOrdinal("Add", x.Key) == 0) ?? false)
                    {
                        var definitions = _commandsBarContextAwareActions["Add"];
                        List<IActionDefinition> actions = new List<IActionDefinition>();
                        foreach (var definition in definitions)
                        {
                            foreach (var command in definition.Commands)
                            {
                                actions.Add(command);
                            }
                        }

                        addList.AddRange(actions);
                        _commandsBarContextAwareActions.Remove("Add");
                    }
                    result.Add(new CommandsBarDefinition("Add", "Add", addList));

                    var layoutList = new List<IActionDefinition>()
                    {
                        new ActionDefinition(Id, "AlignH", "Align Horizontally",
                            Properties.Resources.layout_horizontal_big,
                            Properties.Resources.layout_horizontal, false),
                        new ActionDefinition(Id, "AlignV", "Align Vertically", Properties.Resources.layout_vertical_big,
                            Properties.Resources.layout_vertical, false),
                        new ActionDefinition(Id, "AlignT", "Align Top", Properties.Resources.layout_top,
                            Properties.Resources.layout_top, false),
                        new ActionDefinition(Id, "AlignB", "Align Bottom", Properties.Resources.layout_bottom_big,
                            Properties.Resources.layout_bottom, false),
                        new ActionDefinition(Id, "AlignL", "Align Left", Properties.Resources.layout_left_big,
                            Properties.Resources.layout_left, false),
                        new ActionDefinition(Id, "AlignR", "Align Right", Properties.Resources.layout_right_big,
                            Properties.Resources.layout_right, false),
                    };
                    if (_executionMode == ExecutionMode.Pioneer || _executionMode == ExecutionMode.Expert)
                    {
                        layoutList.Add(new ActionDefinition(Id, "Layout", "Automatic Layout",
                            Properties.Resources.graph_star_big,
                            Properties.Resources.graph_star));
                    }
                    if (_commandsBarContextAwareActions?.Any(x => string.CompareOrdinal("Layout", x.Key) == 0) ?? false)
                    {
                        var definitions = _commandsBarContextAwareActions["Layout"];
                        List<IActionDefinition> actions = new List<IActionDefinition>();
                        foreach (var definition in definitions)
                        {
                            foreach (var command in definition.Commands)
                            {
                                actions.Add(command);
                            }
                        }

                        layoutList.AddRange(actions);
                        _commandsBarContextAwareActions.Remove("Layout");
                    }
                    result.Add(new CommandsBarDefinition("Layout", "Layout", layoutList));
                }

                var viewList = new List<IActionDefinition>()
                {
                    new ActionDefinition(Id, "MarkerToggle", "Toggle Markers",
                        Properties.Resources.cubes_big, Properties.Resources.cubes, true)
                };
                if (_commandsBarContextAwareActions?.Any(x => string.CompareOrdinal("View", x.Key) == 0) ?? false)
                {
                    var definitions = _commandsBarContextAwareActions["View"];
                    List<IActionDefinition> actions = new List<IActionDefinition>();
                    foreach (var definition in definitions)
                    {
                        foreach (var command in definition.Commands)
                        {
                            actions.Add(command);
                        }
                    }

                    viewList.AddRange(actions);
                    _commandsBarContextAwareActions.Remove("View");
                }
                result.Add(new CommandsBarDefinition("View", "View", viewList));

                var zoomList = new List<IActionDefinition>()
                {
                    new ActionDefinition(Id, "ZoomIn", "Zoom In", Properties.Resources.zoom_in_big,
                        Properties.Resources.zoom_in),
                    new ActionDefinition(Id, "ZoomOut", "Zoom Out", Properties.Resources.zoom_out_big,
                        Properties.Resources.zoom_out),
                    new ActionDefinition(Id, "ZoomNormal", "100%", Properties.Resources.view_1_1_big,
                        Properties.Resources.view_1_1),
                    new ActionDefinition(Id, "ZoomFit", "Zoom to Fit", Properties.Resources.window_size_big,
                        Properties.Resources.window_size),
                };
                if (_commandsBarContextAwareActions?.Any(x => string.CompareOrdinal("Zoom", x.Key) == 0) ?? false)
                {
                    var definitions = _commandsBarContextAwareActions["Zoom"];
                    List<IActionDefinition> actions = new List<IActionDefinition>();
                    foreach (var definition in definitions)
                    {
                        foreach (var command in definition.Commands)
                        {
                            actions.Add(command);
                        }
                    }

                    zoomList.AddRange(actions);
                    _commandsBarContextAwareActions.Remove("Zoom");
                }
                result.Add(new CommandsBarDefinition("Zoom", "Zoom", zoomList));

                var snapshotList = new List<IActionDefinition>()
                {
                    new ActionDefinition(Id, "Clipboard", "Copy to Clipboard", Properties.Resources.clipboard_big,
                        Properties.Resources.clipboard),
                    new ActionDefinition(Id, "File", "Copy to File", Properties.Resources.floppy_disk_big,
                        Properties.Resources.floppy_disk),
                };
                if (_commandsBarContextAwareActions?.Any(x => string.CompareOrdinal("Snapshot", x.Key) == 0) ?? false)
                {
                    var definitions = _commandsBarContextAwareActions["Snapshot"];
                    List<IActionDefinition> actions = new List<IActionDefinition>();
                    foreach (var definition in definitions)
                    {
                        foreach (var command in definition.Commands)
                        {
                            actions.Add(command);
                        }
                    }

                    snapshotList.AddRange(actions);
                    _commandsBarContextAwareActions.Remove("Snapshot");
                }
                result.Add(new CommandsBarDefinition("Snapshot", "Create Snapshot", snapshotList));

                if (_executionMode != ExecutionMode.Business && _executionMode != ExecutionMode.Management)
                {
                    var removeList = new List<IActionDefinition>()
                    {
                        new ActionDefinition(Id, "RemoveDiagram", "Remove Current Diagram",
                            Resources.model_big_delete,
                            Resources.model_delete)
                    };
                    if (_commandsBarContextAwareActions?.Any(x => string.CompareOrdinal("Remove", x.Key) == 0) ?? false)
                    {
                        var definitions = _commandsBarContextAwareActions["Remove"];
                        List<IActionDefinition> actions = new List<IActionDefinition>();
                        foreach (var definition in definitions)
                        {
                            foreach (var command in definition.Commands)
                            {
                                actions.Add(command);
                            }
                        }

                        removeList.AddRange(actions);
                        _commandsBarContextAwareActions.Remove("Remove");
                    }
                    result.Add(new CommandsBarDefinition("Remove", "Remove", removeList));
                }

                var fixList = new List<IActionDefinition>()
                {
                    new ActionDefinition(Id, "FixDiagram", "Fix Current Diagram",
                        Properties.Resources.tools_big,
                        Properties.Resources.tools)
                };
                if (_commandsBarContextAwareActions?.Any(x => string.CompareOrdinal("Fix", x.Key) == 0) ?? false)
                {
                    var definitions = _commandsBarContextAwareActions["Fix"];
                    List<IActionDefinition> actions = new List<IActionDefinition>();
                    foreach (var definition in definitions)
                    {
                        foreach (var command in definition.Commands)
                        {
                            actions.Add(command);
                        }
                    }

                    fixList.AddRange(actions);
                    _commandsBarContextAwareActions.Remove("Fix");
                }
                result.Add(new CommandsBarDefinition("Fix", "Fix", fixList));

                if (_commandsBarContextAwareActions?.Any() ?? false)
                {
                    foreach (var definitions in _commandsBarContextAwareActions.Values)
                    {
                        List<IActionDefinition> actions = new List<IActionDefinition>();
                        foreach (var definition in definitions)
                        {
                            foreach (var command in definition.Commands)
                            {
                                actions.Add(command);
                            }
                        }

                        result.Add(new CommandsBarDefinition(definitions[0].Name, definitions[0].Label, actions));
                    }
                }

                return result;
            }
        }

        [InitializationRequired]
        public void ExecuteCustomAction([NotNull] IActionDefinition action)
        {
            var schemaManager = new ModelConfigPropertySchemaManager(_diagram.Model);
            var schema = schemaManager.GetSchema();
            var hpt = schema.GetPropertyType("Diagram Layout Horizontal Spacing");
            var vpt = schema.GetPropertyType("Diagram Layout Vertical Spacing");
            var h = _diagram.Model.GetProperty(hpt) as IPropertyInteger;
            var v = _diagram.Model.GetProperty(vpt) as IPropertyInteger;
            var hFloat = (float) (h?.Value ?? 150f);
            var vFloat = (float) (h?.Value ?? 100f);

            switch (action.Name)
            {
                case "CreateExtInteractor":
                    var interactor = _diagram.Model?.AddEntity<IExternalInteractor>();
                    var p1 = GetFreePoint(new PointF(hFloat, vFloat), new SizeF(100, 100), hFloat, vFloat);
                    AddShape(_diagram.AddShape(interactor, p1));
                    CheckRefresh();
                    break;
                case "CreateProcess":
                    var process = _diagram.Model?.AddEntity<IProcess>();
                    var p2 = GetFreePoint(new PointF(hFloat, vFloat), new SizeF(100, 100), hFloat, vFloat);
                    AddShape(_diagram.AddShape(process, p2));
                    CheckRefresh();
                    break;
                case "CreateDataStore":
                    var dataStore = _diagram.Model?.AddEntity<IDataStore>();
                    var p3 = GetFreePoint(new PointF(hFloat, vFloat), new SizeF(100, 100), hFloat, vFloat);
                    AddShape(_diagram.AddShape(dataStore, p3));
                    CheckRefresh();
                    break;
                case "CreateTrustBoundary":
                    var trustBoundary = _diagram.Model?.AddGroup<ITrustBoundary>();
                    var p4 = GetFreePoint(new PointF(350, 200), new SizeF(600, 300), hFloat, 200 + vFloat);
                    AddShape(_diagram.AddShape(trustBoundary, p4, new SizeF(600, 300)));
                    CheckRefresh();
                    break;
                case "CreateThreatType":
                    using (var dialog = new ThreatTypeCreationDialog(_diagram.Model))
                    {
                        dialog.ShowDialog(Form.ActiveForm);
                    }
                    break;
                case "RemoveDiagram":
                    if (MessageBox.Show(Form.ActiveForm,
                            "Are you sure you want to remove the current Diagram from the Model?",
                            "Delete Diagram", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        if (_factory.Delete(this))
                        {
                            ShowMessage("Diagram removed successfully.");
                        }
                        else
                        {
                            ShowWarning?.Invoke("Diagram removal has failed.");
                        }
                    }
                    break;
                case "FixDiagram":
                    var fixDiagram = new FixDiagram();
                    if (fixDiagram.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                    {
                        switch (fixDiagram.Issue)
                        {
                            case DiagramIssue.TooSmall:
                                HandleTooSmall();
                                break;
                            case DiagramIssue.TooLarge:
                                HandleTooLarge();
                                break;
                        }
                    }
                    break;
                case "AlignH":
                    _graph.AlignHorizontally();
                    break;
                case "AlignV":
                    _graph.AlignVertically();
                    break;
                case "AlignT":
                    _graph.AlignTops();
                    break;
                case "AlignB":
                    _graph.AlignBottoms();
                    break;
                case "AlignL":
                    _graph.AlignLeftSides();
                    break;
                case "AlignR":
                    _graph.AlignRightSides();
                    break;
                case "Layout":
                    if (MessageBox.Show("Are you sure you want to automatically layout the Diagram?",
                        "Automatic Layout confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        _graph.DoLayout(h?.Value ?? 200, v?.Value ?? 100);
                    break;
                case "MarkerToggle":
                    switch (MarkerStatusTrigger.CurrentStatus)
                    {
                        case MarkerStatus.Full:
                            MarkerStatusTrigger.RaiseMarkerStatusUpdated(MarkerStatus.Hidden);
                            break;
                        case MarkerStatus.Hidden:
                            MarkerStatusTrigger.RaiseMarkerStatusUpdated(MarkerStatus.Full);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case "ZoomIn":
                    _graph.ZoomIn();
                    break;
                case "ZoomOut":
                    _graph.ZoomOut();
                    break;
                case "ZoomNormal":
                    _graph.ZoomNormal();
                    break;
                case "ZoomFit":
                    _graph.ZoomToFit();
                    break;
                case "Clipboard":
                    _loading = true;
                    _graph.ToClipboard();
                    _loading = false;
                    break;
                case "File":
                    var dialog2 = new SaveFileDialog();
                    dialog2.CreatePrompt = false;
                    dialog2.OverwritePrompt = true;
                    dialog2.AddExtension = true;
                    dialog2.AutoUpgradeEnabled = true;
                    dialog2.CheckPathExists = true;
                    dialog2.DefaultExt = "png";
                    dialog2.Filter = "PNG (*.png)|*.png|JPEG (*.jpg)|*.jpg|GIF (*.gif)|Bitmap (*.bmp)";
                    dialog2.SupportMultiDottedExtensions = true;
                    dialog2.Title = "Export Diagram as Image";
                    dialog2.RestoreDirectory = true;
                    if (dialog2.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                    {
                        _loading = true;
                        _graph.ToFile(dialog2.FileName);
                        _loading = false;
                    }
                    break;
                default:
                    if (action.Tag is IShapesContextAwareAction shapesAction)
                    {
                        var selection = _graph.Selection.ToArray();
                        List<IShape> shapes = new List<IShape>();
                        List<ILink> links = new List<ILink>();
                        foreach (var shape in selection)
                        {
                            RecursivelyAddShapes(shapes, links, shape);
                        }

                        if (shapes.Any())
                            shapesAction.Execute(shapes, links);
                    }
                    break;
            }
        }

        private void HandleTooSmall()
        {
            var schemaManager = new DiagramPropertySchemaManager(_diagram.Model);
            var propertyType = schemaManager.GetDpiFactorPropertyType();
            if (propertyType != null)
            {
                if ((_diagram.GetProperty(propertyType) ?? 
                     _diagram.AddProperty(propertyType, null)) is IPropertyDecimal property)
                {
                    property.Value /= 2;
                    MessageBox.Show(this,
                        "The Diagram will now be closed.\nPlease open it again and apply the minor fixes which may eventually be required",
                        "Diagram fix", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ParentForm?.Close();
                }
            }
        }

        private void HandleTooLarge()
        {
            var schemaManager = new DiagramPropertySchemaManager(_diagram.Model);
            var propertyType = schemaManager.GetDpiFactorPropertyType();
            if (propertyType != null)
            {
                if ((_diagram.GetProperty(propertyType) ?? 
                     _diagram.AddProperty(propertyType, null)) is IPropertyDecimal property)
                {
                    property.Value *= 2;
                }
            }
        }

        private void RecursivelyAddShapes([NotNull] List<IShape> shapes, [NotNull] List<ILink> links, 
            [NotNull] GoObject shape, IGroup root = null)
        {
            if (shape is GraphEntity node)
                shapes.Add(node.EntityShape);
            else if (shape is GraphGroup group)
            {
                if (root == null && group.GroupShape.Identity is IGroup rootGroup)
                    root = rootGroup;

                shapes.Add(group.GroupShape);
                var entities = new List<IEntity>();
                foreach (var child in group)
                {
                    RecursivelyAddShapes(shapes, links, child, root);
                    if (child is GraphEntity graphEntity && graphEntity.EntityShape.Identity is IEntity entity)
                        entities.Add(entity);
                }

                var internalLinks = _diagram.Links?
                    .Where(x => x.DataFlow?.Source is IEntity source && x.DataFlow?.Target is IEntity target && 
                                ((entities.Contains(source) && entities.Contains(target)) ||
                                (root != null && ReferToSameParent(root, source, target))) &&
                                !links.Contains(x))
                    .ToArray();
                if (internalLinks?.Any() ?? false)
                {
                    links.AddRange(internalLinks);
                }
            } else if (shape is GraphLink glink && glink.Link is ILink link && !links.Contains(link))
            {
                links.Add(link);
            }
        }

        private bool ReferToSameParent([NotNull] IGroup parent, [NotNull] IEntity first, [NotNull] IEntity second)
        {
            return (first.Parent == parent && second.Parent != parent && CheckParentRecursively(parent, second)) ||
                   (first.Parent != parent && second.Parent == parent && CheckParentRecursively(parent, first));
        }

        private bool CheckParentRecursively([NotNull] IGroup parent, [NotNull] IGroupElement child)
        {
            bool result = false;

            if (child.Parent == parent)
                result = true;
            else if (child.Parent is IGroupElement parentGroup)
            {
                result = CheckParentRecursively(parent, parentGroup);
            }

            return result;
        }

        private PointF GetFreePoint(PointF center, SizeF size, float xSpace, float ySpace)
        {
            PointF result = center;

            var upperLeft = new PointF(center.X - (size.Width / 2f), center.Y - (size.Height / 2f));

            if (!_graph.Doc.IsUnoccupied(new RectangleF(upperLeft, new SizeF(size.Width + 20f, size.Height + 20f)), null))
            {
                result = GetFreePoint(new PointF(center.X + xSpace, center.Y + ySpace), size, xSpace, ySpace);
            }
            else
            {
                var groups = _groups.Values;
                if (groups.Any(x => x.ContainsPoint(upperLeft) || x.ContainsPoint(new PointF(center.X + (size.Width / 2f), center.Y + (size.Height / 2f)))))
                    result = GetFreePoint(new PointF(center.X + xSpace, center.Y + ySpace), size, xSpace, ySpace);
            }

            return result;
        }
    }
}