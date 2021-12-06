using System.Collections.Generic;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Utilities.WinForms
{
    public class SeverityComboBox : GridComboBoxExEditControl
    {
        public SeverityComboBox(IEnumerable<ISeverity> severities)
        {
            DataSource = new List<ISeverity>(severities);
            DropDownStyle = ComboBoxStyle.DropDownList;
        }
    }
}