using System.Collections.Generic;

namespace ThreatsManager.MsTmt.Model
{
    public class Property
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public PropertyType Type { get; set; }
        public IEnumerable<string> Values { get; set; }
    }
}