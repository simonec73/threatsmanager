using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using Exceptionless;
using Exceptionless.Models.Collections;
using ThreatsManager.Dialogs;
using PostSharp.Patterns.Threading;
using ThreatsManager.Engine;
using ThreatsManager.Engine.Config;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Properties;
using ThreatsManager.Utilities;
using DevComponents.DotNetBar.Metro.ColorTables;
using Exceptionless.Logging;
using ThreatsManager.Interfaces;
using ThreatsManager.Utilities.WinForms;
using PostSharp.Patterns.Recording;

namespace ThreatsManager
{
    public partial class MainForm : RibbonForm
    {
        #region Private member variables.
        private IThreatModel _model;
        private LocationType _currentLocationType;
        private string _currentLocation;
        private IPackageManager _currentPackageManager;
        private bool _closing;
        private ExecutionMode _executionMode;
        private string _oldCaption;

        private bool _errorsOnLoading;
        private List<string> _missingTypes = new List<string>();

        private int _mergeIndex;

        private int _ribbonHeight;

        private JumpListManager _jumpListManager;
        #endregion

        private const string LatestVersion = "LatestVersion";
        private const string Highlights = "Highlights";

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

        #region Main form events.
        private async void MainForm_Load(object sender, EventArgs e)
        {
            Text = Resources.Title;

            InitializeTelemetry();
            InitializeConfiguration();

            #region Resource Guard.
            var resourceGuard = new ResourcesGuard();
            resourceGuard.StartChecking();
            #endregion
            
            _ribbonTabView.Visible = true;

            LoadExtensions();
            _mergeIndex = _ribbon.Items.IndexOf(_title);

            await OpenInitialFile();

            LoadStatusInfoProviders();

            LoadKnownDocuments();

            CheckUpdateAvailability();

            ConfigureTimer();

            KnownTypesBinder.TypeNotFound += OnTypeNotFound;

            UndoRedoManager.DirtyChanged += UndoRedoManager_DirtyChanged;
            UndoRedoManager.ErrorRaised += UndoRedoManager_ErrorRaised;
            UndoRedoManager.Undone += UndoRedoManager_Undone;
            UndoRedoManager.Redone += UndoRedoManager_Redone;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (UndoRedoManager.IsDirty)
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
                        _ribbon.Height = _ribbonHeight + (int)(30 * Dpi.Factor.Width);
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
                            categoryName: "Recent Files");
                    }
                }
            }

            _jumpListManager.Refresh();
        }
        #endregion

        #region UndoRedoManager events.
        private void UndoRedoManager_Redone(string message)
        {
            UpdateFormsList();
            ShowDesktopAlert($"{message} redone.");
        }

        private void UndoRedoManager_Undone(string message)
        {
            UpdateFormsList();
            ShowDesktopAlert($"{message} undone.");
        }

        private void UndoRedoManager_ErrorRaised(string message)
        {
            ShowDesktopAlert(message, true);
        }

        private void UndoRedoManager_DirtyChanged(bool obj)
        {
            RefreshCaption();
        }
        #endregion

        #region Initializations.
        private void InitializeTelemetry()
        {
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
        }

        private void InitializeConfiguration()
        {
#if PORTABLE
            ExtensionsConfigurationManager.SetConfigurationUserLevel(System.Configuration.ConfigurationUserLevel.None);
#endif

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

            if (!InitializeExtensionsManagement())
                Close();
            #endregion

            SpellCheckConfig.UserDictionary = config.UserDictionary;
        }
        #endregion

        #region Exceptionless.
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
        #endregion

        #region Standard Ribbon buttons.
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

        private void _undo_Click(object sender, EventArgs e)
        {
            UndoRedoManager.Undo();

            object found = _model.Entities?.FirstOrDefault(x => (x as IRecordable)?.Recorder == null);
            if (found == null)
                found = _model.DataFlows?.FirstOrDefault(x => (x as IRecordable)?.Recorder == null);
            if (found == null)
            {
                var diagrams = _model.Diagrams?.ToArray();
                if (diagrams?.Any() ?? false)
                {
                    found = diagrams?.FirstOrDefault(x => (x as IRecordable)?.Recorder == null);
                    if (found == null)
                    {
                        foreach (var diagram in diagrams)
                        {
                            found = diagram.Entities?.FirstOrDefault(x => (x as IRecordable)?.Recorder == null);
                            if (found == null)
                                found = diagram.Links?.FirstOrDefault(x => (x as IRecordable)?.Recorder == null);
                        }
                    }
                }
            }
        }

        private void _redo_Click(object sender, EventArgs e)
        {
            UndoRedoManager.Redo();
        }
        #endregion

        #region Buttons: File.
        private void _new_Click(object sender, EventArgs e)
        {
            bool abort = false;
            if (UndoRedoManager.IsDirty)
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
                _protectionData = null;
                InitializeStatus(null);
                ShowDesktopAlert("Document successfully initialized.");
            }
        }

        private async void _open_Click(object sender, EventArgs e)
        {
            bool abort = false;

            if (UndoRedoManager.IsDirty)
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

                if (packageManagers?.Any() ?? false)
                {
                    _openFile.Filter = packageManagers.GetFilter();

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

            if (packageManagers?.Any() ?? false)
            {
                _saveAsFile.Filter = packageManagers.GetFilter();
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

                    if (SaveAs(packageManager, LocationType.FileSystem, _saveAsFile.FileName))
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
        }

        private void _close_Click(object sender, EventArgs e)
        {
            bool abort = false;

            if (UndoRedoManager.IsDirty)
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
        #endregion

        #region Threat Model initializations and events.
        private void InitializeStatus(IThreatModel model)
        {
            if (_model != null)
            {
                UndoRedoManager.Detach(_model);
                ThreatModelManager.Remove(_model.Id);
            }

            if (model == null)
            {
                _model = ThreatModelManager.GetDefaultInstance();
                _model.Owner = UserName.GetDisplayName();
            }
            else
            {
                _model = model;
            }

            //ThreatModelManager.Model.ChildCreated += ModelChildCreated;
            //ThreatModelManager.Model.ChildRemoved += ModelChildRemoved;

            CloseAllForms();
            UpdateStatusInfoProviders();
            UndoRedoManager.Attach(_model, _model);
            UndoRedoManager.Clear();
            RefreshCaption();

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
            var currentChild = ActiveMdiChild?.Text.Replace("\n", " ");
            var titleText = GetTitleText(modelName, currentChild);

            if (!CheckCaptionWidth(titleText))
            {
                var found = false;
                if ((modelName?.Length ?? 0) > 40)
                {
                    modelName = modelName.Substring(0, 39) + "…";
                    titleText = GetTitleText(modelName, currentChild);
                    found = CheckCaptionWidth(titleText);
                }

                if (!found)
                {
                    if ((modelName?.Length ?? 0) > 20)
                    {
                        modelName = modelName.Substring(0, 19) + "…";
                        titleText = GetTitleText(modelName, currentChild);
                        found = CheckCaptionWidth(titleText);
                    }
                }

                if (!found)
                {
                    if (!string.IsNullOrWhiteSpace(currentChild))
                    {
                        titleText = $"{(UndoRedoManager.IsDirty ? "*" : "")}[{currentChild}]";
                        if (!CheckCaptionWidth(titleText))
                            titleText = string.Empty;
                    }
                    else
                        titleText = string.Empty;
                }
            }

            if (string.CompareOrdinal(titleText, _oldCaption) != 0)
            {
                _oldCaption = titleText;
                _title.Text = titleText;
                _title.Visible = !string.IsNullOrWhiteSpace(titleText);
                _title.Width = (int) ((TextRenderer.MeasureText(titleText, _title.Font).Width + _title.PaddingLeft + _title.PaddingRight + 20) / Dpi.Factor.Width);
                _ribbon.Refresh();
            }

            Text = $@"{(UndoRedoManager.IsDirty ? "*" : "")}{Resources.Title} - {_model?.Name}";

        }

        private string GetTitleText(string modelName, string currentChild)
        {
            var text = $@"{(UndoRedoManager.IsDirty ? "*" : "")}{modelName}";
            return string.IsNullOrWhiteSpace(currentChild) ? text : $"{text} - [{currentChild}]";
        }

        private bool CheckCaptionWidth(string caption)
        {
            var width = TextRenderer.MeasureText(caption, _title.Font).Width + _title.PaddingLeft + _title.PaddingRight + 20;
            var maxWidth = this.Width - 1000 * Dpi.Factor.Width;

            return width <= maxWidth;
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
    }
}