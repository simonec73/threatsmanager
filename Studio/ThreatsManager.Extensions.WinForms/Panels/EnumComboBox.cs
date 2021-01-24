using System.Collections.Generic;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;

namespace ThreatsManager.Extensions.Panels
{
    public class EnumComboBox : GridComboBoxExEditControl
    {
        public EnumComboBox(IEnumerable<string> enumValues)
        {
            DataSource = new List<string>(enumValues);
            DropDownStyle = ComboBoxStyle.DropDownList;
        }
    }
}