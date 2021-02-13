using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.ThreatSources
{
    [Extension("FDBD9823-8FCC-44BF-A81E-C1D475C6D2E7", "Capec Import", 20, ExecutionMode.Pioneer)]
    public partial class CapecImportPanelFactory : IPanelFactory<Form>, IMainRibbonExtension, IPanelFactoryActionsRequestor
    {
        #region IPanelFactory implementation.
        /// <summary>
        /// The Capec Import Panel Factory should create a single instance.
        /// </summary>
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            var result = new CapecImportPanel();

            action = new ActionDefinition(result.Id, "CapecImport", "Import Capec Threats", Properties.Resources.import_big,
                Properties.Resources.import);

            return result;
        }

        public IPanel<Form> Create([NotNull] IActionDefinition action)
        {
            return new CapecImportPanel();
        }
        #endregion
    }
}