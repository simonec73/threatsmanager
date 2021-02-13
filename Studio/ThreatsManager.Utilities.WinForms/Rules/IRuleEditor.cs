using DevComponents.DotNetBar;
using ThreatsManager.AutoGenRules.Engine;

namespace ThreatsManager.Utilities.WinForms.Rules
{
    public interface IRuleEditor : IRuleEditorDialog
    {
        void AddButton(ButtonItem button, Scope scope);
    }

    public interface IRuleEditorDialog
    {
        SelectionRule Rule { get; set; }
    }
}