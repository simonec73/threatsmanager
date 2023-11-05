using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Serialization;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Utilities.Aspects.Engine
{
    /// <summary>
    /// Attribute that configures automatically AutoApplySchemas for the current object, when the property containing the IThreatModel is set.
    /// </summary>
    [PSerializable]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, typeof(SimpleNotifyPropertyChangedAttribute))]
    public class AutoApplySchemasAttribute : LocationInterceptionAspect
    {
        /// <summary>
        /// Code executed when the property is set.
        /// </summary>
        /// <param name="args">Arguments describing the operation.</param>
        public override void OnSetValue(LocationInterceptionArgs args)
        {
            var model = args.Value as IThreatModel ?? (args.Value as IThreatModelChild)?.Model;

            if (model != null && args.Instance is IPropertiesContainer container && !UndoRedoManager.IsRedoing && !UndoRedoManager.IsUndoing)
            {
                model.AutoApplySchemas(container);
            }

            base.OnSetValue(args);
        }
    }
}
