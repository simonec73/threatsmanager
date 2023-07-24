using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
using PostSharp.Serialization;
using System.ComponentModel;

namespace ThreatsManager.Utilities.Aspects.Engine
{
    /// <summary>
    /// Aspect used to introduce automatic notification of property updates.
    /// </summary>
    [PSerializable]
    public class SimpleNotifyPropertyChangedAttribute : LocationInterceptionAspect
    {
        /// <summary>
        /// Intercept property of field setting.
        /// </summary>
        /// <param name="args">Argument of the interception.</param>
        public override void OnSetValue(LocationInterceptionArgs args)
        {
            var oldValue = args.GetCurrentValue();
            var newValue = args.Value;

            base.OnSetValue(args);

            if (args.Instance is INotifyPropertyChangedInvoker invoker &&
                ((newValue == null && oldValue != null) || 
                (newValue != null && !newValue.Equals(oldValue))))
            {
                invoker.InvokeNotifyPropertyChanged(args.LocationName);
            } 
        }
    }

    /// <summary>
    /// Aspect used to introduce the INotifyPropertyChanged interface.
    /// </summary>
    [PSerializable]
    [IntroduceInterface(typeof(INotifyPropertyChanged))]
    [IntroduceInterface(typeof(INotifyPropertyChangedInvoker))]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, typeof(SimpleNotifyPropertyChangedAttribute))]
    public class IntroduceNotifyPropertyChangedAttribute : InstanceLevelAspect, INotifyPropertyChanged, INotifyPropertyChangedInvoker
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void InvokeNotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(Instance, new PropertyChangedEventArgs(propertyName));
        }
    }

    public interface INotifyPropertyChangedInvoker
    {
        void InvokeNotifyPropertyChanged(string propertyName);
    }
}
