using System;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Panels.Configuration;
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
        private ImageSize _imageSize;

        public Form PanelContainer { get; set; }

        public IDiagram Diagram => _diagram;

        public void SetDiagram([NotNull] IDiagram diagram)
        {
            _loading = true;
            _diagram = diagram;
            _properties.Item = _diagram;

            if (_diagram != null)
            {
                _diagram.Model.ChildCreated += OnModelChildCreated;
                _diagram.Model.ChildRemoved += OnModelChildRemoved;

                var dpi = GetDiagramDpi(diagram);
                var factor = Dpi.Factor.Height / dpi;

                _graph.Initialize(_diagram);

                SetDiagram(factor);

                AddPalettes();
                ConfigurePanelItemContextMenu();
            }
        }

        internal void SetDiagram(float dpiFactor)
        {
            _loading = true;

            var configuration = new ExtensionConfigurationManager(_diagram.Model,
                (new ConfigurationPanelFactory()).GetExtensionId());
            _iconSize = configuration.DiagramIconSize;
            _iconCenterSize = configuration.DiagramIconCenterSize;
            _markerSize = configuration.DiagramMarkerSize;
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
                    if (Dpi.Factor.Height >= 8)
                    {
                        configuration.DiagramIconSize = 256;
                        _imageSize = ImageSize.Big;
                    }
                    else if (Dpi.Factor.Height >= 4)
                    {
                        configuration.DiagramIconSize = 128;
                        _imageSize = ImageSize.Big;
                    }
                    else if (Dpi.Factor.Height >= 3)
                    {
                        configuration.DiagramIconSize = 96;
                        _imageSize = ImageSize.Big;
                    }
                    else if (Dpi.Factor.Height >= 2)
                    {
                        configuration.DiagramIconSize = 64;
                        _imageSize = ImageSize.Big;
                    }
                    else
                    {
                        configuration.DiagramIconSize = 32;
                        _imageSize = ImageSize.Medium;
                    }
                    configuration.Apply();
                    break;
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

                        _graph.Zoom(((float)configuration.DiagramZoomFactor) / 100f);

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
                                        groupNode.RefreshBorder();
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
                result = diagram.Dpi.Value / 100;
            else
            {
                var schemaManager = new DiagramPropertySchemaManager(diagram.Model);
                result = schemaManager.GetDpiFactor(diagram);
                if (result == 0f)
                    result = Dpi.Factor.Height;
            }

            return result;
        }

        private void SetDiagramDpi([NotNull] IDiagram diagram)
        {
            diagram.Dpi = (int) (Dpi.Factor.Height * 100);
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