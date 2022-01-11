using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using Exceptionless;
using Exceptionless.Models.Collections;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Dialogs;
using PostSharp.Patterns.Threading;
using ThreatsManager.Engine;
using ThreatsManager.Engine.Config;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Exceptions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DevComponents.DotNetBar.Metro.ColorTables;
using Exceptionless.Logging;
using ThreatsManager.Interfaces;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager
{
    public partial class MainForm : RibbonForm
    {
        #region Private member variables.
        private IThreatModel _model;
        private LocationType _currentLocationType;
        private string _currentLocation;
        private IPackageManager _currentPackageManager;
        //private SecureString _password;
        private bool _closing;
        private ExecutionMode _executionMode;

        private bool _errorsOnLoading;
        private List<string> _missingTypes = new List<string>();

        private int _mergeIndex;

        private int _ribbonHeight;

        private JumpListManager _jumpListManager;
        #endregion

        #region Nested classes.
        private class AcquireLockOutput
        {
            public AcquireLockOutput(string location,string latest,
                OpenOutcome outcome)
            {
                Location = location;
                Latest = latest;
                Outcome = outcome;
            }

            public string Location { get; }
            public string Latest { get; }
            public OpenOutcome Outcome { get; }
        }
        #endregion

        #if MICROSOFT_EDITION
        private const string LatestVersion = "MSLatestVersion";
        private const string Highlights = "MSHighlights";
        #else
        private const string LatestVersion = "LatestVersion";
        private const string Highlights = "Highlights";
        #endif

        public MainForm()
        {
            InitializeComponent();

            PanelContainerForm.InstanceCreated += OnPanelContainerFormCreated;
            PanelContainerForm.InstanceClosed += OnPanelContainerFormClosed;
            PanelContainerForm.InstanceTextChanged += OnPanelContainerFormTextChanged;

            _ribbonHeight = _ribbon.Height;
            _title.PaddingBottom = (int) (_title.PaddingBottom * Dpi.Factor.Height);

            DocumentLocker.OwnershipObtained += OwnershipObtained;
            DocumentLocker.OwnershipRequested += OwnershipRequested;
        }

        #region Form events.
        private async void MainForm_Load(object sender, EventArgs e)
        {
            Text = Resources.Title;

            #region Telemetry
            bool disableTelemetry = true;
            var consentString = Application.UserAppDataRegistry?.GetValue("Consent") as string;
            if (consentString == null)
            {
                if (MessageBox.Show(this, Resources.ConsentMessage,
                        Resources.ConsentCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                        MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    Application.UserAppDataRegistry?.SetValue("Consent", true);
                    disableTelemetry = false;
                }
                else
                {
                    disableTelemetry = true;
                }
            }
            else if (bool.TryParse(consentString, out var consent))
            {
                disableTelemetry = !consent;
            }
            

            if (disableTelemetry)
            {
                _styleManager.MetroColorParameters = new MetroColorGeneratorParameters(Color.White, Color.DarkOrange);
            }
            else
            {
                ExceptionlessClient.Default.Configuration.IncludePrivateInformation = false;
                ExceptionlessClient.Default.Configuration.UseSessions(true, new TimeSpan(0, 0, 10, 0), true);
                ExceptionlessClient.Default.Configuration.ReadFromAttributes();
#if LOGGING
#pragma warning disable SecurityIntelliSenseCS // MS Security rules violation
            var logFile = Path.Combine(Program.Folder, "tms_exceptionless.log");
#pragma warning restore SecurityIntelliSenseCS // MS Security rules violation
                
            ExceptionlessClient.Default.Configuration.UseFileLogger(logFile);
#endif
                ExceptionlessClient.Default.Register(false);
                ExceptionlessClient.Default.SubmittingEvent += OnSubmittingEvent;
                ExceptionlessClient.Default.Configuration.Settings.Changed += OnExceptionlessSettingsChanged;
            }
            #endregion

            #region Initial Configuration.
            var config = ExtensionsConfigurationManager.GetConfigurationSection();

            if (!config.Setup && !config.SmartSave &&
                !(config.Prefixes?.OfType<PrefixConfig>().Any() ?? false) && 
                !(config.Certificates?.OfType<CertificateConfig>().Any() ?? false) &&
                !(config.Folders?.OfType<FolderConfig>().Any() ?? false))
            {
                using (var init = new InitializationDialog())
                {
                    if (init.ShowDialog(this) == DialogResult.OK)
                    {
                        if (init.AutomaticExtensionsConfig)
                        {
                            config.Folders.Add(new FolderConfig()
                            {
                                Name = Path.Combine(
                                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                    "ThreatsManagerPlatform")
                            });

                            config.Prefixes.Add(new PrefixConfig()
                            {
                                Name = "ThreatsManager"
                            });

                            var assembly = Assembly.GetExecutingAssembly();
#pragma warning disable SecurityIntelliSenseCS // MS Security rules violation
                            var certificate = assembly.GetModules().First().GetSignerCertificate();
#pragma warning restore SecurityIntelliSenseCS // MS Security rules violation
                            if (certificate != null)
                            {
                                var certConfig = new CertificateConfig(certificate);
                                config.Certificates.Add(certConfig);
                            }
                        }

                        config.Mode = init.Mode;

                        if (init.SmartSave)
                        {
                            config.SmartSave = true;
                            config.SmartSaveCount = init.SmartSaveCount;
                            config.SmartSaveInterval = init.SmartSaveInterval;
                        }

                        config.UserDictionary = Path.Combine(Program.Folder, "ThreatsManagerPlatform_userdict.txt");
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(config.UserDictionary))
                        {
                            config.UserDictionary = Path.Combine(Program.Folder, "ThreatsManagerPlatform_userdict.txt");
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(config.ExtensionsConfigFolder))
                    config.ExtensionsConfigFolder = Program.Folder;

                config.Setup = true;
                config.CurrentConfiguration.Save();
            }
            #endregion

            #region Resource Guard.
            var resourceGuard = new ResourcesGuard();
            resourceGuard.StartChecking();
            #endregion

            #region User dictionary configuration.
            if (!string.IsNullOrWhiteSpace(config.UserDictionary) && !File.Exists(config.UserDictionary))
            {
                try
                {
                    using (var textStream = File.CreateText(config.UserDictionary))
                    {
                        textStream.Close();
                    }
                }
                catch
                {
                    ShowDesktopAlert(Resources.RansomwareProtectionMessage, true);
                }
            }
            #endregion

            #region Extensions configuration.
            if (string.IsNullOrWhiteSpace(config.ExtensionsConfigFolder))
            {
                config.ExtensionsConfigFolder = Program.Folder;
                config.CurrentConfiguration.Save();
            }
            ExtensionUtils.ExtensionConfigurationFolder = config.ExtensionsConfigFolder;

            InitializeExtensionsManagement();
            #endregion

            try
            {
                var commandLineArgs = Environment.GetCommandLineArgs();
                if (commandLineArgs.Length == 2)
                {
                    var fileName = commandLineArgs[1];
                    if (File.Exists(fileName))
                    {
                        OpenOutcome outcome = OpenOutcome.KO;

                        if ((string.CompareOrdinal(Path.GetExtension(fileName.ToLower()), ".tm") == 0) ||
                            string.CompareOrdinal(Path.GetExtension(fileName.ToLower()), ".tmj") == 0)
                        {
                            var packageManager = Manager.Instance.GetExtensions<IPackageManager>()?
                                .FirstOrDefault(x => x.CanHandle(LocationType.FileSystem, fileName));
                            if (packageManager != null)
                            {
                                outcome = await OpenAsync(packageManager, LocationType.FileSystem, fileName);
                            }
                            else
                            {
                                ShowDesktopAlert("The selected document cannot be opened, most probably because its location is not supported.", true);
                            }

                            if (outcome == OpenOutcome.OK && _model != null)
                            {
                                _model.SuspendDirty();
                                ShowDesktopAlert("Document successfully opened.");
                            }
                        }
                        else if ((string.CompareOrdinal(Path.GetExtension(fileName.ToLower()), ".tmt") == 0) ||
                                 string.CompareOrdinal(Path.GetExtension(fileName.ToLower()), ".tmk") == 0)
                        {
                            InitializeStatus(null);
                            _model?.SuspendDirty();
                            IThreatModel template = null;

                            try
                            {
                                template = TemplateManager.OpenTemplate(fileName);
                                _model?.Merge(template, new DuplicationDefinition()
                                {
                                    AllEntityTemplates = true,
                                    AllThreatTypes = true,
                                    AllMitigations = true,
                                    AllPropertySchemas = true,
                                    AllSeverities = true,
                                    AllStrengths = true
                                });
                                ShowDesktopAlert($"Template '{template.Name}' has been applied successfully.");
                                outcome = OpenOutcome.OK;
                            }
                            finally
                            {
                                if (template != null)
                                    TemplateManager.CloseTemplate(template.Id);
                            }
                        }

                        if (outcome != OpenOutcome.OK || _model == null)
                        {
                            InitializeStatus(null);
                            _model.SuspendDirty();
                        }
                    }
                    else
                    {
                        InitializeStatus(null);
                        _model.SuspendDirty();
                    }
                }
                else
                {
                    InitializeStatus(null);
                    _model.SuspendDirty();
                }

                LoadExtensions();
                _mergeIndex = _ribbon.Items.IndexOf(_title);

                LoadStatusInfoProviders();

                // TODO: chiudere la notifica di caricamento delle estensioni.

                LoadKnownDocuments();

                CheckUpdateAvailability();

                ConfigureTimer();

                SpellCheckConfig.UserDictionary = config.UserDictionary;
                KnownTypesBinder.TypeNotFound += OnTypeNotFound;
            }
            finally
            {
                _model?.ResumeDirty();
            }
        }

        private void OnTypeNotFound(string assemblyName, string typeName)
        {
            _errorsOnLoading = true;

            if (string.CompareOrdinal(assemblyName, "mscorlib") == 0)
            {
                var regex = new Regex(@"\[\[(?<class>[.\w]*), (?<assembly>[.\w]*)\]\]");
                var match = regex.Match(typeName);
                if (match.Success)
                {
                    assemblyName = match.Groups["assembly"].Value;
                    typeName = match.Groups["class"].Value;
                }
            }
            var name = $"{assemblyName}#{typeName}";

            if (!_missingTypes.Contains(name))
            {
                _missingTypes.Add(name);
                var parts = typeName.Split('.');
                ShowDesktopAlert($"Document uses type {parts.Last()} from {assemblyName}, which is unknown.\nThe document will be loaded but some information may be missing.", true);
            }
        }

        private void OnDirtyChanged(IDirty dirtyObject, bool dirty)
        {
            RefreshCaption();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_model?.IsDirty ?? false)
            {
                var dialogResult = MessageBox.Show(this,
                    "The document has changed. Do you want to Save it before exiting?\n\nPlease press Yes to save and close.\nPress No to close without saving.\nPress Cancel to abort.",
                    "Current Document has changed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);

                if (dialogResult == DialogResult.Yes)
                    _save_Click(sender, e);
                else if (dialogResult == DialogResult.Cancel)
                    e.Cancel = true;
            }

            if (!e.Cancel)
            {
                DocumentLocker.ReleaseLock();
                ResourcesGuard.StopChecking();
                _closing = true;
            }
        }

        [Dispatched(true)]
        private void OnSubmittingEvent(object sender, EventSubmittingEventArgs e)
        {
            if (!e.IsUnhandledError)
                return;

            MessageBox.Show(
                "Apologies, but it looks like a problem has just occurred.\nThreats Manager Studio needs to be closed.\n\nThe good news is that nothing is lost: you can still save your work.",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            try
            {
                EmergencySave();

                Application.Exit();
            }
            catch (Exception exception)
            {
                exception.ToExceptionless().AddTags("Handled").Submit();
            }
        }
        
        private void EmergencySave()
        {
            var packageManagers = Manager.Instance.GetExtensions<IPackageManager>()?
                .Where(x => x.SupportedLocations.HasFlag(LocationType.FileSystem)).ToArray();

            if (packageManagers?.Any() ?? false)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var packageManager in packageManagers)
                {
                    if (builder.Length > 0)
                        builder.Append("|");

                    builder.Append(packageManager.GetFilter(LocationType.FileSystem));
                }

                _saveAsFile.Title = "Emergency Save";
                _saveAsFile.Filter = builder.ToString();
                _saveAsFile.FileName = _model?.Name;

                if (_currentPackageManager != null && !string.IsNullOrWhiteSpace(_currentLocation))
                {
                    int pos = -1;
                    for (int i = 0; i < packageManagers.Length; i++)
                    {
                        if (packageManagers[i] == _currentPackageManager)
                        {
                            pos = i + 1;
                            break;
                        }
                    }

                    if (pos > 0)
                    {
                        _saveAsFile.FilterIndex = pos;
                    }

                    _saveAsFile.FileName = Path.Combine(Path.GetDirectoryName(_currentLocation),
                        $"{Path.GetFileNameWithoutExtension(_currentLocation)}.recover{Path.GetExtension(_currentLocation)}");
                }

                if (_saveAsFile.ShowDialog(this) == DialogResult.OK)
                {
                    var packageManager = packageManagers.ElementAt(_openFile.FilterIndex - 1);

                    Save(packageManager, LocationType.FileSystem, _saveAsFile.FileName);
                }
            }
            else
            {
                ShowDesktopAlert("An exception has occurred and Threats Manager Studio needs to close. An issue is also preventing emergency save.", true);
            }
        }

        private void OnExceptionlessSettingsChanged(object sender, ChangedEventArgs<KeyValuePair<string, string>> e)
        {
            if (string.CompareOrdinal(e.Item.Key, LatestVersion) == 0)
            {
                CheckUpdateAvailability();
            }
        }

        [Background]
        private void CheckUpdateAvailability()
        {
            var newVersion = CheckIfUpdateRequired();
            if (newVersion != null)
            {
                var highlights = ExceptionlessClient.Default.Configuration.Settings.GetString(Highlights);
                if (string.CompareOrdinal(highlights, "null") == 0)
                    highlights = null;

                ShowUpdateMessage(newVersion, highlights);
            }
        }

        [Dispatched(true)]
        private void ShowUpdateMessage(Version version, string highlights)
        {
            using (var dialog = new NewVersionDialog())
            {
                dialog.Initialize(version, highlights);
                dialog.ShowDialog(this);
            }
        }

        private Version CheckIfUpdateRequired()
        {
            Version result = null;

            var latestVersion = ExceptionlessClient.Default.Configuration.Settings.GetString(LatestVersion);
            if (!string.IsNullOrWhiteSpace(latestVersion))
            {
                var version = new Version(latestVersion);

                if (version > Assembly.GetExecutingAssembly().GetName().Version)
                    result = version;
            }

            return result;
        }

        private void _windows_Click(object sender, EventArgs e)
        {
            var buttons = _windows.SubItems.OfType<ButtonItem>().ToArray();

            if (buttons.Length >= 2)
            {
                var nextForm = buttons[0].Tag as PanelContainerForm;

                bool found = false;
                var activeForm = this.ActiveMdiChild as PanelContainerForm;

                if (activeForm != null)
                {
                    foreach (var curr in buttons)
                    {
                        if (found)
                        {
                            nextForm = curr.Tag as PanelContainerForm;
                            break;
                        }

                        if (curr.Tag is PanelContainerForm currForm && currForm == activeForm)
                        {
                            found = true;
                        }
                    }
                }

                nextForm?.Activate();
            }
        }

        private void _feedback_Click(object sender, EventArgs e)
        {
            using (var dialog = new FeedbackDialog())
            {
                if (dialog.ShowDialog(this) == DialogResult.OK &&
                    !string.IsNullOrWhiteSpace(dialog.Comments))
                {
                    ExceptionlessClient.Default.CreateLog("Feedback", dialog.Smile ? "Smile" : "Frown", LogLevel.Info)
                        .SetProperty("Email", dialog.Email)
                        .SetProperty("Authorization", dialog.Authorization.ToString())
                        .AddObject(dialog.Comments, "Comments")
                        .Submit();

                    MessageBox.Show(Properties.Resources.Feedback_ThankYou,
                        "Thank YOU!", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
            }
        }

        private void _closeCurrentWindow_Click(object sender, EventArgs e)
        {
            ActiveMdiChild?.Close();
        }

        private void _closeAllWindows_Click(object sender, EventArgs e)
        {
            var forms = Application.OpenForms.OfType<PanelContainerForm>().ToArray();
            foreach (var form in forms)
            {
                form.Close();
            }
        }
        #endregion

        #region Buttons: File.
        private void _new_Click(object sender, EventArgs e)
        {
            bool abort = false;
            if (_model?.IsDirty ?? false)
            {
                var dialogResult = MessageBox.Show(this,
                    "The document has changed. Do you want to Save it before opening the new one?\n\nPlease press Yes to save it and open the new file.\nPress No to proceed without saving the current document.\nPress Cancel to abort.",
                    "Current Document has changed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);

                if (dialogResult == DialogResult.Yes)
                    _save_Click(sender, e);
                else if (dialogResult == DialogResult.Cancel)
                    abort = true;
            }

            if (!abort)
            {
                DocumentLocker.ReleaseLock();
                _lockRequest.Visible = false;
                _currentLocationType = LocationType.FileSystem;
                _currentLocation = null;
                InitializeStatus(null);
                ShowDesktopAlert("Document successfully initialized.");
            }
        }

        private async void _open_Click(object sender, EventArgs e)
        {
            bool abort = false;

            if (_model?.IsDirty ?? false)
            {
                var dialogResult = MessageBox.Show(this,
                    "The document has changed. Do you want to Save it before opening the new one?\n\nPlease press Yes to save it and open the new file.\nPress No to proceed without saving the current document.\nPress Cancel to abort.",
                    "Current Document has changed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);

                if (dialogResult == DialogResult.Yes)
                    _save_Click(sender, e);
                else if (dialogResult == DialogResult.Cancel)
                    abort = true;
            }

            if (!abort)
            {
                var packageManagers = Manager.Instance.GetExtensions<IPackageManager>()?
                    .Where(x => x.SupportedLocations.HasFlag(LocationType.FileSystem)).ToArray();

                StringBuilder builder = new StringBuilder();
                foreach (var packageManager in packageManagers)
                {
                    if (builder.Length > 0)
                        builder.Append("|");

                    builder.Append(packageManager.GetFilter(LocationType.FileSystem));
                }

                _openFile.Filter = builder.ToString();

                if (_openFile.ShowDialog(this) == DialogResult.OK &&
                    string.CompareOrdinal(_openFile.FileName, _currentLocation) != 0)
                {
                    var packageManager = packageManagers.ElementAt(_openFile.FilterIndex - 1);
                    if (packageManager.CanHandle(LocationType.FileSystem, _openFile.FileName) &&
                        (await OpenAsync(packageManager, LocationType.FileSystem, _openFile.FileName)) == OpenOutcome.OK)
                    {
                        _lockRequest.Visible = false;
                        UpdateFormsList();
                        AddKnownDocument(packageManager, LocationType.FileSystem, _openFile.FileName);
                        ShowDesktopAlert("Document successfully opened.");
                    }
                    else
                    {
                        ShowDesktopAlert("The selected document cannot be opened, most probably because its location is not supported.", true);
                    }
                }
            }
        }

        private void _save_Click(object sender, EventArgs e)
        {
            if (_currentPackageManager != null && !string.IsNullOrWhiteSpace(_currentLocation))
            {
                if (Save())
                {

                    ShowDesktopAlert("Document saved successfully.");
                }
                else
                {
                    ShowDesktopAlert("Document save failed.", true);
                }
            }
            else
            {
                _saveAs_Click(sender, e);
            }
        }

        private void _saveAs_Click(object sender, EventArgs e)
        {
            var packageManagers = Manager.Instance.GetExtensions<IPackageManager>()?
                .Where(x => x.SupportedLocations.HasFlag(LocationType.FileSystem)).ToArray();

            StringBuilder builder = new StringBuilder();
            foreach (var packageManager in packageManagers)
            {
                if (builder.Length > 0)
                    builder.Append("|");

                builder.Append(packageManager.GetFilter(LocationType.FileSystem));
            }
            _saveAsFile.Filter = builder.ToString();
            _saveAsFile.FileName = _model?.Name;

            if (_currentPackageManager != null && !string.IsNullOrWhiteSpace(_currentLocation))
            {
                int pos = -1;
                for (int i = 0; i < packageManagers.Length; i++)
                {
                    if (packageManagers[i] == _currentPackageManager)
                    {
                        pos = i + 1;
                        break;
                    }
                }

                if (pos > 0)
                {
                    _saveAsFile.FilterIndex = pos;
                }

                _saveAsFile.FileName = _currentLocation;
            }

            if (_saveAsFile.ShowDialog(this) == DialogResult.OK)
            {
                var packageManager = packageManagers.ElementAt(_saveAsFile.FilterIndex - 1);

                if (Save(packageManager, LocationType.FileSystem, _saveAsFile.FileName))
                {
                    _currentPackageManager = packageManager;
                    _currentLocationType = LocationType.FileSystem;
                    _currentLocation = _saveAsFile.FileName;

                    AddKnownDocument(packageManager, LocationType.FileSystem, _saveAsFile.FileName);

                    ShowDesktopAlert("Document saved successfully.");
                }
                else
                {
                    ShowDesktopAlert("Document save failed.", true);
                }
            }
        }

        private void _close_Click(object sender, EventArgs e)
        {
            bool abort = false;

            if (_model?.IsDirty ?? false)
            {
                var dialogResult = MessageBox.Show(this,
                    "The document has changed. Do you want to Save it?\n\nPlease press Yes to save and then close it.\nPress No to close without saving.\nPress Cancel to abort.",
                    "Current Document has changed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);

                if (dialogResult == DialogResult.Yes)
                    _save_Click(sender, e);
                else if (dialogResult == DialogResult.Cancel)
                    abort = true;
            }

            if (!abort)
            {
                DocumentLocker.ReleaseLock();
                _lockRequest.Visible = false;
                _currentLocationType = LocationType.FileSystem;
                _currentLocation = null;
                InitializeStatus(null);
                GcCollect();
                ShowDesktopAlert("Document closed.");
            }
        }

        private void _exit_Click(object sender, EventArgs e)
        {
            try
            {
                DocumentLocker.ReleaseLock();
                Application.Exit();
            }
            catch (Exception)
            {
                try
                {
                    Environment.FailFast("Failure on exit");
                }
                catch
                {
                }
            }
        }

        private void _extensionsConfig_Click(object sender, EventArgs e)
        {
            using (ExtensionsConfigDialog dialog = new ExtensionsConfigDialog())
            {
                dialog.ShowDialog(this);
            }
        }

        private void _about_Click(object sender, EventArgs e)
        {
            using (var about = new AboutBox())
            {
                about.ShowDialog(this);
            }
        }
 
        private void _options_Click(object sender, EventArgs e)
        {
            using (var options = new OptionsForm(_model))
            {
                if (options.ShowDialog(this) == DialogResult.OK)
                {
                    ConfigureTimer();
                    var config = ExtensionsConfigurationManager.GetConfigurationSection();
                    SpellCheckConfig.UserDictionary = config.UserDictionary;
                }
            }
        }
        #endregion

        #region Private auxiliary members.
        [Dispatched]
        private void ShowDesktopAlert(string text, bool warning = false)
        {
            DesktopAlertWindow alert = new DesktopAlertWindow
            {
                Text = text,
                AutoSize = false,
                AlertPosition = eAlertPosition.BottomRight,
                AutoCloseTimeOut = 10
            };
            //alert.DefaultAlertSize = Dpi.Size(800, 128);
            if (warning)
            {
                alert.BackColor = ColorScheme.GetColor(0xCA5010);
                alert.ForeColor = Color.White;
            }
            else
            {
                alert.BackColor = ColorScheme.GetColor(0x4472C4);
                alert.ForeColor = Color.White;
                alert.PlaySound = false;
            }
            alert.Show();

        }

        private void InitializeStatus(IThreatModel model)
        {
            if (_model != null)
            {
                _model.DirtyChanged -= OnDirtyChanged;
                ThreatModelManager.Remove(_model.Id);
            }

            if (model == null)
            {
                _model = ThreatModelManager.GetDefaultInstance();
                _model.SuspendDirty();
                _model.Owner = UserName.GetDisplayName();
                _model.ResumeDirty();
            }
            else
            {
                _model = model;
            }
            _model.DirtyChanged += OnDirtyChanged;

            //ThreatModelManager.Model.ChildCreated += ModelChildCreated;
            //ThreatModelManager.Model.ChildRemoved += ModelChildRemoved;

            CloseAllForms();
            UpdateStatusInfoProviders();
            RefreshCaption();
            _model.ResetDirty();

            ((INotifyPropertyChanged) _model).PropertyChanged += OnThreatModelPropertyChanged;
        }

        //private void ModelChildRemoved(IIdentity obj)
        //{
        //    if (obj is IDiagram diagram)
        //    {

        //    }
        //}

        //private void ModelChildCreated(IIdentity obj)
        //{
        //    if (obj is IDiagram diagram)
        //    {

        //    }
        //}

        [Dispatched]
        private void RefreshCaption()
        {
            var modelName = _model?.Name;
            if ((modelName?.Length ?? 0) > 200)
                modelName = modelName.Substring(0, 200) + "…";
            var text = $@"{((_model?.IsDirty ?? false) ? "*" : "")}{modelName}";
            Text = $@"{((_model?.IsDirty ?? false) ? "*" : "")}{Resources.Title} - {_model?.Name}";

            var currentChild = ActiveMdiChild?.Text.Replace("\n", " ");
            _title.Text = string.IsNullOrWhiteSpace(currentChild) ? text : $"{text} - [{currentChild}]";
            _title.Width = TextRenderer.MeasureText(_title.Text, _title.Font).Width + _title.PaddingLeft + _title.PaddingRight;
            _ribbon.Refresh();
        }

        private void OnThreatModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IThreatModel threatModel && 
                threatModel == _model &&
                string.CompareOrdinal(e.PropertyName, "Name") == 0)
            {
                RefreshCaption();
            }
        }

        private async Task<OpenOutcome> OpenAsync([NotNull] IPackageManager packageManager, 
            LocationType locationType, [Required] string location)
        {
            OpenOutcome result = OpenOutcome.KO;

            IThreatModel model = null;

            var oldMissingTypes = new List<string>(_missingTypes);
            var oldErrorsOnLoading = _errorsOnLoading;
            _missingTypes.Clear();
            _errorsOnLoading = false;
            bool messageRaised = false;

            try
            {
                var enabledExtensions = Manager.Instance.Configuration.EnabledExtensions
                    .Select(x => Manager.Instance.GetExtensionMetadata(x));

                var latest = packageManager.GetLatest(locationType, location, out var dateTime);

                if (locationType == LocationType.FileSystem)
                {
                    var output = await AcquireLockAsync(location, latest);
                    result = output?.Outcome ?? OpenOutcome.KO;
                    if (result == OpenOutcome.OK)
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        location = output.Location;
                        latest = output.Latest;
                    }
                    else
                    {
                        return result;
                    }
                }

                if (latest != null) // && 
                    //MessageBox.Show(Form.ActiveForm, 
                    //$"A newer version is available, created on {dateTime.ToString("g")}.\nDo you want to open the newest version instead?",
                    //"Open newest version", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    location = latest;
                }

                if (packageManager is ISecurePackageManager securePM)
                {
                    // TODO: Handle the Secure Package Manager.
                }
                else
                {
                    model = packageManager.Load(locationType, location, enabledExtensions, false);
                }

                if (!(model?.Strengths?.Any() ?? false))
                    model?.InitializeStandardStrengths();
                if (!(model?.Severities?.Any() ?? false))
                    model?.InitializeStandardSeverities();
            }
            catch (ThreatModelOpeningFailureException exc)
            {
                exc.ToExceptionless().Submit();
                model = null;
                using (var dialog = new ErrorDialog
                       {
                           Title = "Threat Model Opening failure",
                           Description = exc.Message
                       })
                {
                    dialog.ShowDialog(this);
                }

                messageRaised = true;
            }
            catch (ExistingModelException)
            {
                ShowDesktopAlert("The model is already open.\nClose it if you want to load it again.", true);
                model = null;
                messageRaised = true;
            }
            catch (FileNotFoundException)
            {
                ShowDesktopAlert("File has not been found.", true);
                model = null;
                messageRaised = true;
            }
            catch (DirectoryNotFoundException)
            {
                ShowDesktopAlert("Directory has not been found.", true);
                model = null;
                messageRaised = true;
            }
            catch (UnauthorizedAccessException)
            {
                ShowDesktopAlert("The model cannot be opened because you do now have the required rights.", true);
                model = null;
                messageRaised = true;
            }
            catch (IOException e)
            {
                ShowDesktopAlert(e.Message, true);
                model = null;
                messageRaised = true;
            }
            catch (Exception e)
            {
                ShowDesktopAlert($"An exception occurred loading the Threat Model:\n{e.Message}", true);
                e.ToExceptionless().Submit();
                model = null;
                messageRaised = true;
            }

            if (model != null)
            {
                result = OpenOutcome.OK;
                _currentLocationType = locationType;
                _currentLocation = location;
                _currentPackageManager = packageManager;
                InitializeStatus(model);
                model.SetLocation(location);
            }
            else
            {
                _missingTypes.AddRange(oldMissingTypes);
                _errorsOnLoading = oldErrorsOnLoading;

                if (!messageRaised)
                    ShowDesktopAlert("File cannot be opened.", true);
            }

            return result;
        }

        private async Task<AcquireLockOutput> AcquireLockAsync(string location, string latest)
        {
            var openOutcome = OpenOutcome.KO;

            var lockInfo = await DocumentLocker.AcquireLockAsync(location, false);
            if (lockInfo?.Owned ?? false)
            {
                openOutcome = OpenOutcome.OK;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(lockInfo?.Owner) || string.IsNullOrWhiteSpace(lockInfo?.Machine))
                {
                    ShowDesktopAlert("Lock cannot be acquired.", true);
                    openOutcome = OpenOutcome.KO;
                } 
                else
                {
                    var action = new ThreatModelInUseNotification()
                        .Show(lockInfo.Owner, lockInfo.Machine, lockInfo.Timestamp, lockInfo.PendingRequests);

                    switch (action)
                    {
                        case ThreatModelInUseAction.WorkWithCopyNotify:
                            await DocumentLocker.AcquireLockAsync(location);
                            if (_selectFolder.ShowDialog(this) == DialogResult.OK)
                            {
                                if (string.Compare(_selectFolder.SelectedPath, Path.GetDirectoryName(location),
                                    StringComparison.InvariantCultureIgnoreCase) != 0)
                                {
                                    var newLocation = Path.Combine(_selectFolder.SelectedPath,
                                        Path.GetFileName(location));
                                    var newLatest = Path.Combine(_selectFolder.SelectedPath,
                                        Path.GetFileName(latest));
                                    File.Copy(location, newLocation, true);
                                    if (!string.IsNullOrWhiteSpace(latest))
                                        File.Copy(latest, newLatest, true);

                                    location = newLocation;
                                    latest = newLatest;

                                    openOutcome = OpenOutcome.OK;

                                    ShowDesktopAlert(
                                        "A copy of the file has been opened.\nYou will be informed as soon as the original one has been released.",
                                        false);
                                }
                            }
                            else
                            {
                                ShowDesktopAlert("File is already in use and cannot be opened.\nYou will be informed as soon as it is released.", false);
                                openOutcome = OpenOutcome.Ownership;
                            }

                            break;
                        case ThreatModelInUseAction.WorkWithCopy:
                            if (_selectFolder.ShowDialog(this) == DialogResult.OK)
                            {
                                if (string.Compare(_selectFolder.SelectedPath, Path.GetDirectoryName(location),
                                    StringComparison.InvariantCultureIgnoreCase) != 0)
                                {
                                    var newLocation = Path.Combine(_selectFolder.SelectedPath,
                                        Path.GetFileName(location));
                                    var newLatest = Path.Combine(_selectFolder.SelectedPath,
                                        Path.GetFileName(latest));
                                    File.Copy(location, newLocation, true);
                                    if (!string.IsNullOrWhiteSpace(latest))
                                        File.Copy(latest, newLatest, true);

                                    location = newLocation;
                                    latest = newLatest;

                                    lockInfo = await DocumentLocker.AcquireLockAsync(location);
                                    openOutcome = OpenOutcome.OK;
                                    ShowDesktopAlert("A copy of the file has been opened.");
                                }
                            }
                            else
                            {
                                ShowDesktopAlert("File is already in use and cannot be opened.", true);
                                openOutcome = OpenOutcome.Ownership;
                            }
                            break;
                        case ThreatModelInUseAction.Notify:
                            await DocumentLocker.AcquireLockAsync(location);
                            ShowDesktopAlert("File is already in use and cannot be opened.\nYou will be informed as soon as it is released.", true);
                            openOutcome = OpenOutcome.Ownership;
                            break;
                        default:
                            ShowDesktopAlert("File is already in use and cannot be opened.", true);
                            openOutcome = OpenOutcome.Ownership;
                            break;
                    }
                }
            }

            return new AcquireLockOutput(location, latest, openOutcome);
        }

        private bool Save()
        {
            var config = ExtensionsConfigurationManager.GetConfigurationSection();
            var result = Save(_currentPackageManager, _currentLocationType, _currentLocation, config.SmartSave);
            if (config.SmartSaveCount > 0)
                _currentPackageManager.AutoCleanup(_currentLocationType, _currentLocation, config.SmartSaveCount);

            return result;
        }

        private bool Save([NotNull] IPackageManager packageManager, 
            LocationType locationType, 
            [Required] string location,
            bool autoAddDateTime = false)
        {
            bool result = false;

            try
            {
                var enabledExtensions = Manager.Instance.Configuration.EnabledExtensions
                    .Select(x => Manager.Instance.GetExtensionMetadata(x));

                var beforeSave = ExtensionUtils.GetExtensions<IBeforeSaveProcessor>()?.ToArray();
                if (beforeSave?.Any() ?? false)
                {
                    foreach (var bs in beforeSave)
                        bs.Execute(_model);
                }

                if (packageManager is ISecurePackageManager securePM)
                {
                    // TODO: Handle the Secure Package Manager.
                }
                else
                {
                    result = packageManager.Save(_model, locationType, location, 
                        _errorsOnLoading | autoAddDateTime, enabledExtensions, out var newLocation);
                    _model?.SetLocation(newLocation);
                }
            }
            catch (UnauthorizedAccessException)
            {
                ShowDesktopAlert("The model cannot be saved because you do now have the required rights.", true);
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
            }

            if (result)
            {
                _model?.ResetDirty();
            }

            return result;
        }
        
        [Dispatched]
        private void OwnershipObtained(string fileName)
        {
            ShowDesktopAlert($"Lock on Threat Model '{fileName}' acquired. You can open it, now.");
        }

        [Dispatched]
        private void OwnershipRequested(string name, string computer, DateTime timestamp, int count)
        {
            string userCountString = count < 1 ? "No" : (count - 1).ToString();

            if (count > 1)
                _lockRequest.Symbol = "59387";
            _lockRequest.Tooltip = $"Threat Model requested by {name}";
            _lockRequest.Visible = true;
            _statusBar.Refresh();

            ShowDesktopAlert($"Full access to the current Threat Model has been requested by user '{name}' from machine '{computer}' on {timestamp.ToString("g")}." +
                             $"\n{userCountString} other user(s) are also queued to get full access.");
 
        }

        private void LoadKnownDocuments()
        {
            var configSection = ExtensionsConfigurationManager.GetConfigurationSection();
            var knownDocuments = configSection.KnownDocuments?.OfType<KnownDocumentConfig>()
                .OrderBy(x => Path.GetFileNameWithoutExtension(x.Path)).ToArray();
            if (knownDocuments?.Any() ?? false)
            {
                foreach (var document in knownDocuments)
                {
                    var button = new ButtonItem()
                    {
                        Text = Path.GetFileNameWithoutExtension(document.Path),
                        AutoExpand = false,
                        Tag = document,
                        Tooltip = document.Path
                    };
                    button.Click += KnownDocumentClick;
                    _recentDocuments.SubItems.Add(button);
       
                    button.Text = Compact(button.Text);
                }
            }
        }

        private async void KnownDocumentClick(object sender, EventArgs e)
        {
            if (sender is ButtonItem buttonItem && buttonItem.Tag is KnownDocumentConfig documentConfig &&
                string.CompareOrdinal(documentConfig.Path, _currentLocation) != 0)
            {
                var packageManager = Manager.Instance.GetExtension<IPackageManager>(documentConfig.PackageManager);
                if (packageManager != null)
                {
                    bool abort = false;

                    if (_model?.IsDirty ?? false)
                    {
                        var dialogResult = MessageBox.Show(this,
                            "The document has changed. Do you want to Save it before opening the new one?\n\nPlease press Yes to save it and open the new file.\nPress No to proceed without saving the current document.\nPress Cancel to abort.",
                            "Current Document has changed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button1);

                        if (dialogResult == DialogResult.Yes)
                            _save_Click(sender, e);
                        else if (dialogResult == DialogResult.Cancel)
                            abort = true;
                    }

                    if (!abort)
                    {
                        if (packageManager.CanHandle(documentConfig.LocationType, documentConfig.Path))
                        {
                            var outcome = await OpenAsync(packageManager, documentConfig.LocationType, documentConfig.Path);

                            switch (outcome)
                            {
                                case OpenOutcome.OK:
                                    UpdateFormsList();
                                    ShowDesktopAlert("Document successfully opened.");
                                    break;
                                case OpenOutcome.KO:
                                    RemoveKnownDocument(packageManager, documentConfig.LocationType, documentConfig.Path);
                                    break;
                                case OpenOutcome.Ownership:
                                    break;
                            }
                        }
                        else
                        {
                            ShowDesktopAlert("The selected document cannot be opened, most probably because its location is not supported.", true);
                        }
                    }
                }
            }
        }

        private void AddKnownDocument([NotNull] IPackageManager packageManager, 
            LocationType locationType, 
            [Required] string location)
        {
            var configSection = ExtensionsConfigurationManager.GetConfigurationSection();
            if (configSection.KnownDocuments.OfType<KnownDocumentConfig>()
                .All(x => string.CompareOrdinal(x.Path, location) != 0))
            {
                var document = new KnownDocumentConfig()
                {
                    Path = location,
                    LocationType = locationType,
                    PackageManager = packageManager.GetExtensionId()
                };
                configSection.KnownDocuments.Add(document);

                if (configSection.KnownDocuments.Count > 10)
                {
                    var toBeRemoved = configSection.KnownDocuments[0];
                    configSection.KnownDocuments.RemoveAt(0);
                    var buttonItem = _recentDocuments.SubItems.OfType<ButtonItem>()
                        .FirstOrDefault(x => x.Tag is KnownDocumentConfig knownDocument && string.CompareOrdinal(toBeRemoved.Path, knownDocument.Path) == 0);
                    if (buttonItem != null)
                        _recentDocuments.SubItems.Remove(buttonItem);
                }

                configSection.CurrentConfiguration.Save();

                var button = new ButtonItem()
                {
                    Text = Path.GetFileNameWithoutExtension(location),
                    Tag = document,
                    Tooltip = location
                };
                button.Click += KnownDocumentClick;
                _recentDocuments.SubItems.Add(button);

                button.Text = Compact(button.Text);
            }
        }

        private void RemoveKnownDocument([NotNull] IPackageManager packageManager, 
            LocationType locationType, 
            [Required] string location)
        {
            var configSection = ExtensionsConfigurationManager.GetConfigurationSection();
            var id = packageManager.GetExtensionId();
            var existing = configSection.KnownDocuments.OfType<KnownDocumentConfig>()
                .FirstOrDefault(x => string.CompareOrdinal(location, x.Path) == 0 && locationType == x.LocationType && string.CompareOrdinal(x.PackageManager, id) == 0);

            if (existing != null)
            {
                configSection.KnownDocuments.Remove(existing);
                configSection.CurrentConfiguration.Save();

                var button = _recentDocuments.SubItems.OfType<ButtonItem>().FirstOrDefault(x => x.Tag.Equals(existing));
                if (button != null)
                    _recentDocuments.SubItems.Remove(button);
            }
        }

        public string Compact(string text)
        {
            Size s = TextRenderer.MeasureText(text, this.Font);

            if (s.Width <= 350)
                return text;

            int len = 0;
            int seg = text.Length;
            string fit = "";

            while (seg > 1)
            {
                seg -= seg / 2;

                int left = len + seg;
                int right = text.Length;

                if (left > right)
                    continue;

                string tst = text.Substring(0, left) +
                             "..." + text.Substring(right);

                s = TextRenderer.MeasureText(tst, this.Font);

                if (s.Width <= 350)
                {
                    len += seg;
                    fit = tst;
                }
            }

            if (len == 0) // string can't fit into control
            {
                return "...";
            }

            return fit;
        }
        #endregion

        #region PanelContainerForm event handlers.
        private void OnPanelContainerFormCreated(PanelContainerForm form)
        {
            bool isDiagram = form.Controls.OfType<IShowDiagramPanel<Form>>().Any();

            var button = new ButtonItem()
            {
                Text = form.Text,
                Tag = form,
                Image = isDiagram ? Icons.Resources.model : Properties.Resources.window,
                ImageSmall = Properties.Resources.window_small,
                ImagePosition = eImagePosition.Left,
                ButtonStyle = eButtonStyle.ImageAndText
            };
            button.Click += OnWindowButtonClicked;
            _windows.SubItems.Add(button);
        }

        private void OnWindowButtonClicked(object sender, EventArgs e)
        {
            if (sender is ButtonItem button && button.Tag is PanelContainerForm form)
            {
                form.Activate();
            }
        }

        private void OnPanelContainerFormClosed(PanelContainerForm form)
        {
            var button = _windows.SubItems.OfType<ButtonItem>()
                .FirstOrDefault(x => x.Tag is PanelContainerForm pcForm && pcForm == form);
            if (button != null)
            {
                _windows.SubItems.Remove(button);
            }

            GcCollect();
        }

        private void OnPanelContainerFormTextChanged(PanelContainerForm form)
        {
            var button = _windows.SubItems.OfType<ButtonItem>()
                .FirstOrDefault(x => x.Tag is PanelContainerForm pcForm && pcForm == form);
            if (button != null)
            {
                button.Text = form.Text;
            }
        }

        [Background]
        private void GcCollect()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
        #endregion

        private void ConfigureTimer()
        {
            int interval = 0;

            var config = ExtensionsConfigurationManager.GetConfigurationSection();
            if (config.SmartSave)
            {
                interval = config.SmartSaveInterval;
            }
            
            if (interval > 0)
            {
                _autosaveTimer.Interval = interval * 60000;
                _autosaveTimer.Start();
            }
            else
            {
                _autosaveTimer.Stop();
            }
        }

        private void _autosaveTimer_Tick(object sender, EventArgs e)
        {
            if (_model?.IsDirty ?? false)
            {
                if (_currentPackageManager != null && !string.IsNullOrWhiteSpace(_currentLocation))
                {
                    if (Save())
                    {
                        ShowDesktopAlert("Document saved successfully.");
                    }
                    else
                    {
                        ShowDesktopAlert("Document save failed.", true);
                    }
                }            
            }
        }

        private void _controlMinimize_Click(object sender, EventArgs e)
        {
            SuspendLayout();
            WindowState = FormWindowState.Minimized;
            ResumeLayout();
        }

        private void _controlMaximize_Click(object sender, EventArgs e)
        {
            SuspendLayout();
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
                //_controlMaximize.Symbol = "59588";
            }
            else
            {
                WindowState = FormWindowState.Maximized;
                //_controlMaximize.Symbol = "59562";
            }
            ResumeLayout();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (_ribbonHeight > 0)
            {
                switch (WindowState)
                {
                    case FormWindowState.Normal:
                        _ribbon.CaptionVisible = true;
                        _controlMaximize.Visible = false;
                        _controlMinimize.Visible = false;
                        _controlExit.Visible = false;
                        _title.Visible = false;
                        _ribbon.Height = _ribbonHeight + (int) (30 * Dpi.Factor.Width);
                        break;
                    case FormWindowState.Maximized:
                        _ribbon.CaptionVisible = false;
                        _controlMaximize.Visible = true;
                        _controlMinimize.Visible = true;
                        _controlExit.Visible = true;
                        _title.Visible = true;
                        _ribbon.Height = _ribbonHeight;
                        break;
                }
            }
        }

        private void MainForm_MdiChildActivate(object sender, EventArgs e)
        {
            RefreshCaption();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            _jumpListManager = new JumpListManager();
            _jumpListManager.AutoRefresh = false;
            _jumpListManager.ShowFrequentFiles = false;
            _jumpListManager.ShowRecentFiles = false;

            var configSection = ExtensionsConfigurationManager.GetConfigurationSection();
            var knownDocuments = configSection.KnownDocuments?.OfType<KnownDocumentConfig>()
                .OrderBy(x => Path.GetFileNameWithoutExtension(x.Path)).ToArray();
            if (knownDocuments?.Any() ?? false)
            {
                foreach (var document in knownDocuments)
                {
                    if (document.LocationType == LocationType.FileSystem)
                    {
                        _jumpListManager.AddLink(Path.GetFileNameWithoutExtension(document.Path),
                            document.Path,
                            categoryName:"Recent Files");
                    }
                }
            }

            _jumpListManager.Refresh();
        }
    }
}