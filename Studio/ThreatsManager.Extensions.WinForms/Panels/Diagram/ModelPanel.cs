using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using Northwoods.Go;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Threading;
using ThreatsManager.Extensions.Dialogs;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    public partial class ModelPanel : UserControl, IShowDiagramPanel<Form>, ICustomRibbonExtension, 
        IInitializableObject, IContextAwareExtension, IDesktopAlertAwareExtension, IPanelOpenerExtension, 
        IExecutionModeSupport
    {
        #region Private member variables.
        private bool _loading;
        private IDiagram _diagram;
        private ModelPanelFactory _factory;
        private readonly Dictionary<Guid, GraphEntity> _entities = new Dictionary<Guid, GraphEntity>();
        private readonly Dictionary<Guid, GraphGroup> _groups = new Dictionary<Guid, GraphGroup>();
        private readonly Dictionary<Guid, GraphLink> _links = new Dictionary<Guid, GraphLink>();
        private ExecutionMode _executionMode;
        #endregion

        public ModelPanel()
        {
            InitializeComponent();

            _properties.OpenDiagram += OpenDiagram;

            _existingToolsPanel.SuspendLayout();
            _existingTypes.Height = (int) (20 * Dpi.Factor.Height);
            _existingTypes.ItemHeight = (int) (20 * Dpi.Factor.Height);
            _existingToolsPanel.ResumeLayout();
            _templateToolsPanel.SuspendLayout();
            _templateTypes.Height = (int) (20 * Dpi.Factor.Height);
            _templateTypes.ItemHeight = (int) (20 * Dpi.Factor.Height);
            _templateToolsPanel.ResumeLayout();

            if (Dpi.Factor.Height >= 1.5)
            {
                _existingExternalInteractor.Image = Resources.external;
                _existingProcess.Image = Resources.process;
                _existingDataStore.Image = Resources.storage;
                _existingTrustBoundary.Image = Resources.trust_boundary;
                _templateExternalInteractor.Image = Resources.external;
                _templateProcess.Image = Resources.process;
                _templateDataStore.Image = Resources.storage;
                _templateTrustBoundary.Image = Resources.trust_boundary;
            }
            else
            {
                _existingExternalInteractor.Image = Resources.external_small;
                _existingProcess.Image = Resources.process_small;
                _existingDataStore.Image = Resources.storage_small;
                _existingTrustBoundary.Image = Resources.trust_boundary_small;
                _templateExternalInteractor.Image = Resources.external_small;
                _templateProcess.Image = Resources.process_small;
                _templateDataStore.Image = Resources.storage_small;
                _templateTrustBoundary.Image = Resources.trust_boundary_small;
            }

            InitializeStandardPalette();
            InitializeTemplatesPalette();
            InitializeExistingPalette();

            GraphEntity.ParentChanged += GraphEntityParentChanged;
            GraphGroup.ParentChanged += GraphGroupParentChanged;

            EventsDispatcher.Register("AdjustDpiFactor", AdjustDpiFactor);
            EventsDispatcher.Register("ResetFlows", ResetFlows);
            EventsDispatcher.Deregister("RefreshDiagram", RefreshDiagram);

            UndoRedoManager.Undone += RefreshOnUndoRedo;
            UndoRedoManager.Redone += RefreshOnUndoRedo;
        }

        private void RefreshOnUndoRedo(string text)
        {
            var diagram = _diagram.Model?.Diagrams?.FirstOrDefault(x => x.Id == _diagram.Id);
            if (diagram == null)
                this.ParentForm?.Close();
            else
                SetDiagram(Dpi.Factor.Width);
        }

        private void AdjustDpiFactor(object obj)
        {
            if (obj is AdjustFactorParams parameters && parameters.DiagramId == _diagram.Id)
            {
                var factor = ((float)parameters.Factor) / 100f;

                SetDiagram(factor);
            }
        }

        private void ResetFlows(object obj)
        {
            if ((((Guid) obj) == _diagram.Id) && _links.Any())
            {
                foreach (var link in _links.Values)
                {
                    link.RealLink.ClearPoints();
                    link.RealLink.CalculateStroke();
                }
            }
        }

        private void RefreshDiagram(object obj)
        {
            if (obj == null || (((Guid)obj) == _diagram.Id))
            {
                SetDiagram(1.0f);
            }
        }

        public ModelPanel([NotNull] ModelPanelFactory factory) : this()
        {
            _factory = factory;
        }
        
        public event Action<string> ShowMessage;
        
        public event Action<string> ShowWarning;

        [Background]
        private void GraphGroupParentChanged(IIdentity identity)
        {
            if (!_loading)
            {
                if (identity is IGroup group)
                {
                    Application.DoEvents();

                    var shape = GetGroup(group);
                    if (shape != null)
                    {
                        var oldParentShape = shape.Parent as GraphGroup;
                        var parent = (group as IGroupElement)?.Parent;

                        GraphGroup parentShape;
                        if (parent != null)
                            parentShape = GetGroup(parent);
                        else
                            parentShape = null;

                        if (oldParentShape != parentShape)
                        {

                        }

                        //if (parentShape != null)
                        //    parentShape.Add(shape);

                        Validate(shape);
                    }
                }
            }
        }

        [Background]
        private void GraphEntityParentChanged(IIdentity identity)
        {
            if (!_loading)
            {
                if (identity is IEntity entity)
                {
                    Application.DoEvents();

                    var shape = GetEntity(entity);
                    if (shape != null)
                    {
                        var oldParentShape = shape.Parent as GraphGroup;
                        var parent = entity.Parent;

                        GraphGroup parentShape;
                        if (parent != null)
                            parentShape = GetGroup(parent);
                        else
                            parentShape = null;

                        if (oldParentShape != parentShape)
                        {

                        }

                        //if (parentShape != null)
                        //{
                            //shape.Parent?.Remove();
                            //parentShape.Add(shape);
                        //}

                        Validate(shape);
                    }
                }
            }
        }

        [Dispatched(false)]
        private void Validate(GraphEntity graphEntity)
        {
            graphEntity?.Validate();
        }

        [Dispatched(false)]
        private void Validate(GraphGroup graphGroup)
        {
            graphGroup?.Validate();
        }

        public bool IsInitialized => _diagram != null;

        public Guid Id => _diagram?.Id ?? Guid.Empty;

        internal Bitmap GetBitmap()
        {
            return _graph.ToBitmap();
        }

        internal Image GetMetafile()
        {
            return _graph.ToMetafile();
        }

        private void OpenDiagram(Guid diagramId)
        {
            var diagram = _diagram.Model?.GetDiagram(diagramId);
            var factory = ExtensionUtils.GetExtensionByLabel<IPanelFactory>("Diagram");
            if (factory != null && diagram != null)
                OpenPanel?.Invoke(factory, diagram);
        }

        private void _graph_ObjectGotSelection(object sender, GoSelectionEventArgs e)
        {
            if (!_loading)
            {
                if (_graph.Selection.Count == 1 && e.GoObject is GraphText graphText)
                {
                    if (graphText.Parent is GraphLink link)
                    {
                        _properties.Item = link.Link.DataFlow;
                    }
                    else
                    {
                        _properties.Item = _diagram;
                    }
                }
                
                OnObjectSelected(_graph.Selection.Count > 0);
            }
        }

        private void _graph_ObjectLostSelection(object sender, GoSelectionEventArgs e)
        {
            if (!_loading && _graph.Selection.Count == 0)
            {
                _properties.Item = _diagram;
                OnObjectSelected(false);
            }
        }

        private void _graph_SelectionDeleting(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (var item in _graph.Selection)
                HandleDelete(item);
        }

        private void HandleDelete([NotNull] GoObject item)
        {
            if (item is GraphEntity node)
            {
                RemoveRegisteredEvents(node);

                var id = node.EntityShape.AssociatedId;
                node.Deactivated = true;
                RemoveRelatedLinks(id);
                _entities.Remove(id);
                _diagram.RemoveEntityShape(id);

                CheckRefresh();
            }

            if (item is GraphGroup group)
            {
                group.SelectedShape -= OnSelectedShape;
  
                //if (group.GroupShape?.Identity is IThreatEventsContainer container)
                //{
                //    container.ThreatEventAdded -= OnThreatEventAddedToShape;
                //    container.ThreatEventRemoved -= OnThreatEventRemovedFromShape;
                //}

                var id = group.GroupShape.AssociatedId;
                group.Deactivated = true;
                RemoveChildShapes(id);
                group.Dispose();
                _groups.Remove(id);
                _diagram.RemoveGroupShape(id);
            }

            if (item is GraphLink link)
            {
                RemoveRegisteredEvents(link);
                var id = link.Link.AssociatedId;
                _links.Remove(id);
                _diagram.RemoveLink(id);
            }
        }

        private void RemoveRegisteredEvents(GraphEntity node)
        {
            node.SelectedShape -= OnSelectedShape;
            node.SelectedThreatEvent -= OnSelectedThreatEvent;
            node.OpenDiagram -= OnOpenDiagram;

            if (node.EntityShape?.Identity is IThreatEventsContainer container)
            {
                container.ThreatEventAdded -= OnThreatEventAddedToShape;
                container.ThreatEventRemoved -= OnThreatEventRemovedFromShape;
            }

            node.Dispose();
        }

        private void RemoveRegisteredEvents(GraphLink link)
        {
            link.SelectedLink -= OnSelectedLink;
            link.SelectedThreatEvent -= OnSelectedThreatEvent;

            if (link.Link.DataFlow is IThreatEventsContainer container)
            {
                container.ThreatEventAdded -= OnThreatEventAddedToShape;
                container.ThreatEventRemoved -= OnThreatEventRemovedFromShape;
            }

            link.Dispose();
        }

        private void RemoveChildShapes(Guid id)
        {
            var entityShapes = _entities.Where(x => x.Value.EntityShape.Identity is IEntity entity &&
                                                 entity.ParentId == id).ToArray();
            if (entityShapes.Any())
            {
                foreach (var entity in entityShapes)
                {
                    RemoveRelatedLinks(entity.Key);

                    _entities.Remove(entity.Key);
                    _diagram.RemoveEntityShape(entity.Key);
                }

                CheckRefresh();
            }

            var groupShapes = _groups.Where(x => x.Value.GroupShape.Identity is IGroupElement group &&
                                                 group.ParentId == id).ToArray();
            if (groupShapes.Any())
            {
                foreach (var group in groupShapes)
                {
                    RemoveChildShapes(group.Key);
                    _groups.Remove(group.Key);
                    _diagram.RemoveGroupShape(group.Key);
                }
            }
        }

        private void RemoveRelatedLinks(Guid id)
        {
            var links = _links.Where(x => x.Value.Link.DataFlow is IDataFlow dataFlow &&
                                          (dataFlow.SourceId == id || dataFlow.TargetId == id)).ToArray();
            if (links.Any())
            {
                foreach (var link in links)
                {
                    _links.Remove(link.Key);
                    _diagram.RemoveLink(link.Key);
                }
            }
        }

        private void OnSelectedThreatEvent([NotNull] IThreatEvent threatEvent)
        {
            _properties.Item = threatEvent;
        }

        private void OnObjectSelected(bool selected = true)
        {
            ChangeCustomActionStatus?.Invoke("AlignH", selected);
            ChangeCustomActionStatus?.Invoke("AlignV", selected);
            ChangeCustomActionStatus?.Invoke("AlignT", selected);
            ChangeCustomActionStatus?.Invoke("AlignB", selected);
            ChangeCustomActionStatus?.Invoke("AlignL", selected);
            ChangeCustomActionStatus?.Invoke("AlignR", selected);

            if (_actions != null)
            {
                foreach (var action in _actions)
                {
                    if (action.Scope.HasFlag(Scope.Entity) ||
                        action.Scope.HasFlag(Scope.Group) ||
                        action.Scope.HasFlag(Scope.DataFlow))
                    {
                        ChangeCustomActionStatus?.Invoke(action.Label, selected);
                    }
                }
            }
        }

        private void _properties_Enter(object sender, EventArgs e)
        {
            OnObjectSelected(false);
        }

        private void _properties_Leave(object sender, EventArgs e)
        {
            OnObjectSelected(_graph.Selection.Count > 0);
        }

        private void _standardPalette_MouseEnter(object sender, EventArgs e)
        {
            PanelItemListForm.CloseAll();
        }

        private void _existingPalette_MouseEnter(object sender, EventArgs e)
        {
            PanelItemListForm.CloseAll();
        }

        private void _properties_MouseEnter(object sender, EventArgs e)
        {
            PanelItemListForm.CloseAll();
        }

        public void SetExecutionMode(ExecutionMode mode)
        {
            _executionMode = mode;
            _properties.SetExecutionMode(mode);

            if (mode == ExecutionMode.Business || mode == ExecutionMode.Management)
            {
                _leftSplitter.Expanded = false;
                _tabContainer.Visible = false;
                _leftSplitter.Visible = false;
                _graph.SetModifiable(false);
                _properties.ReadOnly = true;
            }
        }
    }
}
