using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;

namespace ThreatsManager.Extensions.Dialogs
{
    public partial class ComparisonDialog : Form, IExecutionModeSupport
    {
        public ComparisonDialog()
        {
            InitializeComponent();
        }

        public void Initialize([NotNull] object source, [NotNull] object target)
        {
            _sourceEditor.Item = source;
            _targetEditor.Item = target;
        }

        public void SetExecutionMode(ExecutionMode mode)
        {
            _sourceEditor.SetExecutionMode(mode);
            _targetEditor.SetExecutionMode(mode);
        }
    }
}
