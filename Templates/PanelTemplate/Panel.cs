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
    public partial class $safeitemrootname$ : UserControl, IShowThreatModelPanel
    {
        public $safeitemrootname$()
        {
            InitializeComponent();
        }

        #region Implementation of interface IShowThreatModelPanel.
        public Form ContainingForm { get; set; }

        public void SetThreatModel(IThreatModel model)
        {
            if (model != null)
            {
                // TODO: add your code here.
            }
        }
        #endregion

        // TODO: fill the fourth parameter with the big icon (64x64 pixels) and the fifth with a smaller icon (32x32 pixels)
        public IActionDefinition ActionDefinition => 
            new ActionDefinition(Guid.NewGuid(), "$itemrootname$", "$itemrootname$", null, null);
    }
}
