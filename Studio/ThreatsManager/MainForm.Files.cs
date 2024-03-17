using DevComponents.DotNetBar;
using Exceptionless;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Threading;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThreatsManager.Dialogs;
using ThreatsManager.Engine;
using ThreatsManager.Engine.Config;
using ThreatsManager.Interfaces.Exceptions;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Exceptions;

namespace ThreatsManager
{
    public partial class MainForm
    {
        private IProtectionData _protectionData;

        #region Nested classes.
        private class AcquireLockOutput
        {
            public AcquireLockOutput(string location, string latest,
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

        private class PasswordProtectionData : IPasswordProtectionData
        {
            private readonly SecureString _password;

            public PasswordProtectionData([NotNull] SecureString password)
            {
                _password = password;
            }

            public ProtectionType ProtectionType => ProtectionType.Password;

            public SecureString Password => _password;

            public string Algorithm => null;

            public string HMAC => null;

            public byte[] Salt => null;

            public int Iterations => 0;
        }
        #endregion

        #region Load.
        private async Task OpenInitialFile()
        {
            var commandLineArgs = Environment.GetCommandLineArgs();
            if (commandLineArgs.Length == 2)
            {
                var fileName = commandLineArgs[1];
                if (File.Exists(fileName))
                {
                    OpenOutcome outcome = OpenOutcome.KO;

                    var packageManager = GetPackageManagerForFile(fileName);
                    if (packageManager != null)
                    {
                        outcome = await OpenAsync(packageManager, LocationType.FileSystem, fileName);

                        if (outcome == OpenOutcome.OK && _model != null)
                        {
                            ShowDesktopAlert("Document successfully opened.");
                        }
                    }
                    else
                    {
                        var kbManager = GetKBManagerForFile(fileName);

                        if (kbManager != null)
                        {
                            InitializeStatus(null);
                            outcome = Import(kbManager, LocationType.FileSystem, fileName);
                            if (outcome == OpenOutcome.OK)
                                ShowDesktopAlert("The Knowledge Base has been applied successfully.");
                        }
                        else
                        {
                            ShowDesktopAlert("The selected document cannot be opened, most probably because its location or type is not supported.", true);
                        }
                    }

                    if (outcome != OpenOutcome.OK || _model == null)
                    {
                        InitializeStatus(null);
                    }
                }
                else
                {
                    InitializeStatus(null);
                }
            }
            else
            {
                InitializeStatus(null);
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
            _protectionData = null;
            bool messageRaised = false;

            try
            {
                var enabledExtensions = Manager.Instance.Configuration.EnabledExtensions
                    .Select(x => Manager.Instance.GetExtensionMetadata(x));

                var latest = packageManager.GetLatest(locationType, location, out var dateTime);

                if (locationType == LocationType.FileSystem)
                {
                    var output = await AcquireLockAsync(location, latest);
                    var outcome = output?.Outcome ?? OpenOutcome.KO;
                    if (outcome == OpenOutcome.OK)
                    {
                        location = output.Location;
                        latest = output.Latest;
                    }
                    else
                    {
                        return outcome;
                    }
                }

                if (latest != null)
                {
                    location = latest;
                }

                if (packageManager is ISecurePackageManager securePM)
                {
                    if (securePM.RequiredProtection.HasFlag(ProtectionType.Password))
                    {
                        if (GetPassword(out var password))
                        {
                            if (password != null)
                            {
                                _protectionData = new PasswordProtectionData(password);
                                securePM.SetProtectionData(_protectionData);
                            }
                        }
                        else
                            return OpenOutcome.KO;
                    }
                }

                model = packageManager.Load(locationType, location, enabledExtensions, false);

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
            catch (FileFormatException e)
            {
                ShowDesktopAlert(e.Message, true);
                model = null;
                messageRaised = true;
            }
            catch (EncryptionRequiredException e)
            {
                ShowDesktopAlert(e.Message, true);
                model = null;
                messageRaised = true;
            }
            catch (FileEncryptedException e)
            {
                ShowDesktopAlert(e.Message, true);
                model = null;
                messageRaised = true;
            }
            catch (FileNotEncryptedException e)
            {
                ShowDesktopAlert(e.Message, true);
                model = null;
                messageRaised = true;
            }
            catch (InvalidHMACException e)
            {
                ShowDesktopAlert(e.Message, true);
                model = null;
                messageRaised = true;
            }
            catch (UnsupportedEncryptionException e)
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
                while (!CheckLoops(model))
                {
                }

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
                    ShowDesktopAlert("The selected document cannot be opened, most probably because its location is not supported.", true);
            }

            return result;
        }

        private OpenOutcome Import([NotNull] IKnowledgeBaseManager kbManager,
            LocationType locationType, [Required] string location)
        {
            OpenOutcome result = OpenOutcome.KO;

            try
            {
                if (kbManager.Import(_model, DuplicationDefinition.KBDefault, locationType, location))
                    result = OpenOutcome.OK;
            }
            catch (FileNotFoundException)
            {
                ShowDesktopAlert("File has not been found.", true);
            }
            catch (DirectoryNotFoundException)
            {
                ShowDesktopAlert("Directory has not been found.", true);
            }
            catch (UnauthorizedAccessException)
            {
                ShowDesktopAlert("The Knowledge Base cannot be imported because you do now have the required rights.", true);
            }
            catch (IOException e)
            {
                ShowDesktopAlert(e.Message, true);
            }
            catch (Exception e)
            {
                ShowDesktopAlert($"An exception occurred importing the Knowledge Base:\n{e.Message}", true);
                e.ToExceptionless().Submit();
            }

            return result;
        }

        private bool CheckLoops([NotNull] IThreatModel model)
        {
            bool result = true;

            var groups = model.Groups?.ToArray();
            if (groups?.Any() ?? false)
            {
                foreach (var current in groups)
                {
                    if (DefinesLoop(current, groups, out var cycle))
                    {
                        result = false;
                        HandleLoop(model, current, cycle);
                        break;
                    }
                }
            }

            return result;
        }

        private bool DefinesLoop(IGroup group, IEnumerable<IGroup> groups, out IEnumerable<IGroup> cycle)
        {
            var result = false;
            cycle = null;

            var seen = new List<IGroup>();
            seen.Add(group);

            var current = group;

            while (current != null)
            {
                if (current is IGroupElement child)
                {
                    current = child.Parent;

                    if (current != null)
                    {
                        if (!seen.Contains(current))
                        {
                            seen.Add(current);
                        }
                        else
                        {
                            result = true;
                            cycle = seen;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        [Dispatched]
        private void HandleLoop(IThreatModel model, IGroup group, IEnumerable<IGroup> cycle)
        {
            MessageBox.Show(Form.ActiveForm,
                $"The model has an infinite loop started with Trust Boundary '{group.Name}'.\nThreats Manager Studio will remove all the affected Trust Boundaries to address the issue.\nYou will need to adjust the Threat Model.",
                "Loop found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            foreach (var curr in cycle)
            {
                var children = model.Entities?.Where(x => x.ParentId == curr.Id).ToArray();
                if (children?.Any() ?? false)
                {
                    foreach (var child in children)
                    {
                        child.SetParent(null);
                    }
                }

                model.RemoveGroup(curr.Id);
            }
        }
        #endregion

        #region Save.
        private void EmergencySave()
        {
            var packageManagers = Manager.Instance.GetExtensions<IPackageManager>()?
                .Where(x => x.SupportedLocations.HasFlag(LocationType.FileSystem)).ToArray();

            var done = false;

            if (packageManagers?.Any() ?? false)
            {
                _saveAsFile.Title = "Emergency Save";
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

                    _saveAsFile.FileName = Path.Combine(Path.GetDirectoryName(_currentLocation),
                        $"{Path.GetFileNameWithoutExtension(_currentLocation)}.recover{Path.GetExtension(_currentLocation)}");

                    if (_saveAsFile.ShowDialog(this) == DialogResult.OK)
                    {
                        done = Save(_currentPackageManager, LocationType.FileSystem, _saveAsFile.FileName);
                    }
                }
            }
            
            if (!done)
            {
                ShowDesktopAlert("An exception has occurred and Threats Manager Studio needs to close. An issue is also preventing emergency save.", true);
            }
        }

        private bool Save()
        {
            var config = ExtensionsConfigurationManager.GetConfigurationSection();
            var result = Save(_currentPackageManager, _currentLocationType, _currentLocation, config.SmartSave);
            if (config.SmartSaveCount > 0)
                _currentPackageManager.AutoCleanup(_currentLocationType, _currentLocation, config.SmartSaveCount);

            return result;
        }

        private bool SaveAs([NotNull] IPackageManager packageManager,
            LocationType locationType,
            [Required] string location,
            bool autoAddDateTime = false)
        {
            if (packageManager is ISecurePackageManager securePM)
            {
                if (securePM.RequiredProtection.HasFlag(ProtectionType.Password))
                {
                    if (GetPassword(out var password, true))
                    {
                        if (password != null)
                        {
                            _protectionData = new PasswordProtectionData(password);
                        }
                    }
                    else
                        return false;
                }
            }

            return Save(packageManager, locationType, location, autoAddDateTime);
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
                    if (_protectionData != null)
                    {
                        securePM.SetProtectionData(_protectionData);
                    }
                }

                result = packageManager.Save(_model, locationType, location,
                    _errorsOnLoading | autoAddDateTime, enabledExtensions, out var newLocation);
                _model?.SetLocation(newLocation);
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
                UndoRedoManager.ResetDirty();
            }

            return result;
        }

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
            if (UndoRedoManager.IsDirty)
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
        #endregion

        #region File Lock.
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
        #endregion

        #region Auxiliary functions to handle Package and Knowledge Base Managers.
        private IPackageManager GetPackageManagerForFile([Required] string filepath)
        {
            return ExtensionUtils.GetExtensions<IPackageManager>()?
                    .FirstOrDefault(x => x.CanHandle(LocationType.FileSystem, filepath));
        }

        private IKnowledgeBaseManager GetKBManagerForFile([Required] string filepath)
        {
            return ExtensionUtils.GetExtensions<IKnowledgeBaseManager>()?
                    .FirstOrDefault(x => x.CanHandle(LocationType.FileSystem, filepath));
        }

        [Dispatched]
        private bool GetPassword(out SecureString secureString, bool newPassword = false)
        {
            var result = false;

            secureString = null;

            var dialog = new PasswordDialog();
            dialog.VerificationRequired = newPassword;
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                secureString = dialog.Password;
                result = true;
            }

            return result;
        }
        #endregion

        #region Serialization/Deserialization events.
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
        #endregion

        #region Known documents.
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
                        Text = Path.GetFileNameWithoutExtension(document.Path).Replace("&", "&&"),
                        AutoExpand = false,
                        Tag = document,
                        Tooltip = document.Path.Replace("&", "&&")
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
                    Text = Path.GetFileNameWithoutExtension(location).Replace("&", "&&"),
                    Tag = document,
                    Tooltip = location.Replace("&", "&&")
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
    }
}
