using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreatsManager.ImportersExporters.Importers
{
    public class ParameterManager
    {
        private static readonly Stack<ParameterManager> _stack = new Stack<ParameterManager>();
        private readonly IEnumerable<Parameter> _parameters;
        private readonly Dictionary<string, string> _values = new Dictionary<string, string>();

        public static event Func<string, string> GetParameter;

        private ParameterManager(IEnumerable<Parameter> parameters)
        {
            if (parameters?.Any() ?? false) 
                _parameters = parameters.ToArray();
            else
                _parameters = null;
        }

        public static void Initialize(IEnumerable<Parameter> parameters)
        {
            if (Instance != null)
                _stack.Push(Instance);

            Instance = new ParameterManager(parameters);
        }

        public static void Initialize(IEnumerable<Parameter> parameters, IEnumerable<ParameterValue> parameterValues)
        {
            if (Instance == null)
                throw new InvalidOperationException("Initialize is only possible for nested ParameterManagers.");

            var oldInstance = Instance;
            Initialize(parameters);

            var ps = parameterValues?.ToArray();
            if (ps?.Any() ?? false)
            {
                foreach (var p in ps)
                {
                    Instance.Set(p.Name, oldInstance.ApplyParameters(p.Value));
                }
            }
        }

        public static void Release()
        {
            if (_stack.Count > 0)
                Instance = _stack.Pop();
            else
                Instance = null;
        }

        public static ParameterManager Instance { get; private set; }

        public string Get([Required] string name)
        {
            string result;
            if (!_values.TryGetValue(name, out result))
            {
                var parameter = _parameters?.FirstOrDefault(x => string.CompareOrdinal(name, x.Name) == 0);
                if (parameter != null)
                {
                    result = GetParameter?.Invoke(parameter.Prompt);
                    if (result != null)
                        _values[name] = result;
                }
            }

            return result;
        }

        public bool Set([Required] string name, string value)
        {
            bool result = false;

            if (_parameters?.Any(x => string.CompareOrdinal(name, x.Name) == 0) ?? false)
            {
                _values[name] = value;
                result = true;
            }

            return result;
        }

        public string ApplyParameters(string text)
        {
            string result = null;

            if (!string.IsNullOrWhiteSpace(text) && (_parameters?.Any() ?? false))
            {
                StringBuilder builder = new StringBuilder(text);

                foreach (var parameter in _parameters)
                {
                    builder.Replace(parameter.Name, Get(parameter.Name));
                }

                result = builder.ToString();
            }
            else
                result = text;

            return result;
        }
    }
}
