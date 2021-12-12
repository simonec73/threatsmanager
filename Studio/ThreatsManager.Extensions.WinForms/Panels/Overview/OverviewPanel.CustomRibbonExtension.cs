using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Dialogs;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Panels.Overview
{
#pragma warning disable CS0067
    public partial class OverviewPanel
    {
        public event Action<string, bool> ChangeCustomActionStatus;

        public string TabLabel => "Dashboard";

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
            //string text = null;
            //bool warning = false;

            try
            {
                switch (action.Name)
                {
                    case "ConfigureEstimator":
                        var dialog = new ResidualRiskEstimatorConfigurationDialog();
                        dialog.Initialize(_model);
                        if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        {
                            _roadmap.RefreshChart(_model);
                            _roadmapCharts.Initialize(_model);
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
    }
}