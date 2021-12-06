using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using Exceptionless;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Engine;
using ThreatsManager.Interfaces.Extensions;
using PostSharp.Patterns.Threading;
using ThreatsManager.Dialogs;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager
{
    public partial class MainForm
    {
        #region Private member variables.
        /// <summary>
        /// Dictionary containing the ID of the Extension associated to the list of Buttons it has generated.
        /// </summary>
        private readonly Dictionary<Guid, IEnumerable<ButtonItem>> _buttons = new Dictionary<Guid, IEnumerable<ButtonItem>>();
        /// <summary>
        /// Dictionary containing the ID of the Action associated to the PanelsListRibbonAction button.
        /// </summary>
        private readonly Dictionary<Guid, ButtonItem> _panelsListButtons = new Dictionary<Guid, ButtonItem>();
        /// <summary>
        /// Dictionary containing the ID of the Extension associated to the Extension itself.
        /// </summary>
        private readonly Dictionary<Guid, IMainRibbonExtension> _mainRibbonExtensions = new Dictionary<Guid, IMainRibbonExtension>();
        /// <summary>
        /// Dictionary containing the ID of the Extension associated to the ID of the Action containing the list of panels.
        /// </summary>
        private readonly Dictionary<Guid, Guid> _panelListActionId = new Dictionary<Guid, Guid>();
        /// <summary>
        /// Dictionary containing the ID of the child action (associated to the Panel) to the ID of the container Action.
        /// </summary>
        private readonly Dictionary<Guid, Guid> _childParentActions = new Dictionary<Guid, Guid>();

        private bool _diagramNameIsDirty;

        private readonly Dictionary<Ribbon, List<RibbonBar>> _ribbonBars = new Dictionary<Ribbon, List<RibbonBar>>();

        private struct TitleUpdate
        {
            public Guid Id;
            public string Text;
        }

        private readonly Queue<TitleUpdate> _titleUpdates = new Queue<TitleUpdate>();
        #endregion

        #region Main entry points for the Extension management functions.
        private void InitializeExtensionsManagement()
        {
            Manager.Instance.ShowMessage += DesktopAlertAwareExtensionOnShowMessage;
            Manager.Instance.ShowWarning += DesktopAlertAwareExtensionOnShowWarning;
            Manager.Instance.PanelForEntityRequired += CreatePanel;

            var config = ExtensionsConfigurationManager.GetConfigurationSection();
            _executionMode = config.Mode;
            Manager.Instance.LoadExtensions(_executionMode, ExceptExtension);

            HandleExtensionInitializers();

            DiagramNameUpdater();
            var actions = Manager.Instance.GetExtensions<IContextAwareAction>();
            ItemEditor.InitializeContextMenu(actions);
        }

        private bool ExceptExtension([NotNull] Assembly assembly)
        {
            var result = false;

            uint clientId = 0;

            var attributes = CustomAttributeData.GetCustomAttributes(assembly);
            if (attributes.Any())
            {
                var typeName = typeof(ExtensionsContainerAttribute).FullName;
                {
                    foreach (var attribute in attributes)
                    {
                        if (string.CompareOrdinal(attribute.AttributeType.FullName, typeName) == 0)
                        {
                            switch (attribute.ConstructorArguments.Count)
                            {
                                case 2:
                                    if (attribute.ConstructorArguments[1].ArgumentType == typeof(uint))
                                    {
                                        clientId = (uint) attribute.ConstructorArguments[1].Value;
                                    }
                                    break;
                                case 3:
                                    clientId = (uint) attribute.ConstructorArguments[2].Value;
                                    break;
                            }
                        }
                    }
                }
            }

#if MICROSOFT_EDITION
            result = (clientId != 0) && (clientId != 2875541912);
#else
            result = clientId != 0;
#endif
            
            return result;
        }

        [Background]
        private void LoadExtensions()
        {
            var enabledExtensions = Manager.Instance.Configuration.EnabledExtensions;

            var mres = new Dictionary<int, List<IMainRibbonExtension>>();
     
            #region Load MainRibbonExtensions.
            var mainRibbonExtensions = Manager.Instance.GetExtensionsMetadata<IMainRibbonExtension>()?
                .Where(x => enabledExtensions.ContainsCaseInsensitive(x.Key.Id))
                .OrderBy(x => x.Key.Priority).ToArray();
            if (mainRibbonExtensions != null)
            {
                foreach (var extension in mainRibbonExtensions)
                {
                    if (extension.Value is IMainRibbonExtension mainRibbonExtension)
                    {
                        var priority = extension.Key.Priority;
                        List<IMainRibbonExtension> list = null;
                        if (mres.TryGetValue(priority, out var value))
                        {
                            list = value;
                        }                        
                        else
                        {
                            list = new List<IMainRibbonExtension>();
                            mres.Add(priority, list);
                        }

                        list.Add(mainRibbonExtension);
                    }

                    if (extension.Value is IAsker asker)
                    {
                        asker.Ask += HandleAsk;
                    }
                }
            }
            #endregion

            if (mres != null)
            {
                LoadRibbonExtensions(mres);
            }

            #region Load the other Actions.
            var actions = Manager.Instance.GetExtensionsMetadata<IContextAwareAction>()?
                .Where(x => enabledExtensions.ContainsCaseInsensitive(x.Key.Id))
                .ToArray();
            if (actions?.Any() ?? false)
            {
                foreach (var action in actions)
                {
                    if (action.Value is IExecutionModeSupport executionModeSupport)
                    {
                        executionModeSupport.SetExecutionMode(_executionMode);
                    }

                    if (action.Value is IAsker asker)
                    {
                        asker.Ask += HandleAsk;
                    }
                }
            }
            #endregion

            UpdateFormsList();
            RibbonRefresh();
        }

        [Dispatched]
        private void LoadRibbonExtensions([NotNull] Dictionary<int, List<IMainRibbonExtension>> extensions)
        {
            _ribbonPanelHome.SuspendLayout();
            _ribbonPanelAnalyze.SuspendLayout();
            _ribbonPanelExport.SuspendLayout();
            _ribbonPanelImport.SuspendLayout();
            _ribbonPanelInsert.SuspendLayout();
            _ribbonPanelIntegrate.SuspendLayout();
            _ribbonPanelReview.SuspendLayout();
            _ribbonPanelView.SuspendLayout();
            _ribbonPanelConfigure.SuspendLayout();
            _ribbonPanelHelp.SuspendLayout();

            var ordered = extensions
                .OrderBy(x => x.Key)
                .Select(x => x.Value);

            foreach (var list in ordered)
            {
                foreach (var item in list)
                {
                    if (item is IDesktopAlertAwareExtension desktopAlertAwareExtension)
                    {
                        desktopAlertAwareExtension.ShowMessage += DesktopAlertAwareExtensionOnShowMessage;
                        desktopAlertAwareExtension.ShowWarning += DesktopAlertAwareExtensionOnShowWarning;
                    }

                    LoadMainRibbonExtension(item);
                }
            }

            foreach (var pair in _ribbonBars)
            {
                var ribbon = GetRibbon(pair.Key);
                var width = ribbon.Panel.Controls.OfType<Control>().Sum(x => x.Width);
                if (ribbon != null)
                {
                    foreach (var bar in pair.Value)
                    {
                        bar.Left = width;
                        width += 10;
                        Add(ribbon, bar);
                    }
                }
            }

            _ribbonPanelHome.ResumeLayout();
            _ribbonPanelAnalyze.ResumeLayout();
            _ribbonPanelExport.ResumeLayout();
            _ribbonPanelImport.ResumeLayout();
            _ribbonPanelIntegrate.ResumeLayout();
            _ribbonPanelInsert.ResumeLayout();
            _ribbonPanelReview.ResumeLayout();
            _ribbonPanelView.ResumeLayout();
            _ribbonPanelConfigure.ResumeLayout();
            _ribbonPanelHelp.ResumeLayout();

            RibbonRefresh();
        }

        private void LoadMainRibbonExtension([NotNull] IMainRibbonExtension mainRibbonExtension)
        {
            mainRibbonExtension.ChangeRibbonActionStatus += ChangeRibbonActionStatus;
            mainRibbonExtension.IteratePanels += IteratePanels;
            mainRibbonExtension.RefreshPanels += extension => UpdateFormsList();
            mainRibbonExtension.ClosePanels += ClosePanels;

            if (mainRibbonExtension is IStatusInfoProviderUpdateRequestor updater)
                updater.UpdateStatusInfoProviders += UpdateStatusInfoProviders;

            if (mainRibbonExtension is IExecutionModeSupport executionModeSupport)
            {
                executionModeSupport.SetExecutionMode(_executionMode);
            }

            var actions = mainRibbonExtension.RibbonActions?.ToArray();
            if (actions?.Any() ?? false)
            {
                List<RibbonBar> list;
                RibbonBar bar;
                if (_ribbonBars.TryGetValue(mainRibbonExtension.Ribbon, out var bars))
                {
                    list = bars;
                    bar = bars.FirstOrDefault(x => string.CompareOrdinal(x.Text, mainRibbonExtension.Bar) == 0);
                    if (bar == null)
                    {
                        bar = mainRibbonExtension.Bar.CreateBar();
                        list.Add(bar);
                    }
                }
                else
                {
                    list = new List<RibbonBar>();
                    _ribbonBars.Add(mainRibbonExtension.Ribbon, list);
                    bar = mainRibbonExtension.Bar.CreateBar();
                    list.Add(bar);
                }

                var buttonsPerFactory = new List<ButtonItem>();
                foreach (var action in actions)
                {
                    var button = bar.CreateButton(action, _superTooltip);
                    if (button != null)
                    {
                        button.Click += Button_Click;
                        Add(bar, button);
                        buttonsPerFactory.Add(button);

                        if (string.CompareOrdinal(mainRibbonExtension.PanelsListRibbonAction, action.Name) == 0)
                        {
                            AppendFormsList(mainRibbonExtension, button);
                        }
                    }
                }

                SetButtonsForExtension(mainRibbonExtension, buttonsPerFactory);
            }

            MinimalLoadMainRibbonExtension(mainRibbonExtension);
        }

        private void MinimalLoadMainRibbonExtension([NotNull] IMainRibbonExtension mainRibbonExtension)
        {
            if (mainRibbonExtension is IPanelFactoryActionsRequestor requestor)
            {
                requestor.PanelCreationRequired += CreatePanel;
                requestor.PanelDeletionRequired += DeletePanel;
                requestor.PanelShowRequired += ShowPanel;
            }

            // ReSharper disable once SuspiciousTypeConversion.Global
            if (mainRibbonExtension is IContextAwareExtension contextAwareExtension)
            {
                var actions = Manager.Instance.GetExtensionsMetadata<IContextAwareAction>()?
                    .Where(x => (contextAwareExtension.SupportedScopes & x.Value.Scope) != 0)
                    .OrderBy(x => x.Key.Priority)
                    .Select(x => x.Value)
                    .ToArray();
                if (actions?.Any() ?? false)
                    contextAwareExtension.SetContextAwareActions(actions);
            }
        }

        [Background]
        private void UpdateFormsList()
        {
            foreach (var keyvalue in _mainRibbonExtensions)
            {
                var buttons = GetButtons(keyvalue.Value);
                foreach (var button in buttons)
                {
                    if (button.Tag is IActionDefinition action &&
                        string.CompareOrdinal(keyvalue.Value.PanelsListRibbonAction, action.Name) == 0)
                    {
                        AppendFormsList(keyvalue.Value, button);
                        if (button.SubItems.Count > 0)
                            button.Enabled = true;
                    }
                }
            }

            RibbonRefresh();
        }

        private void HandleAsk(IAsker asker, object context, string caption, string text, bool warning, RequestOptions options)
        {
            MessageBoxButtons buttons;
            MessageBoxDefaultButton defaultButton;
            switch (options)
            {
                case RequestOptions.YesNo:
                    buttons = MessageBoxButtons.YesNo;
                    defaultButton = MessageBoxDefaultButton.Button2;
                    break;
                case RequestOptions.YesNoCancel:
                    buttons = MessageBoxButtons.YesNoCancel;
                    defaultButton = MessageBoxDefaultButton.Button3;
                    break;
                case RequestOptions.OkCancel:
                    buttons = MessageBoxButtons.OKCancel;
                    defaultButton = MessageBoxDefaultButton.Button2;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(options), options, null);
            }

            var result = MessageBox.Show(Form.ActiveForm, text, caption, buttons, 
                warning ? MessageBoxIcon.Warning : MessageBoxIcon.Information, defaultButton);

            AnswerType answer;
            switch (result)
            {
                case DialogResult.OK:
                    answer = AnswerType.Ok;
                    break;
                case DialogResult.Cancel:
                    answer = AnswerType.Cancel;
                    break;
                case DialogResult.Yes:
                    answer = AnswerType.Yes;
                    break;
                case DialogResult.No:
                    answer = AnswerType.No;
                    break;
                default:
                    answer = AnswerType.None;
                    break;
            }

            asker.Answer(context, answer);
        }

        private void HandleExtensionInitializers()
        {
            var initializers = ExtensionUtils.GetExtensions<IExtensionInitializer>()?.ToArray();
            if (initializers?.Any() ?? false)
            {
                foreach (var initializer in initializers)
                {
                    initializer.Initialize();

                    if (initializer is IDesktopAlertAwareExtension alertAwareExtension)
                    {
                        alertAwareExtension.ShowMessage += DesktopAlertAwareExtensionOnShowMessage;
                        alertAwareExtension.ShowWarning += DesktopAlertAwareExtensionOnShowWarning;
                    }
                }
            }
        }
        #endregion

        #region Managing events from Factories.
        private Dictionary<string, List<Guid>> _instances = new Dictionary<string, List<Guid>>();

        [Dispatched]
        private void CreatePanel([NotNull] IPanelFactory factory, IIdentity identity)
        {
            if (factory is IPanelFactory<Form> formFactory)
            {
                switch (factory.Behavior)
                {
                    case InstanceMode.Multiple:
                        AddPanel(formFactory, identity);
                        break;
                    default:
                        if (AddInstance(formFactory, identity))
                        {
                            AddPanel(formFactory, identity);
                        }
                        else
                        {
                            DisplayForm(GetFormId(formFactory, identity));
                        }

                        break;
                }
            }
        }

        private bool AddInstance([NotNull] IPanelFactory<Form> factory, IIdentity identity)
        {
            bool result = false;

            Guid id = identity?.Id ?? Guid.Empty;
            var factoryId = factory.GetExtensionId();
            if (_instances.TryGetValue(factoryId, out var instances))
            {
                if ((factory.Behavior == InstanceMode.Single && instances.Count == 0) ||
                    (factory.Behavior == InstanceMode.SinglePerObject && !instances.Contains(id)))
                {
                    instances.Add(id);
                    result = true;
                }
            }
            else
            {
                instances = new List<Guid> {id};
                _instances[factoryId] = instances;
                result = true;
            }

            return result;
        }

        private void AddPanel([NotNull] IPanelFactory<Form> factory, IIdentity identity)
        {
            bool found = false;

            var formId = GetFormId(factory, identity);
            if (identity is IDiagram diagram)
            {
                found = DisplayForm(formId);
            }

            if (!found)
            {
                ExceptionlessClient.Default.CreateFeatureUsage(factory.GetExtensionLabel()).Submit();
                var panel = factory.Create(identity, out IActionDefinition action);
                var form = CreateForm(factory, panel, action, formId, identity);
                form.Closed += OnFormClosed;
            }
        }

        private void OnFormClosed(object sender, EventArgs args)
        {
            if (sender is PanelContainerForm panelContainerForm)
            {
                var parts = panelContainerForm.FormId.Split('_');
                var factoryId = parts[0];
                var id = new Guid(parts[1]);
                if (_instances.TryGetValue(factoryId, out var instances))
                {
                    instances.Remove(id);
                }

                panelContainerForm.Closed -= OnFormClosed;
            }
        }

        private string GetFormId([NotNull] IPanelFactory<Form> factory, IIdentity identity)
        {
            Guid id = identity?.Id ?? Guid.Empty;
            return GetFormId(factory, id);
        }

        private string GetFormId([NotNull] IPanelFactory<Form> factory, Guid identityId)
        {
            return $"{factory.GetExtensionId()}_{identityId.ToString()}";
        }

        private bool DisplayForm([Required] string formId)
        {
            bool result = false;

            var existing = MdiChildren.OfType<PanelContainerForm>()
                .FirstOrDefault(x => string.CompareOrdinal(x.FormId, formId) == 0);
            if (existing != null)
            {
                result = true;
                existing.Activate();
            }

            return result;
        }

        [Dispatched]
        private void DeletePanel([NotNull] IPanelFactory factory, [NotNull] IPanel panel)
        {
            foreach (var item in _panelsListButtons.Values)
            {
                var subItems = item.SubItems.OfType<BaseItem>().ToArray();
                foreach (BaseItem subItem in subItems)
                {
                    if (subItem.Tag is IActionDefinition action &&
                        panel is IPanel<Form> formPanel &&
                        formPanel.PanelContainer is PanelContainerForm containerForm)
                    {
                        var actionForm = FindForm(action);
                        if (actionForm == containerForm)
                        {
                            subItem.Click -= Button_Click;
                            item.SubItems.Remove(subItem);

                            if (action is INotifyPropertyChanged childButtonAction)
                            {
                                childButtonAction.PropertyChanged -= OnChildButtonActionPropertyChanged;
                            }

                            _childParentActions.Remove(action.Id);
                            break;
                        }
                    }
                }

                if (!item.SubItems.OfType<BaseItem>().Any() &&
                    item.Tag is IActionDefinition itemAction)
                {
                    item.Enabled = itemAction.Enabled;
                }
            }

            (panel as IPanel<Form>)?.PanelContainer?.Close();
        }

        [Dispatched]
        private void ChangeRibbonActionStatus([NotNull] IMainRibbonExtension extension, [Required] string actionName, bool status)
        {
            var buttons = GetButtons(extension);
            if (buttons != null)
            {
                foreach (var button in buttons)
                {
                    if (button?.Tag is IActionDefinition action)
                    {
                        if (string.CompareOrdinal(actionName, action.Name) == 0)
                        {
                            button.Enabled = status;
                        }
                    }
                }
            }
        }

        [Dispatched()]
        private void IteratePanels([NotNull] IMainRibbonExtension factory)
        {
            bool found = false;

            var children = MdiChildren.ToArray();
            if (children.Any())
            {
                bool current = false;
                foreach (var child in children)
                {
                    if (current && child.Tag == factory)
                    {
                        current = false;
                        found = true;
                        child.Focus();
                        break;
                    }

                    if (child == ActiveMdiChild)
                        current = true;
                }

                if (current)
                {
                    foreach (var child in children)
                    {
                        if (child.Tag == factory)
                        {
                            found = true;
                            child.Focus();
                            break;
                        }
                    }
                }
            }
            
            if (!found)
            {
                if (_panelListActionId.TryGetValue(factory.Id, out var actionId) &&
                    _panelsListButtons.TryGetValue(actionId, out var button))
                {
                    var subItem = button.SubItems.OfType<ButtonItem>().FirstOrDefault();
                    subItem?.RaiseClick();
                }                
            }
        }

        [Dispatched()]
        private void ShowPanel(IPanelFactory factory, IPanel panel)
        {
            (panel as IPanel<Form>)?.PanelContainer?.Focus();
        }

        [Dispatched]
        private void OnChildButtonActionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IActionDefinition action)
            {
                var button = GetChildButton(action.Id);

                if (button != null)
                {
                    if (string.CompareOrdinal(e.PropertyName, "Label") == 0)
                    {
                        button.Text = action.Label;
                    }
                }
            }
        }
        #endregion

        #region Ribbon creation and management.
        [Dispatched]
        private void Add([NotNull] RibbonTabItem tab, [NotNull] RibbonBar bar)
        {
            tab.Panel.Controls.Add(bar);
            if (!tab.Visible)
                tab.Visible = true;
        }
        
        [Dispatched(true)]
        private void Add([NotNull] RibbonBar bar, [NotNull] BaseItem item)
        {
            bar.Items.Add(item);
        }

        private RibbonTabItem GetRibbon(Ribbon ribbon)
        {
            return _ribbon.Items.OfType<RibbonTabItem>().FirstOrDefault(x => Enum.TryParse<Ribbon>(x.Text, out var ribbonButton) && ribbonButton == ribbon);
        }

        private RibbonBar GetRibbonBar([NotNull] RibbonPanel ribbon, [Required] string bar)
        {
            return ribbon.Controls.OfType<RibbonBar>().FirstOrDefault(x => bar.IsEqual(x.Text));
        }

        [Dispatched(true)]
        private void RibbonRefresh()
        {
            _ribbon.SuspendLayout();
            _ribbon.Refresh();
            if (_ribbon.SelectedRibbonTabItem == null || !_ribbon.SelectedRibbonTabItem.Visible)
            {
                _ribbon.SelectFirstVisibleRibbonTab();
            }
            _ribbon.SelectedRibbonTabItem.Panel.LayoutRibbons();
            _ribbon.ResumeLayout();

            _ribbon.RecalcLayout();
        }

        [Dispatched]
        private void Button_Click(object sender, EventArgs e)
        {
            if (sender is ButtonItem button)
            {
                if (button.Tag is IActionDefinition actionDefinition)
                {
                    if (_mainRibbonExtensions.TryGetValue(actionDefinition.Id, out var extension))
                    {
                        ExceptionlessClient.Default.CreateFeatureUsage(extension.GetExtensionLabel()).Submit();
                        try
                        {
                            extension.ExecuteRibbonAction(_model, actionDefinition);
                        }
                        catch (Exception exc)
                        {
                            exc.ToExceptionless().Submit();
                        }
                    }
                    else
                    {
                        var form = FindForm(actionDefinition);
                        if (form == null)
                        {
                            var parentId = _childParentActions[actionDefinition.Id];
                            if (_mainRibbonExtensions.TryGetValue(parentId, out var parentExtension) &&
                                parentExtension is IPanelFactory<Form> factory)
                            {
                                ExceptionlessClient.Default.CreateFeatureUsage(factory.GetExtensionLabel()).Submit();
                                var panel = factory.Create(actionDefinition);
                                if (actionDefinition.Tag is IIdentity identity)
                                    CreateForm(factory, panel, actionDefinition, GetFormId(factory, identity), identity);
                            }
                        }
                        else
                        {
                            form.Activate();
                        }
                    }
                }                   
            }
        }
        #endregion

        #region Forms management.
        private PanelContainerForm FindForm([NotNull] IActionDefinition actionDefinition)
        {
            return MdiChildren.OfType<PanelContainerForm>()
                .FirstOrDefault(x => string.CompareOrdinal(x.Name, actionDefinition.Id.ToString("N")) == 0);
        }

        private void AppendFormsList([NotNull] IMainRibbonExtension extension, [NotNull] ButtonItem button)
        {
            var list = extension.GetStartPanelsList(_model)?.ToArray();
            if (list?.Any() ?? false)
            {
                foreach (var item in list)
                {
                    AddDiagramToFormList(button, item);
                }
            }
        }

        private void AddDiagramToFormList([NotNull] ButtonItem button, [NotNull] IActionDefinition item)
        {
            if (button.Tag is IActionDefinition action && !_childParentActions.ContainsKey(item.Id))
            {
                _childParentActions.Add(item.Id, action.Id);

                var childButton = button.CreateButton(item, _superTooltip);
                if (childButton != null)
                {
                    childButton.Click += Button_Click;
                    button.SubItems.Add(childButton);

                    // ReSharper disable once SuspiciousTypeConversion.Global
                    if (item is INotifyPropertyChanged childButtonAction)
                    {
                        childButtonAction.PropertyChanged += OnChildButtonActionPropertyChanged;
                    }
                }
            }
        }

        private PanelContainerForm CreateForm([NotNull] IPanelFactory<Form> factory, 
            [NotNull] IPanel<Form> panel, [NotNull] IActionDefinition action, [Required] string formId, 
            IIdentity identity)
        {
            PanelContainerForm result = null;

            if (panel is UserControl control)
            {
                result = new PanelContainerForm
                {
                    Name = action.Id.ToString("N"), 
                    Tag = factory,
                    FormId = formId
                };
                panel.PanelContainer = result;

                if (panel is IDesktopAlertAwareExtension desktopAlertAwareExtension)
                {
                    desktopAlertAwareExtension.ShowMessage += DesktopAlertAwareExtensionOnShowMessage;
                    desktopAlertAwareExtension.ShowWarning += DesktopAlertAwareExtensionOnShowWarning;
                }

                if (panel is IExecutionModeSupport executionModePanel)
                {
                    executionModePanel.SetExecutionMode(_executionMode);
                }

                if (panel is IShowDiagramPanel<Form> showDiagram)
                {
                    if (showDiagram.Diagram == null)
                    {
                        var diagram = _model?.AddDiagram();
                        if (diagram != null)
                        {
                            result.Text = diagram.Name;
                            ((INotifyPropertyChanged)diagram).PropertyChanged += OnDiagramNameChanged;

                            showDiagram.SetDiagram(diagram);

                            if (factory is IMainRibbonExtension mainRibbonExtension)
                            {
                                AddChildButton(mainRibbonExtension, action);
                            }
                        }

                        result.FormId = GetFormId(factory, diagram);
                    }
                    else
                    {
                        result.Text = showDiagram.Diagram.Name;
                        ((INotifyPropertyChanged)showDiagram.Diagram).PropertyChanged += OnDiagramNameChanged;

                        if (factory is IMainRibbonExtension mainRibbonExtension && 
                            AddChildButton(mainRibbonExtension, action) != null)
                        {
                            MinimalLoadMainRibbonExtension(mainRibbonExtension);
                        }

                        result.FormId = GetFormId(factory, showDiagram.Diagram);
                    }
                }

                if (panel is IShowThreatModelPanel<Form> showThreatModel)
                {
                    result.Text = action.Label;
                    showThreatModel.SetThreatModel(_model);

                    if (factory is IMainRibbonExtension mainRibbonExtension)
                    {
                        AddChildButton(mainRibbonExtension, action);
                    }
                }

                if (panel is IShowScenarioPanel<Form> showScenario && identity is IThreatEventScenario scenario)
                {
                    result.Text = action.Label;
                    showScenario.SetScenario(scenario);
                }

                if (panel is IStaticPanel<Form> staticPanel)
                {
                    result.Text = action.Label;

                    if (factory is IMainRibbonExtension mainRibbonExtension)
                    {
                        AddChildButton(mainRibbonExtension, action);
                    }
                }

                if (panel is ICustomRibbonExtension ribbonExtension)
                {
                    result.InitializeRibbon(ribbonExtension, _mergeIndex);
                }
                else
                {
                    result.HideRibbon();
                }
   
                if (panel is IPanelOpenerExtension panelCreationRequiredExtension)
                {
                    panelCreationRequiredExtension.OpenPanel += CreatePanel;
                }

                result.Panel = panel;
                DisplayChild(result);
                control.Visible = true;
            }

            return result;
        }

        // Workaround for a crash of the MdiForm
        // See: https://social.msdn.microsoft.com/Forums/en-US/e5d0721e-166a-426f-adff-ef00c2f541e0/error-creating-window-handle-innerexception-object-reference-not-set-to-an-instance-of-an-object?forum=csharpgeneral.
        private void DisplayChild(Form child) 
        {
            if (ActiveMdiChild != null && ActiveMdiChild.WindowState == FormWindowState.Maximized) 
            {
                ActiveMdiChild.WindowState = FormWindowState.Normal;
            }
            child.MdiParent = this;
            child.Show();
            child.WindowState = FormWindowState.Maximized;
        }

        [Background]
        private void OnDiagramNameChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IDiagram diagram && string.CompareOrdinal(e.PropertyName, "Name") == 0)
            {
                foreach (var form in MdiChildren)
                {
                    if (form is PanelContainerForm containerForm &&
                        containerForm.Panel is IShowDiagramPanel<Form> diagramPanel &&
                        diagramPanel.Diagram == diagram &&
                        string.CompareOrdinal(form.Text, diagram.Name) != 0)
                    {
                        _titleUpdates.Enqueue(new TitleUpdate
                        {
                            Id = diagramPanel.Id,
                            Text = diagram.Name
                        });

                        SetFormText(form, diagram);
                        _diagramNameIsDirty = true;
                    }
                }
            }
        }

        [Dispatched]
        private void SetFormText([NotNull] Form form, [NotNull] IDiagram diagram)
        {
            form.Text = diagram.Name;
        }

        [Background(IsLongRunning = true)]
        private void DiagramNameUpdater()
        {
            do
            {
                if (_diagramNameIsDirty)
                {
                    RefreshThis();
                    while (_titleUpdates.Count > 0)
                    {
                        RefreshButtons(_titleUpdates.Dequeue());
                    }
                    _diagramNameIsDirty = false;
                }
                Thread.Sleep(1000);
            } while (!_closing);
        }

        [Dispatched(true)]
        private void RefreshThis()
        {
            RefreshCaption();
            Application.DoEvents();
        }

        [Dispatched(true)]
        private void RefreshButtons([NotNull] TitleUpdate update)
        {
            var button = GetChildButton(update.Id);
            if (button != null)
            {
                button.Text = update.Text.Replace("&", "&&");
                Application.DoEvents();
            }
        }

        private void DesktopAlertAwareExtensionOnShowWarning(string text)
        {
            ShowDesktopAlert(text, true);
        }

        private void DesktopAlertAwareExtensionOnShowMessage(string text)
        {
            ShowDesktopAlert(text);
        }

        private void CloseAllForms()
        {
            foreach (var item in _panelsListButtons.Values)
            {
                var subItems = item.SubItems.OfType<BaseItem>().ToArray();
                foreach (BaseItem subItem in subItems)
                {
                    if (subItem.Tag is IActionDefinition action)
                    {
                        subItem.Click -= Button_Click;
                        item.SubItems.Remove(subItem);

                        if (action is INotifyPropertyChanged childButtonAction)
                        {
                            childButtonAction.PropertyChanged -= OnChildButtonActionPropertyChanged;
                        }

                        _childParentActions.Remove(action.Id);
                    }
                }

                if (item.Tag is IActionDefinition itemAction)
                {
                    item.Enabled = itemAction.Enabled;
                }
            }

            foreach (var form in MdiChildren)
            {
                form.Close();
            }
        }

        private void ClosePanels(IPanelFactory factory)
        {
            foreach (var form in MdiChildren)
            {
                if (form is PanelContainerForm panelContainerForm &&
                    panelContainerForm.Tag is IPanelFactory panelFactory &&
                    panelFactory.GetExtensionId() == factory.GetExtensionId())
                {
                    form.Close();
                }
            }
        }
        #endregion

        #region Factories, Buttons and Childs management.
        private void SetButtonsForExtension(IMainRibbonExtension extension, [NotNull] IEnumerable<ButtonItem> buttons)
        {
            _mainRibbonExtensions[extension.Id] = extension;
            var buttonsArray = buttons.ToArray();
            _buttons[extension.Id] = buttonsArray;

            foreach (var button in buttonsArray)
            {
                if (button.Tag is IActionDefinition action &&
                    string.CompareOrdinal(extension.PanelsListRibbonAction, action.Name) == 0)
                {
                    _panelsListButtons[action.Id] = button;
                    _panelListActionId[extension.Id] = action.Id;
                }
            }
        }

        private IEnumerable<ButtonItem> GetButtons([NotNull] IMainRibbonExtension extension)
        {
            if (_buttons.TryGetValue(extension.Id, out var buttons))
                return buttons;
            else
                return null;
        }

        private ButtonItem GetChildButton(Guid id)
        {
            ButtonItem result = null;

            if (_childParentActions.TryGetValue(id, out var parentId) &&
                _panelsListButtons.TryGetValue(parentId, out var parentButtonItem))
            {
                result = parentButtonItem.SubItems.OfType<ButtonItem>().FirstOrDefault(x => (x.Tag as IActionDefinition)?.Id == id);
            }

            return result;
        }

        private ButtonItem AddChildButton([NotNull] IMainRibbonExtension extension, [NotNull] IActionDefinition action)
        {
            ButtonItem result = null;

            if (_panelListActionId.TryGetValue(extension.Id, out var parentActionId) &&
                _panelsListButtons.TryGetValue(parentActionId, out var button) &&
                !_childParentActions.ContainsKey(action.Id))
            {
                _childParentActions.Add(action.Id, parentActionId);

                result = button.CreateButton(action, _superTooltip);
                if (result != null)
                {
                    result.Click += Button_Click;
                    button.SubItems.Add(result);

                    // ReSharper disable once SuspiciousTypeConversion.Global
                    if (action is INotifyPropertyChanged childButtonAction)
                    {
                        childButtonAction.PropertyChanged += OnChildButtonActionPropertyChanged;
                    }
                }
            }

            return result;
        }
        #endregion
    }
}