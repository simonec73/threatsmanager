using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.MsTmt.Dialogs
{
    // ReSharper disable CoVariantArrayConversion
    public partial class ThreatAssignmentDialog : Form, IInitializableObject
    {
        private IThreatModel _threatModel;

        public ThreatAssignmentDialog()
        {
            InitializeComponent();

            _targetObjectType.Items.Add(Scope.ExternalInteractor.GetEnumLabel());
            _targetObjectType.Items.Add(Scope.Process.GetEnumLabel());
            _targetObjectType.Items.Add(Scope.DataStore.GetEnumLabel());
            _targetObjectType.Items.Add(Scope.DataFlow.GetEnumLabel());
        }

        public ThreatAssignmentDialog([NotNull] IThreatModel threatModel) : this()
        {
            _threatModel = threatModel;
        }

        public bool IsInitialized => _threatModel != null;

        public IIdentity SelectedIdentity => _targetObject.SelectedItem as IIdentity;

        private void _ok_Click(object sender, EventArgs e)
        {
        }

        [InitializationRequired]
        public void Initialize([Required] string name, string description)
        {
            _threatName.Text = name;
            _threatDescription.Text = description;
        }

        private void _targetObjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            _targetObject.Items.Clear();

            var scope = _targetObjectType.SelectedItem?.ToString().GetEnumValue<Scope>();
            IEnumerable<IIdentity> identities = null;

            switch (scope)
            {
                case Scope.ExternalInteractor:
                    identities = _threatModel.Entities?.OfType<IExternalInteractor>();
                    break;
                case Scope.Process:
                    identities = _threatModel.Entities?.OfType<IProcess>();
                    break;
                case Scope.DataStore:
                    identities = _threatModel.Entities?.OfType<IDataStore>();
                    break;
                case Scope.DataFlow:
                    identities = _threatModel.DataFlows;
                    break;
            }

            var array = identities?.ToArray();
            if (array?.Any() ?? false)
            {
                _targetObject.Items.AddRange(array);
            }
        }

        private void _targetObject_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ok.Enabled = _targetObject.SelectedItem != null;
        }
    }
}
