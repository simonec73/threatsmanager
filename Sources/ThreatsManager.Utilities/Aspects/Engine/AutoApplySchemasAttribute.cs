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
    public class AutoApplySchemasAttribute : LocationInterceptionAspect
    {
        /// <summary>
        /// Code executed when the property is set.
        /// </summary>
        /// <param name="args">Arguments describing the operation.</param>
        public override void OnSetValue(LocationInterceptionArgs args)
        {
            if (args.Value is IThreatModel model && args.Instance is IPropertiesContainer container)
            {
                model.AutoApplySchemas(container);
            }

            base.OnSetValue(args);
        }
    }
}
