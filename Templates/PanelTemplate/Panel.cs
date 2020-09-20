using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace $rootnamespace$
{
    /// <summary>
    /// $safeitemname$ is used to implement a Panel that will be shown as part of a Form.
    /// </summary>
    // TODO: Change Label, Priority and ExecutionMode. 
    public partial class $safeitemrootname$ : UserControl, IShowThreatModelPanel<Form>
    {
        private readonly Guid _id = Guid.NewGuid();

        public $safeitemrootname$()
        {
            InitializeComponent();
        }

        #region Implementation of interface IShowThreatModelPanel.
        public Guid Id => _id;
        public Form PanelContainer { get; set; }

        public void SetThreatModel(IThreatModel model)
        {
            if (model != null)
            {
                // TODO: add your code here.
            }
        }
        #endregion
    }
}
