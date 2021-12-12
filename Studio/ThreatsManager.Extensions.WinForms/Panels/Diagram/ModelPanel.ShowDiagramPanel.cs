using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    public partial class ModelPanel
    {
        private DpiState _dpiState;

        public Form PanelContainer { get; set; }

        public IDiagram Diagram => _diagram;

        public void SetDiagram([NotNull] IDiagram diagram)
        {
            _loading = true;
            _diagram = diagram;
            _properties.Item = diagram;

            if (_diagram.Model != null)
            {
                _diagram.Model.ChildCreated += OnModelChildCreated;
                _diagram.Model.ChildRemoved += OnModelChildRemoved;

                var groups = _diagram.Groups?.ToArray();
                var entities = _diagram.Entities?.ToArray();

                _dpiState = CalculateDpiActions(diagram);
                _graph.Initialize(diagram, _dpiState);
                var doc = _graph.Doc;

                if (doc != null)
                {
                    try
                    {
                        doc.StartTransaction();

                        if (groups?.Any() ?? false)
                        {
                            AddShapes(groups);
                        }

                        if (entities?.Any() ?? false)
                        {
                            var npEntities = AddShapes(entities)?.ToArray();

                            if (npEntities?.Any() ?? false)
                            {
                                // We have at least an unprocessed shape!
                                foreach (var entity in npEntities)
                                {
                                    GraphOnCreateIdentity(entity.AssociatedId, entity.Position);
                                    AddShape(entity);
                                }
                            }

                            var links = _diagram.Links?.ToArray();

                            if (links != null)
                            {
                                foreach (var link in links)
                                {
                                    AddLink(link);
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
                    }
                    finally
                    {
                        _loading = false;
                        doc.FinishTransaction($"Set Diagram {diagram.Name}");
                    }
                }
            }
            else
            {
                throw new ArgumentException(Properties.Resources.DiagramNoModelError);
            }
        }

        private DpiState CalculateDpiActions(IDiagram diagram)
        {
            DpiState result = DpiState.Ok;

            var schemaManager = new DiagramPropertySchemaManager(diagram.Model);
            var dpiFactor = schemaManager.GetDpiFactor(diagram);
            if (dpiFactor > 0f)
            {
                if (dpiFactor > Dpi.Factor.Height + 0.25)
                    result = DpiState.TooBig;
                else if (dpiFactor < Dpi.Factor.Height - 0.25)
                    result = DpiState.TooSmall;
            }
            else
            {
                schemaManager.SetDpiFactor(diagram);

                var links = diagram.Links?.ToArray();

                if (links?.Any() ?? false)
                {
                    var propertyType = schemaManager.GetLinksSchema()?.GetPropertyType("Points");
                    if (propertyType != null)
                    {
                        var totalDistanceFactor = 0.0;
                        var count = 0;

                        foreach (var link in links)
                        {
                            var property = link.GetProperty(propertyType);
                            if (property is IPropertyArray array)
                            {
                                var points = array.Value?.Select(x => float.Parse(x, NumberFormatInfo.InvariantInfo))
                                    .ToArray();
                                if (points?.Any() ?? false)
                                {
                                    var source = link.DataFlow.Source;
                                    var sourceShape = diagram.Entities.FirstOrDefault(x => x.AssociatedId == source.Id);
                                    if (sourceShape != null)
                                    {
                                        var position = sourceShape.Position;
                                        var x = points[0];
                                        var y = points[1];

                                        var distance =
                                            Math.Sqrt(Math.Pow(position.X - x, 2) + Math.Pow(position.Y - y, 2));

                                        totalDistanceFactor += distance / Dpi.Factor.Width;
                                        count++;
                                    }
                                }
                            }
                        }

                        if (count > 0)
                        {
                            var distanceFactor = totalDistanceFactor / count;

                            if (distanceFactor < 13f)
                                result = DpiState.TooSmall;
                            else if (distanceFactor > 26f)
                                result = DpiState.TooBig;
                        }
                    }
                }
            }

            return result;
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