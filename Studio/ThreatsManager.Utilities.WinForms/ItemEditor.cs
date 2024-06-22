﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Layout;
using DevComponents.DotNetBar.SuperGrid;
using DevComponents.Editors;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Threading;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities.WinForms.Dialogs;

namespace ThreatsManager.Utilities.WinForms
{
    /// <summary>
    /// Item editor.
    /// </summary>
    public partial class ItemEditor : UserControl, IExecutionModeSupport
    {
        #region Private classes.
        private class Actions : Actions<object>
        {
        }

        private class Actions<T> : IActions
        {
            private T _reference;

            public Actions()
            {
                _reference = default(T);
            }

            public Actions(T reference)
            {
                _reference = reference;
            }

            public Func<Control, string, T, bool> Created { get; set; }
            public Func<Control, string, T, bool> Removed { get; set; }
            public Func<Control, string, string, T, bool> Changed { get; set; }
            public Action<Control, T> Cleared { get; set; }

            public bool RaiseCreated(Control control, string name)
            {
                return Created?.Invoke(control, name, _reference) ?? false;
            }

            public bool RaiseRemoved(Control control, string name)
            {
                return Removed?.Invoke(control, name, _reference) ?? false;
            }

            public bool RaiseChanged(Control control, string oldName, string newName)
            {
                return Changed?.Invoke(control, oldName, newName, _reference) ?? false;
            }

            public void RaiseCleared(Control control)
            {
                Cleared?.Invoke(control, _reference);
            }

            public void Dispose()
            {
                _reference = default(T);
                Created = null;
                Removed = null;
                Changed = null;
                Cleared = null;
            }
        }

        private interface IActions : IDisposable
        {
            bool RaiseCreated(Control control, string name);
            bool RaiseRemoved(Control control, string name);
            bool RaiseChanged(Control control, string oldName, string newName);
            void RaiseCleared(Control control);
        }
        #endregion

        #region Private member variables.
        private object _item;
        private object _container;
        private bool _loading;
        private bool _readonly;
        private Label _parentLabel;
        private IThreatModel _model;
        private ExecutionMode _executionMode = ExecutionMode.Expert;
        private static IEnumerable<IContextAwareAction> _actions;
        private MenuDefinition _threatEventMenuDefinition;
        private MenuDefinition _vulnerabilityMenuDefinition;
        private IThreatEvent _menuThreatEvent;
        private IVulnerability _menuVulnerability;
        private MenuDefinition _threatEventMitigationMenuDefinition;
        private MenuDefinition _vulnerabilityMitigationMenuDefinition;
        private IThreatEventMitigation _menuThreatEventMitigation;
        private IVulnerabilityMitigation _menuVulnerabilityMitigation;
        private MenuDefinition _threatTypeMitigationMenuDefinition;
        private MenuDefinition _weaknessMitigationMenuDefinition;
        private IThreatTypeMitigation _menuThreatTypeMitigation;
        private IWeaknessMitigation _menuWeaknessMitigation;
        private RichTextBoxSpellAsYouTypeAdapter _spellDescription;
        private readonly List<RichTextBoxSpellAsYouTypeAdapter> _spellAdapters = new List<RichTextBoxSpellAsYouTypeAdapter>();
        #endregion

        public event Action<Guid> OpenDiagram;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ItemEditor()
        {
            InitializeComponent();

            try
            {
                _spellAsYouType.UserDictionaryFile = SpellCheckConfig.UserDictionary;
            }
            catch
            {
                // User Dictionary File is optional. If it is not possible to create it, then let's simply block it.
                _spellAsYouType.UserDictionaryFile = null;
            }

            _spellDescription = _spellAsYouType.AddSpellCheck(_itemDescription);

            EventsDispatcher.Register("ItemChanged", ItemChangedHandler);
            EventsDispatcher.Register("DeletingItem", DeletingItemHandler);

            UndoRedoManager.Undone += RefreshOnUndoRedo;
            UndoRedoManager.Redone += RefreshOnUndoRedo;
        }

        public void SetExecutionMode(ExecutionMode mode)
        {
            _executionMode = mode;
        }

        #region Public properties.
        /// <summary>
        /// Item to be edited.
        /// </summary>
        public object Item
        {
            get => _item;
            set
            {
                if (value != _item)
                {
                    lock (this)
                    {
                        if (value != _item)
                        {
                            _loading = true;

                            try
                            {
                                DeregisterCurrentEventHandlers();
                                _item = value;
                                _container = null;

                                if (value != null)
                                {
                                    if (value is INotifyPropertyChanged notifyPropertyChanged)
                                        notifyPropertyChanged.PropertyChanged += OnPropertyChanged;

                                    if (value is IUndoable undoable && undoable.IsUndoEnabled)
                                    {
                                        undoable.Undone += OnUndone;

                                        if (value is IEntity entity)
                                            _container = entity.Model;
                                        else if (value is IDataFlow flow)
                                            _container = flow.Model;
                                        else if (value is IGroup group)
                                            _container = group.Model;
                                        else if (value is IPropertySchema propertySchema)
                                            _container = propertySchema.Model;
                                        else if (value is IDiagram diagram)
                                            _container = diagram.Model;
                                        else if (value is ISeverity severity)
                                            _container = severity.Model;
                                        else if (value is IStrength strength)
                                            _container = strength.Model;
                                        else if (value is IThreatType threatType)
                                            _container = threatType.Model;
                                        else if (value is IMitigation mitigation)
                                            _container = mitigation.Model;
                                        else if (value is IThreatActor threatActor)
                                            _container = threatActor.Model;
                                        else if (value is IEntityTemplate entityTemplate)
                                            _container = entityTemplate.Model;
                                        else if (value is IFlowTemplate flowTemplate)
                                            _container = flowTemplate.Model;
                                        else if (value is ITrustBoundaryTemplate trustBoundaryTemplate)
                                            _container = trustBoundaryTemplate.Model;
                                        else if (value is IThreatEvent threatEvent)
                                            _container = threatEvent.Parent;
                                        else if (value is IWeakness weakness)
                                            _container = weakness.Model;
                                        else if (value is IVulnerability vulnerability)
                                            _container = vulnerability.Parent;
                                        else if (value is IEntityShape entityShape && entityShape.Identity is IThreatModelChild child1)
                                        {
                                            var diagrams = child1.Model?.Diagrams?.ToArray();
                                            if (diagrams?.Any() ?? false)
                                            {
                                                foreach (var d in diagrams)
                                                {
                                                    if (d.Entities.Contains(entityShape))
                                                    {
                                                        _container = d;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else if (value is IGroupShape groupShape && groupShape.Identity is IThreatModelChild child2)
                                        {
                                            var diagrams = child2.Model?.Diagrams?.ToArray();
                                            if (diagrams?.Any() ?? false)
                                            {
                                                foreach (var d in diagrams)
                                                {
                                                    if (d.Groups.Contains(groupShape))
                                                    {
                                                        _container = d;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else if (value is ILink link && link.DataFlow is IThreatModelChild child3)
                                        {
                                            var diagrams = child3.Model?.Diagrams?.ToArray();
                                            if (diagrams?.Any() ?? false)
                                            {
                                                foreach (var d in diagrams)
                                                {
                                                    if (d.Links.Contains(link))
                                                    {
                                                        _container = d;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else if (value is IThreatTypeMitigation ttMitigation)
                                        {
                                            _container = ttMitigation.ThreatType;
                                        }
                                        else if (value is IThreatEventMitigation teMitigation)
                                        {
                                            _container = teMitigation.ThreatEvent;
                                        }
                                        else if (value is IThreatEventScenario teScenario)
                                        {
                                            _container = teScenario.ThreatEvent;
                                        }
                                        else if (value is IWeaknessMitigation wMitigation)
                                        {
                                            _container = wMitigation.Weakness;
                                        }
                                        else if (value is IVulnerabilityMitigation vMitigation)
                                        {
                                            _container = vMitigation.Vulnerability;
                                        }

                                        if (_container is IUndoable containerUndoable)
                                            containerUndoable.Undone += OnContainerUndone;
                                    }

                                    if (value is IGroupElement groupElement)
                                    {
                                        groupElement.ParentChanged += GroupElementOnParentChanged;
                                        var parent = groupElement.Parent;
                                        if (parent is INotifyPropertyChanged parentNotifyPropertyChanged)
                                            parentNotifyPropertyChanged.PropertyChanged += OnParentPropertyChanged;
                                    }

                                    if (value is IThreatModelChild threatModelChild)
                                    {
                                        if (threatModelChild.Model is IThreatModel model)
                                        {
                                            if (model != _model)
                                            {
                                                if (_model != null)
                                                {
                                                    _model.ChildPropertyAdded -= ChildPropertyAdded;
                                                    _model.ChildPropertyValueChanged -= ChildPropertyChanged;
                                                    _model.ChildPropertyRemoved -= ChildPropertyRemoved;
                                                }

                                                _model = model;
                                                _model.ChildPropertyAdded += ChildPropertyAdded;
                                                _model.ChildPropertyValueChanged += ChildPropertyChanged;
                                                _model.ChildPropertyRemoved += ChildPropertyRemoved;

                                            }
                                        }
                                        else
                                        {
                                            if (_model != null)
                                            {
                                                _model.ChildPropertyAdded -= ChildPropertyAdded;
                                                _model.ChildPropertyValueChanged -= ChildPropertyChanged;
                                                _model.ChildPropertyRemoved -= ChildPropertyRemoved;
                                                _model = null;
                                            }
                                        }
                                    }

                                    ShowItem(value);
                                }
                                else
                                {
                                    ClearDynamicLayout();
                                }

                                _itemNameLayout.Visible = value != null;
                                _itemTypeLayout.Visible = value != null;
                                _refreshLayout.Visible = value != null;
                                _itemPictureLayout.Visible = value != null;
                                _description.Visible = value != null;
                            }
                            finally
                            {
                                _loading = false;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Flag to specify if the editor should be in Read Only mode.
        /// </summary>
        /// <remarks>It should be configured before setting the Item.</remarks>
        public bool ReadOnly
        {
            get => _readonly;
            set
            {
                _readonly = value;
                _itemName.ReadOnly = value;
                _itemDescription.ReadOnly = value;
            }
        }
        #endregion

        #region Events.
        private void DeregisterCurrentEventHandlers()
        {
            if (_item != null)
            {
                if (_spellAdapters.Any())
                {
                    foreach (var adapter in _spellAdapters)
                        adapter.Dispose();
                    _spellAdapters.Clear();
                }

                if (_item is INotifyPropertyChanged notifyPropertyChanged)
                    notifyPropertyChanged.PropertyChanged -= OnPropertyChanged;

                if (_item is IUndoable undoable)
                    undoable.Undone -= OnUndone;

                if (_container is IUndoable containerUndoable)
                    containerUndoable.Undone -= OnContainerUndone;

                if (_item is IGroupElement groupElement)
                {
                    groupElement.ParentChanged -= GroupElementOnParentChanged;
                    if (groupElement.Parent is INotifyPropertyChanged parentNotifyPropertyChanged)
                        parentNotifyPropertyChanged.PropertyChanged -= OnParentPropertyChanged;
                }

                if (_item is IThreatEventsContainer threatEventsContainer)
                {
                    threatEventsContainer.ThreatEventAdded -= ThreatEventAdded;
                    threatEventsContainer.ThreatEventRemoved -= ThreatEventRemoved;

                    if (_threatEventMenuDefinition != null)
                        _threatEventMenuDefinition.MenuClicked -= OnThreatEventMenuClicked;
                }

                if (_item is IVulnerabilitiesContainer vulnerabilitiesContainer)
                {
                    vulnerabilitiesContainer.VulnerabilityAdded -= VulnerabilityAdded;
                    vulnerabilitiesContainer.VulnerabilityRemoved -= VulnerabilityRemoved;

                    if (_vulnerabilityMenuDefinition != null)
                        _vulnerabilityMenuDefinition.MenuClicked -= OnVulnerabilityMenuClicked;
                }

                if (_item is IThreatModel model)
                {
                    model.ContributorAdded -= ContributorAdded;
                    model.ContributorRemoved -= ContributorRemoved;
                    model.AssumptionAdded -= AssumptionAdded;
                    model.AssumptionRemoved -= AssumptionRemoved;
                    model.DependencyAdded -= DependencyAdded;
                    model.DependencyRemoved -= DependencyRemoved;
                }

                if (_item is IDataFlow dataFlow)
                {
                    if (dataFlow.Source is IEntity source)
                    {
                        if (source is INotifyPropertyChanged sourceNotifyPropertyChanged)
                            sourceNotifyPropertyChanged.PropertyChanged -= OnSourcePropertyChanged;
                        source.ImageChanged -= OnSourceImageChanged;
                    }
                    var labelSource = GetControl("Flow", "Source");
                    if (labelSource != null)
                        _superTooltip.SetSuperTooltip(labelSource, null);

                    if (dataFlow.Target is IEntity target)
                    {
                        if (target is INotifyPropertyChanged targetNotifyPropertyChanged)
                            targetNotifyPropertyChanged.PropertyChanged -= OnTargetPropertyChanged;
                        target.ImageChanged -= OnTargetImageChanged;
                    }
                    var labelTarget = GetControl("Flow", "Target");
                    if (labelTarget != null)
                        _superTooltip.SetSuperTooltip(labelTarget, null);
                }

                if (_item is IEntity entity)
                {
                    entity.ImageChanged -= OnImageChanged;
                }

                if (_item is IEntityTemplate entityTemplate)
                {
                    entityTemplate.ImageChanged -= OnTemplateImageChanged;
                }

                if (_item is IThreatEvent threatEvent)
                {
                    if (threatEvent.Parent is IEntity parent)
                    {
                        parent.ImageChanged -= OnThreatEventImageChanged;
                    }
                    threatEvent.ThreatEventMitigationAdded -= ThreatEventMitigationAdded;
                    threatEvent.ThreatEventMitigationRemoved -= ThreatEventMitigationRemoved;
                    if (threatEvent.Parent is INotifyPropertyChanged threatEventParentNotifyPropertyChanged)
                        threatEventParentNotifyPropertyChanged.PropertyChanged -= OnThreatEventParentPropertyChanged;
                    var labetThreatEvent = GetControl("Threat Event", "Associated To");
                    if (labetThreatEvent != null)
                        _superTooltip.SetSuperTooltip(labetThreatEvent, null);
                }

                if (_item is IThreatType threatType)
                {
                    threatType.ThreatTypeMitigationAdded -= ThreatTypeMitigationAdded;
                    threatType.ThreatTypeMitigationRemoved -= ThreatTypeMitigationRemoved;
                }

                if (_item is IThreatEventMitigation mitigation)
                {
                    if (mitigation.ThreatEvent.Parent is IEntity mitigationEntity)
                    {
                        mitigationEntity.ImageChanged -= OnThreatEventMitigationImageChanged;
                    }
                    if (mitigation.ThreatEvent.Parent is INotifyPropertyChanged mtpNotifyPropertyChanged)
                        mtpNotifyPropertyChanged.PropertyChanged -= OnThreatEventParentPropertyChanged;
                    var labelThreatEventMitigation = GetControl("Threat Event Mitigation", "Associated To");
                    if (labelThreatEventMitigation != null)
                        _superTooltip.SetSuperTooltip(labelThreatEventMitigation, null);
                }

                var panels = _dynamicLayout.Controls.OfType<ExpandablePanel>().ToArray();
                if (panels.Any())
                {
                    foreach (var panel in panels)
                    {
                        var layoutControl = panel.Controls.OfType<LayoutControl>().FirstOrDefault();
                        var controls = layoutControl?.Controls.OfType<Control>().ToArray();
                        if (controls?.Any() ?? false)
                        {
                            foreach (var control in controls)
                            {
                                if (control is TextBox textBox)
                                {
                                    ClearEventInvocations(control, "TextChanged");
                                } else if (control is RichTextBox richTextBox)
                                {
                                    ClearEventInvocations(control, "TextChanged");
                                } else if (control is SwitchButton)
                                {
                                    ClearEventInvocations(control, "ValueChanged");
                                } else if (control is IntegerInput)
                                {
                                    ClearEventInvocations(control, "ValueChanged");
                                } else if (control is DoubleInput)
                                {
                                    ClearEventInvocations(control, "ValueChanged");
                                } else if (control is TokenEditor tokenEditor)
                                {
                                    ClearEventInvocations(tokenEditor.EditTextBox, "KeyPress");
                                    ClearEventInvocations(control, "SelectedTokensChanged");
                                } else if (control is ComboBox)
                                {
                                    ClearEventInvocations(control, "SelectedIndexChanged");
                                } else if (control is Button)
                                {
                                    ClearEventInvocations(control, "Click");
                                } else if (control is SuperGridControl grid)
                                {
                                    var rows = grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
                                    if (rows.Any())
                                    {
                                        foreach (var row in rows)
                                        {
                                            ClearEventInvocations(row.Cells[0], "PropertyChanged");
                                        }
                                    }
                                } else if (control is ListBox)
                                {
                                    ClearEventInvocations(control, "DoubleClick");
                                }
                            }
                        }

                        var layoutItems = layoutControl?.RootGroup.Items.OfType<LayoutControlItem>().ToArray();
                        if (layoutItems?.Any() ?? false)
                        {
                            foreach (var layoutItem in layoutItems)
                            {
                                ClearEventInvocations(layoutItem, "MarkupLinkClick");
                            }
                        }
                    }
                }
            }

            if (_model != null)
            {
                _model.ChildPropertyAdded -= ChildPropertyAdded;
                _model.ChildPropertyValueChanged -= ChildPropertyChanged;
                _model.ChildPropertyRemoved -= ChildPropertyRemoved;
            }

            var releasers = _releasers.ToArray();
            if (releasers.Any())
            {
                foreach (var releaser in releasers)
                {
                    releaser.Release();
                }
            }

            _releasers.Clear();
        }

        private void ClearEventInvocations([NotNull] object item, [Required] string eventName)
        {
            var type = item.GetType();
            FieldInfo field = null;
            while (type != null)
            {
                field = type.GetField(eventName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
                if (field != null && (field.FieldType == typeof(MulticastDelegate) || field.FieldType.IsSubclassOf(typeof(MulticastDelegate))))
                    break;

                field = type.GetField("EVENT_" + eventName.ToUpper(), BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
                if (field != null)
                    break;
                type = type.BaseType;
            }

            if (field != null)
                field.SetValue(item, null);
        }

        private void GroupElementOnParentChanged(IGroupElement item, IGroup oldParent, IGroup newParent)
        {
            if (item == _item)
            {
                if (oldParent is INotifyPropertyChanged oldNotifyPropertyChanged)
                    oldNotifyPropertyChanged.PropertyChanged -= OnParentPropertyChanged;
                if (newParent is INotifyPropertyChanged newNotifyPropertyChanged)
                    newNotifyPropertyChanged.PropertyChanged += OnParentPropertyChanged;
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!_loading)
            {
                if (!this.ContainsFocus)
                    ShowItem(_item);
                else
                {
                    var identity = sender as IIdentity;
                    var dataFlow = sender as IDataFlow;
                    var threatType = sender as IThreatType;
                    var threatEvent = sender as IThreatEvent;

                    // TODO: cover the other "standard" properties and objects.
                    switch (e.PropertyName)
                    {
                        case "Name":
                            if (identity != null)
                                _itemName.Text = identity.Name;
                            break;
                        case "Description":
                            if (identity != null)
                                _itemDescription.Text = identity.Description;
                            break;
                        case "FlowType":
                            if (dataFlow != null && GetControl("Flow", "Flow Type") is ComboBox comboFlow)
                            {
                                comboFlow.SelectedItem = dataFlow.FlowType.GetEnumLabel();
                            }
                            break;
                        case "Severity":
                            if (threatType != null && GetControl("Threat Type", "Severity") is ComboBox comboTT)
                            {
                                comboTT.SelectedItem = threatType.Severity.Name;
                            } else if (threatEvent != null &&
                                       GetControl("Threat Event", "Severity") is ComboBox comboTE)
                            {
                                comboTE.SelectedItem = threatEvent.Severity.Name;
                            }
                            break;
                    }
                }
            }
        }

        [Dispatched]
        private void OnParentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((sender is IGroup group) && (_item is IGroupElement element) && (group.Id == element.ParentId))
            {
                switch (e.PropertyName)
                {
                    case "Name":
                        _parentLabel.Text = group.Name;
                        break;
                }
            }
        }

        private void OnUndone(object item, bool removed)
        {
            if (removed)
                Item = null;
            else
            {
                try
                {
                    _loading = true;
                    ShowItem(item);
                }
                finally
                {
                    _loading = false;
                }
            }
        }

        private void OnContainerUndone(object item, bool removed)
        {
            if (removed)
            {
                Item = null;
            }
            else
            {
                if (item is IThreatModel threatModel)
                {
                    if (_item is IIdentity identity && threatModel.GetIdentity(identity.Id) == null)
                        Item = null;
                    else if (_item is ISeverity severity && threatModel.GetSeverity(severity.Id) == null)
                        Item = null;
                    else if (_item is IStrength strength && threatModel.GetStrength(strength.Id) == null)
                        Item = null;
                }
                else if (item is IDiagram diagram)
                {
                    if (_item is IEntityShape entityShape && diagram.GetEntityShape(entityShape.AssociatedId) == null)
                        Item = null;
                    else if (_item is IGroupShape groupShape && diagram.GetGroupShape(groupShape.AssociatedId) == null)
                        Item = null;
                    else if (_item is ILink link && diagram.GetLink(link.AssociatedId) == null)
                        Item = null;
                } else if (item is IEntity entity)
                {
                    if (_item is IThreatEvent threatEvent && entity.GetThreatEvent(threatEvent.Id) == null)
                        Item = null;
                    else if (_item is IVulnerability vulnerability && entity.GetVulnerability(vulnerability.Id) == null)
                        Item = null;
                } else if (item is IDataFlow flow)
                {
                    if (_item is IThreatEvent threatEvent && flow.GetThreatEvent(threatEvent.Id) == null)
                        Item = null;
                    else if (_item is IVulnerability vulnerability && flow.GetVulnerability(vulnerability.Id) == null)
                        Item = null;
                } else if (item is IThreatEvent threatEvent)
                {
                    if (_item is IThreatEventMitigation tem && threatEvent.GetMitigation(tem.MitigationId) == null)
                        Item = null;
                    else if (_item is IThreatEventScenario tes && threatEvent.GetScenario(tes.Id) == null)
                        Item = null;
                    else if (_item is IVulnerability vulnerability && threatEvent.GetVulnerability(vulnerability.Id) == null)
                        Item = null;
                } else if (item is IThreatType threatType)
                {
                    if (_item is IThreatTypeMitigation ttm && threatType.GetMitigation(ttm.MitigationId) == null)
                        Item = null;
                    else if (_item is IThreatTypeWeakness ttw && threatType.GetWeakness(ttw.WeaknessId) == null)
                        Item = null;
                } else if (item is IVulnerability vulnerability)
                {
                    if (_item is IVulnerabilityMitigation vm && vulnerability.GetMitigation(vm.MitigationId) == null)
                        Item = null;
                } else if (item is IWeakness weakness)
                {
                    if (_item is IWeaknessMitigation wm && weakness.GetMitigation(wm.WeaknessId) == null)
                        Item = null;
                }
            }
        }

        private void ItemChangedHandler(object item)
        {
            if (item?.Equals(_item) ?? false)
            {
                Item = null;
                Item = item;
            }
        }

        private void DeletingItemHandler(object item)
        {
            if (item?.Equals(_item) ?? false)
            {
                Item = null;
            }
        }
        #endregion

        #region Showing Items.
        [Dispatched]
        private void ShowItem([NotNull] object item)
        {
            SuspendLayout();
            ClearDynamicLayout();

            var identity = item as IIdentity;
            var typeMitigation = item as IThreatTypeMitigation;
            var eventMitigation = item as IThreatEventMitigation;
            var weaknessMitigation = item as IWeaknessMitigation;
            var vulnerabilityMitigation = item as IVulnerabilityMitigation;

            if (typeMitigation != null)
            {
                identity = typeMitigation.Mitigation;
                _itemName.ReadOnly = true;
                _itemDescription.ReadOnly = true;
            }
            else if (eventMitigation != null)
            {
                _itemName.ReadOnly = true;
                _itemDescription.ReadOnly = true;
                _itemName.Text = eventMitigation.Name;
                _itemDescription.Text = eventMitigation.Description;
            }
            else if (weaknessMitigation != null)
            {
                identity = weaknessMitigation.Mitigation;
                _itemName.ReadOnly = true;
                _itemDescription.ReadOnly = true;
            }
            else if (vulnerabilityMitigation != null)
            {
                identity = vulnerabilityMitigation.Mitigation;
                _itemName.ReadOnly = true;
                _itemDescription.ReadOnly = true;
            }
            else
            {
                _itemName.ReadOnly = _readonly;
                _itemDescription.ReadOnly = _readonly;
            }

            if (identity != null)
            {
                _itemName.Text = identity.Name;
                _itemDescription.Text = identity.Description;
            }

            if (item is IVulnerabilitiesContainer vulnerabilitiesContainer)
            {
                AddVulnerabilitiesContainerSection(vulnerabilitiesContainer);
            }

            if (item is IThreatEventsContainer threatEventsContainer)
            {
                AddThreatEventsContainerSection(threatEventsContainer);
            }

            if (item is IPropertiesContainer propertiesContainer)
            {
                AddPropertiesContainerSection(propertiesContainer);
            }

            if (item is IDataFlow dataFlow)
            {
                ShowItem(dataFlow);
            }

            if (item is IDataStore dataStore)
            {
                ShowItem(dataStore);
            }

            if (item is IDiagram diagram)
            {
                ShowItem(diagram);
            }

            if (item is IEntityTemplate template)
            {
                ShowItem(template);
            }

            if (item is IFlowTemplate flowTemplate)
            {
                ShowItem(flowTemplate);
            }

            if (item is ITrustBoundaryTemplate trustBoundaryTemplate)
            {
                ShowItem(trustBoundaryTemplate);
            }

            if (item is IExternalInteractor external)
            {
                ShowItem(external);
            }

            if (item is IProcess process)
            {
                ShowItem(process);
            }

            if (item is ITrustBoundary trustBoundary)
            {
                ShowItem(trustBoundary);
            }

            if (item is IThreatModel threatModel)
            {
                ShowItem(threatModel);
            }

            if (item is IThreatEvent threatEvent)
            {
                ShowItem(threatEvent);
            }

            if (item is IVulnerability vulnerability)
            {
                ShowItem(vulnerability);
            }

            if (item is IThreatEventScenario scenario)
            {
                ShowItem(scenario);
            }

            if (item is IThreatType threatType)
            {
                ShowItem(threatType);
            }

            if (item is IWeakness weakness)
            {
                ShowItem(weakness);
            }

            if (item is IThreatActor threatActor)
            {
                ShowItem(threatActor);
            }

            if (item is IMitigation mitigation)
            {
                ShowItem(mitigation);
            }

            if (typeMitigation != null)
            {
                ShowItem(typeMitigation);
            }

            if (weaknessMitigation != null)
            {
                ShowItem(weaknessMitigation);
            }

            if (eventMitigation != null)
            {
                ShowItem(eventMitigation);
            }

            if (vulnerabilityMitigation != null)
            {
                ShowItem(vulnerabilityMitigation);
            }

            if (identity != null)
                AddInformationSection(identity);
            else if (eventMitigation != null) 
                AddInformationSection(eventMitigation);

            ResumeLayout();
        }

        private void ClearDynamicLayout()
        {
            _dynamicLayout.Controls.Clear();
        }

        private void AddInformationSection([NotNull] IIdentity identity)
        {
            var groupElement = identity as IGroupElement;
            IItemTemplate itemTemplate = null;
            if (identity is IEntity entity)
                itemTemplate = entity.Template;
            else if (identity is IDataFlow flow)
                itemTemplate = flow.Template;
            else if (identity is ITrustBoundary trustBoundary)
                itemTemplate = trustBoundary.Template;

            if (_executionMode == ExecutionMode.Pioneer || _executionMode == ExecutionMode.Expert || groupElement != null || itemTemplate != null)
            {
                var infoSection = AddSection("Information");
                infoSection.SuspendLayout();

                if (_executionMode == ExecutionMode.Pioneer || _executionMode == ExecutionMode.Expert)
                {
                    AddSingleLineLabel(infoSection, "ID", identity.Id.ToString("D"));
                    if (identity is IThreatModelChild threatModelChild)
                    {
                        AddSingleLineLabel(infoSection, "Threat Model", threatModelChild.Model?.Name);
                    }
                }

                if (groupElement != null)
                {
                    _parentLabel = AddSingleLineLabel(infoSection, "Container", groupElement.Parent?.Name);
                }

                if (itemTemplate != null)
                {
                    var template = AddHyperlink(infoSection, "Template", itemTemplate, itemTemplate.GetImage(ImageSize.Small));
                    _superTooltip.SetSuperTooltip(template, _model.GetSuperTooltipInfo(itemTemplate));
                }

                if ((_executionMode == ExecutionMode.Pioneer || _executionMode == ExecutionMode.Expert) && identity is ISourceInfo sourceInfo)
                {
                    if (!string.IsNullOrWhiteSpace(sourceInfo.VersionAuthor))
                        AddSingleLineLabel(infoSection, "Author", sourceInfo.VersionAuthor);
                    if (!string.IsNullOrWhiteSpace(sourceInfo.VersionId))
                        AddSingleLineLabel(infoSection, "Version", sourceInfo.VersionId);
                    if (!string.IsNullOrWhiteSpace(sourceInfo.SourceTMName))
                        AddSingleLineLabel(infoSection, "Source TM", sourceInfo.SourceTMName);
                }

                FinalizeSection(infoSection);

                infoSection.ResumeLayout();
            }
        }

        private void AddInformationSection([NotNull] IThreatEventMitigation eventMitigation)
        {
            if (_executionMode == ExecutionMode.Pioneer || _executionMode == ExecutionMode.Expert)
            {
                var infoSection = AddSection("Information");
                infoSection.SuspendLayout();
                AddSingleLineLabel(infoSection, "Threat Model", eventMitigation.Model?.Name);
                if (!string.IsNullOrWhiteSpace(eventMitigation.VersionAuthor))
                    AddSingleLineLabel(infoSection, "Author", eventMitigation.VersionAuthor);
                if (!string.IsNullOrWhiteSpace(eventMitigation.VersionId))
                    AddSingleLineLabel(infoSection, "Version", eventMitigation.VersionId);
                if (!string.IsNullOrWhiteSpace(eventMitigation.SourceTMName))
                    AddSingleLineLabel(infoSection, "Source TM", eventMitigation.SourceTMName);

                FinalizeSection(infoSection);

                infoSection.ResumeLayout();
            }
        }

        private void AddPropertiesContainerSection([NotNull] IPropertiesContainer container)
        {
            var schemas = GetSchemas(container.Properties);
            if (schemas != null)
            {
                foreach (var schema in schemas)
                {
                    if (schema.Visible)
                        AddSchemaSection(schema, container);
                }
            }
        }

        private void AddSchemaSection([NotNull] IPropertySchema schema, [NotNull] IPropertiesContainer container)
        {
            var requiredExecutionMode = schema.RequiredExecutionMode;
            bool show = true;
            switch (requiredExecutionMode)
            {
                case ExecutionMode.Pioneer:
                    show = _executionMode == ExecutionMode.Pioneer;
                    break;
                case ExecutionMode.Expert:
                    show = _executionMode == ExecutionMode.Pioneer ||
                           _executionMode == ExecutionMode.Expert;
                    break;
                case ExecutionMode.Simplified:
                    show = (_executionMode != ExecutionMode.Management) &&
                           (_executionMode != ExecutionMode.Business);
                    break;
                case ExecutionMode.Management:
                    show = _executionMode != ExecutionMode.Business;
                    break;
            }

            if (show)
            {
                var properties = container.Properties?
                    .Where(x => (x.PropertyType?.Visible ?? false) && x.PropertyType.SchemaId == schema.Id)
                    .OrderBy(x => x.PropertyType?.Priority ?? 0)
                    .ToArray();
                if (properties?.Any() ?? false)
                {
                    LayoutControl section = null;

                    foreach (var property in properties)
                    {
                        var ro = _readonly || property.ReadOnly || property.PropertyType.ReadOnly;

                        if (!string.IsNullOrWhiteSpace(property.PropertyType?.CustomPropertyViewer))
                        {
                            var factory =
                                ExtensionUtils.GetExtensionByLabel<IPropertyViewerFactory>(property.PropertyType
                                    .CustomPropertyViewer);
                            if (factory != null)
                            {
                                var viewer = factory.CreatePropertyViewer(container, property);
                                if (viewer != null)
                                {
                                    var blocks = viewer.Blocks;
                                    if (blocks?.Any() ?? false)
                                    {
                                        if (section == null)
                                        {
                                            section = AddSection(schema.Name);
                                            section.SuspendLayout();
                                        }

                                        section.Tag = viewer;
                                        AddPropertyViewerBlocks(section, blocks, ro);
                                    }
                                }
                            }
                        } else if (property is IPropertySingleLineString propertySingleLineString)
                        {
                            if (section == null)
                            {
                                section = AddSection(schema.Name);
                                section.SuspendLayout();
                            }
                            var text = AddSingleLineText(section, propertySingleLineString, ro);
                            var spell = _spellAsYouType.AddSpellCheck(text);
                            if (spell != null)
                                _spellAdapters.Add(spell);
                        }
                        else if (property is IPropertyString propertyString)
                        {
                            if (section == null)
                            {
                                section = AddSection(schema.Name);
                                section.SuspendLayout();
                            }
                            var richTextBox = AddText(section, propertyString, ro);
                            var spell = _spellAsYouType.AddSpellCheck(richTextBox);
                            if (spell != null)
                                _spellAdapters.Add(spell);
                        }
                        else if (property is IPropertyBool propertyBool)
                        {
                            if (section == null)
                            {
                                section = AddSection(schema.Name);
                                section.SuspendLayout();
                            }
                            AddBool(section, propertyBool, ro);
                        }
                        else if (property is IPropertyTokens propertyTokens)
                        {
                            if (section == null)
                            {
                                section = AddSection(schema.Name);
                                section.SuspendLayout();
                            }
                            AddTokens(section, propertyTokens, ro);
                        }
                        else if (property is IPropertyArray propertyArray)
                        {
                            if (section == null)
                            {
                                section = AddSection(schema.Name);
                                section.SuspendLayout();
                            }
                            AddList(section, propertyArray, ro);
                        }
                        else if (property is IPropertyDecimal propertyDecimal)
                        {
                            if (section == null)
                            {
                                section = AddSection(schema.Name);
                                section.SuspendLayout();
                            }
                            AddDecimal(section, propertyDecimal, ro);
                        }
                        else if (property is IPropertyInteger propertyInteger)
                        {
                            if (section == null)
                            {
                                section = AddSection(schema.Name);
                                section.SuspendLayout();
                            }
                            AddInteger(section, propertyInteger, ro);
                        }
                        else if (property is IPropertyIdentityReference propertyIdentityReference)
                        {
                            if (section == null)
                            {
                                section = AddSection(schema.Name);
                                section.SuspendLayout();
                            }
                            var identity = propertyIdentityReference.Value;

                            AddSingleLineLabel(section, property.PropertyType.Name,
                                identity?.Name ?? "<Undefined>", identity?.GetImage(ImageSize.Small));
                        }
                        //else if (property is IPropertyJsonSerializableObject)
                        //{
                        //    if (section == null)
                        //    {
                        //        section = AddSection(schema.Name);
                        //        section.SuspendLayout();
                        //    }
                        //    // TODO: add control to show this property. For now, it is not shown.
                        //    AddSingleLineLabel(section, property.PropertyType.Name,
                        //        "<Property is a Json Serializable Object, which is not supported by the Item Editor, yet>");
                        //}
                        else if (property is IPropertyList propertyList)
                        {
                            if (section == null)
                            {
                                section = AddSection(schema.Name);
                                section.SuspendLayout();
                            }
                            AddCombo(section, propertyList, ro);
                        }
                        //else if (property is IPropertyListMulti)
                        //{
                        //    if (section == null)
                        //    {
                        //        section = AddSection(schema.Name);
                        //        section.SuspendLayout();
                        //    }
                        //    // TODO: add control to show this property. For now, it is not shown.
                        //    AddSingleLineLabel(section, property.PropertyType.Name, property.StringValue);
                        //}
                        else if (property is IPropertyUrl propertyUrl)
                        {
                            if (section == null)
                            {
                                section = AddSection(schema.Name);
                                section.SuspendLayout();
                            }
                            AddHyperlink(section, propertyUrl, ro);
                        }
                        else if (property is IPropertyUrlList propertyUrlList)
                        {
                            if (section == null)
                            {
                                section = AddSection(schema.Name);
                                section.SuspendLayout();
                            }
                            AddHyperlinkList(section, propertyUrlList, ro);
                        }
                        else if (property is IPropertyDate propertyDate)
                        {
                            if (section == null)
                            {
                                section = AddSection(schema.Name);
                                section.SuspendLayout();
                            }
                            AddDate(section, propertyDate, ro);
                        }
                        else
                        {
                            if (section == null)
                            {
                                section = AddSection(schema.Name);
                                section.SuspendLayout();
                            }
                            AddSingleLineLabel(section, property.PropertyType.Name,
                                "<Property type is not supported by the Item Editor, yet>");
                        }
                    }

                    if (section != null)
                    {
                        FinalizeSection(section);

                        section.ResumeLayout();
                    }
                }
            }
        }

        private void AddThreatEventsContainerSection([NotNull] IThreatEventsContainer container)
        {
            var infoSection = AddSection("Threat Events");
            infoSection.SuspendLayout();
            var listBox = AddListBox(infoSection, string.Empty, null,
                container.ThreatEvents?.OrderBy(x => x.Name), AddThreatEventHandler, _readonly);
            listBox.DoubleClick += OpenSubItem;

            if (_actions?.Any() ?? false)
            {
                _threatEventMenuDefinition = new MenuDefinition(_actions, Scope.ThreatEvent);
                _threatEventMenuDefinition.MenuClicked += OnThreatEventMenuClicked;
                var menu = _threatEventMenuDefinition.CreateMenu();
                menu.Opening += OnThreatEventMenuOpening;
                listBox.ContextMenuStrip = menu;
            }

            FinalizeSection(infoSection);
            infoSection.ResumeLayout();

            container.ThreatEventAdded += ThreatEventAdded;
            container.ThreatEventRemoved += ThreatEventRemoved;
        }

        private void AddVulnerabilitiesContainerSection([NotNull] IVulnerabilitiesContainer container)
        {
            var infoSection = AddSection("Vulnerabilities");
            infoSection.SuspendLayout();
            var listBox = AddListBox(infoSection, string.Empty, null,
                container.Vulnerabilities?.OrderBy(x => x.Name), AddVulnerabilityHandler, _readonly);
            listBox.DoubleClick += OpenSubItem;

            if (_actions?.Any() ?? false)
            {
                _vulnerabilityMenuDefinition = new MenuDefinition(_actions, Scope.Vulnerability);
                _vulnerabilityMenuDefinition.MenuClicked += OnVulnerabilityMenuClicked;
                var menu = _vulnerabilityMenuDefinition.CreateMenu();
                menu.Opening += OnVulnerabilityMenuOpening;
                listBox.ContextMenuStrip = menu;
            }

            FinalizeSection(infoSection);
            infoSection.ResumeLayout();

            container.VulnerabilityAdded += VulnerabilityAdded;
            container.VulnerabilityRemoved += VulnerabilityRemoved;
        }

        private IEnumerable<IPropertySchema> GetSchemas(IEnumerable<IProperty> properties)
        {
            IEnumerable<IPropertySchema> result = null;

            if (properties?.Any() ?? false)
            {
                List<IPropertySchema> list = new List<IPropertySchema>();
                foreach (var property in properties)
                {
                    var propertyType = property.Model?.GetPropertyType(property.PropertyTypeId);
                    if (propertyType != null)
                    {
                        var schema = property.Model?.GetSchema(propertyType.SchemaId);
                        if (schema != null && !list.Contains(schema))
                        {
                            list.Add(schema);
                        }
                    }
                }

                if (list.Count > 0)
                    result = list.Where(x => x.Visible)
                        .OrderByDescending(x => x.Priority)
                        .ThenByDescending(x => x.Name);
            }

            return result;
        }

        private void ShowItem([NotNull] IDataFlow dataFlow)
        {
            _itemType.Text = "Flow";
            _itemPicture.Image = Resources.flow;

            var section = AddSection("Flow");
            section.SuspendLayout();
            if (dataFlow.Source != null)
            {
                var source = AddHyperlink(section, "Source", dataFlow.Source, dataFlow.Source.GetImage(ImageSize.Small));
                if (dataFlow.Source is INotifyPropertyChanged notifyPropertyChanged)
                    notifyPropertyChanged.PropertyChanged += OnSourcePropertyChanged;
                _superTooltip.SetSuperTooltip(source, _model.GetSuperTooltipInfo(dataFlow.Source));
                dataFlow.Source.ImageChanged += OnSourceImageChanged;
            }
            if (dataFlow.Target != null)
            {
                var target = AddHyperlink(section, "Target", dataFlow.Target, dataFlow.Target.GetImage(ImageSize.Small));
                _superTooltip.SetSuperTooltip(target, _model.GetSuperTooltipInfo(dataFlow.Target));
                if (dataFlow.Target is INotifyPropertyChanged notifyPropertyChanged)
                    notifyPropertyChanged.PropertyChanged += OnTargetPropertyChanged;
                dataFlow.Target.ImageChanged += OnTargetImageChanged;
            }
            AddCombo(section, "Flow Type", dataFlow.FlowType.GetEnumLabel(), EnumExtensions.GetEnumLabels<FlowType>(),
                ChangeFlowType, _readonly);
            FinalizeSection(section);
            section.ResumeLayout();
        }

        private void ShowItem([NotNull] IDataStore dataStore)
        {
            _itemType.Text = "Data Store";
            _itemPicture.Image = dataStore.GetImage(ImageSize.Big);
            dataStore.ImageChanged += OnImageChanged;
        }

        private void ShowItem([NotNull] IDiagram diagram)
        {
            _itemType.Text = "Diagram";
            _itemPicture.Image = Resources.model_big;
        }

        private void ShowItem([NotNull] IEntityTemplate entityTemplate)
        {
            _itemType.Text = "Entity Template";
            _itemPicture.Image = entityTemplate.GetImage(ImageSize.Big);
            entityTemplate.ImageChanged += OnTemplateImageChanged;

            var section = AddSection("Entity Template");
            section.SuspendLayout();
            AddSingleLineLabel(section, "Entity Type", entityTemplate.EntityType.GetEnumLabel());
            FinalizeSection(section);
            section.ResumeLayout();
        }

        private void ShowItem([NotNull] IFlowTemplate flowTemplate)
        {
            _itemType.Text = "Flow Template";
            _itemPicture.Image = flowTemplate.GetImage(ImageSize.Big);

            var section = AddSection("Flow Template");
            section.SuspendLayout();
            AddCombo(section, "Flow Type", flowTemplate.FlowType.GetEnumLabel(), EnumExtensions.GetEnumLabels<FlowType>(),
                ChangeFlowType, _readonly);
            FinalizeSection(section);
            section.ResumeLayout();
        }

        private void ShowItem([NotNull] ITrustBoundaryTemplate trustBounderyTemplate)
        {
            _itemType.Text = "Trust Boundary Template";
            _itemPicture.Image = trustBounderyTemplate.GetImage(ImageSize.Big);
        }

        private void ShowItem([NotNull] IExternalInteractor externalInteractor)
        {
            _itemType.Text = "External Interactor";
            _itemPicture.Image = externalInteractor.GetImage(ImageSize.Big);
            externalInteractor.ImageChanged += OnImageChanged;
        }

        private void ShowItem([NotNull] IProcess process)
        {
            _itemType.Text = "Process";
            _itemPicture.Image = process.GetImage(ImageSize.Big);
            process.ImageChanged += OnImageChanged;
        }

        private void ShowItem([NotNull] ITrustBoundary trustBoundary)
        {
            _itemType.Text = "Trust Boundary";
            _itemPicture.Image = Resources.trust_boundary_big;
        }

        private void ShowItem([NotNull] IThreatModel model)
        {
            _itemType.Text = "Threat Model";
            _itemPicture.Image = Resources.threat_model_big;

            var section = AddSection("Threat Model");
            section.SuspendLayout();
            AddSingleLineLabel(section, "Version", model.CurrentVersion?.VersionId ?? "<not defined>", 50);
            AddSingleLineLabel(section, "Version Author", model.CurrentVersion?.VersionAuthor ?? "<not defined>", 50);
            AddSingleLineText(section, "Owner", model.Owner, ChangeOwner, null, _readonly);
            var contribList = AddList(section, "Contributors", null, model.Contributors, _readonly);
            contribList.Tag = new Actions()
            {
                Created = CreateContributor,
                Changed = ChangeContributor,
                Removed = RemoveContributor,
                Cleared = ClearContributors
            };
            var assumpList = AddList(section, "Assumptions", null, model.Assumptions, _readonly);
            assumpList.Tag = new Actions()
            {
                Created = CreateAssumption,
                Changed = ChangeAssumption,
                Removed = RemoveAssumption,
                Cleared = ClearAssumptions
            };
            var depList = AddList(section, "Dependencies", null, model.ExternalDependencies, _readonly);
            depList.Tag = new Actions()
            {
                Created = CreateDependency,
                Changed = ChangeDependency,
                Removed = RemoveDependency,
                Cleared = ClearDependencies
            };

            FinalizeSection(section);
            section.ResumeLayout();

            model.ContributorAdded += ContributorAdded;
            model.ContributorRemoved += ContributorRemoved;
            model.AssumptionAdded += AssumptionAdded;
            model.AssumptionRemoved += AssumptionRemoved;
            model.DependencyAdded += DependencyAdded;
            model.DependencyRemoved += DependencyRemoved;
        }

        private void ShowItem([NotNull] IThreatEvent threatEvent)
        {
            _itemType.Text = "Threat Event";
            _itemPicture.Image = Resources.threat_event_big;

            var section = AddSection("Threat Event");
            section.SuspendLayout();
            AddHyperlink(section, "Threat Type", threatEvent.ThreatType);
            var label = AddHyperlink(section, "Associated To", threatEvent.Parent, threatEvent.Parent?.GetImage(ImageSize.Small));
            if (threatEvent.Parent is IEntity entity)
            {
                entity.ImageChanged += OnThreatEventImageChanged;
            }
            if (threatEvent.Parent != null)
                _superTooltip.SetSuperTooltip(label, _model.GetSuperTooltipInfo(threatEvent.Parent));
            AddCombo(section, "Severity", threatEvent.Severity?.Name,
                threatEvent.Model?.Severities?.Where(x => x.Visible).OrderByDescending(x => x.Id).Select(x => x.Name),
                ChangeSeverity, _readonly);
            var listBox = AddListBox(section, "Mitigations", null,
                threatEvent.Mitigations, AddThreatEventMitigationEventHandler, _readonly);
            listBox.DoubleClick += OpenSubItem;

            if (_actions?.Any() ?? false)
            {
                _threatEventMitigationMenuDefinition = new MenuDefinition(_actions, Scope.ThreatEventMitigation);
                _threatEventMitigationMenuDefinition.MenuClicked += OnThreatEventMitigationMenuClicked;
                var menu = _threatEventMitigationMenuDefinition.CreateMenu();
                menu.Opening += OnThreatEventMitigationMenuOpening;
                listBox.ContextMenuStrip = menu;
            }

            FinalizeSection(section);
            section.ResumeLayout();

            threatEvent.ThreatEventMitigationAdded += ThreatEventMitigationAdded;
            threatEvent.ThreatEventMitigationRemoved += ThreatEventMitigationRemoved;
            if (threatEvent.Parent is INotifyPropertyChanged notifyPropertyChanged)
                notifyPropertyChanged.PropertyChanged += OnThreatEventParentPropertyChanged;
        }

        private void ShowItem([NotNull] IVulnerability vulnerability)
        {
            _itemType.Text = "Vulnerability";
            _itemPicture.Image = Resources.vulnerability_big;

            var section = AddSection("Vulnerability");
            section.SuspendLayout();
            AddHyperlink(section, "Weakness", vulnerability.Weakness);
            var parent = vulnerability.Parent as IIdentity;
            if (parent != null)
            {
                var label = AddHyperlink(section, "Associated To", parent,parent?.GetImage(ImageSize.Small));
                _superTooltip.SetSuperTooltip(label, _model.GetSuperTooltipInfo(parent));
            }
            if (vulnerability.Parent is IEntity entity)
            {
                entity.ImageChanged += OnVulnerabilityImageChanged;
            }
            AddCombo(section, "Severity", vulnerability.Severity?.Name,
                vulnerability.Model?.Severities?.Where(x => x.Visible).OrderByDescending(x => x.Id).Select(x => x.Name),
                ChangeSeverity, _readonly);
            var listBox = AddListBox(section, "Mitigations", null,
                vulnerability.Mitigations, AddVulnerabilityMitigationEventHandler, _readonly);
            listBox.DoubleClick += OpenSubItem;

            if (_actions?.Any() ?? false)
            {
                _vulnerabilityMitigationMenuDefinition = new MenuDefinition(_actions, Scope.VulnerabilityMitigation);
                _vulnerabilityMitigationMenuDefinition.MenuClicked += OnVulnerabilityMitigationMenuClicked;
                var menu = _vulnerabilityMitigationMenuDefinition.CreateMenu();
                menu.Opening += OnVulnerabilityMitigationMenuOpening;
                listBox.ContextMenuStrip = menu;
            }

            FinalizeSection(section);
            section.ResumeLayout();

            vulnerability.VulnerabilityMitigationAdded += VulnerabilityMitigationAdded;
            vulnerability.VulnerabilityMitigationRemoved += VulnerabilityMitigationRemoved;
            if (vulnerability.Parent is INotifyPropertyChanged notifyPropertyChanged)
                notifyPropertyChanged.PropertyChanged += OnVulnerabilityParentPropertyChanged;
        }

        private void ShowItem([NotNull] IThreatType threatType)
        {
            _itemType.Text = "Threat Type";
            _itemPicture.Image = Resources.threat_type_big;

            var section = AddSection("Threat Type");
            section.SuspendLayout();
            AddCombo(section, "Severity", threatType.Severity?.Name,
                threatType.Model?.Severities?.Where(x => x.Visible).OrderByDescending(x => x.Id).Select(x => x.Name),
                ChangeSeverity, _readonly);
            var listBox = AddListBox(section, "Standard Mitigations", null,
                threatType.Mitigations?.OrderBy(x => x.Mitigation?.Name), AddThreatTypeStandardMitigationEventHandler, _readonly);
            listBox.DoubleClick += OpenSubItem;

            AddListView(section, "Threat Events\napplied to", null,
                threatType.Model?.GetThreatEvents(threatType)?.OrderBy(x => x.Parent.Name));

            if (_actions?.Any() ?? false)
            {
                _threatTypeMitigationMenuDefinition = new MenuDefinition(_actions, Scope.ThreatTypeMitigation);
                _threatTypeMitigationMenuDefinition.MenuClicked += OnThreatTypeMitigationMenuClicked;
                var menu = _threatTypeMitigationMenuDefinition.CreateMenu();
                menu.Opening += OnThreatTypeMitigationMenuOpening;
                listBox.ContextMenuStrip = menu;
            }

            FinalizeSection(section);
            section.ResumeLayout();

            threatType.ThreatTypeMitigationAdded += ThreatTypeMitigationAdded;
            threatType.ThreatTypeMitigationRemoved += ThreatTypeMitigationRemoved;
        }

        private void ShowItem([NotNull] IWeakness weakness)
        {
            _itemType.Text = "Weakness";
            _itemPicture.Image = Resources.weakness_big;

            var section = AddSection("Weakness");
            section.SuspendLayout();
            AddCombo(section, "Severity", weakness.Severity?.Name,
                weakness.Model?.Severities?.Where(x => x.Visible).OrderByDescending(x => x.Id).Select(x => x.Name),
                ChangeSeverity, _readonly);
            var listBox = AddListBox(section, "Standard Mitigations", null,
                weakness.Mitigations?.OrderBy(x => x.Mitigation?.Name), AddWeaknessStandardMitigationEventHandler, _readonly);
            listBox.DoubleClick += OpenSubItem;

            AddListView(section, "Vulnerabilities\napplied to", null,
                weakness.Model?.GetVulnerabilities(weakness)?.OrderBy(x => (x.Parent as IIdentity)?.Name ?? string.Empty));

            if (_actions?.Any() ?? false)
            {
                _weaknessMitigationMenuDefinition = new MenuDefinition(_actions, Scope.WeaknessMitigation);
                _weaknessMitigationMenuDefinition.MenuClicked += OnWeaknessMitigationMenuClicked;
                var menu = _weaknessMitigationMenuDefinition.CreateMenu();
                menu.Opening += OnWeaknessMitigationMenuOpening;
                listBox.ContextMenuStrip = menu;
            }

            FinalizeSection(section);
            section.ResumeLayout();

            weakness.WeaknessMitigationAdded += WeaknessMitigationAdded;
            weakness.WeaknessMitigationRemoved += WeaknessMitigationRemoved;
        }

        private void ShowItem([NotNull] IThreatEventScenario scenario)
        {
            _itemType.Text = "Threat Event Scenario";
            _itemPicture.Image = Resources.scenario_big;

            var section = AddSection("Threat Event Scenario");
            section.SuspendLayout();
            AddHyperlink(section, "Threat Event", scenario.ThreatEvent);
            AddCombo(section, "Severity", scenario.Severity?.Name,
                scenario.Model?.Severities?.OrderByDescending(x => x.Id).Select(x => x.Name),
                ChangeSeverity, _readonly);
            AddCombo(section, "Actor", scenario.Actor?.Name,
                scenario.Model?.ThreatActors?.Select(x => x.Name),
                ChangeActor, _readonly);
            AddText(section, "Motivation", scenario.Motivation, ChangeMotivation, null, _readonly);
            FinalizeSection(section);
            section.ResumeLayout();
        }

        private void ShowItem([NotNull] IThreatActor threatActor)
        {
            _itemType.Text = "Threat Actor";
            _itemPicture.Image = Resources.actor_big;

            var section = AddSection("Threat Actor");
            section.SuspendLayout();
            AddSingleLineLabel(section, "Actor Type", threatActor.ActorType.GetEnumLabel());
            FinalizeSection(section);
            section.ResumeLayout();
        }

        private void ShowItem([NotNull] IMitigation mitigation)
        {
            _itemType.Text = "Mitigation";
            _itemPicture.Image = Resources.standard_mitigations_big;

            var section = AddSection("Mitigation");
            section.SuspendLayout();
            AddCombo(section, "Control", mitigation.ControlType.ToString(),
                Enum.GetValues(typeof(SecurityControlType)).OfType<SecurityControlType>().Select(x => x.GetEnumLabel()),
                SecurityControlTypeChanged, _readonly);

            FinalizeSection(section);
            section.ResumeLayout();
        }

        private void ShowItem([NotNull] IThreatTypeMitigation mitigation)
        {
            _itemType.Text = "Threat Type Mitigation";
            _itemPicture.Image = Resources.standard_mitigations_big;

            var section = AddSection("Threat Type Mitigation");
            section.SuspendLayout();
            AddHyperlink(section, "Threat Type", mitigation.ThreatType);
            AddHyperlink(section, "Mitigation", mitigation.Mitigation);
            AddCombo(section, "Control", mitigation.Mitigation.ControlType.ToString(),
                Enum.GetValues(typeof(SecurityControlType)).OfType<SecurityControlType>().Select(x => x.GetEnumLabel()),
                SecurityControlTypeChanged, true);
            AddCombo(section, "Strength", mitigation.Strength.ToString(),
                _model.Strengths.Where(x => x.Visible).OrderByDescending(x => x.Id).Select(x => x.Name).ToArray(),
                ThreatTypeStrengthChanged, _readonly);

            FinalizeSection(section);
            section.ResumeLayout();
        }

        private void ShowItem([NotNull] IWeaknessMitigation mitigation)
        {
            _itemType.Text = "Weakness Mitigation";
            _itemPicture.Image = Resources.standard_mitigations_big;

            var section = AddSection("Weakness Mitigation");
            section.SuspendLayout();
            AddHyperlink(section, "Weakness", mitigation.Weakness);
            AddHyperlink(section, "Mitigation", mitigation.Mitigation);
            AddCombo(section, "Control", mitigation.Mitigation.ControlType.ToString(),
                Enum.GetValues(typeof(SecurityControlType)).OfType<SecurityControlType>().Select(x => x.GetEnumLabel()),
                SecurityControlTypeChanged, true);
            AddCombo(section, "Strength", mitigation.Strength.ToString(),
                _model.Strengths.Where(x => x.Visible).OrderByDescending(x => x.Id).Select(x => x.Name).ToArray(),
                ThreatTypeStrengthChanged, _readonly);

            FinalizeSection(section);
            section.ResumeLayout();
        }

        private void ShowItem([NotNull] IThreatEventMitigation mitigation)
        {
            _itemType.Text = "Threat Event Mitigation";
            _itemPicture.Image = Resources.mitigations_big;

            var threatEvent = mitigation.ThreatEvent;
            var section = AddSection("Threat Event Mitigation");
            section.SuspendLayout();
            AddHyperlink(section, "Threat Event", threatEvent);
            AddHyperlink(section, "Mitigation", mitigation.Mitigation);
            var label = AddHyperlink(section, "Associated To", threatEvent.Parent,threatEvent.Parent?.GetImage(ImageSize.Small));
            _superTooltip.SetSuperTooltip(label, _model.GetSuperTooltipInfo(threatEvent.Parent));
            if (threatEvent.Parent is IEntity entity)
            {
                entity.ImageChanged += OnThreatEventMitigationImageChanged;
            }
            AddCombo(section, "Control", mitigation.Mitigation.ControlType.ToString(),
                Enum.GetValues(typeof(SecurityControlType)).OfType<SecurityControlType>().Select(x => x.GetEnumLabel()),
                SecurityControlTypeChanged, true);
            AddCombo(section, "Strength", mitigation.Strength.ToString(),
                _model.Strengths.Where(x => x.Visible).OrderByDescending(x => x.Id).Select(x => x.Name).ToArray(),
                ThreatEventStrengthChanged, _readonly);
            AddCombo(section, "Status", mitigation.Status.ToString(),
                Enum.GetValues(typeof(MitigationStatus)).OfType<MitigationStatus>().Select(x => x.GetEnumLabel()),
                MitigationStatusChanged, _readonly);
            AddText(section, "Directives", mitigation.Directives, ChangeDirectives, null, _readonly);

            FinalizeSection(section);
            section.ResumeLayout();

            if (mitigation.ThreatEvent.Parent is INotifyPropertyChanged notifyPropertyChanged)
                notifyPropertyChanged.PropertyChanged += OnThreatEventParentPropertyChanged;
        }

        private void ShowItem([NotNull] IVulnerabilityMitigation mitigation)
        {
            _itemType.Text = "Vulnerability Mitigation";
            _itemPicture.Image = Resources.mitigations_big;

            var section = AddSection("Vulnerability Mitigation");
            section.SuspendLayout();
            AddHyperlink(section, "Vulnerability", mitigation.Vulnerability);
            AddHyperlink(section, "Mitigation", mitigation.Mitigation);

            if (mitigation.Vulnerability?.Parent is IIdentity parent)
            {
                var label = AddSingleLineLabel(section, "Associated To", parent.Name ?? string.Empty, parent.GetImage(ImageSize.Small));
                _superTooltip.SetSuperTooltip(label, _model.GetSuperTooltipInfo(parent));
            }

            if (mitigation.Vulnerability.Parent is IEntity entity)
            {
                entity.ImageChanged += OnVulnerabilityMitigationImageChanged;
            }
            AddCombo(section, "Control", mitigation.Mitigation.ControlType.ToString(),
                Enum.GetValues(typeof(SecurityControlType)).OfType<SecurityControlType>().Select(x => x.GetEnumLabel()),
                SecurityControlTypeChanged, true);
            AddCombo(section, "Strength", mitigation.Strength.ToString(),
                _model.Strengths.Where(x => x.Visible).OrderByDescending(x => x.Id).Select(x => x.Name).ToArray(),
                VulnerabilityStrengthChanged, _readonly);
            AddCombo(section, "Status", mitigation.Status.ToString(),
                Enum.GetValues(typeof(MitigationStatus)).OfType<MitigationStatus>().Select(x => x.GetEnumLabel()),
                MitigationStatusChanged, _readonly);
            AddText(section, "Directives", mitigation.Directives, ChangeDirectives, null, _readonly);

            FinalizeSection(section);
            section.ResumeLayout();

            if (mitigation.Vulnerability.Parent is INotifyPropertyChanged notifyPropertyChanged)
                notifyPropertyChanged.PropertyChanged += OnVulnerabilityParentPropertyChanged;
        }

        private void OpenSubItem(object source, EventArgs args)
        {
            if (source is ListBox listBox)
            {
                var selected = listBox.SelectedItem;
                if (selected != null)
                {
                    using (var dialog = new ItemEditorDialog())
                    {
                        dialog.SetExecutionMode(_executionMode);
                        dialog.ReadOnly = ReadOnly;
                        dialog.Item = selected;
                        dialog.ShowDialog();
                    }
                }
            }
        }
        #endregion

        #region Property updates.
        private void ThreatEventRemoved([NotNull] IThreatEventsContainer container, [NotNull] IThreatEvent threatEvent)
        {
            var control = GetControl("Threat Events", string.Empty);
            if (control is ListBox listBox && listBox.Items.Contains(threatEvent))
            {
                listBox.Items.Remove(threatEvent);
            }
        }

        private void VulnerabilityRemoved([NotNull] IVulnerabilitiesContainer container, [NotNull] IVulnerability vulnerability)
        {
            var control = GetControl("Vulnerabilities", string.Empty);
            if (control is ListBox listBox && listBox.Items.Contains(vulnerability))
            {
                listBox.Items.Remove(vulnerability);
            }
        }

        private void ThreatEventAdded([NotNull] IThreatEventsContainer container, [NotNull] IThreatEvent threatEvent)
        {
            var control = GetControl("Threat Events", string.Empty);
            if (control is ListBox listBox && !listBox.Items.Contains(threatEvent))
            {
                listBox.Items.Add(threatEvent);
            }
        }

        private void VulnerabilityAdded([NotNull] IVulnerabilitiesContainer container, [NotNull] IVulnerability vulnerability)
        {
            var control = GetControl("Vulnerabilities", string.Empty);
            if (control is ListBox listBox && !listBox.Items.Contains(vulnerability))
            {
                listBox.Items.Add(vulnerability);
            }
        }

        private void AddThreatEventHandler(object sender, EventArgs e)
        {
            if (sender is Button button && button.Tag is ListBox listBox &&
                _item is IThreatEventsContainer container)
            {
                IThreatModel model = null;
                if (_item is IThreatModelChild child)
                    model = child.Model;
                else if (_item is IThreatModel tm)
                    model = tm;

                using (var scope = UndoRedoManager.OpenScope("Add Threat Event"))
                {
                    using (var dialog = new ThreatTypeSelectionDialog())
                    {
                        dialog.Initialize(model, container);
                        if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        {
                            var threatType = dialog.ThreatType;
                            if (threatType != null)
                            {
                                container.AddThreatEvent(threatType);
                                scope?.Complete();
                            }
                        }
                    }
                }
            }
        }

        private void AddVulnerabilityHandler(object sender, EventArgs e)
        {
            if (sender is Button button && button.Tag is ListBox listBox &&
                _item is IVulnerabilitiesContainer container)
            {
                IThreatModel model = null;
                if (_item is IThreatModelChild child)
                    model = child.Model;
                else if (_item is IThreatModel tm)
                    model = tm;

                using (var scope = UndoRedoManager.OpenScope("Add Vulnerability"))
                {
                    using (var dialog = new WeaknessSelectionDialog())
                    {
                        dialog.Initialize(model, container);
                        if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        {
                            var weakness = dialog.Weakness;
                            if (weakness != null)
                            {
                                container.AddVulnerability(weakness);
                                scope?.Complete();
                            }
                        }
                    }
                }
            }
        }

        private void ThreatTypeMitigationAdded(IThreatTypeMitigationsContainer container, IThreatTypeMitigation mitigation)
        {
            var control = GetControl("Threat Type", "Standard Mitigations");
            if (control is ListBox listBox && !listBox.Items.Contains(mitigation))
            {
                listBox.Items.Add(mitigation);
            }
        }

        private void WeaknessMitigationAdded(IWeaknessMitigationsContainer container, IWeaknessMitigation mitigation)
        {
            var control = GetControl("Weakness", "Standard Mitigations");
            if (control is ListBox listBox && !listBox.Items.Contains(mitigation))
            {
                listBox.Items.Add(mitigation);
            }
        }

        private void ThreatTypeMitigationRemoved(IThreatTypeMitigationsContainer container, IThreatTypeMitigation mitigation)
        {
            var control = GetControl("Threat Type", "Standard Mitigations");
            if (control is ListBox listBox && listBox.Items.Contains(mitigation))
            {
                listBox.Items.Remove(mitigation);
            }
        }

        private void WeaknessMitigationRemoved(IWeaknessMitigationsContainer container, IWeaknessMitigation mitigation)
        {
            var control = GetControl("Weakness", "Standard Mitigations");
            if (control is ListBox listBox && listBox.Items.Contains(mitigation))
            {
                listBox.Items.Remove(mitigation);
            }
        }

        private void AddThreatTypeStandardMitigationEventHandler(object sender, EventArgs e)
        {
            if (sender is Button button && button.Tag is ListBox listBox &&
                _item is IThreatType threatType)
            {
                using (var dialog = new ThreatTypeMitigationSelectionDialog(threatType))
                {
                    dialog.ShowDialog(Form.ActiveForm);
                }
            }
        }

        private void AddWeaknessStandardMitigationEventHandler(object sender, EventArgs e)
        {
            if (sender is Button button && button.Tag is ListBox listBox &&
                _item is IWeakness weakness)
            {
                using (var dialog = new WeaknessMitigationSelectionDialog(weakness))
                {
                    dialog.ShowDialog(Form.ActiveForm);
                }
            }
        }

        private void ThreatEventMitigationAdded(IThreatEventMitigationsContainer container, IThreatEventMitigation mitigation)
        {
            var control = GetControl("Threat Event", "Mitigations");
            if (control is ListBox listBox && !listBox.Items.Contains(mitigation))
            {
                listBox.Items.Add(mitigation);
            }
        }

        private void VulnerabilityMitigationAdded(IVulnerabilityMitigationsContainer container, IVulnerabilityMitigation mitigation)
        {
            var control = GetControl("Vulnerability", "Mitigations");
            if (control is ListBox listBox && !listBox.Items.Contains(mitigation))
            {
                listBox.Items.Add(mitigation);
            }
        }

        private void ThreatEventMitigationRemoved(IThreatEventMitigationsContainer container, IThreatEventMitigation mitigation)
        {
            var control = GetControl("Threat Event", "Mitigations");
            if (control is ListBox listBox && listBox.Items.Contains(mitigation))
            {
                listBox.Items.Remove(mitigation);
            }
        }

        private void VulnerabilityMitigationRemoved(IVulnerabilityMitigationsContainer container, IVulnerabilityMitigation mitigation)
        {
            var control = GetControl("Vulnerability", "Mitigations");
            if (control is ListBox listBox && listBox.Items.Contains(mitigation))
            {
                listBox.Items.Remove(mitigation);
            }
        }

        private void OnThreatEventParentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IIdentity identity && string.CompareOrdinal(e.PropertyName, "Name") == 0)
            {
                var control = GetControl("Threat Event", "Associated To");
                if (control is LabelX label)
                {
                    label.Text = identity.Name;
                }
                else
                {
                    var control2 = GetControl("Threat Event Mitigation", "Associated To");
                    if (control2 is LabelX label2)
                    {
                        label2.Text = identity.Name;
                    }
                }
            }
        }

        private void OnVulnerabilityParentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IIdentity identity && string.CompareOrdinal(e.PropertyName, "Name") == 0)
            {
                var control = GetControl("Vulnerability", "Associated To");
                if (control is LabelX label)
                {
                    label.Text = identity.Name;
                }
                else
                {
                    var control2 = GetControl("Vulnerability Mitigation", "Associated To");
                    if (control2 is LabelX label2)
                    {
                        label2.Text = identity.Name;
                    }
                }
            }
        }

        private void AddThreatEventMitigationEventHandler(object sender, EventArgs e)
        {
            if (sender is Button button && button.Tag is ListBox listBox &&
                _item is IThreatEvent threatEvent)
            {
                using (var dialog = new ThreatEventMitigationSelectionDialog(threatEvent))
                {
                    dialog.ShowDialog(Form.ActiveForm);
                }
            }
        }

        private void AddVulnerabilityMitigationEventHandler(object sender, EventArgs e)
        {
            if (sender is Button button && button.Tag is ListBox listBox &&
                _item is IVulnerability vulnerability)
            {
                using (var dialog = new VulnerabilityMitigationSelectionDialog(vulnerability))
                {
                    dialog.ShowDialog(Form.ActiveForm);
                }
            }
        }

        private void ChangeFlowType([Required] string text)
        {
            if (_item is IDataFlow dataFlow)
            {
                using (var scope = UndoRedoManager.OpenScope("Change Flow Type"))
                {
                    var flowType = text.GetEnumValue<FlowType>();
                    dataFlow.FlowType = flowType;
                    scope?.Complete();
                }
            }
        }

        private void ChangeSeverity(string name)
        {
            ISeverity severity;

            using (var scope = UndoRedoManager.OpenScope("Change Severity"))
            {
                if (_item is IThreatType threatType)
                {
                    severity =
                        threatType.Model?.Severities?.FirstOrDefault(x => string.CompareOrdinal(x.Name, name) == 0);
                    if (severity != null)
                    {
                        threatType.Severity = severity;
                        scope?.Complete();
                    }
                }
                else if (_item is IThreatEvent threatEvent)
                {
                    severity =
                        threatEvent.Model?.Severities?.FirstOrDefault(x => string.CompareOrdinal(x.Name, name) == 0);
                    if (severity != null)
                    {
                        threatEvent.Severity = severity;
                        scope?.Complete();
                    }
                }
                else if (_item is IThreatEventScenario scenario)
                {
                    severity =
                        scenario.Model?.Severities?.FirstOrDefault(x => string.CompareOrdinal(x.Name, name) == 0);
                    if (severity != null)
                    {
                        scenario.Severity = severity;
                        scope?.Complete();
                    }
                }
                else if (_item is IWeakness weakness)
                {
                    severity =
                        weakness.Model?.Severities?.FirstOrDefault(x => string.CompareOrdinal(x.Name, name) == 0);
                    if (severity != null)
                    {
                        weakness.Severity = severity;
                        scope?.Complete();
                    }
                }
                else if (_item is IVulnerability vulnerability)
                {
                    severity =
                        vulnerability.Model?.Severities?.FirstOrDefault(x => string.CompareOrdinal(x.Name, name) == 0);
                    if (severity != null)
                    {
                        vulnerability.Severity = severity;
                        scope?.Complete();
                    }
                }
            }
        }

        private void ChangeActor(string name)
        {
            IThreatActor actor;

            if (_item is IThreatEventScenario scenario)
            {
                actor =
                    scenario.Model?.ThreatActors?.FirstOrDefault(x => string.CompareOrdinal(x.Name, name) == 0);
                if (actor != null)
                    using (var scope = UndoRedoManager.OpenScope("Change Actor"))
                    {
                        scenario.Actor = actor;
                        scope?.Complete();
                    }
            }
        }

        private void ChangeMotivation(RichTextBox motivation)
        {
            if (_item is IThreatEventScenario scenario)
            {
                using (var scope = UndoRedoManager.OpenScope("Change Scenario Motivation"))
                {
                    scenario.Motivation = motivation?.Text;
                    scope?.Complete();
                }
            }
        }

        private void ChangeDirectives(RichTextBox directives)
        {
            if (_item is IThreatEventMitigation mitigation)
            {
                using (var scope = UndoRedoManager.OpenScope("Change Directives"))
                {
                    mitigation.Directives = directives?.Text;
                    scope?.Complete();
                }
            }
        }

        private void ChangeOwner(TextBox textBox)
        {
            if (_item is IThreatModel model)
            {
                using (var scope = UndoRedoManager.OpenScope("Change Threat Model Owner"))
                {
                    model.Owner = textBox?.Text;
                    scope?.Complete();
                }
            }
        }

        private bool CreateContributor(Control control, string name, object reference)
        {
            bool result = false;

            if (_item is IThreatModel model && !string.IsNullOrWhiteSpace(name))
            {
                using (var scope = UndoRedoManager.OpenScope("Create Contributor"))
                {
                    result = model.AddContributor(name);
                    scope?.Complete();
                }
            }

            return result;
        }

        private bool ChangeContributor(Control control, string oldName, string newName, object reference)
        {
            bool result = false;

            if (_item is IThreatModel model && !string.IsNullOrWhiteSpace(oldName) && !string.IsNullOrWhiteSpace(newName))
            {
                using (var scope = UndoRedoManager.OpenScope("Change Contributor"))
                {
                    result = model.ChangeContributor(oldName, newName);
                    scope?.Complete();
                }
            }

            return result;
        }

        private bool RemoveContributor(Control control, string name, object reference)
        {
            bool result = false;

            if (_item is IThreatModel model && !string.IsNullOrWhiteSpace(name))
            {
                using (var scope = UndoRedoManager.OpenScope("Remove Contributor"))
                {
                    result = model.RemoveContributor(name);
                    scope?.Complete();
                }
            }

            return result;
        }

        private void ClearContributors(Control control, object reference)
        {
            if (_item is IThreatModel model)
            {
                var contributors = model.Contributors?.ToArray();
                if (contributors?.Any() ?? false)
                {
                    using (var scope = UndoRedoManager.OpenScope("Clear Contributors"))
                    {
                        foreach (var contributor in contributors)
                        {
                            model.RemoveContributor(contributor);
                        }
                        scope?.Complete();
                    }
                }
            }
        }

        private bool CreateAssumption(Control control, string name, object reference)
        {
            bool result = false;

            if (_item is IThreatModel model && !string.IsNullOrWhiteSpace(name))
            {
                using (var scope = UndoRedoManager.OpenScope("Create Assumption"))
                {
                    result = model.AddAssumption(name);
                    scope?.Complete();
                }
            }

            return result;
        }

        private bool ChangeAssumption(Control control, string oldName, string newName, object reference)
        {
            bool result = false;

            if (_item is IThreatModel model && !string.IsNullOrWhiteSpace(oldName) && !string.IsNullOrWhiteSpace(newName))
            {
                using (var scope = UndoRedoManager.OpenScope("Change Assumption"))
                {
                    result = model.ChangeAssumption(oldName, newName);
                    scope?.Complete();
                }
            }

            return result;
        }

        private bool RemoveAssumption(Control control, string name, object reference)
        {
            bool result = false;

            if (_item is IThreatModel model && !string.IsNullOrWhiteSpace(name))
            {
                using (var scope = UndoRedoManager.OpenScope("Remove Assumption"))
                {
                    result = model.RemoveAssumption(name);
                    scope?.Complete();
                }
            }

            return result;
        }

        private void ClearAssumptions(Control control, object reference)
        {
            if (_item is IThreatModel model)
            {
                var assumptions = model.Assumptions?.ToArray();
                if (assumptions?.Any() ?? false)
                {
                    using (var scope = UndoRedoManager.OpenScope("Clear Assumptions"))
                    {
                        foreach (var assumption in assumptions)
                        {
                            model.RemoveAssumption(assumption);
                        }
                        scope?.Complete();
                    }
                }
            }
        }

        private bool CreateDependency(Control control, string name, object reference)
        {
            bool result = false;

            if (_item is IThreatModel model && !string.IsNullOrWhiteSpace(name))
            {
                using (var scope = UndoRedoManager.OpenScope("Create Dependency"))
                {
                    result = model.AddDependency(name);
                    scope?.Complete();
                }
            }

            return result;
        }

        private bool ChangeDependency(Control control, string oldName, string newName, object reference)
        {
            bool result = false;

            if (_item is IThreatModel model && !string.IsNullOrWhiteSpace(oldName) && !string.IsNullOrWhiteSpace(newName))
            {
                using (var scope = UndoRedoManager.OpenScope("Change Dependency"))
                {
                    result = model.ChangeDependency(oldName, newName);
                    scope?.Complete();
                }
            }

            return result;
        }

        private bool RemoveDependency(Control control, string name, object reference)
        {
            bool result = false;

            if (_item is IThreatModel model && !string.IsNullOrWhiteSpace(name))
            {
                using (var scope = UndoRedoManager.OpenScope("Remove Dependency"))
                {
                    result = model.RemoveDependency(name);
                    scope?.Complete();
                }
            }

            return result;
        }

        private void ClearDependencies(Control control, object reference)
        {
            if (_item is IThreatModel model)
            {
                var dependencies = model.ExternalDependencies?.ToArray();
                if (dependencies?.Any() ?? false)
                {
                    using (var scope = UndoRedoManager.OpenScope("Clear Dependencies"))
                    {
                        foreach (var dependency in dependencies)
                        {
                            model.RemoveDependency(dependency);
                        }
                        scope?.Complete();
                    }
                }
            }
        }

        private void DependencyRemoved(string text)
        {
            RemoveFromGrid("Threat Model", "Dependencies", text);
        }

        private void DependencyAdded(string text)
        {
            AddToGrid("Threat Model", "Dependencies", text);
        }

        private void AssumptionRemoved(string text)
        {
            RemoveFromGrid("Threat Model", "Assumptions", text);
        }

        private void AssumptionAdded(string text)
        {
            AddToGrid("Threat Model", "Assumptions", text);
        }

        private void ContributorRemoved(string text)
        {
            RemoveFromGrid("Threat Model", "Contributors", text);
        }

        private void ContributorAdded(string text)
        {
            AddToGrid("Threat Model", "Contributors", text);
        }

        private void AddToGrid([Required] string sectionName,
            [Required] string controlName, [Required] string text)
        {
            var control = GetControl(sectionName, controlName);
            if (_item is IThreatModel && control is SuperGridControl grid)
            {
                var row = grid.PrimaryGrid.Rows.OfType<GridRow>()
                    .FirstOrDefault(x => string.CompareOrdinal((string)x.Cells[0].Value, text) == 0);
                if (row == null)
                {
                    row = new GridRow(text);
                    row.Cells[0].Tag = text;
                    row.Cells[0].PropertyChanged += RowCellChanged;
                    grid.PrimaryGrid.Rows.Add(row);
                }
            }
        }

        private void RemoveFromGrid([Required] string sectionName,
            [Required] string controlName, [Required] string text)
        {
            var control = GetControl(sectionName, controlName);
            if (_item is IThreatModel && control is SuperGridControl grid)
            {
                var row = grid.PrimaryGrid.Rows.OfType<GridRow>()
                    .FirstOrDefault(x => string.CompareOrdinal((string)x.Cells[0].Value, text) == 0);
                if (row != null)
                {
                    row.Cells[0].PropertyChanged -= RowCellChanged;
                    grid.PrimaryGrid.Rows.Remove(row);
                }
            }
        }

        private void SecurityControlTypeChanged([Required] string newValue)
        {
            if (_item is IMitigation mitigation)
            {
                using (var scope = UndoRedoManager.OpenScope("Change Control Type"))
                {
                    mitigation.ControlType = newValue.GetEnumValue<SecurityControlType>();
                    scope?.Complete();
                }
            }
        }

        private void ThreatTypeStrengthChanged([Required] string newValue)
        {
            if (_item is IThreatTypeMitigation mitigation)
            {
                var strength = _model.Strengths?.FirstOrDefault(x => string.CompareOrdinal(x.Name, newValue) == 0);
                if (strength != null)
                {
                    using (var scope = UndoRedoManager.OpenScope("Change Strength"))
                    {
                        mitigation.Strength = strength;
                        scope?.Complete();
                    }
                }
            }
        }

        private void ThreatEventStrengthChanged([Required] string newValue)
        {
            if (_item is IThreatEventMitigation mitigation)
            {
                var strength = _model.Strengths?.FirstOrDefault(x => string.CompareOrdinal(x.Name, newValue) == 0);
                if (strength != null)
                {
                    using (var scope = UndoRedoManager.OpenScope("Change Strength"))
                    {
                        mitigation.Strength = strength;
                        scope?.Complete();
                    }
                }
            }
        }

        private void WeaknessStrengthChanged([Required] string newValue)
        {
            if (_item is IWeaknessMitigation mitigation)
            {
                var strength = _model.Strengths?.FirstOrDefault(x => string.CompareOrdinal(x.Name, newValue) == 0);
                if (strength != null)
                {
                    using (var scope = UndoRedoManager.OpenScope("Change Strength"))
                    {
                        mitigation.Strength = strength;
                        scope?.Complete();
                    }
                }
            }
        }

        private void VulnerabilityStrengthChanged([Required] string newValue)
        {
            if (_item is IVulnerabilityMitigation mitigation)
            {
                var strength = _model.Strengths?.FirstOrDefault(x => string.CompareOrdinal(x.Name, newValue) == 0);
                if (strength != null)
                {
                    using (var scope = UndoRedoManager.OpenScope("Change Strength"))
                    {
                        mitigation.Strength = strength;
                        scope?.Complete();
                    }
                }
            }
        }

        private void MitigationStatusChanged([Required] string newValue)
        {
            if (_item is IThreatEventMitigation mitigation)
            {
                using (var scope = UndoRedoManager.OpenScope("Change Mitigation Status"))
                {
                    mitigation.Status = newValue.GetEnumValue<MitigationStatus>();
                    scope?.Complete();
                }
            }
        }

        private void _itemName_TextChanged(object sender, EventArgs e)
        {
            if (!_loading && (_item is IIdentity identity) && (string.CompareOrdinal(identity.Name, _itemName.Text) != 0))
            {
                try
                {
                    _loading = true;
                    using (var scope = UndoRedoManager.OpenScope("Change Name"))
                    {
                        identity.Name = _itemName.Text;
                        scope?.Complete();
                    }
                }
                finally
                {
                    _loading = false;
                }
            }
        }

        private void _itemDescription_TextChanged(object sender, EventArgs e)
        {
            if (!_loading && (_item is IIdentity identity) && (string.CompareOrdinal(identity.Description, _itemDescription.Text) != 0))
            {
                try
                {
                    _loading = true;
                    using (var scope = UndoRedoManager.OpenScope("Change Description"))
                    {
                        identity.Description = _itemDescription.Text;
                        scope?.Complete();
                    }
                }
                finally
                {
                    _loading = false;
                }
            }
        }

        private void ChildPropertyAdded(IIdentity identity, IPropertyType propertyType, IProperty property)
        {
            if (!_loading && identity == _item)
            {
                ShowItem(_item);
            }
        }

        private void ChildPropertyChanged(IIdentity identity, IPropertyType propertyType, IProperty property)
        {
            if (!_loading && identity == _item)
            {
                //ShowItem(_item);
            }
        }

        private void ChildPropertyRemoved(IIdentity identity, IPropertyType propertyType, IProperty property)
        {
            if (!_loading && identity == _item)
            {
                ShowItem(_item);
            }
        }

        private Control GetControl([Required] string section, [NotNull] string label)
        {
            Control result = null;

            var containers = _dynamicLayout.Controls
                .Find(section, true)
                .OfType<ExpandablePanel>()
                .Select(x => x.Controls.OfType<LayoutControl>().FirstOrDefault())
                .ToArray();

            if (containers.Any())
            {
                foreach (var container in containers)
                {
                    var controls = container.RootGroup.Items.OfType<LayoutControlItem>().ToArray();
                    if (controls.Any())
                    {
                        result = controls.FirstOrDefault(x => string.CompareOrdinal(label, x.Text) == 0)?.Control;
                        if (result != null)
                            break;
                    }
                }
            }

            return result;
        }

        private void OnImageChanged([NotNull] IEntity entity, ImageSize size)
        {
            if (_item == entity)
            {
                _itemPicture.Image = entity.GetImage(ImageSize.Big);
            }
        }

        private void OnTemplateImageChanged([NotNull] IEntityTemplate template, ImageSize size)
        {
            if (_item == template)
            {
                _itemPicture.Image = template.GetImage(ImageSize.Big);
            }
        }

        private void OnSourceImageChanged([NotNull] IEntity entity, ImageSize size)
        {
            if (_item is IDataFlow dataFlow && dataFlow.Source == entity)
            {
                var control = GetControl("Flow", "Source");
                if (control is LabelX label)
                    label.Image = entity.GetImage(ImageSize.Small);
            }
        }

        private void OnTargetImageChanged([NotNull] IEntity entity, ImageSize size)
        {
            if (_item is IDataFlow dataFlow && dataFlow.Target == entity)
            {
                var control = GetControl("Flow", "Target");
                if (control is LabelX label)
                    label.Image = entity.GetImage(ImageSize.Small);
            }
        }

        private void OnThreatEventImageChanged([NotNull] IEntity entity, ImageSize size)
        {
            if (_item is IThreatEvent threatEvent && threatEvent.Parent == entity)
            {
                var control = GetControl("Threat Event", "Associated To");
                if (control is LabelX label)
                    label.Image = entity.GetImage(ImageSize.Small);
            }
        }

        private void OnVulnerabilityImageChanged([NotNull] IEntity entity, ImageSize size)
        {
            if (_item is IVulnerability vulnerability && vulnerability.Parent == entity)
            {
                var control = GetControl("Vulnerability", "Associated To");
                if (control is LabelX label)
                    label.Image = entity.GetImage(ImageSize.Small);
            }
        }

        private void OnThreatEventMitigationImageChanged([NotNull] IEntity entity, ImageSize size)
        {
            if (_item is IThreatEventMitigation mitigation && mitigation.ThreatEvent?.Parent == entity)
            {
                var control = GetControl("Threat Event Mitigation", "Associated To");
                if (control is LabelX label)
                    label.Image = entity.GetImage(ImageSize.Small);
            }
        }

        private void OnVulnerabilityMitigationImageChanged([NotNull] IEntity entity, ImageSize size)
        {
            if (_item is IVulnerabilityMitigation mitigation && mitigation.Vulnerability?.Parent == entity)
            {
                var control = GetControl("Vulnerability", "Associated To");
                if (control is LabelX label)
                    label.Image = entity.GetImage(ImageSize.Small);
            }
        }

        private void OnSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_item is IDataFlow dataFlow && sender is IEntity entity && dataFlow.Source == entity)
            {
                switch (e.PropertyName)
                {
                    case "Name":
                        var control = GetControl("Flow", "Source");
                        if (control is LabelX label)
                        {
                            label.Text = entity.Name;
                            _superTooltip.SetSuperTooltip(label, _model.GetSuperTooltipInfo(entity));
                        }
                        break;
                }
            }
        }

        private void OnTargetPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_item is IDataFlow dataFlow && sender is IEntity entity && dataFlow.Target == entity)
            {
                switch (e.PropertyName)
                {
                    case "Name":
                        var control = GetControl("Flow", "Target");
                        if (control is LabelX label)
                        {
                            label.Text = entity.Name;
                            _superTooltip.SetSuperTooltip(label, _model.GetSuperTooltipInfo(entity));
                        }
                        break;
                }
            }
        }
        #endregion

        #region Sections management.
        private LayoutControl AddSection(string name)
        {
            _dynamicLayout.SuspendLayout();

            var panel =
                new GroupPanel
                {
                    CanvasColor = Color.White,
                    DisabledBackColor = Color.Empty,
                    Margin = new System.Windows.Forms.Padding(7),
                    Name = name,
                    Size = new Size(_dynamicLayout.Width - 14, 100),
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    Text = name,
                    Left = 7,
                    Dock = DockStyle.Top,
                };
            panel.Style.TextAlignment = eStyleTextAlignment.Center;
            panel.Style.BorderTop = eStyleBorderType.Solid;
            panel.Style.BorderTopColor = ThreatModelManager.StandardColor;
            panel.Style.BorderTopColorSchemePart = eColorSchemePart.PanelBorder;
            panel.Style.BorderTopWidth = 2;
            panel.Style.MarginTop = 10;
            panel.Style.BackColor = Color.White;
            panel.Style.BackColor2 = Color.White;

            var innerLayoutControl = new LayoutControl
            {
                Name = name,
                Dock = DockStyle.Fill, 
                AutoScroll = true
            };
            panel.Controls.Add(innerLayoutControl);

            _dynamicLayout.Controls.Add(panel);
            _dynamicLayout.ResumeLayout();

            return innerLayoutControl;
        }

        private void FinalizeSection([NotNull] LayoutControl section)
        {
            if (section.Parent is GroupPanel panel)
            {
                var height = 0;

                if (section.RootGroup.Items.Count > 0)
                {
                    var percentage = 0; // Width percentage that has already been occupied.
                    var width = 0;      // Absolute width that has already been occupied.
                    var previous = 0;   // Previous height of the item.

                    var items = section.RootGroup.Items.OfType<LayoutControlItem>();
                    var newLine = false;
                    foreach (var item in items)
                    {
                        if (item.WidthType == eLayoutSizeType.Percent)
                        {
                            if (item.Width > 100 || (item.Width > 95 && width > 0))
                            {
                                newLine = true;
                            }
                            else
                            {
                                // The width is less than 100%.

                                if (percentage == 0)
                                    // In this case, we have the first item.
                                    previous = item.Height;

                                // Update the percentage.
                                percentage += item.Width;

                                if (percentage >= 100)
                                {
                                    newLine = true;
                                }
                            }
                        }
                        else
                        {
                            if (percentage > 95 && item.Width > 10)
                            {
                                newLine = true;
                            }
                            else
                            {
                                if (width == 0)
                                    previous = item.Height;

                                width += item.Width + section.Margin.Horizontal;
                                if (width > panel.Width)
                                {
                                    newLine = true;
                                }
                            }
                        }

                        if (newLine)
                        {
                            // Forced new line. Height must be increased of the previously calculated height.

                            if (previous > 0)
                            {
                                // We must add the previous line height.
                                height += previous + section.Margin.Vertical;
                                previous = item.Height;
                            }
                            else
                            {
                                // In this case, we immediately have a new line.
                                height += item.Height + section.Margin.Vertical; // Note: height is never a percentage.
                            }

                            // Reset the width.
                            width = 0;
                            // Reset the percentage.
                            percentage = 0;
                            // Reset the new line flag.
                            newLine = false;
                        }
                    }

                    if (previous > 0)
                        height += previous + section.Margin.Vertical;

                    if (section.RootGroup.Items.Count == 1)
                        height += 2 * section.Margin.Vertical;
                }

                panel.Height = height + section.RootGroup.CaptionHeight + section.Margin.Vertical + 30;
            }
        }

        private void _dynamicLayout_Resize(object sender, EventArgs e)
        {
            var panels = _dynamicLayout.Controls.OfType<GroupPanel>().ToArray();
            if (panels.Any())
            {
                foreach (var panel in panels)
                {
                    var sections = panel.Controls.OfType<LayoutControl>().ToArray();
                    if (sections.Any())
                    {
                        foreach (var section in sections)
                        {
                            FinalizeSection(section);
                        }
                    }
                }
            }
        }
        #endregion
        
        #region Interactivity and usability.
        private void _description_MarkupLinkClick(object sender, DevComponents.DotNetBar.Layout.MarkupLinkClickEventArgs e)
        {
            try
            {
                //_spellAsYouType.CheckAsYouType = false;

                using (var dialog = new TextEditorDialog
                {
                    Text = _itemDescription.Text, 
                    Multiline = true, 
                    ReadOnly = _itemDescription.ReadOnly
                })
                {
                    if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        _itemDescription.Text = dialog.Text;
                }
            }
            finally
            {
                //_spellAsYouType.CheckAsYouType = true;
            }
        }

        private void _itemDescription_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                if (Regex.IsMatch(e.LinkText, 
                    @"\b(https?|ftp|file)://[-A-Z0-9+&@#/%?=~_|$!:,.;]*[A-Z0-9+&@#/%=~_|$]", 
                    RegexOptions.IgnoreCase))
                {
#pragma warning disable SCS0001 // Command injection possible in {1} argument passed to '{0}'
                    Process.Start(e.LinkText);
#pragma warning restore SCS0001 // Command injection possible in {1} argument passed to '{0}'
                }
            }
            catch
            {
                // Ignore the error because the link is simply not trusted.
            }
        }

        private void _itemPicture_DoubleClick(object sender, EventArgs e)
        {
            if (_item is IEntity entity)
            {
                using (var dialog = new SelectImagesDialog(entity))
                {
                    if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                    {
                        var bigImage = dialog.BigImage;
                        if (bigImage != null)
                        {
                            entity.BigImage = bigImage;
                            _itemPicture.Image = bigImage;
                        }

                        var image = dialog.Image;
                        if (image != null)
                            entity.Image = image;
                        var smallImage = dialog.SmallImage;
                        if (smallImage != null)
                            entity.SmallImage = smallImage;
                    }
                }
            } else if (_item is IEntityTemplate template)
            {
                using (var dialog = new SelectImagesDialog(template))
                {
                    if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                    {
                        var bigImage = dialog.BigImage;
                        if (bigImage != null)
                        {
                            template.BigImage = bigImage;
                            _itemPicture.Image = bigImage;
                        }

                        var image = dialog.Image;
                        if (image != null)
                            template.Image = image;
                        var smallImage = dialog.SmallImage;
                        if (smallImage != null)
                            template.SmallImage = smallImage;
                    }
                }
            }
        }

        private void _superTooltip_MarkupLinkClick(object sender, DevComponents.DotNetBar.MarkupLinkClickEventArgs e)
        {
            if (Guid.TryParse(e.HRef, out var id))
                OpenDiagram?.Invoke(id);
        }
        #endregion

        #region Context Menu.
        /// <summary>
        /// Initialize the Context Menu system for Item Editor.
        /// </summary>
        /// <param name="actions">Known Context Aware Actions.</param>
        public static void InitializeContextMenu(IEnumerable<IContextAwareAction> actions)
        {
            _actions = actions?.ToArray();
        }

        private void OnThreatEventMenuOpening(object sender, CancelEventArgs e)
        {
            _menuThreatEvent = null;

            if (sender is ContextMenuStrip menuStrip && menuStrip.SourceControl is ListBox listBox)
            {
                var index = listBox.IndexFromPoint(listBox.PointToClient(Cursor.Position));
                if (index >= 0 && index < listBox.Items.Count && listBox.Items[index] is IThreatEvent threatEvent)
                {
                    _menuThreatEvent = threatEvent;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void OnVulnerabilityMenuOpening(object sender, CancelEventArgs e)
        {
            _menuVulnerability = null;

            if (sender is ContextMenuStrip menuStrip && menuStrip.SourceControl is ListBox listBox)
            {
                var index = listBox.IndexFromPoint(listBox.PointToClient(Cursor.Position));
                if (index >= 0 && index < listBox.Items.Count && listBox.Items[index] is IVulnerability vulnerability)
                {
                    _menuVulnerability = vulnerability;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void OnThreatEventMenuClicked(IContextAwareAction action, object context)
        {
            var control = GetControl("Threat Events", string.Empty);
            if (control is ListBox listBox && _menuThreatEvent != null)
            {
                action.Execute(_menuThreatEvent);
            }
        }

        private void OnVulnerabilityMenuClicked(IContextAwareAction action, object context)
        {
            var control = GetControl("Vulnerabilities", string.Empty);
            if (control is ListBox listBox && _menuVulnerability != null)
            {
                action.Execute(_menuVulnerability);
            }
        }

        private void OnThreatEventMitigationMenuOpening(object sender, CancelEventArgs e)
        {
            _menuThreatEventMitigation = null;

            if (sender is ContextMenuStrip menuStrip && menuStrip.SourceControl is ListBox listBox)
            {
                var index = listBox.IndexFromPoint(listBox.PointToClient(Cursor.Position));
                if (index >= 0 && index < listBox.Items.Count && listBox.Items[index] is IThreatEventMitigation threatEventMitigation)
                {
                    _menuThreatEventMitigation = threatEventMitigation;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void OnVulnerabilityMitigationMenuOpening(object sender, CancelEventArgs e)
        {
            _menuVulnerabilityMitigation = null;

            if (sender is ContextMenuStrip menuStrip && menuStrip.SourceControl is ListBox listBox)
            {
                var index = listBox.IndexFromPoint(listBox.PointToClient(Cursor.Position));
                if (index >= 0 && index < listBox.Items.Count && listBox.Items[index] is IVulnerabilityMitigation vulnerabilityMitigation)
                {
                    _menuVulnerabilityMitigation = vulnerabilityMitigation;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void OnThreatEventMitigationMenuClicked(IContextAwareAction action, object context)
        {
            var control = GetControl("Threat Event", "Mitigations");
            if (control is ListBox listBox && _menuThreatEventMitigation != null)
            {
                action.Execute(_menuThreatEventMitigation);
            }
        }

        private void OnVulnerabilityMitigationMenuClicked(IContextAwareAction action, object context)
        {
            var control = GetControl("Vulnerability", "Mitigations");
            if (control is ListBox listBox && _menuVulnerabilityMitigation != null)
            {
                action.Execute(_menuVulnerabilityMitigation);
            }
        }

        private void OnThreatTypeMitigationMenuOpening(object sender, CancelEventArgs e)
        {
            _menuThreatTypeMitigation = null;

            if (sender is ContextMenuStrip menuStrip && menuStrip.SourceControl is ListBox listBox)
            {
                var index = listBox.IndexFromPoint(listBox.PointToClient(Cursor.Position));
                if (index >= 0 && index < listBox.Items.Count && listBox.Items[index] is IThreatTypeMitigation threatTypeMitigation)
                {
                    _menuThreatTypeMitigation = threatTypeMitigation;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void OnWeaknessMitigationMenuOpening(object sender, CancelEventArgs e)
        {
            _menuWeaknessMitigation = null;

            if (sender is ContextMenuStrip menuStrip && menuStrip.SourceControl is ListBox listBox)
            {
                var index = listBox.IndexFromPoint(listBox.PointToClient(Cursor.Position));
                if (index >= 0 && index < listBox.Items.Count && listBox.Items[index] is IWeaknessMitigation weaknessMitigation)
                {
                    _menuWeaknessMitigation = weaknessMitigation;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void OnThreatTypeMitigationMenuClicked(IContextAwareAction action, object context)
        {
            var control = GetControl("Threat Type", "Standard Mitigations");
            if (control is ListBox listBox && _menuThreatTypeMitigation != null)
            {
                action.Execute(_menuThreatTypeMitigation);
            }
        }

        private void OnWeaknessMitigationMenuClicked(IContextAwareAction action, object context)
        {
            var control = GetControl("Weakness", "Standard Mitigations");
            if (control is ListBox listBox && _menuWeaknessMitigation != null)
            {
                action.Execute(_menuWeaknessMitigation);
            }
        }
        #endregion

        private void RefreshOnUndoRedo(string text)
        {
            _refresh_Click(this, EventArgs.Empty);
        }

        private void _refresh_Click(object sender, EventArgs e)
        {
            var item = Item;
            Item = null;
            Item = item;
        }
    } 
}
