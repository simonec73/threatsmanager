using System;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Engine;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities.WinForms.Rules;

namespace ThreatsManager.AutoGenRules.Dialogs
{
    public partial class RuleFilterDialog : Form, IRuleEditorDialog, IInitializableObject
    {
        private IThreatModel _model;

        public RuleFilterDialog()
        {
            InitializeComponent();
        }

        #region Public members.
        public void Initialize([NotNull] IThreatModel model)
        {
            _model = model;
            _ruleEditor.Initialize(_model);
        }

        public SelectionRule Rule
        {
            get => _ruleEditor.Rule;
            set => _ruleEditor.Rule = value;
        }

        public bool IsInitialized => _model != null;
        #endregion

        #region Private member functions: other auxiliary functions.
        private void _ok_Click(object sender, EventArgs e)
        {
            if (_ruleEditor.ValidateRule())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void _cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
