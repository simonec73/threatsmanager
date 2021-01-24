using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Quality.Panels.ReviewNotes
{
    [Extension("85D20B43-4798-4509-B1F9-7AE2DA942425", "Review Notes Panel", 100, ExecutionMode.Management)]
    public partial class ReviewNotesPanelFactory : IPanelFactory<Form>, IMainRibbonExtension, IPanelFactoryActionsRequestor
    {
        #region IPanelFactory implementation.
        /// <summary>
        /// The Factory should create a single instance.
        /// </summary>
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            var result = new ReviewNotesPanel();

            action = result.ActionDefinition;

            return result;
        }

        public IPanel<Form> Create([NotNull] IActionDefinition action)
        {
            return new ReviewNotesPanel();
        }
        #endregion
    }
}
