using System.Collections.Generic;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Utilities.WinForms
{
    public class ThreatActorComboBox : GridComboBoxExEditControl
    {
        public ThreatActorComboBox(IEnumerable<IThreatActor> actors)
        {
            DataSource = new List<IThreatActor>(actors);
            DropDownStyle = ComboBoxStyle.DropDownList;
        }
    }
}