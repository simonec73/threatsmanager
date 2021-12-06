using System.Drawing;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Quality.Dialogs;

namespace ThreatsManager.Quality.CalculatedSeverity
{
    public class AdjustSeverityPropertyViewerBlock : IPropertyViewerBlock
    {
        private readonly IThreatEvent _threatEvent;

        public AdjustSeverityPropertyViewerBlock([NotNull] IThreatEvent threatEvent)
        {
            _threatEvent = threatEvent;
        }

        public bool Execute()
        {
            var dialog = new AdjustSeverityDialog(_threatEvent);

            return dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK;
        }

        public PropertyViewerBlockType BlockType => PropertyViewerBlockType.Button;

        public string Label => "Adjust Severity";

        public string Text { get; set; }

        public Bitmap Image => null;
        public bool Printable => false;
    }
}
