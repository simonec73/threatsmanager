using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Configuration data for Extensions.
    /// </summary>
    /// <typeparam name="T">Specific type used for the Configuration Data.</typeparam>
    public class ConfigurationData<T> : ConfigurationData
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name of the configuration.</param>
        /// <param name="value">Current value of the configuration.</param>
        /// <param name="global">Flag specifying if the configuration is global or local to the Threat Model.</param>
        public ConfigurationData([Required] string name, T value, bool global = false) : base(name, value, global)
        {
        }

        /// <summary>
        /// Value of the configuration.
        /// </summary>
        public T TypedValue
        {
            get => (T) Value;
            set => Value = value;
        }
    }

    /// <summary>
    /// Configuration data for Extensions. 
    /// </summary>
    public class ConfigurationData
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name of the configuration.</param>
        /// <param name="value">Current value of the configuration.</param>
        /// <param name="global">Flag specifying if the configuration is global or local to the Threat Model.</param>
        public ConfigurationData([Required] string name, object value, bool global = false)
        {
            Name = name;
            _value = value;
            Global = global;
        }

        /// <summary>
        /// Configuration Name.
        /// </summary>
        public string Name { get; }

        private object _value;

        /// <summary>
        /// Value of the Configuration Data.
        /// </summary>
        public object Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    Updated = true;
                }
            }
        }

        /// <summary>
        /// If true, the configuration is global, otherwise is local to the Threat Model.
        /// </summary>
        public bool Global { get; }

        /// <summary>
        /// Flag highlighting if the value has been updated since the creation of the object.
        /// </summary>
        public bool Updated { get; private set; }
    }
}
