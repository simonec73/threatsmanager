using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Serialization;
using ThreatsManager.Interfaces;

namespace ThreatsManager.Utilities.Aspects
{
    [PSerializable]
    [ProvideAspectRole("Initialization")]
    [AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, StandardRoles.Validation)]
    [LinesOfCodeAvoided(2)]
    public class InitializationRequired : OnMethodBoundaryAspect
    {
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private bool _isDefaultValueInitalized;
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private object _defaultValue;

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
