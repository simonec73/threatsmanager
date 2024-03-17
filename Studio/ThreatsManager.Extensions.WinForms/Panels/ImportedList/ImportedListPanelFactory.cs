using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.ImportedList
{
    [Extension("D38BE3E2-0809-4DEC-8F3A-EA9FBDE9FF2A", "Imported List", 0, ExecutionMode.Expert)]
    public partial class ImportedListPanelFactory : IPanelFactory<Form>, IMainRibbonExtension,
        IPanelFactoryActionsRequestor
    {
        #region IPanelFactory implementation.
        /// <summary>
        /// The Imporeted List Panel Factory should create a single instance.
        /// </summary>
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            var result = new ImportedListPanel();

            action = new ActionDefinition(result.Id, "ImportedList", "Imported\nList", 
                Properties.Resources.arrow_into_big,
                Properties.Resources.arrow_into);

            return result;
        }

        public IPanel<Form> Create([NotNull] IActionDefinition action)
        {
            return new ImportedListPanel();
        }
        #endregion
    }
}