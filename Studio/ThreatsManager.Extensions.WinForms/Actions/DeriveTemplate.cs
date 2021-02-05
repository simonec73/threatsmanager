using System.Drawing;
using System.Windows.Forms;
using ThreatsManager.Extensions.Dialogs;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities.WinForms.Dialogs;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("C6DAFFB6-5CD6-405F-8B8E-6305E91AA665", "Derive Template Context Aware Action", 34, ExecutionMode.Simplified)]
    public class DeriveTemplate : IIdentityContextAwareAction
    {
        public Scope Scope => Scope.Entity | Scope.TrustBoundary | Scope.DataFlow;
        public string Label => "Derive Template";
        public string Group => "ItemActions";
        public Bitmap Icon => null;
        public Bitmap SmallIcon => null;
        public Shortcut Shortcut => Shortcut.None;

        public bool Execute(object item)
        {
            bool result = false;

            if (item is IIdentity identity)
                result = Execute(identity);

            return result;
        }

        public bool IsVisible(object item)
        {
            return true;
        }

        public bool Execute(IIdentity identity)
        {
            if (identity is IEntity entity)
            {
                using (var dialog = new CreateEntityTemplateDialog(entity))
                {
                    if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                    {
                        entity.Model?.AddEntityTemplate(dialog.EntityName,
                            dialog.EntityDescription, dialog.BigImage, dialog.Image, dialog.SmallImage, entity);
                    }
                }
            }
            else if (identity is IDataFlow flow)
            {
                using (var dialog = new GenericIdentityCreationDialog())
                {
                    dialog.IdentityName = flow.Name;
                    dialog.IdentityDescription = flow.Description;
                    dialog.IdentityTypeName = "Flow Template";
                    if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                    {
                        var flowTemplate = flow.Model.AddFlowTemplate(dialog.IdentityName,
                            dialog.IdentityDescription, flow);
                        flow.Model.AutoApplySchemas(flowTemplate);
                    }
                }
            }
            else if (identity is ITrustBoundary trustBoundary)
            {
                using (var dialog = new GenericIdentityCreationDialog())
                {
                    dialog.IdentityName = trustBoundary.Name;
                    dialog.IdentityDescription = trustBoundary.Description;
                    dialog.IdentityTypeName = "Trust Boundary Template";
                    if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                    {
                        var trustBoundaryTemplate = trustBoundary.Model.AddTrustBoundaryTemplate(dialog.IdentityName,
                            dialog.IdentityDescription, trustBoundary);
                        trustBoundary.Model.AutoApplySchemas(trustBoundaryTemplate);
                    }
                }
            }

            return true;
        }
    }
}