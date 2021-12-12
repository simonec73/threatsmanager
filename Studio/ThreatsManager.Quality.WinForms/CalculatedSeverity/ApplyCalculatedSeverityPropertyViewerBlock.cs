using System.Drawing;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Quality.CalculatedSeverity
{
    public class ApplyCalculatedSeverityPropertyViewerBlock : IPropertyViewerBlock
    {
        private readonly IThreatEvent _threatEvent;
        private readonly ISeverity _calculatedSeverity;

        public ApplyCalculatedSeverityPropertyViewerBlock([NotNull] IThreatEvent threatEvent, [NotNull] ISeverity calculatedSeverity)
        {
            _threatEvent = threatEvent;
            _calculatedSeverity = calculatedSeverity;
        }

        public bool Execute()
        {
            var result = false;

            if (_calculatedSeverity != null && _calculatedSeverity.Id != _threatEvent.SeverityId)
            {
                _threatEvent.Severity = _calculatedSeverity;
                result = true;
            }

            return result;
        }

        public PropertyViewerBlockType BlockType => PropertyViewerBlockType.Button;

        public string Label
        {
            get
            {
                string result = null;

                if (_threatEvent != null)
                {
                    if (_calculatedSeverity != null && _calculatedSeverity.Id != _threatEvent.SeverityId)
                    {
                        result = $"Apply {_calculatedSeverity.Name} Severity";
                    }
                }

                return result;
            }
        }

        public string Text { get; set; }

        public Bitmap Image => null;
        public bool Printable => false;
    }
}
