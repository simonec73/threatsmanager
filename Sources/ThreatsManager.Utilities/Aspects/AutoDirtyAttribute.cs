using System.Linq;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
using PostSharp.Extensibility;
using PostSharp.Serialization;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Utilities.Aspects
{
    /// <summary>
    /// Attribute applied to automatically change the status of IDirty objects to Dirty when a Property is set.
    /// </summary>
    /// <remarks>PostSharp is required to make this attribute effective.
    /// <para>If PostSharp is not used, then the same result may be achieved by specifying
    /// <example>IsDirty = true</example> in the interested object.</para></remarks>
    [PSerializable]
    [ProvideAspectRole("Dirty")]
    [MulticastAttributeUsage(MulticastTargets.Class, Inheritance = MulticastInheritance.Multicast)]
    [AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.After, "Initialization")]
    [AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.After, "Notification")]
    public class AutoDirtyAttribute : InstanceLevelAspect
    {
        /// <summary>
        /// Logic to be executed when the Property is Set.
        /// </summary>
        /// <param name="args">Parameters of the interception.</param>
        [OnLocationSetValueAdvice, MulticastPointcut(Targets = MulticastTargets.Property,
                Attributes = MulticastAttributes.Instance | MulticastAttributes.NonAbstract)]
        public void OnPropertySet(LocationInterceptionArgs args)
        {
            // Don't go further if the new value is equal to the old one.
            // (Possibly use object.Equals here).
            if (args.Value == args.GetCurrentValue()) return;

            // Actually sets the value.
            args.ProceedSetValue();

            // Ignore properties with AutoDirtyIgnoreAttribute.
            var propertyInfo = args.Location.PropertyInfo;
            if (propertyInfo.GetCustomAttributes(typeof(AutoDirtyIgnoreAttribute), true).Any())
                return;

            if (Instance is IDirty dirtyObject)
                dirtyObject.SetDirty();
        }
    }
}