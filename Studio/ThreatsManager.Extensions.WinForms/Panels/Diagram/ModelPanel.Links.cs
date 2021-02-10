using System.Drawing;
using System.Globalization;
using System.Linq;
using Northwoods.Go;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    public partial class ModelPanel
    {
        private void AddLink([NotNull] ILink link)
        {
            var from = GetEntity(link.DataFlow.Source);
            var to = GetEntity(link.DataFlow.Target);
            if (from != null && to != null)
            {
                var newLink = new GraphLink(link, _dpiState)
                {
                    Loading = true,
                    FromPort = from.Port,
                    ToPort = to.Port
                };
                if (_actions != null)
                    newLink.SetContextAwareActions(_actions);
                _graph.Doc.Add(newLink);
                _links.Add(link.AssociatedId, newLink);
                newLink.SelectedLink += OnSelectedLink;
                newLink.SelectedThreatEvent += OnSelectedThreatEvent;
       
                var schemaManager = new DiagramPropertySchemaManager(link.DataFlow.Model);
                var pointsPropertyType = schemaManager.GetLinksSchema()?.GetPropertyType("Points");
                var property = link.GetProperty(pointsPropertyType);
                if (property is IPropertyArray propertyArray)
                {
                    var array = propertyArray.Value?.ToArray();
                    var count = array?.Length ?? 0;
                    if (count > 0)
                    {
                        newLink.RealLink.ClearPoints();
                        for (int i = 0; i < count / 2; i++)
                        {
                            float x;
                            float y;
                            switch (_dpiState)
                            {
                                case DpiState.TooSmall:
                                    x = float.Parse(array[i * 2], NumberFormatInfo.InvariantInfo) * 2;
                                    y = float.Parse(array[i * 2 + 1], NumberFormatInfo.InvariantInfo) * 2;
                                    break;
                                case DpiState.TooBig:
                                    x = float.Parse(array[i * 2], NumberFormatInfo.InvariantInfo) / 2;
                                    y = float.Parse(array[i * 2 + 1], NumberFormatInfo.InvariantInfo) / 2;
                                    break;
                                default:
                                    x = float.Parse(array[i * 2], NumberFormatInfo.InvariantInfo);
                                    y = float.Parse(array[i * 2 + 1], NumberFormatInfo.InvariantInfo);
                                    break;
                            }
                            newLink.RealLink.AddPoint(new PointF(x, y));
                        }
                    }
                }

                newLink.Loading = false;
                newLink.UpdateRoute();
            }
        }

        private void OnSelectedLink(ILink link)
        {
            if (!_loading)
            {
                if (_graph.Selection.Count == 1)
                    _properties.Item = link?.DataFlow;
                else
                    _properties.Item = _diagram;
            }

        }

        private void _graph_LinkCreated(object sender, GoSelectionEventArgs e)
        {
            if (e.GoObject is GraphLink glink)
            {
                glink.DpiState = _dpiState;
                if (glink.Parent is GraphGroup group)
                    group.Remove(glink);
                _graph.Doc.LinksLayer.Add(glink);

                if (glink.FromPort.Node is GraphEntity from && glink.ToPort.Node is GraphEntity to)
                {
                    var link = CreateLink(glink, from, to);
                    if (link != null)
                    {
                        glink.Link = link;
                        _links[link.AssociatedId] = glink;
                    }
                }
                if (_actions != null)
                    glink.SetContextAwareActions(_actions);

                glink.SelectedLink += OnSelectedLink;
                glink.SelectedThreatEvent += OnSelectedThreatEvent;
            }
        }

        private ILink CreateLink([NotNull] GraphLink glink, [NotNull] GraphEntity source, [NotNull] GraphEntity target)
        {
            ILink result = null;

            var dataFlow = _diagram.Model?.AddDataFlow(glink.Text, source.EntityShape.AssociatedId,
                target.EntityShape.AssociatedId);

            if (dataFlow == null)
            {
                dataFlow = _diagram.Model?.GetDataFlow(source.EntityShape.AssociatedId,
                    target.EntityShape.AssociatedId);
                if (dataFlow != null)
                {
                    result = _diagram.GetLink(dataFlow.Id);
                    if (result == null)
                    {
                        result = _diagram.AddLink(dataFlow);
                    }
                }
                else
                {
                    ShowWarning?.Invoke("The Flow in the Diagram is in an inconsistent status. Please close the Diagram and try again.");
                }
            }
            else
            {
                result = _diagram.AddLink(dataFlow);
            }

            return result;
        }

        private GraphLink GetLink([NotNull] ILink link)
        {
            GraphLink result = null;

            if (_links.TryGetValue(link.AssociatedId, out var graphLink))
            {
                result = graphLink;
            }

            return result;
        }
    }
}