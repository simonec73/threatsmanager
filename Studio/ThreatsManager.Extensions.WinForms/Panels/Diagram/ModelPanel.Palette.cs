using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Layout;
using Northwoods.Go;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Diagrams;
using ThreatsManager.Extensions.Panels.DiagramConfiguration;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    public partial class ModelPanel
    {
        #region Standard Palette.
        private void InitializeStandardPalette()
        {
            _graph.CreateExternalInteractor += GraphOnCreateExternalInteractor;
            _graph.CreateProcess += GraphOnCreateProcess;
            _graph.CreateDataStore += GraphOnCreateDataStore;
            _graph.CreateTrustBoundary += GraphOnCreateTrustBoundary;
        }

        private void LoadStandardPalette()
        {
            _standardPalette.Document.Clear();

            float width = 0;
            float height = 0;
            var ei = CreateNode("ExternalInteractor", "External Interactor", 
                EntityType.ExternalInteractor.GetEntityImage(_imageSize),
                ref width, ref height);
            var p = CreateNode("Process", "Process",
                EntityType.Process.GetEntityImage(_imageSize),
                ref width, ref height);
            var ds = CreateNode("DataStore", "Data Store",
                EntityType.DataStore.GetEntityImage(_imageSize),
                ref width, ref height);
            var tb = CreateNode("TrustBoundary", "Trust Boundary",
                ImageExtensions.GetTrustBoundaryImage(_imageSize),
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
                    Size = new SizeF(_iconSize, _iconSize),
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
            IEntityShape shape = null;

            if (!UndoRedoManager.IsUndoing && !UndoRedoManager.IsRedoing)
            {
                using (var scope = UndoRedoManager.OpenScope("Create External Interactor"))
                {
                    var interactor = _diagram.Model?.AddEntity<IExternalInteractor>();
                    if (interactor != null)
                    {
                        if (graphGroup?.GroupShape?.Identity is IGroup group)
                            interactor.SetParent(group);
                        shape = _diagram.AddShape(interactor, point);
                        scope?.Complete();
                    }
                }
            }

            if (shape != null)
            {
                var node = AddShape(shape);
                if (node != null)
                {
                    _graph.Selection.Clear();
                    _graph.Selection.Add(node);
                }
            }
        }

        private void GraphOnCreateProcess(PointF point, GraphGroup graphGroup)
        {
            if (!UndoRedoManager.IsUndoing && !UndoRedoManager.IsRedoing)
            {
                using (var scope = UndoRedoManager.OpenScope("Create Process"))
                {
                    var process = _diagram.Model?.AddEntity<IProcess>();
                    if (process != null)
                    {
                        if (graphGroup?.GroupShape?.Identity is IGroup group)
                            process.SetParent(group);
                        var node = AddShape(_diagram.AddShape(process, point));
                        if (node != null)
                        {
                            _graph.Selection.Clear();
                            _graph.Selection.Add(node);
                        }
                        scope?.Complete();
                    }
                }
            }
        }

        private void GraphOnCreateDataStore(PointF point, GraphGroup graphGroup)
        {
            if (!UndoRedoManager.IsUndoing && !UndoRedoManager.IsRedoing)
            {
                using (var scope = UndoRedoManager.OpenScope("Create Data Store"))
                {
                    var dataStore = _diagram.Model?.AddEntity<IDataStore>();
                    if (dataStore != null)
                    {
                        if (graphGroup?.GroupShape?.Identity is IGroup group)
                            dataStore.SetParent(group);
                        var node = AddShape(_diagram.AddShape(dataStore, point));
                        if (node != null)
                        {
                            _graph.Selection.Clear();
                            _graph.Selection.Add(node);
                        }
                        scope?.Complete();
                    }
                }
            }
        }

        private void GraphOnCreateTrustBoundary(PointF point, GraphGroup graphGroup)
        {
            if (!UndoRedoManager.IsUndoing && !UndoRedoManager.IsRedoing)
            {
                using (var scope = UndoRedoManager.OpenScope("Create Trust Boundary"))
                {
                    var newGroup = _diagram.Model?.AddGroup<ITrustBoundary>();
                    if (newGroup is ITrustBoundary trustBoundary)
                    {
                        if (graphGroup?.GroupShape?.Identity is IGroup group)
                            trustBoundary.SetParent(group);
                        var node = AddShape(_diagram.AddShape(trustBoundary, point, new SizeF(600, 300)));
                        if (node != null)
                        {
                            _graph.Selection.Clear();
                            _graph.Selection.Add(node);
                        }
                        scope?.Complete();
                    }
                }
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

            if (!UndoRedoManager.IsUndoing && !UndoRedoManager.IsRedoing)
            {
                using (var scope = UndoRedoManager.OpenScope("Create object from Template"))
                {
                    var template = _diagram.Model?.GetEntityTemplate(id);
                    if (template != null)
                    {
                        var entity = template.CreateEntity(template.Name);
                        if (entity is IGroupElement groupElement && graphGroup?.GroupShape?.Identity is IGroup group)
                            groupElement.SetParent(group);
                        var node = AddShape(_diagram.AddShape(entity, point));
                        if (node != null)
                        {
                            _graph.Selection.Clear();
                            _graph.Selection.Add(node);
                        }
                        scope?.Complete();
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
                            if (node != null)
                            {
                                _graph.Selection.Clear();
                                _graph.Selection.Add(node);
                            }
                            scope?.Complete();
                        }
                    }
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

            if (!UndoRedoManager.IsUndoing && !UndoRedoManager.IsRedoing)
            {
                using (var scope = UndoRedoManager.OpenScope("Add shape for existing object"))
                {
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

                    scope?.Complete();
                }
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

        #region Add Palettes using Palette Providers.
        private void AddPalettes()
        {
            var paletteProviders = ExtensionUtils.GetExtensions<IPaletteProvider>().ToArray();
            if (paletteProviders?.Any() ?? false)
            {
                this.SuspendLayout();

                foreach (var provider in paletteProviders)
                {
                    var palette = CreatePalette(provider);
                    RefreshPaletteContent(palette, provider);
                }

                this.ResumeLayout();
            }
        }

        private void ConfigurePanelItemContextMenu()
        {
            if (_actions?.Any() ?? false)
            {
                var configuration = new DiagramConfigurationManager(_diagram.Model);
                if (configuration != null)
                {
                    var extensionId = configuration.MarkerExtension;
                    var factory = ExtensionUtils.GetExtension<IMarkerProviderFactory>(extensionId);
                    PanelItemListForm.SetActions(_actions
                        .Where(x => factory.ContextScope != Scope.Undefined && x.Scope.HasFlag(factory.ContextScope)));
                }
            }
        }

        private GraphPalette CreatePalette([NotNull] IPaletteProvider provider)
        {
            var paletteTabControlPanel = new SuperTabControlPanel();
            var paletteTabItem = new SuperTabItem();
            var paletteLayoutControl = new LayoutControl();
            var palette = new GraphPalette();
            var filterBox = new DevComponents.DotNetBar.Controls.TextBoxX();
            var searchButton = new Button();
            var filterLayoutControlItem = new LayoutControlItem();
            var searchLayoutControlItem = new LayoutControlItem();

            paletteLayoutControl.SuspendLayout();
            paletteTabControlPanel.SuspendLayout();

            _tabContainer.Tabs.Add(paletteTabItem);
            _tabContainer.Controls.Add(paletteTabControlPanel);

            #region paletteTabControlPanel definition.
            paletteTabControlPanel.Controls.Add(palette);
            paletteTabControlPanel.Controls.Add(paletteLayoutControl);
            paletteTabControlPanel.Dock = DockStyle.Fill;
            paletteTabControlPanel.Location = new Point(0, 40);
            paletteTabControlPanel.Name = $"{provider.Name.Replace(" ", "")}TabControlPanel";
            paletteTabControlPanel.Size = new Size(179, 454);
            paletteTabControlPanel.TabIndex = 0;
            paletteTabControlPanel.TabItem = paletteTabItem;
            #endregion

            #region palette definition.
            palette.AllowDelete = false;
            palette.AllowEdit = false;
            palette.AllowInsert = false;
            palette.AllowLink = false;
            palette.AllowMove = false;
            palette.AllowReshape = false;
            palette.AllowResize = false;
            palette.ArrowMoveLarge = 10F;
            palette.ArrowMoveSmall = 1F;
            palette.AutomaticLayout = false;
            palette.AutoScrollRegion = new Size(0, 0);
            palette.BackColor = Color.White;
            palette.Dock = DockStyle.Fill;
            palette.GridCellSizeHeight = 58F;
            palette.GridCellSizeWidth = 52F;
            palette.GridOriginX = 20F;
            palette.GridOriginY = 5F;
            palette.HidesSelection = true;
            palette.Location = new Point(0, 35);
            palette.Name = $"{provider.Name.Replace(" ", "")}Palette";
            palette.ShowHorizontalScrollBar = Northwoods.Go.GoViewScrollBarVisibility.Hide;
            palette.ShowsNegativeCoordinates = false;
            palette.ShowVerticalScrollBar = Northwoods.Go.GoViewScrollBarVisibility.Show;
            palette.Size = new Size(179, 419);
            palette.TabIndex = 2;
            palette.Text = provider.Name;
            palette.MouseEnter += new EventHandler(this.palette_MouseEnter);
            palette.Tag = provider;
            #endregion

            #region paletteLayoutControl definition.
            paletteLayoutControl.BackColor = Color.White;
            paletteLayoutControl.Controls.Add(searchButton);
            paletteLayoutControl.Controls.Add(filterBox);
            paletteLayoutControl.Dock = DockStyle.Top;
            paletteLayoutControl.ForeColor = Color.Black;
            paletteLayoutControl.Location = new Point(0, 0);
            paletteLayoutControl.Name = $"{provider.Name.Replace(" ", "")}ToolsPanel";
            paletteLayoutControl.RootGroup.Items.AddRange(new LayoutItemBase[] {
            filterLayoutControlItem,
            searchLayoutControlItem});
            paletteLayoutControl.Size = new Size(179, 35);
            paletteLayoutControl.TabIndex = 1;
            #endregion

            #region searchButton definition.
            searchButton.Image = Properties.Resources.nav_refresh_small;
            searchButton.Location = new Point(147, 4);
            searchButton.Margin = new System.Windows.Forms.Padding(0);
            searchButton.Name = $"{provider.Name.Replace(" ", "")}Search";
            searchButton.Size = new Size(28, 27);
            searchButton.TabIndex = 1;
            searchButton.UseVisualStyleBackColor = true;
            searchButton.Click += new EventHandler(search_Click);
            searchButton.Tag = filterBox;
            #endregion

            #region filterBox definition.
            filterBox.BackColor = Color.White;
            filterBox.Border.Class = "TextBoxBorder";
            filterBox.Border.CornerType = eCornerType.Square;
            filterBox.DisabledBackColor = Color.White;
            filterBox.ForeColor = Color.Black;
            filterBox.Location = new Point(4, 4);
            filterBox.Margin = new System.Windows.Forms.Padding(0);
            filterBox.Name = $"{provider.Name.Replace(" ", "")}Filter";
            filterBox.PreventEnterBeep = true;
            filterBox.Size = new Size(135, 20);
            filterBox.TabIndex = 0;
            filterBox.WatermarkText = "Please specify the text to search";
            filterBox.KeyPress += new KeyPressEventHandler(filter_KeyPress);
            filterBox.Tag = palette;
            #endregion

            #region filterLayoutControlItem definition.
            filterLayoutControlItem.Control = filterBox;
            filterLayoutControlItem.Height = 35;
            filterLayoutControlItem.MinSize = new Size(120, 0);
            filterLayoutControlItem.Name = $"{provider.Name.Replace(" ", "")}FilterLCI";
            filterLayoutControlItem.TextVisible = false;
            filterLayoutControlItem.Width = 80;
            filterLayoutControlItem.WidthType = eLayoutSizeType.Percent;
            #endregion

            #region searchLayoutControlItem definition.
            searchLayoutControlItem.Control = searchButton;
            searchLayoutControlItem.Height = 31;
            searchLayoutControlItem.MinSize = new Size(32, 20);
            searchLayoutControlItem.Name = $"{provider.Name.Replace(" ", "")}SearchLCI";
            searchLayoutControlItem.Width = 20;
            searchLayoutControlItem.WidthType = eLayoutSizeType.Percent;
            #endregion

            #region paletteTabItem definition.
            paletteTabItem.AttachedControl = paletteTabControlPanel;
            paletteTabItem.CloseButtonVisible = false;
            paletteTabItem.GlobalItem = false;
            paletteTabItem.Image = provider.Icon;
            paletteTabItem.Name = $"{provider.Name.Replace(" ", "")}TabItem";
            paletteTabItem.Text = " ";
            paletteTabItem.Tooltip = provider.Name;
            #endregion

            paletteTabControlPanel.ResumeLayout(false);
            paletteLayoutControl.ResumeLayout(false);

            return palette;
        }

        private void RefreshPaletteContent([NotNull] GraphPalette palette, 
            [NotNull] IPaletteProvider provider,
            string filter = null)
        {
            var items = provider.GetPaletteItems(_diagram.Model, filter)?.ToArray();

            if (items?.Any() ?? false)
            {
                List<GraphPaletteItemNode> nodes = new List<GraphPaletteItemNode>();
                float width = 0f;
                float height = 0f;

                foreach (var item in items)
                {
                    nodes.Add(CreateNode(item, ref width, ref height));
                }

                for (int i = 0; i < nodes.Count; i++)
                {
                    var position = CalculatePosition(nodes[i], i, width, height);
                    nodes[i].Position = new PointF(10, position.Y);
                    palette.Document.Add(nodes[i]);
                }
            }
        }

        private GraphPaletteItemNode CreateNode([NotNull] PaletteItem item, ref float width, ref float height)
        {
            var result = new GraphPaletteItemNode(item);
            width = Math.Max(width, result.Width);
            height = Math.Max(height, result.Height);

            return result;
        }

        private void ClearPalettesEvents()
        {
            var count = _tabContainer.Tabs.Count;
            if (count > 3)
            {
                for (int i = 3; i < count; i++)
                {
                    if (_tabContainer.Tabs[i] is SuperTabItem paletteTabItem &&
                        paletteTabItem.AttachedControl is SuperTabControlPanel paletteTabControlPanel)
                    {
                        var palette = paletteTabControlPanel.Controls.OfType<GraphPalette>().FirstOrDefault();
                        if (palette != null)
                        {
                            palette.MouseEnter -= new EventHandler(palette_MouseEnter);
                        }

                        var paletteLayoutControl = paletteTabControlPanel.Controls.OfType<LayoutControl>().FirstOrDefault();
                        if (paletteLayoutControl != null)
                        {
                            var searchButton = paletteLayoutControl.Controls.OfType<Button>().FirstOrDefault();
                            if (searchButton != null)
                            {
                                searchButton.Click -= new EventHandler(search_Click);
                            }

                            var filterBox = paletteLayoutControl.Controls.OfType<TextBoxX>().FirstOrDefault();
                            if (filterBox != null)
                            {
                                filterBox.KeyPress -= new KeyPressEventHandler(filter_KeyPress);
                            }
                        }
                    }
                }
            }
        }
        
        private void palette_MouseEnter(object sender, EventArgs e)
        {
            PanelItemListForm.CloseAll();
        }

        private void search_Click(object sender, EventArgs e)
        {
            if (sender is Button searchButton && 
                searchButton.Tag is TextBoxX searchBox && 
                searchBox.Tag is GraphPalette palette && 
                palette.Tag is IPaletteProvider provider)
            {
                RefreshPaletteContent(palette, provider, searchBox.Text);
            }
        }

        private void filter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is TextBoxX searchBox &&
                searchBox.Tag is GraphPalette palette &&
                palette.Tag is IPaletteProvider provider)
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    e.Handled = true;
                    RefreshPaletteContent(palette, provider, searchBox.Text);
                }
                else if (e.KeyChar == (char)Keys.Escape)
                {
                    e.Handled = true;
                    searchBox.Text = string.Empty;
                }
            }
        }
        #endregion
    }
}