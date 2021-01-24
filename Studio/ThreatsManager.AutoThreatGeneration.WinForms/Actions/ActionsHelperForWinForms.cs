using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Engine;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities.WinForms.Rules;

namespace ThreatsManager.AutoThreatGeneration.Actions
{
    internal static class ActionsHelperForWinForms
    {
        public static bool SetRule(this IPropertiesContainer container, [NotNull] IRuleEditorDialog ruleEditor)
        {
            bool result = false;

            ruleEditor.Rule = container.GetRule();

            if (ruleEditor is Form dialog && dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
            {
                container.SetRule(ruleEditor.Rule);
                result = true;
            }

            return result;
        }
    }
}