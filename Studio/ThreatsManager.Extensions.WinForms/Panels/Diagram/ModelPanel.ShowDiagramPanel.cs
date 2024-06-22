﻿using System;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Panels.DiagramConfiguration;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    public partial class ModelPanel
    {
        private int _iconSize;
        private int _iconCenterSize;
        private int _markerSize;
        private int _zoomFactor;
        private int _entityWrapWidth;
        private int _flowWrapWidth;
        private ImageSize _imageSize;

        public Form PanelContainer { get; set; }

        public IIdentity ReferenceObject => _diagram;

        public IDiagram Diagram => _diagram;

        public void SetDiagram([NotNull] IDiagram diagram)
        {
            _loading = true;
            _diagram = diagram;
            _properties.Item = _diagram;

            if (_diagram != null)
            {
                using (var scope = UndoRedoManager.OpenScope("Set Diagram in ModelPanel"))
                {
                    _diagram.Model.ChildCreated += OnModelChildCreated;
                    _diagram.Model.ChildRemoved += OnModelChildRemoved;
                    _diagram.LinkAdded += OnLinkAdded;
                    _diagram.EntityShapeAdded += OnEntityShapeAdded;
                    _diagram.GroupShapeAdded += OnGroupShapeAdded;

                    var dpi = GetDiagramDpi(diagram);
                    var factor = 1f / dpi;

                    _graph.Initialize(_diagram);

                    SetDiagram(factor);

                    AddPalettes();
                    ConfigurePanelItemContextMenu();
                }
            }
        }

        internal void SetDiagram(float dpiFactor)
        {
            _loading = true;

            if (!UndoRedoManager.IsUndoing && !UndoRedoManager.IsRedoing)
            {
                var configuration = new DiagramConfigurationManager(_diagram.Model);
                _iconSize = configuration.DiagramIconSize;
                _iconCenterSize = configuration.DiagramIconCenterSize;
                _markerSize = configuration.DiagramMarkerSize;
                _zoomFactor = configuration.DiagramZoomFactor;
                _entityWrapWidth = configuration.EntityWrappingWidth;
                _flowWrapWidth = configuration.FlowWrappingWidth;
                switch (_iconSize)
                {
                    case 32:
                        _imageSize = ImageSize.Medium;
                        break;
                    case 64:
                    case 96:
                    case 128:
                    case 256:
                        _imageSize = ImageSize.Big;
                        break;
                    default:
                        configuration.DiagramIconSize = 32;
                        _imageSize = ImageSize.Medium;
                        configuration.Apply();
                        break;
                }
            }

            LoadStandardPalette();

            if (_diagram.Model != null)
            {
                var groups = _diagram.Groups?.ToArray();
                var entities = _diagram.Entities?.ToArray();

                var doc = _graph.Doc;

                if (doc != null)
                {
                    try
                    {
                        doc.StartTransaction();

                        doc.Clear();

                        _graph.Zoom(((float)_zoomFactor) / 100f);

                        _entities.Clear();
                        _groups.Clear();
                        _links.Clear();

                        using (var scope = UndoRedoManager.OpenScope("Diagram loading"))
                        {
                            if (groups?.Any() ?? false)
                            {
                                AddShapes(groups, dpiFactor);
                            }

                            if (entities?.Any() ?? false)
                            {
                                var npEntities = AddShapes(entities, dpiFactor)?.ToArray();

                                if (npEntities?.Any() ?? false)
                                {
                                    // We have at least an unprocessed shape!
                                    foreach (var entity in npEntities)
                                    {
                                        GraphOnCreateIdentity(entity.AssociatedId, entity.Position);
                                        AddShape(entity, dpiFactor);
                                    }
                                }

                                var links = _diagram.Links?.ToArray();

                                if (links != null)
                                {
                                    foreach (var link in links)
                                    {
                                        AddLink(link, dpiFactor);
                                    }
                                }
                            }

                            if (groups?.Any() ?? false)
                            {
                                foreach (var group in groups)
                                {
                                    if (group.Identity is IGroup groupIdentity)
                                    {
                                        var groupNode = GetGroup(groupIdentity);
                                    }
                                }
                            }

                            if (_links?.Any() ?? false)
                            {
                                var links = _links.Values.ToArray();
                                foreach (var link in links)
                                    link.UpdateRoute();
                            }

                            _graph.DocPosition = _graph.DocumentTopLeft;

                            SetDiagramDpi(_diagram);

                            scope?.Complete();
                        }
                    }
                    finally
                    {
                        _loading = false;
                        doc.FinishTransaction($"Set Diagram {_diagram.Name}");
                        _graph.Refresh();
                    }
                }
            }
            else
            {
                throw new ArgumentException(Properties.Resources.DiagramNoModelError);
            }
        }

        private float GetDiagramDpi([NotNull] IDiagram diagram)
        {
            float result;

            if (diagram.Dpi.HasValue)
                result = diagram.Dpi.Value / 100f;
            else
            {
                var schemaManager = new DiagramPropertySchemaManager(diagram.Model);
                result = schemaManager.GetDpiFactor(diagram);
                if (result == 0f)
                    result = 1f;
            }

            return result;
        }

        private void SetDiagramDpi([NotNull] IDiagram diagram)
        {
            if (!UndoRedoManager.IsUndoing && !UndoRedoManager.IsRedoing)
                diagram.Dpi = 100;
        }

        private void OnModelChildCreated(IIdentity identity)
        {
            CheckRefresh();
        }

        private void OnModelChildRemoved(IIdentity identity)
        {
            ExistingPaletteRemove(identity);

            if (identity is IEntity entity)
            {
                var entityNode = GetEntity(entity);
                if (entityNode != null)
                {
                    _graph.Doc.Remove(entityNode);
                    _entities.Remove(entity.Id);
                }
            } else if (identity is IGroup group)
            {
                GraphGroup newParent;
                if (group is IGroupElement groupElement && groupElement.Parent is IGroup parent)
                    newParent = GetGroup(parent);
                else
                    newParent = null;

                var groupNode = GetGroup(group);
                if (groupNode != null)
                {
                    ReparentChildren(groupNode, newParent);
                    _graph.Doc.Remove(groupNode);
                    _groups.Remove(group.Id);
                }
            } else if (identity is IDataFlow dataFlow)
            {
                if (_links.TryGetValue(dataFlow.Id, out var graphLink))
                {
                    _graph.Doc.Remove(graphLink);
                    _links.Remove(dataFlow.Id);
                }
            }
        }

        private void OnGroupShapeAdded(IGroupShapesContainer container, IGroupShape shape)
        {
            AddShape(shape);
        }

        private void OnEntityShapeAdded(IEntityShapesContainer container, IEntityShape shape)
        {
            AddShape(shape);
        }

        private void OnLinkAdded(ILinksContainer container, ILink link)
        {
            AddLink(link);
        }

        private void ReparentChildren([NotNull] GraphGroup parent, GraphGroup newParent)
        {
            var entities = _entities.Values.Where(x => x.Parent == parent).ToArray();
            if (entities.Any())
            {
                foreach (var entity in entities)
                {
                    parent.Remove(entity);
                    if (newParent != null)
                        newParent.Add(entity);
                    else
                        _graph.Doc.Add(entity);
                }
            }

            var groups = _groups.Values.Where(x => x.Parent == parent).ToArray();
            if (groups.Any())
            {
                foreach (var group in groups)
                {
                    parent.Remove(group);
                    if (newParent != null)
                        newParent.Add(group);
                    else
                        _graph.Doc.Add(group);
                }
            }

            var links = _links.Values.ToArray();
            if (links.Any())
            {
                foreach (var link in links)
                {
                    link.Parent?.Remove(link);
                    _graph.Doc.LinksLayer.Add(link);
                }
            }
        }
    }
}