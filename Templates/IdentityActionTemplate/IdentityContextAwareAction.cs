using System.Drawing;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;

namespace $rootnamespace$
{
    /// <summary>
    /// $safeitemname$ is used to show actions in Context Menus.
    /// </summary>
    /// <remarks>IIdentityContextAwareAction applies to objects implementing IIdentity.</remarks>
    // TODO: Change Label, Priority and ExecutionMode. 
    [Extension("$guid1$", "$itemname$ Context Aware Action", 100, ExecutionMode.Simplified)]
    public class $safeitemname$ : IIdentityContextAwareAction
    {
        // TODO: specify the right scope.
        public Scope Scope => Scope.Entity;    
        
        // TODO: select the label.
        public string Label => "$itemname$ Action"; 

        // TODO: specify the name of the group, where the Action will be included.
        public string Group => "Group";

        // TODO: select an icon (64x64 pixels) to be shown.
        public Bitmap Icon => null;
        
        // TODO: select a small icon to be shown (32x32 pixels) to be shown. 
        public Bitmap SmallIcon => null;

        // TODO: choose the shortcut to be used for the Action.
        public Shortcut Shortcut => Shortcut.None;

        public bool Execute(object item)
        {
            bool result = false;

            if (item is IIdentity identity)
                result = Execute(identity);

            return result;
        }

        public bool Execute(IIdentity identity)
        {
            throw new System.NotImplementedException();
        }
    }
}