using System;
using System.Windows.Forms;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;

namespace ThreatsManager.Extensions.Panels.Learning
{
    public partial class LearningPanel : UserControl, IStaticPanel<Form>, ICustomRibbonExtension, IDesktopAlertAwareExtension
    {
        private readonly Guid _id = Guid.NewGuid();

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public LearningPanel()
        {
            InitializeComponent();
        }

        #region Implementation of interface IStaticPanel.

        public Guid Id => _id;

        public Form PanelContainer { get; set; }

        public void SetAction(string parameter)
        {
        }
        #endregion
    }
}
