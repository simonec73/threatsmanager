using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Extensions.Dialogs;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Panels.Roadmap
{
#pragma warning disable CS0067
    public partial class RoadmapPanel
    {
        public event Action<string, bool> ChangeCustomActionStatus;

        public string TabLabel => "Roadmap";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                var result = new List<ICommandsBarDefinition>();

                if (_executionMode != ExecutionMode.Business)
                {
                    result.Add(new CommandsBarDefinition("ResidualRisk", "Risk", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "ConfigureEstimator", "Configure Risk Estimator",
                            Properties.Resources.money_bill_fire_big,
                            Properties.Resources.money_bill_fire),
                    }));
                };
                result.Add(new CommandsBarDefinition("Export", "Export", new IActionDefinition[]
                {
                    new ActionDefinition(Id, "ExportCsv", "Export for Azure DevOps",
                        Properties.Resources.xlsx_big,
                        Properties.Resources.xlsx),
                }));
                result.Add(new CommandsBarDefinition("Refresh", "Refresh", new IActionDefinition[]
                {
                    new ActionDefinition(Id, "Refresh", "Refresh List",
                        Resources.refresh_big,
                        Resources.refresh,
                        true, Shortcut.F5),
                }));

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
                    case "ConfigureEstimator":
                        var dialog = new ResidualRiskEstimatorConfigurationDialog();
                        dialog.Initialize(_model);
                        if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        {
                            LoadModel();
                        }
                        break;
                    case "ExportCsv":
                        var saveFileDialog = new SaveFileDialog()
                        {
                            AddExtension = true,
                            AutoUpgradeEnabled = true,
                            CheckFileExists = false,
                            CheckPathExists = true,
                            RestoreDirectory = true,
                            DefaultExt = "csv",
                            Filter = "CSV file (*.csv)|*.csv",
                            Title = "Create CSV file for Azure DevOps",
                            ValidateNames = true
                        };
                        if (saveFileDialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        {
                            ExportCsv(saveFileDialog.FileName);
                        }
                        break;
                    case "Refresh":
                        LoadModel();
                        break;
                }

                //if (warning)
                //    ShowWarning?.Invoke(text);
                //else if (text != null)
                //    ShowMessage?.Invoke($"{text} has been executed successfully.");
            }
            catch
            {
                //ShowWarning?.Invoke($"An error occurred during the execution of the action.");
                throw;
            }
        }

        private void ExportCsv([Required] string fileName)
        {
            var mitigations = _model?.GetUniqueMitigations()?.OrderBy(x => x.Name).ToArray();

            if (mitigations?.Any() ?? false)
            {
                using (var file = new System.IO.FileStream(fileName, FileMode.Create))
                {
                    using (var writer = new StreamWriter(file, Encoding.UTF8))
                    {
                        writer.WriteLine("Work Item Type,Title,Description,State,Priority");

                        foreach (var mitigation in mitigations)
                        {
                            var status = mitigation.GetStatus(out var automatedCalculation);

                            switch (status)
                            {
                                case RoadmapStatus.ShortTerm:
                                    writer.WriteLine($"\"Task\",\"{mitigation.Name}\",\"{TextToHtml(mitigation.Description)}\",\"To Do\",\"1\"");
                                    break;
                                case RoadmapStatus.MidTerm:
                                    writer.WriteLine($"\"Task\",\"{mitigation.Name}\",\"{TextToHtml(mitigation.Description)}\",\"To Do\",\"2\"");
                                    break;
                                case RoadmapStatus.LongTerm:
                                    writer.WriteLine($"\"Task\",\"{mitigation.Name}\",\"{TextToHtml(mitigation.Description)}\",\"To Do\",\"3\"");
                                    break;
                            }
                        }
                    }
                }
            }

        }

        private string TextToHtml(string text)
        {
            string result = null;

            if (!string.IsNullOrWhiteSpace(text))
                result = HttpUtility.HtmlEncode(text).Replace("\r\n", "\r").Replace("\n", "\r")
                    .Replace("\r", "<br>").Replace("  ", " &nbsp;");

            return result;
        }
    }
}