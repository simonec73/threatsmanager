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

namespace ThreatsManager.Extensions.Panels.Word
{
#pragma warning disable CS0067
    public partial class WordReportingPanel
    {
        private string _lastDocument;
        private ProgressDialog _progress;

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
                        new ActionDefinition(Id, "Full", "Generate Document",
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
                    case "Full":
                        if (SaveFull())
                            text = "Document Generation";
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
                        _docStructure.PrimaryGrid.Rows.Clear();
                        if (!string.IsNullOrWhiteSpace(_wordFile.Text))
                        {
                            var file = GetDocumentPath(_model, _wordFile.Text);
                            if (File.Exists(file))
                            {
                                LoadDocStructure(file);
                            }
                        }
                        else
                        {
                            ShowWarning?.Invoke($"No Reference Word file has been selected.");
                        }
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
        private bool SaveFull()
        {
            var result = false;

            if (!string.IsNullOrWhiteSpace(_wordFile.Text))
            {
                var originalPath = GetDocumentPath(_model, _wordFile.Text);
                var fileName = Path.Combine(Path.GetDirectoryName(originalPath),
                    $"{Path.GetFileNameWithoutExtension(originalPath)}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.docx");

                try
                {
                    ShowProgress();

                    try
                    {
                        var ignoredDictionary = GetPlaceholdersWithIgnoredFields()?
                            .ToDictionary(x => x, GetIgnoredFields);
                        result = _reportGenerator.Generate(originalPath, fileName, ignoredDictionary);

                        _lastDocument = fileName;
                    }
                    catch
                    {
                        ShowWarning?.Invoke("Report generation failed.");
                    }
                }
                finally
                {
                    CloseProgress();
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
            _progress.Show(Form.ActiveForm);
        }

        private void UpdateProgress(int percentage)
        {
            if (_progress != null)
                _progress.Value = percentage;
        }

        private void CloseProgress()
        {
            _progress.Close();
            _progress = null;
        }
        #endregion
    }
}