using System;
using System.Drawing;
using ThreatsManager.Extensions.Properties;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("4CB5F79B-189F-499F-AC63-ABF4EEFB462F", "Remove from Model Context Aware Action", 51, ExecutionMode.Simplified)]
    public class RemoveFromModel : IIdentityContextAwareAction, IRemoveIdentityFromModelRequiredAction, IAsker
    {
        public Scope Scope => Scope.Entity | Scope.DataFlow | Scope.Group;
        public string Label => "Remove from Model";
        public string Group => "Remove";
        public Bitmap Icon => null;
        public Bitmap SmallIcon => null;
        public Shortcut Shortcut => Shortcut.None;

        public event Action<IIdentity> IdentityRemovingRequired;
        public event Action<IAsker, object, string, string, bool, RequestOptions> Ask;

        public bool Execute(object item)
        {
            bool result = false;

            if (item is IIdentity identity)
                result = Execute(identity);

            return result;
        }

        public bool IsVisible(object item)
        {
            return true;
        }

        public bool Execute(IIdentity identity)
        {
            Ask?.Invoke(this, identity, Resources.DeleteFromModel, 
                string.Format(Resources.RemoveIdentityFromModel, identity.Name),
                false, RequestOptions.YesNo);

            return true;
        }

        public void Answer(object context, AnswerType answer)
        {
            if (answer == AnswerType.Yes && context is IIdentity identity)
                IdentityRemovingRequired?.Invoke(identity);
        }
    }
}