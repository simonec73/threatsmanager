using PostSharp.Patterns.Contracts;
using System;
using System.Windows.Forms;
using ThreatsManager.Engine;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Dialogs
{
    public partial class FixDiagram : Form
    {
        private readonly Guid _diagramId;
        private int _adjustFactor = 100;

        public FixDiagram([NotNull] IDiagram diagram)
        {
            _diagramId = diagram.Id;
            InitializeComponent();
        }

        private void _adjust_ValueChanged(object sender, EventArgs e)
        {
            var value = (AdjustmentFactor) _adjust.Value;
            switch (value)
            {
                case AdjustmentFactor.Zoom25:
                    _adjustFactor = 25;
                    break;
                case AdjustmentFactor.Zoom50:
                    _adjustFactor = 50;
                    break;
                case AdjustmentFactor.Zoom75:
                    _adjustFactor = 75;
                    break;
                case AdjustmentFactor.Zoom100:
                    _adjustFactor = 100;
                    break;
                case AdjustmentFactor.Zoom125:
                    _adjustFactor = 125;
                    break;
                case AdjustmentFactor.Zoom150:
                    _adjustFactor = 150;
                    break;
                case AdjustmentFactor.Zoom200:
                    _adjustFactor = 200;
                    break;
            }

            _adjustText.Text = $"{_adjustFactor}%";
        }

        private void _ok_Click(object sender, EventArgs e)
        {
            if (_adjustFactor != 100)
                EventsDispatcher.RaiseEvent("AdjustDpiFactor", new AdjustFactorParams(_diagramId, _adjustFactor));
            this.Close();
        }

        private void _apply_Click(object sender, EventArgs e)
        {
            if (_adjustFactor != 100)
            {
                EventsDispatcher.RaiseEvent("AdjustDpiFactor", new AdjustFactorParams(_diagramId, _adjustFactor));
                _adjust.Value = (int)AdjustmentFactor.Zoom100;
            }
        }

        private void _resetFlows_Click(object sender, EventArgs e)
        {
            EventsDispatcher.RaiseEvent("ResetFlows", _diagramId);
        }
    }
}
