using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Panels;

namespace ThreatsManager.DevOps.Panels.Configuration
{
    [Extension("7F507E55-E545-409F-9BE8-C8EBE280B2AC", "DevOps Configuration Panel", 50, ExecutionMode.Management)]
    public class ConfigurationPanelFactory : IConfigurationPanelFactory<Form>
    {
        public IConfigurationPanel<Form> Create()
        {
            return new ConfigurationPanel();
        }
    }
}
