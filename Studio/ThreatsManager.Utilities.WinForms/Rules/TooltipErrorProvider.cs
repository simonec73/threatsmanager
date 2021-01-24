using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Validator;
using ThreatsManager.Utilities.WinForms.Properties;

namespace ThreatsManager.Utilities.WinForms.Rules
{
    public partial class TooltipErrorProvider : SuperTooltip, IErrorProvider
    {
        public TooltipErrorProvider()
        {
            InitializeComponent();
        }

        public string Caption { get; set; }

        public void ClearError(Control control)
        {
            this.SetSuperTooltip(control, null);
        }

        public void SetError(Control control, string value)
        {
            string caption;
            if (string.IsNullOrWhiteSpace(this.Caption))
                caption = string.Format(Resources.TooltipCaption, control.Name);
            else
                caption = Caption;

            SuperTooltipInfo info = new SuperTooltipInfo()
            {
                HeaderText = caption,
                HeaderVisible = true,
                FooterVisible = false,
                BodyText = value,
                Color = eTooltipColor.System
            };

            this.SetSuperTooltip(control, info);
        }
    }
}
