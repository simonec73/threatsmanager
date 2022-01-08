using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Serialization;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Utilities
{
    public class KnownTypesBinder : ISerializationBinder
    {
        private static readonly Dictionary<string, Type> _knownTypes = new Dictionary<string, Type>();
        private static readonly Dictionary<string, Type> _equivalences = new Dictionary<string, Type>();

        public static event Action<string, string> TypeNotFound;

        static KnownTypesBinder()
        {
            AddKnownType(typeof(object));
            AddKnownType(typeof(bool));
            AddKnownType(typeof(string));
            AddKnownType(typeof(int));
            AddKnownType(typeof(decimal));
            AddKnownType(typeof(Guid));
            AddKnownType(typeof(System.Drawing.PointF));
            AddKnownType(typeof(IListItem));
            AddKnownType(typeof(ListItem));
            AddKnownType(typeof(ExtensionConfigurationData));
        }

        public static void AddKnownType([NotNull] Type type)
        {
            var name = type.FullName;
            if (!string.IsNullOrWhiteSpace(name) && !_knownTypes.ContainsKey(name))
                _knownTypes.Add(name, type);
        }

        public static void AddEquivalence([Required] string assemblyName, [Required] string typeName,
            [NotNull] Type equivalentType)
        {
            var name = $"{assemblyName}#{typeName}";
            if (!_equivalences.ContainsKey(name))
                _equivalences.Add(name, equivalentType);
        }

        public bool HasUnknownTypes { get; private set; }

        public Type BindToType(string assemblyName, [Required] string typeName)
        {
            Type result;

            try
            {
                var qualifiedTypeName = $"{assemblyName}#{typeName}";
                if (_knownTypes.ContainsKey(typeName))
                    result = _knownTypes[typeName];
                else if (_equivalences.ContainsKey(qualifiedTypeName))
                {
                    result = _equivalences[qualifiedTypeName];
                }
                else
                {
#if NETCOREAPP
                    if (string.CompareOrdinal(assemblyName, "mscorlib") == 0)
                    {
                        assemblyName = "System.Private.CoreLib";
                    }
#else
                    if (string.CompareOrdinal(assemblyName, "System.Private.CoreLib") == 0)
                    {
                        assemblyName = "mscorlib";
                    }
#endif
                    var assembly = Assembly.Load(assemblyName);
                    result = GetGenericType(typeName, assembly);

                    if (result == null)
                    {
                        if (typeName.EndsWith("[]"))
                        {
                            int length = typeName.IndexOf('[');
                            if (length >= 0)
                            {
                                string name = typeName.Substring(0, length);
                                if (_knownTypes.ContainsKey(name))
                                {
                                    result = assembly.GetType(typeName);
                                    if (result != null)
                                    {
                                        _knownTypes.Add(typeName, result);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                result = typeof(object);
                HasUnknownTypes = true;
                TypeNotFound?.Invoke(assemblyName, typeName);
            }

            return result;
        }

        public void BindToName([NotNull] Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = serializedType.Assembly.FullName;
            typeName = serializedType.FullName;
        }

        private Type GetGenericType(string typeName, Assembly assembly)
        {
            Type result = null;

            int length = typeName.IndexOf('[');
            if (length >= 0)
            {
                string name = typeName.Substring(0, length);
                Type type = assembly.GetType(name);
                if (type?.IsGenericType ?? false)
                {
                    List<Type> typeList = new List<Type>();
                    int position = 0;
                    int startIndex = 0;
                    int last = typeName.Length - 1;
                    for (int index = length + 1; index < last; ++index)
                    {
                        switch (typeName[index])
                        {
                            case '[':
                                if (position == 0)
                                    startIndex = index + 1;
                                position++;
                                break;
                            case ']':
                                position--;
                                if (position == 0)
                                {
                                    var tn = typeName.Substring(startIndex, index - startIndex);
                                    var split = tn.Split(',');
                                    var innerTypeName = split[0].Trim();
                                    var innerAssemblyName = split[1].Trim();
                                    var innerQualifiedTypeName = $"{innerAssemblyName}#{innerTypeName}";
                                    if (_knownTypes.ContainsKey(innerTypeName))
                                    {
                                        typeList.Add(_knownTypes[innerTypeName]);
                                    } else if (_equivalences.ContainsKey(innerQualifiedTypeName))
                                    {
                                        typeList.Add(_equivalences[innerQualifiedTypeName]);
                                    }
                                }
                                break;
                        }
                    }

                    result = type.MakeGenericType(typeList.ToArray());
                }
            }

            return result;
        }
    }
}