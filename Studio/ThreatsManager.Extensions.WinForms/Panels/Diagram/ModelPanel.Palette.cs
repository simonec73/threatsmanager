using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DevComponents.DotNetBar;
using Northwoods.Go;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    public partial class ModelPanel
    {
        #region Standard Palette.
        private void LoadStandardPalette()
        {
            _graph.CreateExternalInteractor += GraphOnCreateExternalInteractor;
            _graph.CreateProcess += GraphOnCreateProcess;
            _graph.CreateDataStore += GraphOnCreateDataStore;
            _graph.CreateTrustBoundary += GraphOnCreateTrustBoundary;

            float width = 0;
            float height = 0;
            var ei = CreateNode("ExternalInteractor", "External Interactor", 
                _imageSize == ImageSize.Big ? Resources.external_big : Resources.external, 
                ref width, ref height);
            var p = CreateNode("Process", "Process", 
                _imageSize == ImageSize.Big ? Resources.process_big : Resources.process, 
                ref width, ref height);
            var ds = CreateNode("DataStore", "Data Store",
                _imageSize == ImageSize.Big ? Resources.storage_big : Resources.storage, 
                ref width, ref height);
            var tb = CreateNode("TrustBoundary", "Trust Boundary",
                _imageSize == ImageSize.Big ? Resources.trust_boundary_big : Resources.trust_boundary, 
                ref width, ref height);

            ei.Position = CalculatePosition(ei, 0, width, height);
            _standardPalette.Document.Add(ei);

            p.Position = CalculatePosition(p, 1, width, height);
            _standardPalette.Document.Add(p);

            ds.Position = CalculatePosition(ds, 2, width, height);
            _standardPalette.Document.Add(ds);

            tb.Position = CalculatePosition(tb, 3, width, height);
            _standardPalette.Document.Add(tb);
        }

        private GoSimpleNode CreateNode([Required] string name,
            string label, [NotNull] Bitmap image, ref float width, ref float height)
        {
            var result = new GoSimpleNode()
            {
                Text = name,
                Label = new GoText() {Text = label, Selectable = false},
                Icon = new GoImage()
                {
                    Image = image,
                    Size = _imageSize == ImageSize.Big ? new Size(64, 64) : new Size(32, 32),
                    Selectable = false
                },
            };
            width = Math.Max(width, result.Width);
            height = Math.Max(height, result.Height);

            return result;
        }

        private PointF CalculatePosition(GoNode node, int index, float width, float height)
        {
            return new PointF((width - node.Width) / 2f + 20, 10 + index * (height + 10 * Dpi.Factor.Height));
        }

        private void GraphOnCreateExternalInteractor(PointF point, GraphGroup graphGroup)
        {
            var interactor = _diagram.Model?.AddEntity<IExternalInteractor>();
            if (interactor != null)
            {
                if (graphGroup?.GroupShape?.Identity is IGroup group)
                    interactor.SetParent(group);
                var node = AddShape(_diagram.AddShape(interactor, point));
                _graph.Selection.Clear();
                _graph.Selection.Add(node);
            }
        }

        private void GraphOnCreateProcess(PointF point, GraphGroup graphGroup)
        {
            var process = _diagram.Model?.AddEntity<IProcess>();
            if (process != null)
            {
                if (graphGroup?.GroupShape?.Identity is IGroup group)
                    process.SetParent(group);
                var node = AddShape(_diagram.AddShape(process, point));
                _graph.Selection.Clear();
                _graph.Selection.Add(node);
            }
        }

        private void GraphOnCreateDataStore(PointF point, GraphGroup graphGroup)
        {
            var dataStore = _diagram.Model?.AddEntity<IDataStore>();
            if (dataStore != null)
            {
                if (graphGroup?.GroupShape?.Identity is IGroup group)
                    dataStore.SetParent(group);
                var node = AddShape(_diagram.AddShape(dataStore, point));
                _graph.Selection.Clear();
                _graph.Selection.Add(node);
            }
        }

        private void GraphOnCreateTrustBoundary(PointF point, GraphGroup graphGroup)
        {
            var newGroup = _diagram.Model?.AddGroup<ITrustBoundary>();
            if (newGroup is ITrustBoundary trustBoundary)
            {
                if (graphGroup?.GroupShape?.Identity is IGroup group)
                    trustBoundary.SetParent(group);
                var node = AddShape(_diagram.AddShape(trustBoundary, point, new SizeF(600, 300)));
                _graph.Selection.Clear();
                _graph.Selection.Add(node);
            }
        }
        #endregion

        #region Templates Palette.
        private void InitializeTemplatesPalette()
        {
            _graph.CreateIdentity += GraphOnCreateTemplate;
        }

        private void GraphOnCreateTemplate(Guid id, PointF point, GraphGroup graphGroup)
        {
            Stack<GraphGroup> groups = new Stack<GraphGroup>();

            var template = _diagram.Model?.GetEntityTemplate(id);
            if (template != null)
            {
                var entity = template.CreateEntity(template.Name);
                if (entity is IGroupElement groupElement && graphGroup?.GroupShape?.Identity is IGroup group)
                    groupElement.SetParent(group);
                var node = AddShape(_diagram.AddShape(entity, point));
                _graph.Selection.Clear();
                _graph.Selection.Add(node);
            }
            else
            {
                var tbTemplate = _diagram.Model?.GetTrustBoundaryTemplate(id);
                if (tbTemplate != null)
                {
                    var trustBoundary = tbTemplate.CreateTrustBoundary(tbTemplate.Name);
                    if (graphGroup?.GroupShape?.Identity is IGroup group)
                        trustBoundary.SetParent(group);
                    var node = AddShape(_diagram.AddShape(trustBoundary, point, new SizeF(600, 300)));
                    _graph.Selection.Clear();
                    _graph.Selection.Add(node);
                }
            }
        }

        private void _templateTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckTemplateRefresh();
        }

        private void _templatesRefresh_Click(object sender, EventArgs e)
        {
            CheckTemplateRefresh();
        }

        private void CheckTemplateRefresh()
        {
            if (_templateTypes.SelectedItem == _templateExternalInteractor)
                RefreshTemplatesPalette(Scope.ExternalInteractor);
            else if (_templateTypes.SelectedItem == _templateProcess)
                RefreshTemplatesPalette(Scope.Process);
            else if (_templateTypes.SelectedItem == _templateDataStore)
                RefreshTemplatesPalette(Scope.DataStore);
            else if (_templateTypes.SelectedItem == _templateTrustBoundary)
                RefreshTemplatesPalette(Scope.TrustBoundary);
        }

        private void RefreshTemplatesPalette(Scope scope)
        {
            switch (scope)
            {
                case Scope.ExternalInteractor:
                    _templatesPalette.Document.Clear();
                    var externalInteractors =
                        _diagram.Model?.EntityTemplates?.Where(x => x.EntityType == EntityType.ExternalInteractor).OrderBy(x => x.Name);
                    if (externalInteractors != null)
                        AddToTemplatesPalette(externalInteractors);
                    break;
                case Scope.Process:
                    _templatesPalette.Document.Clear();
                    var processes =
                        _diagram.Model?.EntityTemplates?.Where(x => x.EntityType == EntityType.Process).OrderBy(x => x.Name);
                    if (processes != null)
                        AddToTemplatesPalette(processes);
                    break;
                case Scope.DataStore:
                    _templatesPalette.Document.Clear();
                    var dataStores =
                        _diagram.Model?.EntityTemplates?.Where(x => x.EntityType == EntityType.DataStore).OrderBy(x => x.Name);
                    if (dataStores != null)
                        AddToTemplatesPalette(dataStores);
                    break;
                case Scope.TrustBoundary:
                    _templatesPalette.Document.Clear();
                    var trustBoundaries = 
                        _diagram.Model?.TrustBoundaryTemplates?.OrderBy(x => x.Name);
                    if (trustBoundaries != null)
                        AddToTemplatesPalette(trustBoundaries);
                    break;
            }
        }

        private void AddToTemplatesPalette([NotNull] IEnumerable<IItemTemplate> templates)
        {
            List<GoSimpleNode> nodes = new List<GoSimpleNode>();
            float width = 0f;
            float height = 0f;

            var toShow = templates.ToArray();

            if (toShow.Any())
            {
                foreach (var template in toShow)
                {
                    nodes.Add(CreateNode(template.Id.ToString(), template.Name, 
                        template.GetImage(_imageSize), ref width, ref height));
                }

                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].Position = CalculatePosition(nodes[i], i, width, height);
                    _templatesPalette.Document.Add(nodes[i]);
                }
            }
        }
        #endregion

        #region Existing Items Palette.
        private void InitializeExistingPalette()
        {
            _graph.CreateIdentity += GraphOnCreateIdentity;
        }

        private void GraphOnCreateIdentity(Guid id, PointF point, GraphGroup graphGroup = null)
        {
            Stack<GraphGroup> groups = new Stack<GraphGroup>();

            var entity = _diagram.Model?.GetEntity(id);
            if (entity != null)
            {
                RecurseEntityCreation(entity, point, groups);
            }
            else
            {
                var group = _diagram.Model?.GetGroup(id);
                if (group != null)
                {
                    RecurseGroupCreation(group, point, groups);
                }
            }

            while (groups.Count > 0)
            {
                var current = groups.Pop();
                current.RefreshBorder();
            }
        }

        private void RecurseEntityCreation([NotNull] IEntity entity, PointF point, [NotNull] Stack<GraphGroup> groups)
        {
            ExistingPaletteRemove(entity);

            if (entity.Parent != null)
            {
                var group = GetGroup(entity.Parent);
                if (group == null)
                {
                    var parentPoint = new PointF(point.X - 50, point.Y - 50);
                    RecurseGroupCreation(entity.Parent, parentPoint, groups);
                }
                else
                    RecurseExistingGroups(entity.Parent, group, groups);
            }

            IShape shape = _diagram.AddShape(entity, point); 
            if (shape != null)
                AddShape(shape);
        }

        private void RecurseGroupCreation([NotNull] IGroup group, PointF point, [NotNull] Stack<GraphGroup> groups)
        {
            ExistingPaletteRemove(group);

            if (group is ITrustBoundary trustBoundary)
            {
                if (trustBoundary.Parent != null)
                {
                    var parentGroup = GetGroup(trustBoundary.Parent);
                    if (parentGroup == null)
                    {
                        var parentPoint = new PointF(point.X - 50, point.Y - 50);
                        RecurseGroupCreation(trustBoundary.Parent, parentPoint, groups);
                    }
                    else
                        RecurseExistingGroups(trustBoundary.Parent, parentGroup, groups);
                }

                var shape = _diagram.AddShape(trustBoundary, point, new SizeF(600, 300));
                if (shape != null && AddShape(shape) is GraphGroup currentGroup)
                    groups.Push(currentGroup);

            }
        }

        private void RecurseExistingGroups([NotNull] IGroup group, [NotNull] Stack<GraphGroup> groups)
        {
            var graphGroup = GetGroup(group);
            if (graphGroup != null)
                RecurseExistingGroups(group, graphGroup, groups);
        }

        private void RecurseExistingGroups([NotNull] IGroup group, 
            [NotNull] GraphGroup graphGroup, [NotNull] Stack<GraphGroup> groups)
        {
            groups.Push(graphGroup);

            if (group is ITrustBoundary trustBoundary && trustBoundary.Parent != null)
                RecurseExistingGroups(trustBoundary.Parent, groups);
        }

        private bool ExistingPaletteContains([NotNull] IIdentity identity)
        {
            return _existingPalette.Document.FindNode(identity.Id.ToString()) != null;
        }

        private bool ExistingPaletteRemove([NotNull] IIdentity identity)
        {
            var node = _existingPalette.Document.FindNode(identity.Id.ToString());

            return _existingPalette.Document.Remove(node);
        }

        private void _existingTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckRefresh();
        }

        private void _existingRefresh_Click(object sender, EventArgs e)
        {
            CheckRefresh();
        }

        private void CheckRefresh()
        {
            if (_existingTypes.SelectedItem == _existingExternalInteractor)
                RefreshExistingPalette(Scope.ExternalInteractor);
            else if (_existingTypes.SelectedItem == _existingProcess)
                RefreshExistingPalette(Scope.Process);
            else if (_existingTypes.SelectedItem == _existingDataStore)
                RefreshExistingPalette(Scope.DataStore);
            else if (_existingTypes.SelectedItem == _existingTrustBoundary)
                RefreshExistingPalette(Scope.TrustBoundary);
        }

        private void RefreshExistingPalette(Scope scope)
        {
            switch (scope)
            {
                case Scope.ExternalInteractor:
                    _existingPalette.Document.Clear();
                    var externalInteractors =
                        _diagram.Model?.Entities?.OfType<IExternalInteractor>().OrderBy(x => x.Name);
                    if (externalInteractors != null)
                        AddEntitiesToExistingPalette(externalInteractors);
                    break;
                case Scope.Process:
                    _existingPalette.Document.Clear();
                    var processes =
                        _diagram.Model?.Entities?.OfType<IProcess>().OrderBy(x => x.Name);
                    if (processes != null)
                        AddEntitiesToExistingPalette(processes);
                    break;
                case Scope.DataStore:
                    _existingPalette.Document.Clear();
                    var dataStores =
                        _diagram.Model?.Entities?.OfType<IDataStore>().OrderBy(x => x.Name);
                    if (dataStores != null)
                        AddEntitiesToExistingPalette(dataStores);
                    break;
                case Scope.TrustBoundary:
                    _existingPalette.Document.Clear();
                    var trustBoundaries =
                        _diagram.Model?.Groups?.OfType<ITrustBoundary>().OrderBy(x => x.Name);
                    if (trustBoundaries != null)
                        AddEntitiesToExistingPalette(trustBoundaries);
                    break;
            }
        }

        private void AddEntitiesToExistingPalette([NotNull] IEnumerable<IIdentity> entities)
        {
            List<GoSimpleNode> nodes = new List<GoSimpleNode>();
            float width = 0f;
            float height = 0f;

            var toShow = entities.Where(x => (_diagram.Entities?.All(y => y.AssociatedId != x.Id) ?? true) &&
                                             (_diagram.Groups?.All(y => y.AssociatedId != x.Id) ?? true)).ToArray();

            if (toShow.Any())
            {
                foreach (var entity in toShow)
                {
                    nodes.Add(CreateNode(entity.Id.ToString(), entity.Name, 
                        entity.GetImage(_imageSize), ref width, ref height));
                }

                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].Position = CalculatePosition(nodes[i], i, width, height);
                    _existingPalette.Document.Add(nodes[i]);
                }
            }
        }
        #endregion

        #region Threat Types Palette.
        private GraphThreatTypeNode CreateNode([NotNull] IThreatType threatType, ref float width, ref float height)
        {
            var result = new GraphThreatTypeNode(threatType);
            width = Math.Max(width, result.Width);
            height = Math.Max(height, result.Height);

            return result;
        }

        private void _threatsSearch_Click(object sender, EventArgs e)
        {
            RefreshThreatsPalette(_threatsFilter.Text);
        }

        private void RefreshThreatsPalette(string filter)
        {
            _threatsPalette.Document.Clear();
            IEnumerable<IThreatType> threats;
            if (string.IsNullOrWhiteSpace(filter))
            {
                threats = _diagram.Model?.ThreatTypes?.OrderBy(x => x.Name).ToArray();
            }
            else
            {
                threats = _diagram.Model?.SearchThreatTypes(filter);
            }

            if (threats?.Any() ?? false)
                AddThreatsToExistingPalette(threats);
        }
        
        private void AddThreatsToExistingPalette([NotNull] IEnumerable<IThreatType> threats)
        {
            List<GraphThreatTypeNode> nodes = new List<GraphThreatTypeNode>();
            float width = 0f;
            float height = 0f;

            foreach (var threat in threats)
            {
                nodes.Add(CreateNode(threat, ref width, ref height));             
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                var position = CalculatePosition(nodes[i], i, width, height);
                nodes[i].Position = new PointF(10, position.Y);
                _threatsPalette.Document.Add(nodes[i]);
            }
        }
        #endregion
    }
}