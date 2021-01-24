using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.DevOps.Panels.ImplementationReview
{
    [Extension("07FB9D18-5D0A-461A-AF3B-EE697BB25E2A", "Implementation Review Panel", 75, ExecutionMode.Simplified)]
    public partial class ImplementationReviewPanelFactory : IPanelFactory<Form>, IMainRibbonExtension
    {
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            throw new NotImplementedException();
        }

        public IPanel<Form> Create(IActionDefinition action)
        {
            throw new NotImplementedException();
        }
    }
}
