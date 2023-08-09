using System.Windows.Forms;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Panels;

namespace ThreatsManager.Extensions.Panels.DiagramConfiguration
{
    [Extension("D8DD1BEE-CF6C-41A3-BC68-D95B1A70A4D8", "Diagram Configuration Panel", 51, ExecutionMode.Business)]
    public class DiagramConfigurationPanelFactory : IConfigurationPanelFactory<Form>
    {
        public IConfigurationPanel<Form> Create()
        {
            return new DiagramConfigurationPanel();
        }
    }
}
