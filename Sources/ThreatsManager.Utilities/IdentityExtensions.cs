using System;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Utilities
{
    /// <summary>
    /// Extensions used to manage the Identity.
    /// </summary>
    public static class IdentityExtensions
    {
        /// <summary>
        /// Get the Type Name for the Identity.
        /// </summary>
        /// <param name="identity">Identity whose Type Name must be returned.</param>
        /// <returns>Type Name for the Identity.</returns>
        /// <remarks>The Type Name is provided by the attribute Type Label, if present. 
        /// Otherwise, the name of the type is used.</remarks>
        public static string GetIdentityTypeName(this IIdentity identity)
        {
            return GetIdentityTypeName(identity?.GetType());
        }

        /// <summary>
        /// Get the Type Name for the Type.
        /// </summary>
        /// <param name="type">Type whose Type Name must be returned.</param>
        /// <returns>Type Name for the Type.</returns>
        /// <remarks>The Type Name is provided by the attribute Type Label, if present. 
        /// Otherwise, the name of the type is used.</remarks>
        public static string GetIdentityTypeName(this Type type)
        {
            TypeLabelAttribute[] attribs = type?.GetCustomAttributes(
                typeof(TypeLabelAttribute), false) as TypeLabelAttribute[];

            return attribs?.Length > 0 ? attribs[0].Label : type.Name;
        }

        /// <summary>
        /// Get the Type Initial for the Identity.
        /// </summary>
        /// <param name="identity">Identity whose Type Initial must be retuned.</param>
        /// <returns>Type Initial for the Identity.</returns>
        /// <remarks>The Type Initial is provided by the attribute Type Initial, if present.
        /// If it is not present, it returns null.</remarks>
        public static string GetIdentityTypeInitial(this IIdentity identity)
        {
            return GetIdentityTypeInitial(identity?.GetType());
        }

        /// <summary>
        /// Get the Type Initial for the Type.
        /// </summary>
        /// <param name="type">Type whose Type Initial must be retuned.</param>
        /// <returns>Type Initial for the Type.</returns>
        /// <remarks>The Type Initial is provided by the attribute Type Initial, if present.
        /// If it is not present, it returns null.</remarks>
        public static string GetIdentityTypeInitial(this Type type)
        {
            TypeInitialAttribute[] attribs = type?.GetCustomAttributes(
                typeof(TypeInitialAttribute), false) as TypeInitialAttribute[];

            return attribs?.Length > 0 ? attribs[0].Initial : null;
        }
    }
}
