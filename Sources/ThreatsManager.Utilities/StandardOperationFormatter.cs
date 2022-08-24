using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Recording.Operations;
using System.ComponentModel;
using System.Reflection;

namespace ThreatsManager.Utilities
{
    class StandardOperationFormatter : OperationFormatter
    {
        public StandardOperationFormatter(OperationFormatter next) : base(next)
        {
        }

        protected override string FormatOperationDescriptor(IOperationDescriptor operation)
        {
            if (operation.OperationKind != OperationKind.Method)
                return null;

            var descriptor = (MethodExecutionOperationDescriptor)operation;


            if ((descriptor.Method?.IsSpecialName ?? false) && (descriptor.Method?.Name?.StartsWith("set_") ?? false))
            {
                // We have a property setter.

                var property = descriptor.Method.DeclaringType.GetProperty(
                       descriptor.Method.Name.Substring(4),
                       BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                var attributes =
                     (DisplayNameAttribute[])property.GetCustomAttributes(typeof(DisplayNameAttribute), false);

                if (attributes.Length > 0)
                    return string.Format("Set {0} to {1}", attributes[0].DisplayName, descriptor.Arguments[0] ?? "null");
            }
            else if (descriptor.MethodName?.StartsWith("set_") ?? false)
            {
                return $"Set {descriptor.MethodName.Substring(4)}";
            }
            else
            {
                // We have another method.

                var attributes = (DisplayNameAttribute[])
                   descriptor.Method.GetCustomAttributes(typeof(DisplayNameAttribute), false);

                if (attributes.Length > 0)
                    return attributes[0].DisplayName;
            }

            return null;
        }
    }
}
