using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThreatsManager.Utilities.Aspects.Engine
{
    /// <summary>
    /// This attribute is assigned to a property of an object and allows to automatically assign a value to another property of the same object,
    /// when the value of the property is changed. For example, if it is applied to a property Model receiving a IThreatModel and if it has as "Id"
    /// sourcePropertyName, and "ParentId" as targetPropertyName, then when the property Model is changed, the aspect retrieves the value of
    /// property Id of the new IThreatModel and assigns it to property ParentId of the object containing property Model.
    /// </summary>
    [PSerializable]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, typeof(AutoApplySchemasAttribute))]
    public class UpdateIdAttribute : LocationInterceptionAspect
    {
        private string _sourcePropertyName;
        private string _targetPropertyName;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sourcePropertyName">Name of the Property of the object assigned to the setter containing the value to be retrieved.</param>
        /// <param name="targetPropertyName">Name of the Property of the object containing the setter, where the value must be set.</param>
        public UpdateIdAttribute(string sourcePropertyName, string targetPropertyName)
        {
            _sourcePropertyName = sourcePropertyName;
            _targetPropertyName = targetPropertyName;
        }

        /// <summary>
        /// Code executed when the property is set.
        /// </summary>
        /// <param name="args">Arguments describing the operation.</param>
        public override void OnSetValue(LocationInterceptionArgs args)
        {
            if (!string.IsNullOrWhiteSpace(_sourcePropertyName) &&
                !string.IsNullOrWhiteSpace(_targetPropertyName))
            {
                var sourceProperty = args?.Value?.GetType()?.GetProperty(_sourcePropertyName,
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                var targetProperty = args?.Instance?.GetType()?.GetProperty(_targetPropertyName,
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                if ((sourceProperty != null && targetProperty != null &&
                    sourceProperty.PropertyType == targetProperty.PropertyType))
                {
                    targetProperty.SetValue(args.Instance, sourceProperty.GetValue(args.Value));
                } else if (sourceProperty == null && targetProperty != null)
                {
                    if (targetProperty.PropertyType == typeof(Guid))
                    {
                        targetProperty.SetValue(args.Instance, Guid.Empty);
                    } else if (targetProperty.PropertyType == typeof(int))
                    {
                        targetProperty.SetValue(args.Instance, 0);
                    } else if (!targetProperty.PropertyType.IsPrimitive)
                    {
                        targetProperty.SetValue(args.Instance, null);
                    }
                }
            }

            base.OnSetValue(args);
        }
    }
}
