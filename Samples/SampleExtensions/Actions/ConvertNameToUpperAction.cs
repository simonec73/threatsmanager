using System.Drawing;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace SampleExtensions.Actions
{
    /// <summary>
    /// A simple Action that creates an item in the context menu for the Entities.
    /// </summary>
    /// <remarks>The Extension attribute is required to allow the class to be recognized as an extension.</remarks>
    [Extension("AEFC2C88-6D98-47BA-9A7E-27C7F5855D0F",
        "Convert Name to Upper Case Context Aware Action", 100, ExecutionMode.Simplified)]
    public class ConvertNameToUpperAction : IIdentityContextAwareAction
    {
        /// <summary>
        /// Scope of the Extension. This identifies where the Extension will be applied.
        /// </summary>
        /// <remarks>Considering that we are talking about an Identity Context Aware Action, only objects
        /// inheriting from IIdentity will be served by this Extension. The Scope allows further limiting the scope.
        /// In this case, only External Interactors, Processes and Data Stores will be served.</remarks>
        public Scope Scope => Scope.Entity;

        /// <summary>
        /// Label to be used for the item in the Context Menu.
        /// </summary>
        public string Label => "Convert Name to Upper";

        /// <summary>
        /// The Group allows grouping items in the Context Menu.
        /// </summary>
        /// <remarks>Items in the same Group will be separated from items of other Groups by separators.
        /// The order of the items in the Group is defined by the Priority of the Extension.
        /// The order of the Groups is defined by the Extension with the highest Priority in the Group
        /// (which means lower value of the Priority set in the Extensions Metadata).</remarks>
        public string Group => "Other";

        /// <summary>
        /// Full-size icon to be used for the item in the Context Menu.
        /// </summary>
        /// <remarks>It is typically a 64bit per 64bit bitmap.</remarks>
        public Bitmap Icon => null;

        /// <summary>
        /// Small-size icon to be used for the item in the Context Menu.
        /// </summary>
        /// <remarks>It is typically a 32bit per 32bit bitmap.</remarks>
        public Bitmap SmallIcon => null;

        /// <summary>
        /// Shortcut for the item.
        /// </summary>
        public Shortcut Shortcut => Shortcut.None;

        /// <summary>
        /// Execute the Extension.
        /// </summary>
        /// <param name="item">Item object of the Extension execution. This represents the Context.</param>
        /// <returns>True if the execution succeeds, false otherwise.</returns>
        /// <remarks>This method is defined in IContextAwareAction.
        /// Typically, it simply calls Execute(identity) as shown below.</remarks>
        public bool Execute(object item)
        {
            return (item is IIdentity identity) && Execute(identity);
        }

        /// <summary>
        /// Function called before showing the Context menu with the Action, to determine if it should be visible.
        /// </summary>
        /// <param name="item">Context used to decide if the action must be shown.
        /// <para>Given that the Extension is an IIdentityContextAwareAction and that the Scope is IEntity,
        /// the object is necessarily a <see cref="ThreatsManager.Interfaces.ObjectModel.Entities.IEntity"/>.</para></param>
        /// <returns>True if the Action is visible, false otherwise.</returns>
        public bool IsVisible(object item)
        {
            return true;
        }

        /// <summary>
        /// Execute the Extension.
        /// </summary>
        /// <param name="identity">Identity object of the Extension execution. This represents the Context.</param>
        /// <returns>True if the execution succeeds, false otherwise.</returns>
        /// <remarks>This method is defined in IIdentityContextAwareAction.</remarks>
        public bool Execute(IIdentity identity)
        {
            bool result = false;

            if (!string.IsNullOrWhiteSpace(identity?.Name))
            {
                // A vary simple implementation, for example purposes.
                // Much more complex things may be done starting from the identity:
                // for example, it is possible to get a reference to the Model, by casting
                // the identity to IThreatModelChild, because most identities are children of the Threat Model.

                identity.Name = identity.Name.ToUpper();
                result = true;
            }

            return result;
        }
    }
}
