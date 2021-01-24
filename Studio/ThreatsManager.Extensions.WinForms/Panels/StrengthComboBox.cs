using System.Collections.Generic;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Panels
{
    public class StrengthComboBox : GridComboBoxExEditControl
    {
        public StrengthComboBox(IEnumerable<IStrength> strengths)
        {
            DataSource = new List<IStrength>(strengths);
            DropDownStyle = ComboBoxStyle.DropDownList;
        }
    }
}