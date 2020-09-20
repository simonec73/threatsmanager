using System.Collections.Generic;

namespace ThreatsManager.MsTmt.Model
{
    public class ThreatType
    {
        public ThreatType(string key, string name, string description, string priority, 
            string includeFilter, string excludeFilter, IEnumerable<PropertyDefinition> properties)
        {
            Key = key;
            Name = name;
            Description = description;
            Priority = priority;
            IncludeFilter = includeFilter;
            ExcludeFilter = excludeFilter;
            Properties = properties;
        }

        public string Key { get; }
        public string Name { get; }
        public string Description { get; }
        public string Priority { get; }
        public string IncludeFilter { get; }
        public string ExcludeFilter { get; }
        public IEnumerable<PropertyDefinition> Properties { get; }
    }
}