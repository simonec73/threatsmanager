using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Serialization;
using ThreatsManager.Interfaces;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Utilities.Aspects
{
    [PSerializable]
    [ProvideAspectRole("Initialization")]
    [AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, StandardRoles.Validation)]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, typeof(SimpleNotifyPropertyChangedAttribute))]
    [AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, "ChangeTracking")]
    [LinesOfCodeAvoided(2)]
    public class InitializationRequired : OnMethodBoundaryAspect
    {
#pragma warning disable IDE0044 // Add readonly modifier
        private bool _isDefaultValueInitalized;
        private object _defaultValue;
#pragma warning restore IDE0044 // Add readonly modifier

        public InitializationRequired()
        {
            _isDefaultValueInitalized = false;
            _defaultValue = null;
        }

        public InitializationRequired(object defaultValue)
        {
            _isDefaultValueInitalized = true;
            _defaultValue = defaultValue;
        }

        public sealed override void OnEntry(MethodExecutionArgs args)
        {
            if (args.Instance is IInitializableObject initializableObject && !initializableObject.IsInitialized)
            {
                if (_isDefaultValueInitalized)
                    args.ReturnValue = _defaultValue;

                args.FlowBehavior = FlowBehavior.Return;
            }
        }
    }
}
