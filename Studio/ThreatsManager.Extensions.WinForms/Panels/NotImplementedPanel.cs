using System;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Panels
{
#pragma warning disable CS0067
    public partial class NotImplementedPanel : UserControl, IShowThreatModelPanel<Form>
    {
        private readonly Guid _id = Guid.NewGuid();

        public NotImplementedPanel()
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
        }
        #endregion
    }
}
