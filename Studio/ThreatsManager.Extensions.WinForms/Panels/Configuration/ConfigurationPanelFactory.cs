using System.Windows.Forms;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Panels;

namespace ThreatsManager.Extensions.Panels.Configuration
{
    [Extension("2842988B-59B9-41F6-9A5E-19E794C4E344", "Extensions Configuration Panel", 50, ExecutionMode.Business)]
    public class ConfigurationPanelFactory : IConfigurationPanelFactory<Form>
    {
        public IConfigurationPanel<Form> Create()
        {
            return new ConfigurationPanel();
        }
    }
}
