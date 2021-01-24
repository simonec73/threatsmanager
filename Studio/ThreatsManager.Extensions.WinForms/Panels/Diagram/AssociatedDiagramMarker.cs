using System;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using Northwoods.Go;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    [Serializable]
    public sealed class AssociatedDiagramMarker : GoImage, IDisposable
    {
        private readonly IEntity _entity;
        private bool _visible = false;

        private AssociatedDiagramMarker() {
            Printable = false;
            Resizable = false;
            Deletable = false;
            Copyable = false;
            Selectable = false;
            Image = Resources.model_big;
            Size = new SizeF(16.0f * Dpi.Factor.Width, 16.0f * Dpi.Factor.Height);

            MarkerStatusTrigger.MarkerStatusUpdated += MarkerStatusTriggerOnMarkerStatusUpdated;
        }

        public void Dispose()
        {
            MarkerStatusTrigger.MarkerStatusUpdated -= MarkerStatusTriggerOnMarkerStatusUpdated;
            if (_entity != null)
            {
                DiagramAssociationHelper.DiagramAssociated -= OnDiagramAssociated;
                DiagramAssociationHelper.DiagramDisassociated -= OnDiagramDisassociated;
                _entity.Model.ChildRemoved -= OnModelChildRemoved;
            }
        }

        private void MarkerStatusTriggerOnMarkerStatusUpdated(MarkerStatus status)
        {
            Visible = _visible && (MarkerStatusTrigger.CurrentStatus == MarkerStatus.Full);
        }

        public AssociatedDiagramMarker([NotNull] IEntity entity) : this()
        {
            _entity = entity;
            _visible = GetAssociatedDiagram() != null;
            Visible = _visible && (MarkerStatusTrigger.CurrentStatus == MarkerStatus.Full);
            DiagramAssociationHelper.DiagramAssociated += OnDiagramAssociated;
            DiagramAssociationHelper.DiagramDisassociated += OnDiagramDisassociated;
            entity.Model.ChildRemoved += OnModelChildRemoved;
        }

        public Action<IDiagram> DiagramClicked;

        // can't get any selection handles
        public override void AddSelectionHandles(GoSelection sel, GoObject selectedObj) { }

        public override bool OnSingleClick(GoInputEventArgs evt, GoView view)
        {
            if (!evt.Alt && !evt.Control && !evt.DoubleClick && !evt.Shift && (evt.Buttons == MouseButtons.Left))
            {
                var diagram = GetAssociatedDiagram();
                if (diagram != null)
                    DiagramClicked?.Invoke(diagram);
            }

            return base.OnSingleClick(evt, view);
        }

        private void OnDiagramAssociated(IEntity entity, IDiagram diagram)
        {
            if (entity != null && _entity != null && entity.Id == _entity.Id)
            {
                _visible = true;
                Visible = (MarkerStatusTrigger.CurrentStatus == MarkerStatus.Full);
            }
        }

        private void OnDiagramDisassociated(IEntity entity)
        {
            if (entity != null && _entity != null && entity.Id == _entity.Id)
            {
                _visible = false;
                Visible = false;
            }
        }

        private void OnModelChildRemoved([NotNull] IIdentity identity)
        {
            if (identity is IDiagram diagram && _entity != null && _entity.Model is IThreatModel model)
            {
                var schemaManager = new AssociatedDiagramPropertySchemaManager(model);
                var propertyType = schemaManager.GetAssociatedDiagramIdPropertyType();
                if (propertyType != null)
                {
                    if (_entity.GetProperty(propertyType) is IPropertyIdentityReference property)
                    {
                        var associatedDiagramId = property.ValueId;
                        if (diagram.Id == associatedDiagramId)
                        {
                            property.Value = null;
                            _visible = false;
                            Visible = false;
                        }
                    }
                }
            }
        }

        private IDiagram GetAssociatedDiagram()
        {
            IDiagram result = null;

            if (_entity?.Model is IThreatModel model)
            {
                var schemaManager = new AssociatedDiagramPropertySchemaManager(model);
                var propertyType = schemaManager.GetAssociatedDiagramIdPropertyType();
                if (propertyType != null)
                {
                    var property = _entity.GetProperty(propertyType);
                    if (property != null && Guid.TryParse(property.StringValue, out var id))
                    {
                        result = model.GetDiagram(id);
                    }
                }
            }

            return result;
        }
    }
}