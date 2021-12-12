using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
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

            public Func<string, T, bool> Created { get; set; }
            public Func<string, T, bool> Removed { get; set; }
            public Func<string, string, T, bool> Changed { get; set; }
            public Action<T> Cleared { get; set; }

            public bool RaiseCreated(string name)
            {
                return Created?.Invoke(name, _reference) ?? false;
            }

            public bool RaiseRemoved(string name)
            {
                return Removed?.Invoke(name, _reference) ?? false;
            }

            public bool RaiseChanged(string oldName, string newName)
            {
                return Changed?.Invoke(oldName, newName, _reference) ?? false;
            }

            public void RaiseCleared()
            {
                Cleared?.Invoke(_reference);
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
            bool RaiseCreated(string name);
            bool RaiseRemoved(string name);
            bool RaiseChanged(string oldName, string newName);
            void RaiseCleared();
        }
        #endregion
        
        #region Private member variables.
        private object _item;
        private bool _loading;
        private bool _readonly;
        private Label _parentLabel;
        private IThreatModel _model;
        private ExecutionMode _executionMode = ExecutionMode.Expert;
        private static IEnumerable<IContextAwareAction> _actions;
        private MenuDefinition _threatEventMenuDefinition;
        private IThreatEvent _menuThreatEvent;
        private MenuDefinition _threatEventMitigationMenuDefinition;
        private IThreatEventMitigation _menuThreatEventMitigation;
        private MenuDefinition _threatTypeMitigationMenuDefinition;
        private IThreatTypeMitigation _menuThreatTypeMitigation;
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

            AddSpellCheck(_itemDescription);
        }

        private void AddSpellCheck([NotNull] TextBoxBase control)
        {
            try
            {
                if (control is RichTextBox richTextBox)
                {
                    _spellAsYouType.AddTextComponent(new RichTextBoxSpellAsYouTypeAdapter(richTextBox,
                        _spellAsYouType.ShowCutCopyPasteMenuOnTextBoxBase));

                }
                else
                {
                    _spellAsYouType.AddTextBoxBase(control);
                }
            }
            catch
            {
            }
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

                                if (value != null)
                                {

                                    if (value is INotifyPropertyChanged notifyPropertyChanged)
                                        notifyPropertyChanged.PropertyChanged += OnPropertyChanged;
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
                                                    _model.ChildPropertyChanged -= ChildPropertyChanged;
                                                    _model.ChildPropertyRemoved -= ChildPropertyRemoved;
                                                }

                                                _model = model;
                                                _model.ChildPropertyAdded += ChildPropertyAdded;
                                                _model.ChildPropertyChanged += ChildPropertyChanged;
                                                _model.ChildPropertyRemoved += ChildPropertyRemoved;

                                            }
                                        }
                                        else
                                        {
                                            if (_model != null)
                                            {
                                                _model.ChildPropertyAdded -= ChildPropertyAdded;
                                                _model.ChildPropertyChanged -= ChildPropertyChanged;
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
                if (_item is INotifyPropertyChanged notifyPropertyChanged)
                    notifyPropertyChanged.PropertyChanged -= OnPropertyChanged;

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
                        ((INotifyPropertyChanged) source).PropertyChanged -= OnSourcePropertyChanged;
                        source.ImageChanged -= OnSourceImageChanged;
                    }
                    var labelSource = GetControl("Flow", "Source");
                    if (labelSource != null)
                        _superTooltip.SetSuperTooltip(labelSource, null);

                    if (dataFlow.Target is IEntity target)
                    {
                        ((INotifyPropertyChanged) target).PropertyChanged -= OnTargetPropertyChanged;
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
                    ((INotifyPropertyChanged) threatEvent.Parent).PropertyChanged -= OnThreatEventParentPropertyChanged;
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
                    ((INotifyPropertyChanged) mitigation.ThreatEvent.Parent).PropertyChanged -= OnThreatEventParentPropertyChanged;
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
                                    try
                                    {
                                        _spellAsYouType.RemoveTextBoxBase(textBox);
                                    }
                                    catch
                                    {
                                    }
                                } else if (control is RichTextBox richTextBox)
                                {
                                    ClearEventInvocations(control, "TextChanged");
                                    var component = _spellAsYouType.GetTextComponents()?
                                        .OfType<RichTextBoxSpellAsYouTypeAdapter>()
                                        .FirstOrDefault(x => x.TextBox == richTextBox);
                                    if (component != null)
                                    {
                                        try
                                        {
                                            _spellAsYouType.RemoveTextComponent(component);
                                        }
                                        catch
                                        {
                                        }
                                    }
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
                _model.ChildPropertyChanged -= ChildPropertyChanged;
                _model.ChildPropertyRemoved -= ChildPropertyRemoved;
            }

            var expandablePanels = _dynamicLayout.Controls.OfType<ExpandablePanel>().ToArray();
            if (expandablePanels.Any())
            {
                foreach (var expandablePanel in expandablePanels)
                {
                    var layoutControls = expandablePanel.Controls.OfType<LayoutControl>().ToArray();
                    if (layoutControls.Any())
                    {
                        foreach (var layoutControl in layoutControls)
                        {
                            var layoutControlItems =
                                layoutControl.RootGroup.Items.OfType<LayoutControlItem>().ToArray();
                            if (layoutControlItems.Any())
                            {
                                foreach (var layoutControlItem in layoutControlItems)
                                {
                                    RemoveEvents(layoutControlItem);
                                }
                            }
                        }
                    }
                }
            }
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

            if (typeMitigation != null)
            {
                identity = typeMitigation.Mitigation;
                _itemName.ReadOnly = true;
                _itemDescription.ReadOnly = true;
            }
            else if (eventMitigation != null)
            {
                identity = eventMitigation.Mitigation;
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

            if (item is IThreatEventScenario scenario)
            {
                ShowItem(scenario);
            }

            if (item is IThreatType threatType)
            {
                ShowItem(threatType);
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

            if (eventMitigation != null)
            {
                ShowItem(eventMitigation);
            }

            if (identity != null)
                AddInformationSection(identity);

            ResumeLayout();
        }

        private void ClearDynamicLayout()
        {
            //try
            //{
            //    _spellAsYouType.RemoveAllTextComponents();
            //    _spellAsYouType.AddTextComponent(new RichTextBoxSpellAsYouTypeAdapter(_itemDescription, 
            //        _spellAsYouType.ShowCutCopyPasteMenuOnTextBoxBase));
            //}
            //catch
            //{
            //}

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
                    AddSingleLineLabel(infoSection, "Template", itemTemplate.Name);
                }

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
                    var section = AddSection(schema.Name);
                    section.SuspendLayout();

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
                                    AddPropertyViewer(section, viewer, ro);
                                }
                            }
                        } else if (property is IPropertySingleLineString propertySingleLineString)
                        {
                            var text = AddSingleLineText(section, propertySingleLineString, ro);
                            AddSpellCheck(text);
                        }
                        else if (property is IPropertyString propertyString)
                        {
                            var richTextBox = AddText(section, propertyString, ro);
                            AddSpellCheck(richTextBox);
                        }
                        else if (property is IPropertyBool propertyBool)
                        {
                            AddBool(section, propertyBool, ro);
                        }
                        else if (property is IPropertyTokens propertyTokens)
                        {
                            AddTokens(section, propertyTokens, ro);
                        }
                        else if (property is IPropertyArray propertyArray)
                        {
                            AddList(section, propertyArray, ro);
                        }
                        else if (property is IPropertyDecimal propertyDecimal)
                        {
                            AddDecimal(section, propertyDecimal, ro);
                        }
                        else if (property is IPropertyInteger propertyInteger)
                        {
                            AddInteger(section, propertyInteger, ro);
                        }
                        else if (property is IPropertyIdentityReference propertyIdentityReference)
                        {
                            var identity = propertyIdentityReference.Value;

                            AddSingleLineLabel(section, property.PropertyType.Name,
                                identity?.Name ?? "<Undefined>",
                                Dpi.Factor.Width > 1.5
                                    ? identity?.GetImage(ImageSize.Medium)
                                    : identity?.GetImage(ImageSize.Small));
                        }
                        else if (property is IPropertyJsonSerializableObject)
                        {
                            // TODO: add control to show this property. For now, it is not shown.
                            AddSingleLineLabel(section, property.PropertyType.Name,
                                "<Property is a Json Serializable Object, which is not supported by the Item Editor, yet>");
                        }
                        else if (property is IPropertyList propertyList)
                        {
                            AddCombo(section, propertyList, ro);
                            //AddSingleLineLabel(section, property.PropertyType.Name, property.StringValue);
                        }
                        else if (property is IPropertyListMulti)
                        {
                            // TODO: add control to show this property. For now, it is not shown.
                            AddSingleLineLabel(section, property.PropertyType.Name, property.StringValue);
                        }
                        else
                        {
                            AddSingleLineLabel(section, property.PropertyType.Name,
                                "<Property type is not supported by the Item Editor, yet>");
                        }
                    }

                    FinalizeSection(section);

                    section.ResumeLayout();
                }
            }
        }

        private void AddThreatEventsContainerSection([NotNull] IThreatEventsContainer container)
        {
            var infoSection = AddSection("Threat Events");
            infoSection.SuspendLayout();
            var listBox = AddListBox(infoSection, string.Empty,
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
                var source = AddSingleLineLabel(section, "Source", dataFlow.Source.Name, 
                    Dpi.Factor.Width > 1.5 ? dataFlow.Source.GetImage(ImageSize.Medium) : dataFlow.Source.GetImage(ImageSize.Small));
                ((INotifyPropertyChanged) dataFlow.Source).PropertyChanged += OnSourcePropertyChanged;
                _superTooltip.SetSuperTooltip(source, _model.GetSuperTooltipInfo(dataFlow.Source));
                dataFlow.Source.ImageChanged += OnSourceImageChanged;
            }
            if (dataFlow.Target != null)
            {
                var target = AddSingleLineLabel(section, "Target", dataFlow.Target.Name, 
                    Dpi.Factor.Width > 1.5 ? dataFlow.Target.GetImage(ImageSize.Medium) : dataFlow.Target.GetImage(ImageSize.Small));
                _superTooltip.SetSuperTooltip(target, _model.GetSuperTooltipInfo(dataFlow.Target));
                ((INotifyPropertyChanged) dataFlow.Target).PropertyChanged += OnTargetPropertyChanged;
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
            AddSingleLineText(section, "Owner", model.Owner, ChangeOwner, null, _readonly);
            var contribList = AddList(section, "Contributors", model.Contributors, _readonly);
            contribList.Tag = new Actions()
            {
                Created = CreateContributor,
                Changed = ChangeContributor,
                Removed = RemoveContributor,
                Cleared = ClearContributors
            };
            var assumpList = AddList(section, "Assumptions", model.Assumptions, _readonly);
            assumpList.Tag = new Actions()
            {
                Created = CreateAssumption,
                Changed = ChangeAssumption,
                Removed = RemoveAssumption,
                Cleared = ClearAssumptions
            };
            var depList = AddList(section, "Dependencies", model.ExternalDependencies, _readonly);
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
            var label = AddSingleLineLabel(section, "Associated To", threatEvent.Parent.Name,
                Dpi.Factor.Width > 1.5 ? threatEvent.Parent?.GetImage(ImageSize.Medium) : threatEvent.Parent?.GetImage(ImageSize.Small));
            if (threatEvent.Parent is IEntity entity)
            {
                entity.ImageChanged += OnThreatEventImageChanged;
            }
            _superTooltip.SetSuperTooltip(label, _model.GetSuperTooltipInfo(threatEvent.Parent));
            AddCombo(section, "Severity", threatEvent.Severity?.Name, 
                threatEvent.Model?.Severities?.Where(x => x.Visible).OrderByDescending(x => x.Id).Select(x => x.Name),
                ChangeSeverity, _readonly);
            var listBox = AddListBox(section, "Mitigations",
                threatEvent.Mitigations, AddMitigationEventHandler, _readonly);
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
            ((INotifyPropertyChanged) threatEvent.Parent).PropertyChanged += OnThreatEventParentPropertyChanged;
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
            var listBox = AddListBox(section, "Standard Mitigations",
                threatType.Mitigations?.OrderBy(x => x.Mitigation?.Name), AddStandardMitigationEventHandler, _readonly);
            listBox.DoubleClick += OpenSubItem;

            AddListView(section, "Threat Events\napplied to", 
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

        private void ShowItem([NotNull] IThreatEventMitigation mitigation)
        {
            _itemType.Text = "Threat Event Mitigation";
            _itemPicture.Image = Resources.mitigations_big;

            var section = AddSection("Threat Event Mitigation");
            section.SuspendLayout();
            AddHyperlink(section, "Threat Event", mitigation.ThreatEvent);
            AddHyperlink(section, "Mitigation", mitigation.Mitigation);
            var label = AddSingleLineLabel(section, "Associated To", mitigation.ThreatEvent.Parent.Name,
                Dpi.Factor.Width > 1.5 ? mitigation.ThreatEvent?.Parent?.GetImage(ImageSize.Medium) : mitigation.ThreatEvent?.Parent?.GetImage(ImageSize.Small));
            _superTooltip.SetSuperTooltip(label, _model.GetSuperTooltipInfo(mitigation.ThreatEvent.Parent));
            if (mitigation.ThreatEvent.Parent is IEntity entity)
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

            ((INotifyPropertyChanged) mitigation.ThreatEvent.Parent).PropertyChanged += OnThreatEventParentPropertyChanged;
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

        private void ThreatEventAdded([NotNull] IThreatEventsContainer container, [NotNull] IThreatEvent threatEvent)
        {
            var control = GetControl("Threat Events", string.Empty);
            if (control is ListBox listBox && !listBox.Items.Contains(threatEvent))
            {
                listBox.Items.Add(threatEvent);
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
                using (var dialog = new ThreatTypeSelectionDialog())
                {
                    dialog.Initialize(model, container);
                    if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                    {
                        var threatType = dialog.ThreatType;
                        if (threatType != null)
                            container.AddThreatEvent(threatType);
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

        private void ThreatTypeMitigationRemoved(IThreatTypeMitigationsContainer container, IThreatTypeMitigation mitigation)
        {
            var control = GetControl("Threat Type", "Standard Mitigations");
            if (control is ListBox listBox && listBox.Items.Contains(mitigation))
            {
                listBox.Items.Remove(mitigation);
            }
        }

        private void AddStandardMitigationEventHandler(object sender, EventArgs e)
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

        private void ThreatEventMitigationAdded(IThreatEventMitigationsContainer container, IThreatEventMitigation mitigation)
        {
            var control = GetControl("Threat Event", "Mitigations");
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

        private void AddMitigationEventHandler(object sender, EventArgs e)
        {
            if (sender is Button button && button.Tag is ListBox listBox &&
                _item is IThreatEvent threatEvent)
            {
                using (var dialog = new ThreatEventMitigationSelectionDialog(threatEvent))
                {
                    dialog.ShowDialog(Form.ActiveForm);
                }            }            
        }

        private void ChangeFlowType([Required] string text)
        {
            if (_item is IDataFlow dataFlow)
            {
                var flowType = text.GetEnumValue<FlowType>();
                dataFlow.FlowType = flowType;
            }
        }

        private void ChangeSeverity(string name)
        {
            ISeverity severity;

            if (_item is IThreatType threatType)
            {
                severity =
                    threatType.Model?.Severities?.FirstOrDefault(x => string.CompareOrdinal(x.Name, name) == 0);
                if (severity != null)
                    threatType.Severity = severity;
            }

            if (_item is IThreatEvent threatEvent)
            {
                severity =
                    threatEvent.Model?.Severities?.FirstOrDefault(x => string.CompareOrdinal(x.Name, name) == 0);
                if (severity != null)
                    threatEvent.Severity = severity;
            }

            if (_item is IThreatEventScenario scenario)
            {
                severity =
                    scenario.Model?.Severities?.FirstOrDefault(x => string.CompareOrdinal(x.Name, name) == 0);
                if (severity != null)
                    scenario.Severity = severity;
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
                    scenario.Actor = actor;
            }
        }

        private void ChangeMotivation(RichTextBox motivation)
        {
            if (_item is IThreatEventScenario scenario)
            {
                scenario.Motivation = motivation?.Text;
            }
        }

        private void ChangeDirectives(RichTextBox directives)
        {
            if (_item is IThreatEventMitigation mitigation)
            {
                mitigation.Directives = directives?.Text;
            }
        }

        private void ChangeOwner(TextBox textBox)
        {
            if (_item is IThreatModel model)
            {
                model.Owner = textBox?.Text;
            }
        }

        private bool CreateContributor(string name, object reference)
        {
            bool result = false;

            if (_item is IThreatModel model && !string.IsNullOrWhiteSpace(name))
            {
                result = model.AddContributor(name);
            }

            return result;
        }

        private bool ChangeContributor(string oldName, string newName, object reference)
        {
            bool result = false;

            if (_item is IThreatModel model && !string.IsNullOrWhiteSpace(oldName) && !string.IsNullOrWhiteSpace(newName))
            {
                result = model.ChangeContributor(oldName, newName);
            }

            return result;
        }

        private bool RemoveContributor(string name, object reference)
        {
            bool result = false;

            if (_item is IThreatModel model && !string.IsNullOrWhiteSpace(name))
            {
                result = model.RemoveContributor(name);
            }

            return result;
        }

        private void ClearContributors(object reference)
        {
            if (_item is IThreatModel model)
            {
                var contributors = model.Contributors?.ToArray();
                if (contributors?.Any() ?? false)
                {
                    foreach (var contributor in contributors)
                    {
                        model.RemoveContributor(contributor);
                    }
                }
            }
        }

        private bool CreateAssumption(string name, object reference)
        {
            bool result = false;

            if (_item is IThreatModel model && !string.IsNullOrWhiteSpace(name))
            {
                result = model.AddAssumption(name);
            }

            return result;
        }

        private bool ChangeAssumption(string oldName, string newName, object reference)
        {
            bool result = false;

            if (_item is IThreatModel model && !string.IsNullOrWhiteSpace(oldName) && !string.IsNullOrWhiteSpace(newName))
            {
                result = model.ChangeAssumption(oldName, newName);
            }

            return result;
        }

        private bool RemoveAssumption(string name, object reference)
        {
            bool result = false;

            if (_item is IThreatModel model && !string.IsNullOrWhiteSpace(name))
            {
                result = model.RemoveAssumption(name);
            }

            return result;
        }

        private void ClearAssumptions(object reference)
        {
            if (_item is IThreatModel model)
            {
                var assumptions = model.Assumptions?.ToArray();
                if (assumptions?.Any() ?? false)
                {
                    foreach (var assumption in assumptions)
                    {
                        model.RemoveAssumption(assumption);
                    }
                }
            }
        }

        private bool CreateDependency(string name, object reference)
        {
            bool result = false;

            if (_item is IThreatModel model && !string.IsNullOrWhiteSpace(name))
            {
                result = model.AddDependency(name);
            }

            return result;
        }

        private bool ChangeDependency(string oldName, string newName, object reference)
        {
            bool result = false;

            if (_item is IThreatModel model && !string.IsNullOrWhiteSpace(oldName) && !string.IsNullOrWhiteSpace(newName))
            {
                result = model.ChangeDependency(oldName, newName);
            }

            return result;
        }

        private bool RemoveDependency(string name, object reference)
        {
            bool result = false;

            if (_item is IThreatModel model && !string.IsNullOrWhiteSpace(name))
            {
                result = model.RemoveDependency(name);
            }

            return result;
        }

        private void ClearDependencies(object reference)
        {
            if (_item is IThreatModel model)
            {
                var dependencies = model.ExternalDependencies?.ToArray();
                if (dependencies?.Any() ?? false)
                {
                    foreach (var dependency in dependencies)
                    {
                        model.RemoveDependency(dependency);
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
                var row = new GridRow(text);
                row.Cells[0].Tag = text;
                row.Cells[0].PropertyChanged += RowCellChanged;
                grid.PrimaryGrid.Rows.Add(row);
            }
        }
     
        private void RemoveFromGrid([Required] string sectionName, 
            [Required] string controlName, [Required] string text)
        {
            var control = GetControl(sectionName, controlName);
            if (_item is IThreatModel && control is SuperGridControl grid)
            {
                var row = grid.PrimaryGrid.Rows.OfType<GridRow>()
                    .FirstOrDefault(x => string.CompareOrdinal((string) x.Cells[0].Value, text) == 0);
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
                mitigation.ControlType = newValue.GetEnumValue<SecurityControlType>();
            }
        }

        private void ThreatTypeStrengthChanged([Required] string newValue)
        {
            if (_item is IThreatTypeMitigation mitigation)
            {
                var strength = _model.Strengths?.FirstOrDefault(x => string.CompareOrdinal(x.Name, newValue) == 0);
                if (strength != null)
                    mitigation.Strength = strength;
            }
        }

        private void ThreatEventStrengthChanged([Required] string newValue)
        {
            if (_item is IThreatEventMitigation mitigation)
            {
                var strength = _model.Strengths?.FirstOrDefault(x => string.CompareOrdinal(x.Name, newValue) == 0);
                if (strength != null)
                    mitigation.Strength = strength;
            }
        }

        private void MitigationStatusChanged([Required] string newValue)
        {
            if (_item is IThreatEventMitigation mitigation)
            {
                mitigation.Status = newValue.GetEnumValue<MitigationStatus>();
            }
        }

        private void _itemName_TextChanged(object sender, EventArgs e)
        {
            if ((_item is IIdentity identity) && (string.CompareOrdinal(identity.Name, _itemName.Text) != 0))
            {
                try
                {
                    _loading = true;
                    identity.Name = _itemName.Text;
                }
                finally
                {
                    _loading = false;
                }
            }
        }

        private void _itemDescription_TextChanged(object sender, EventArgs e)
        {
            if ((_item is IIdentity identity) && (string.CompareOrdinal(identity.Description, _itemDescription.Text) != 0))
            {
                try
                {
                    _loading = true;
                    identity.Description = _itemDescription.Text;
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

        private void OnThreatEventMitigationImageChanged([NotNull] IEntity entity, ImageSize size)
        {
            if (_item is IThreatEventMitigation mitigation && mitigation.ThreatEvent?.Parent == entity)
            {
                var control = GetControl("Threat Event Mitigation", "Associated To");
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
            var expandablePanel =
                new ExpandablePanel
                {
                    CanvasColor = Color.White,
                    //ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled,
                    DisabledBackColor = Color.Empty,
                    HideControlsWhenCollapsed = true,
                    //Location = new System.Drawing.Point(4, 77),
                    Margin = new System.Windows.Forms.Padding(7),
                    Name = name,
                    Size = new Size(_dynamicLayout.Width - 14, 100),
                    TitleText = name,
                    Left = 7,
                    Dock = DockStyle.Top,
                    TitleHeight = (int) (17 * Dpi.Factor.Height)
                };
            expandablePanel.Style.Alignment = StringAlignment.Center;
            expandablePanel.Style.BackColor1.Color = Color.White;
            expandablePanel.Style.BackColor2.Color = Color.White;
            expandablePanel.Style.BorderColor.ColorSchemePart = eColorSchemePart.BarDockedBorder;
            expandablePanel.Style.ForeColor.ColorSchemePart = eColorSchemePart.ItemText;
            expandablePanel.TitleStyle.Alignment = StringAlignment.Center;
            expandablePanel.TitleStyle.BackColor1.Color = Color.White;
            expandablePanel.TitleStyle.BackColor2.Color = Color.White;
            expandablePanel.TitleStyle.Border = eBorderType.SingleLine;
            expandablePanel.TitleStyle.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
            expandablePanel.TitleStyle.ForeColor.Color = Color.Black;
            expandablePanel.TitleStyle.UseMnemonic = false;

            var innerLayoutControl = new LayoutControl
            {
                Name = name,
                Dock = DockStyle.Fill, 
                AutoScroll = true
            };
            expandablePanel.Controls.Add(innerLayoutControl);

            _dynamicLayout.Controls.Add(expandablePanel);
            _dynamicLayout.ResumeLayout();

            return innerLayoutControl;
        }

        private void FinalizeSection([NotNull] LayoutControl section, bool collapsed = false)
        {
            if (section.Parent is ExpandablePanel panel)
            {
                int height = 0;

                if (section.RootGroup.Items.Count > 0)
                {
                    height = panel.TitleHeight;

                    int percentage = 0;
                    int width = 0;
                    int previous = 0;

                    var items = section.RootGroup.Items.OfType<LayoutControlItem>();
                    foreach (var item in items)
                    {
                        if (item.WidthType == eLayoutSizeType.Percent)
                        {
                            if (item.Width > 100)
                            {
                                if (previous > 0)
                                {
                                    height += previous + section.Margin.Vertical;
                                    width = 0;
                                    previous = 0;
                                }
                                height += item.Height + section.Margin.Vertical;
                                percentage = 0;
                            }
                            else
                            {
                                if (percentage == 0)
                                    previous = item.Height;

                                percentage += item.Width;

                                if (percentage >= 100)
                                {
                                    height += previous + section.Margin.Vertical;
                                    previous = 0;
                                    percentage = 0;
                                }
                            }
                        }
                        else
                        {
                            if (width == 0)
                                previous = item.Height;
                            
                            width += item.Width + section.Margin.Horizontal;
                            if (width > panel.Width)
                            {
                                width = item.Width + section.Margin.Horizontal;
                                height += previous + section.Margin.Vertical;
                                previous = item.Height;
                            }
                        }
                    }

                    if (previous != 0)
                        height += previous + section.Margin.Vertical;
                }

                panel.Height = height + (int) (10 * Dpi.Factor.Height);

                // TODO: check collapse, because it does not seem to work (in forms it creates a scroll bar)!
                //if (collapsed)
                //{
                //    panel.Expanded = false;
                //}
            }
        }

        private void _dynamicLayout_Resize(object sender, EventArgs e)
        {
            var panels = _dynamicLayout.Controls.OfType<ExpandablePanel>().ToArray();
            if (panels.Any())
            {
                foreach (var panel in panels)
                {
                    var sections = panel.Controls.OfType<LayoutControl>().ToArray();
                    if (sections.Any())
                    {
                        foreach (var section in sections)
                        {
                            FinalizeSection(section, !panel.Expanded);
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

        private void OnThreatEventMenuClicked(IContextAwareAction action, object context)
        {
            var control = GetControl("Threat Events", string.Empty);
            if (control is ListBox listBox && _menuThreatEvent != null)
            {
                action.Execute(_menuThreatEvent);
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

        private void OnThreatEventMitigationMenuClicked(IContextAwareAction action, object context)
        {
            var control = GetControl("Threat Event", "Mitigations");
            if (control is ListBox listBox && _menuThreatEventMitigation != null)
            {
                action.Execute(_menuThreatEventMitigation);
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

        private void OnThreatTypeMitigationMenuClicked(IContextAwareAction action, object context)
        {
            var control = GetControl("Threat Type", "Standard Mitigations");
            if (control is ListBox listBox && _menuThreatTypeMitigation != null)
            {
                action.Execute(_menuThreatTypeMitigation);
            }
        }
        #endregion

        private void _refresh_Click(object sender, EventArgs e)
        {
            var item = Item;
            Item = null;
            Item = item;
        }
    } 
}
