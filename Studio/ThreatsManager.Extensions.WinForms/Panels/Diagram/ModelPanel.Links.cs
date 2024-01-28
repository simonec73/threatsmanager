using Northwoods.Go;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    public partial class ModelPanel
    {
        private void AddLink([NotNull] ILink link, float dpiFactor = 1.0f)
        {
            if (!_links.ContainsKey(link.AssociatedId))
            {
                var from = GetEntity(link.DataFlow.Source);
                var to = GetEntity(link.DataFlow.Target);
                if (from != null && to != null)
                {
                    var newLink = new GraphLink(link, dpiFactor, _markerSize)
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

                    newLink.Loading = false;
                    newLink.UpdateRoute();
                }
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
            if (e.GoObject is GraphLink glink && !UndoRedoManager.IsUndoing && !UndoRedoManager.IsRedoing)
            {
                using (var scope = UndoRedoManager.OpenScope("Create Link"))
                {
                    if (glink.Parent is GraphGroup group)
                        group.Remove(glink);
                    _graph.Doc.LinksLayer.Add(glink);

                    if (glink.FromPort.Node is GraphEntity from && glink.ToPort.Node is GraphEntity to)
                    {
                        var link = CreateLink(glink, from, to);
                        if (link != null)
                        {
                            glink.UpdateParameters(_markerSize);
                            glink.Link = link;
                            _links[link.AssociatedId] = glink;
                        }
                    }
                    if (_actions != null)
                        glink.SetContextAwareActions(_actions);

                    scope?.Complete();

                    glink.SelectedLink += OnSelectedLink;
                    glink.SelectedThreatEvent += OnSelectedThreatEvent;
                }
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
                        _links.Add(dataFlow.Id, glink);
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
                _links.Add(dataFlow.Id, glink);
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