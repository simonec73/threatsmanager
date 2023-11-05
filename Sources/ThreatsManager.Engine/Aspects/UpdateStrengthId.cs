using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Patterns.Model;
using PostSharp.Serialization;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Engine.Aspects
{
    /// <summary>
    /// Attribute assigned to a Strength property to automatically assign the corresponding ID property.
    /// </summary>
    [PSerializable]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, typeof(SimpleNotifyPropertyChangedAttribute))]
    public class UpdateStrengthIdAttribute : LocationInterceptionAspect
    {
        /// <summary>
        /// Code executed when the property is set.
        /// </summary>
        /// <param name="args">Arguments describing the operation.</param>
        public override void OnSetValue(LocationInterceptionArgs args)
        {
            if (!UndoRedoManager.IsUndoing && !UndoRedoManager.IsRedoing &&
                args.Value is IStrength strength &&
                args.Instance is IStrengthIdChanger target)
            {
                var oldValue = target.GetStrengthId();
                var newValue = strength.Id;
                if (oldValue != newValue)
                    target.SetStrengthId(newValue);
            }

            base.OnSetValue(args);
        }
    }
}
