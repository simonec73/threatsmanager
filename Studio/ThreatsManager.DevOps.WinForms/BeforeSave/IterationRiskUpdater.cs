﻿using System.Runtime.InteropServices;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using ThreatsManager.DevOps.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps.BeforeSave
{
    [Extension("5335AAAC-FEAE-4E79-9A8B-4008134CD106", "Iteration Risk Updater", 50, ExecutionMode.Management)]
    public class IterationRiskUpdater : IBeforeSaveProcessor
    {
        public void Execute([NotNull] IThreatModel model)
        {
            using (var scope = UndoRedoManager.OpenScope("Update Iteration Risk"))
            {
                var schemaManager = new DevOpsConfigPropertySchemaManager(model);
                var iteration = schemaManager.CurrentIteration ?? schemaManager.PreviousIteration;
                if (iteration != null)
                {
                    var extensionId = ExtensionUtils.GetExtensionByLabel<IConfigurationPanelFactory<Form>>(
                        "Extensions Configuration Panel")?.GetExtensionId();

                    if (extensionId != null)
                    {
                        var normalizationReference = model.GetExtensionConfiguration(extensionId)?
                            .GlobalGet<int>("normalization") ?? 0;

                        var risk = model.EvaluateRisk(normalizationReference);
                        if (risk > 0f)
                        {
                            schemaManager.SetIterationRisk(iteration, risk);
                            scope?.Complete();
                        }
                    }
                }
            }
        }
    }
}
