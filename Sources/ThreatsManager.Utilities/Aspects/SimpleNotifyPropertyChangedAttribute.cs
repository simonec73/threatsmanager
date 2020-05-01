using System;
using System.ComponentModel;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
using PostSharp.Extensibility;
using PostSharp.Reflection;
using PostSharp.Serialization;

namespace ThreatsManager.Utilities.Aspects
{
    [PSerializable]
    [ProvideAspectRole("Notification")]
    [MulticastAttributeUsage(MulticastTargets.Class, Inheritance = MulticastInheritance.Multicast)]
    [IntroduceInterface(typeof(INotifyPropertyChanged), OverrideAction = InterfaceOverrideAction.Ignore)]
    [AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.After, "Initialization")]
    public class SimpleNotifyPropertyChangedAttribute : InstanceLevelAspect, INotifyPropertyChanged
    {
        [ImportMember("OnPropertyChanged", IsRequired = false,
            Order = ImportMemberOrder.AfterIntroductions)]
        public Action<string> OnPropertyChangedMethod;

        [IntroduceMember(Visibility = Visibility.Family,
            IsVirtual = true, OverrideAction = MemberOverrideAction.Ignore)]
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(Instance,
                new PropertyChangedEventArgs(propertyName));
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.Ignore)]
        public event PropertyChangedEventHandler PropertyChanged;

        [OnLocationSetValueAdvice, MulticastPointcut(Targets = MulticastTargets.Property,
             Attributes = MulticastAttributes.Instance | MulticastAttributes.NonAbstract)]
        public void OnPropertySet(LocationInterceptionArgs args)
        {
            // Don't go further if the new value is equal to the old one.
            // (Possibly use object.Equals here).
            if (args.Value == args.GetCurrentValue()) return;

            // Actually sets the value.
            args.ProceedSetValue();

            OnPropertyChangedMethod?.Invoke(args.Location.Name);

        }

    }
}
