using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Icons;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Extensions.Dialogs;

namespace ThreatsManager.Extensions.Panels.Word
{
#pragma warning disable CS0067
    public partial class WordReportingPanel
    {
        private string _lastDocument;
        private ProgressDialog _progress;
        private int _index;
        private int _count;

        public event Action<string, bool> ChangeCustomActionStatus;

        public string TabLabel => "Word Report";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                var result = new List<ICommandsBarDefinition>
                {
                    new CommandsBarDefinition("Export", "Export", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "GenAll", "Generate All Documents",
                            Properties.Resources.docx_big,
                            Properties.Resources.docx),
                        new ActionDefinition(Id, "GenCurrent", "Generate Current Document",
                            Properties.Resources.docx_big,
                            Properties.Resources.docx),
                    }),
                    new CommandsBarDefinition("Open", "Open", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "Open", "Open Last Document",
                            Properties.Resources.folder_open_document_big,
                            Properties.Resources.folder_open_document),
                    }),
                    new CommandsBarDefinition("Refresh", "Refresh", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "Refresh", "Refresh",
                            Resources.refresh_big,
                            Resources.refresh,
                            true, Shortcut.F5),
                    }),
                };

                return result;
            }
        }

        [InitializationRequired]
        public void ExecuteCustomAction([NotNull] IActionDefinition action)
        {
            string text = null;
            bool warning = false;

            try
            {
                switch (action.Name)
                {
                    case "GenAll":
                        if (GenerateAll())
                            text = "All Documents Generation";
                        break;
                    case "GenCurrent":
                        _index = 0;
                        _count = 1;
                        if (_currentRow?.Tag is WordReportDefinition definition)
                        {
                            if (Generate(definition))
                                text = $"'{definition.Name}' Document Generation";
                        }
                        else
                        {
                            text = "No document is selected.";
                            warning = true;
                        }
                        break;
                    case "Open":
                        if (!string.IsNullOrWhiteSpace(_lastDocument) && File.Exists(_lastDocument) &&
                            string.CompareOrdinal(Path.GetExtension(_lastDocument), ".docx") == 0)
                        {
                            text = "Document Opening";
                            Process.Start(_lastDocument);
                        }
                        break;
                    case "Refresh":
                        RefreshThreatModel();
                        break;
                }

                if (warning)
                    ShowWarning?.Invoke(text);
                else if (text != null)
                    ShowMessage?.Invoke($"{text} has been executed successfully.");
            }
            catch
            {
                ShowWarning?.Invoke($"An error occurred during the execution of the action.");
                throw;
            }
        }

        #region Full Save.
        private bool GenerateAll()
        {
            bool result = false;

            var definitions = ReportDefinitions?.ToArray();
            if (definitions?.Any() ?? false)
            {
                _index = 0;
                _count = definitions.Length;
                string name = null;
                try
                {
                    foreach (var definition in definitions)
                    {
                        name = definition.Name;
                        Generate(definition);
                        _index++;
                        ShowMessage?.Invoke($"Generated report '{name}'.");
                    }
                    result = true;
                }
                catch (Exception exc)
                {
                    ShowWarning?.Invoke($"Generation of report '{name}' failed with error '{exc.Message}'.");
                }
            }

            return result;
        }

        private bool Generate([NotNull] WordReportDefinition definition)
        {
            var result = false;

            var fileName = definition.Path;
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var originalPath = GetDocumentPath(_model, fileName);
                if (File.Exists(originalPath))
                {
                    var newFileName = Path.Combine(Path.GetDirectoryName(originalPath),
                        $"{Path.GetFileNameWithoutExtension(originalPath)}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.docx");

                    try
                    {
                        if (_index == 0)
                            ShowProgress();

                        try
                        {
                            var ignoredDictionary = GetPlaceholdersWithIgnoredFields()?
                                .ToDictionary(x => x, GetIgnoredFields);
                            result = _reportGenerator.Generate(originalPath, newFileName, ignoredDictionary);

                            _lastDocument = newFileName;
                        }
                        catch (Exception exc)
                        {
                            ShowWarning?.Invoke($"Generation of report '{definition.Name}' failed with error '{exc.Message}'.");
                        }
                    }
                    finally
                    {
                        if (_index + 1 == _count)
                        {                           
                            CloseProgress();
                        }
                    }
                }
                else
                {
                    ShowWarning?.Invoke("The Reference Word File does not exist.");
                }
            }
            else
            {
                ShowWarning?.Invoke("Please select the reference Word File.");
            }

            return result;
        }

        private void ShowProgress()
        {
            _progress = new ProgressDialog();
            _progress.Label = "Export Word Report(s) is in progress...";
            _progress.Show(Form.ActiveForm);
        }

        private void UpdateProgress(int percentage)
        {
            if (_progress != null)
                _progress.Value = (int) (((percentage / (_count * 100f) + ((float)_index) / _count)) * 100);
        }

        private void CloseProgress()
        {
            _progress.Close();
            _progress = null;
        }
        #endregion
    }
}