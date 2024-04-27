﻿using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls.WebParts;
using Northwoods.Go;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    public partial class ModelPanel
    {
        private IEnumerable<IShape> AddShapes([NotNull] IEnumerable<IShape> shapes, float dpiFactor = 1.0f)
        {
            IEnumerable<IShape> result = null;

            var cannotProcess = new List<IShape>();

            var shapesList = shapes.ToArray();

            foreach (var shape in shapesList)
            {
                if (shape.Identity is IGroupElement child)
                {
                    if (child.Parent == null || _groups.ContainsKey(child.ParentId))
                    {
                        AddShape(shape, dpiFactor);
                    }
                    else
                    {
                        cannotProcess.Add(shape);
                    }
                }
                else
                {
                    AddShape(shape, dpiFactor);
                }
            }

            if (cannotProcess.Count > 0 && cannotProcess.Count != shapesList.Count())
                result = AddShapes(cannotProcess, dpiFactor);
            else
                result = cannotProcess;

            return result;
        }

        private GoNode AddShape([NotNull] IShape shape, float dpiFactor = 1.0f)
        {
            GoNode result = null;

            if (_entities.ContainsKey(shape.AssociatedId))
            {
                result = _entities[shape.AssociatedId];
            }
            else if (_groups.ContainsKey(shape.AssociatedId))
            {
                result = _groups[shape.AssociatedId];
            }
            else
            {
                GraphGroup parent = null;

                if (shape.Identity is IGroupElement child)
                {
                    _groups.TryGetValue(child.ParentId, out parent);
                }

                if (shape is IEntityShape entityShape)
                {
                    var node = new GraphEntity(entityShape, dpiFactor, _iconSize,
                        _iconCenterSize, _imageSize, _markerSize, _entityWrapWidth);
                    if (_actions != null)
                        node.SetContextAwareActions(_actions);
                    node.SelectedShape += OnSelectedShape;
                    node.SelectedThreatEvent += OnSelectedThreatEvent;
                    node.OpenDiagram += OnOpenDiagram;
                    if (parent == null)
                        _graph.Doc.Add(node);
                    else
                        parent.Add(node);
                    _entities.Add(shape.AssociatedId, node);

                    node.Validate();

                    result = node;
                }
                else if (shape is IGroupShape groupShape && groupShape.Identity != null &&
                    !_groups.ContainsKey(groupShape.AssociatedId))
                {
                    var group = new GraphGroup(groupShape, dpiFactor, _markerSize);
                    if (_actions != null)
                        group.SetContextAwareActions(_actions);
                    group.SelectedShape += OnSelectedShape;
                    if (parent == null)
                        _graph.Doc.Add(group);
                    else
                        parent.Add(group);
                    _groups.Add(shape.AssociatedId, group);

                    group.Validate();

                    result = group;
                }

                if (result != null && shape.Identity is IThreatEventsContainer container)
                {
                    container.ThreatEventAdded += OnThreatEventAddedToShape;
                    container.ThreatEventRemoved += OnThreatEventRemovedFromShape;
                }
            }

            return result;
        }

        private void OnOpenDiagram(IDiagram diagram)
        {
            var factory = ExtensionUtils.GetExtensionByLabel<IPanelFactory>("Diagram");
            if (factory != null && diagram != null)
                OpenPanel?.Invoke(factory, diagram);

        }

        private void OnThreatEventRemovedFromShape(IThreatEventsContainer container, IThreatEvent threatEvent)
        {
            if (container is IEntity entity)
            {
                var node = GetEntity(entity);
                if (node != null)
                {
                    //node.UpdateParameters(_iconSize, _iconCenterSize, _imageSize, _markerSize);
                    node.ThreatsMarker = entity.ThreatEvents?.Any() ?? false;
                }
            }
        }

        private void OnThreatEventAddedToShape(IThreatEventsContainer container, IThreatEvent threatEvent)
        {
            if (container is IEntity entity)
            {
                var node = GetEntity(entity);
                if (node != null)
                {
                    //node.UpdateParameters(_iconSize, _iconCenterSize, _imageSize, _markerSize);
                    node.ThreatsMarker = entity.ThreatEvents?.Any() ?? false;
                }
            }
        }

        private GraphEntity GetEntity([NotNull] IEntity entity)
        {
            return _entities.TryGetValue(entity.Id, out var result) ? result : null;
        }
        
        private GraphGroup GetGroup([NotNull] IGroup group)
        {
            return _groups.TryGetValue(group.Id, out var result) ? result : null;
        }

        private void OnSelectedShape(IShape shape)
        {
            if (!_loading)
            {
                if (_graph.Selection.Count == 1)
                    _properties.Item = shape?.Identity;
                else
                    _properties.Item = _diagram;
            }
        }
    }
}