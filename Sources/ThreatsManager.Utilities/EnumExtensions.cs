using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;

namespace ThreatsManager.Utilities
{
    public static class EnumExtensions
    {
        public static string GetEnumLabel(this Enum value)
        {
            Type type = value.GetType();
            var name = value.ToString();

            return GetEnumLabel(name, type.GetField(name));
        }

        public static string GetEnumDescription(this Enum value)
        {
            Type type = value.GetType();
            var name = value.ToString();

            return GetEnumDescription(name, type.GetField(name));
        }

        public static Type GetEnumType(this Enum value)
        {
            Type type = value.GetType();
            var name = value.ToString();

            return GetEnumType(name, type.GetField(name));
        }

        public static T GetEnumFromType<T>(this Type type)
        {
            T result = default(T);

            var fields = typeof(T).GetFields();

            if (fields?.Any() ?? false)
            {
                var typeInfo = type.GetTypeInfo();

                foreach (var field in fields)
                {
                    var attribs = field.GetCustomAttributes<EnumTypeAttribute>().ToArray();

                    var text = attribs.Any() ? attribs[0].AssociatedType : null;

                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        var associatedType = Type.GetType(text, false);
                        if (typeInfo.ImplementedInterfaces.Contains(associatedType))
                        {
                            result = (T) field.GetRawConstantValue();
                            break;
                        }
                    }
                }
            }

            return result;
        }

        public static T GetEnumValue<T>([Required] this string text)
        {
            T result = default(T);

            Type type = typeof(T);

            var fields = type.GetFields();
            foreach (var field in fields)
            {
                var attribs = field.GetCustomAttributes<EnumLabelAttribute>().ToArray();

                if (attribs.Any(x => string.CompareOrdinal(x.Label, text) == 0) ||
                    string.CompareOrdinal(text, field.Name) == 0)
                {
                    result = (T)field.GetValue(type);
                    break;
                }
            }

            return result;
        }

        public static string GetXmlEnumLabel(this Enum value)
        {
            Type type = value.GetType();
            var name = value.ToString();

            return GetXmlEnumLabel(name, type.GetField(name));
        }

        public static IEnumerable<string> GetXmlEnumLabels<T>()
        {
            Type type = typeof(T);
            var values = Enum.GetNames(type);

            return values.Select(x => GetXmlEnumLabel(x, type.GetField(x)));
        }

        private static string GetXmlEnumLabel([Required] string name, [NotNull] FieldInfo fieldInfo)
        {
            var attribs = fieldInfo.GetCustomAttributes<XmlEnumAttribute>().ToArray();

            return attribs.Any() ? attribs[0].Name : name;
        }

        public static IEnumerable<string> GetEnumLabels<T>()
        {
            Type type = typeof(T);

            return type.GetFields().Where(x => x.IsStatic)
                .Select(x => new {Field = x, Attribute = x.GetCustomAttribute<EnumOrderAttribute>()})
                .Select(x => new {Name = GetEnumLabel(x.Field.Name, x.Field), Order = x.Attribute?.Order})
                .OrderBy(x => x.Order)
                .Select(x => x.Name);
        }

        public static T GetEnum<T>([Required] this string text, T defaultValue) where T : struct
        {
            if (!Enum.TryParse<T>(text, out var result))
                result = defaultValue;

            return result;
        }

        private static string GetEnumLabel([Required] string name, [NotNull] FieldInfo fieldInfo)
        {
            var attribs = fieldInfo.GetCustomAttributes<EnumLabelAttribute>().ToArray();

            return attribs.Any() ? attribs[0].Label : name;
        }

        private static string GetEnumDescription([Required] string name, [NotNull] FieldInfo fieldInfo)
        {
            var attribs = fieldInfo.GetCustomAttributes<EnumDescriptionAttribute>().ToArray();

            return attribs.Any() ? attribs[0].Text : null;
        }

        private static Type GetEnumType([Required] string name, [NotNull] FieldInfo fieldInfo)
        {
            var attribs = fieldInfo.GetCustomAttributes<EnumTypeAttribute>().ToArray();

            var text = attribs.Any() ? attribs[0].AssociatedType : null;
            Type result = null;
            if (!string.IsNullOrWhiteSpace(text))
                result = Type.GetType(text, false);

            return result;
        }

        public static IEnumerable<T> GetUIVisible<T>()
        {
            Type type = typeof(T);

            return Enum.GetValues(type).OfType<T>().Where(x => IsUIVisible(type.GetField(x.ToString())));
        }

        public static bool IsUIVisible(this Enum value)
        {
            Type type = value.GetType();
            return IsUIVisible(type.GetField(value.ToString()));
        }

        private static bool IsUIVisible([NotNull] FieldInfo fieldInfo)
        {
            var attribs = fieldInfo.GetCustomAttributes<UiHiddenAttribute>().ToArray();

            return !attribs.Any();
        }

        public static IEnumerable<Enum> GetFlags(this Enum input)
        {
            foreach (Enum value in Enum.GetValues(input.GetType()))
                if (input.HasFlag(value))
                    yield return value;
        }

        public static KnownColor GetEnumTextColor(this Enum value)
        {
            Type type = value.GetType();
            var name = value.ToString();

            return GetEnumTextColor(name, type.GetField(name));
        }

        private static KnownColor GetEnumTextColor([Required] string name, [NotNull] FieldInfo fieldInfo)
        {
            var attribs = fieldInfo.GetCustomAttributes<TextColorAttribute>().ToArray();

            return attribs.Any() ? attribs[0].Color : KnownColor.Black;
        }

        public static KnownColor GetEnumBackColor(this Enum value)
        {
            Type type = value.GetType();
            var name = value.ToString();

            return GetEnumBackColor(name, type.GetField(name));
        }

        private static KnownColor GetEnumBackColor([Required] string name, [NotNull] FieldInfo fieldInfo)
        {
            var attribs = fieldInfo.GetCustomAttributes<BackColorAttribute>().ToArray();

            return attribs.Any() ? attribs[0].Color : KnownColor.White;
        }

    }
}