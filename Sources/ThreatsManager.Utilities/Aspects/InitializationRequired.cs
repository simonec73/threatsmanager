using PostSharp.Aspects;
using PostSharp.Aspects.Configuration;
using PostSharp.Aspects.Dependencies;
using PostSharp.Aspects.Serialization;
using PostSharp.Serialization;
using System;
using System.Reflection;
using ThreatsManager.Interfaces;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Utilities.Aspects
{
    /// <summary>
    /// Interface implemented to prevent entering a method if the object is not initialized.
    /// </summary>
    [PSerializable]
    [OnMethodBoundaryAspectConfiguration(SerializerType = typeof(MsilAspectSerializer))]
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

        /// <summary>
        /// Default constructor.
        /// </summary>
        public InitializationRequired()
        {
            _isDefaultValueInitalized = false;
            _defaultValue = null;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="defaultValue">Default value to use as return value of the method, if the object is not initialized.</param>
        public InitializationRequired(object defaultValue)
        {
            _isDefaultValueInitalized = true;
            _defaultValue = defaultValue;
        }

        /// <summary>
        /// Method called entering the method.
        /// </summary>
        /// <param name="args">Method execution arguments.</param>
        public sealed override void OnEntry(MethodExecutionArgs args)
        {
            if (args.Instance is IInitializableObject initializableObject && !initializableObject.IsInitialized)
            {
                if (_isDefaultValueInitalized)
                {
                    args.ReturnValue = _defaultValue;
                }
                else
                {
                    var info = args.Method as MethodInfo;
                    if (info != null)
                    {
                        var type = info.ReturnType;
                        if ((type?.IsValueType ?? false) && (type != typeof(void)))
                        {
                            args.ReturnValue = Activator.CreateInstance(type);
                        }
                        else
                        {
                            args.ReturnValue = null;
                        }
                    }
                }

                args.FlowBehavior = FlowBehavior.Return;
            }
        }
    }
}
