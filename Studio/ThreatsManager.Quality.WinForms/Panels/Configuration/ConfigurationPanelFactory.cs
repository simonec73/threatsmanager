using System.Windows.Forms;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Panels;

namespace ThreatsManager.Quality.Panels.Configuration
{
    [Extension("C21ED74F-3D9F-45B2-B155-922C724F3075", "Quality Configuration Panel", 50, ExecutionMode.Business)]
    public class ConfigurationPanelFactory : IConfigurationPanelFactory<Form>
    {
        public IConfigurationPanel<Form> Create()
        {
            return new ConfigurationPanel();
        }
    }
}
