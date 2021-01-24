using System.Collections.Generic;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;

namespace ThreatsManager.DevOps.Dialogs
{
    public class ItemsComboBox : GridComboBoxExEditControl
    {
        public ItemsComboBox(IEnumerable<string> items)
        {
            DataSource = new List<string>(items);
            DropDownStyle = ComboBoxStyle.DropDownList;
        }
    }
}