using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Dialogs
{
    public partial class AddSpecializedMitigationDialog : Form
    {
        private readonly IMitigation _mitigation;

        public AddSpecializedMitigationDialog()
        {
            InitializeComponent();
        }

        public AddSpecializedMitigationDialog([NotNull] IMitigation mitigation) : this()
        {
            _mitigationName.Text = mitigation.Name;

            _templates.ItemHeight = (int)(16 * Dpi.Factor.Height);

            _mitigation = mitigation;
            var specialized = mitigation.Specialized?.ToArray();

            IEnumerable<IItemTemplate> templates = mitigation.Model?.EntityTemplates?
                .Where(x => !(specialized?.Any(y => y.TargetId == x.Id) ?? false))
                .OrderBy(x => x.Name)
                .ToArray();         
            if (templates?.Any() ?? false)
            {
                _templates.Items.AddRange(templates.Select(x => new ComboBoxItem(x, x.GetImage(ImageSize.Small))).ToArray());
            }

            templates = mitigation.Model?.FlowTemplates?
                .Where(x => !(specialized?.Any(y => y.TargetId == x.Id) ?? false))
                .OrderBy(x => x.Name)
                .ToArray();
            if (templates?.Any() ?? false)
            {
                _templates.Items.AddRange(templates.Select(x => new ComboBoxItem(x, x.GetImage(ImageSize.Small))).ToArray());
            }
        }
    }
}
