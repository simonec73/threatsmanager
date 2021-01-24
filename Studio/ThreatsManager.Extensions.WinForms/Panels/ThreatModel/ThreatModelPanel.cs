using System;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Panels.ThreatModel
{
#pragma warning disable CS0067
    public partial class ThreatModelPanel : UserControl, IShowThreatModelPanel<Form>, IInitializableObject, 
        IDesktopAlertAwareExtension, IExecutionModeSupport
    {
        private readonly Guid _id = Guid.NewGuid();

        public ThreatModelPanel()
        {
            InitializeComponent();
        }
        
        public event Action<string> ShowMessage;
        
        public event Action<string> ShowWarning;

        #region Implementation of interface IShowThreatModelPanel.
        public Guid Id => _id;

        public Form PanelContainer { get; set; }

        public void SetThreatModel([NotNull] IThreatModel threatModel)
        {
            _itemEditor.Item = threatModel;
        }
        #endregion

        public bool IsInitialized => _itemEditor.Item != null;

        public void SetExecutionMode(ExecutionMode mode)
        {
            _itemEditor.SetExecutionMode(mode);
        }
    }
}
