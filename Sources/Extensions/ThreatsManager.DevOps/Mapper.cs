using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.DevOps
{
    class Mapper<T>
    {
        private Dictionary<string, T> _map = new Dictionary<string, T>();
        private Dictionary<string, T> _standard = new Dictionary<string,T>();

        public void SetStandardMapping([Required] string key, T value)
        {
            _standard[key] = value;
        }

        public IEnumerable<string> Keys
        {
            get
            {
                IEnumerable<string> result = null;

                if (_map.Any() || _standard.Any())
                {
                    var list = new List<string>();
                    if (_map.Any())
                        list.AddRange(_map.Keys.ToArray());

                    if (_standard.Any())
                    {
                        foreach (var item in _standard.Keys)
                        {
                            if (!list.Contains(item))
                                list.Add(item);
                        }
                    }

                    result = list;
                }

                return result;
            }
        }

        public T Get([Required] string key)
        {
            T result = default(T);

            if (_map.ContainsKey(key))
                result = _map[key];
            else if (_standard.ContainsKey(key))
                result = _standard[key];

            return result;
        }

        public IEnumerable<string> Get(T key)
        {
            var mapList = _map
                .Where(x => x.Value.Equals(key))
                .Select(x => x.Key).ToArray();

            var list = new List<string>();
            if (mapList.Any())
            {
                list.AddRange(mapList);
            }
                
            var standardList = _standard
                .Where(x => x.Value.Equals(key))
                .Select(x => x.Key).ToArray();

            if (standardList.Any())
            {
                foreach (var item in standardList)
                {
                    if (!list.Contains(item))
                        list.Add(item);
                }
            }

            return list;
        }

        public void Add([Required] string key, T value)
        {
            _map[key] = value;
        }

        public void Remove([Required] string key)
        {
            _map.Remove(key);
        }

        public T this[string key]
        {
            get => Get(key);
            set => Add(key, value);
        }

        public IEnumerable<KeyValuePair<string, T>> ToArray()
        {
            var result = new Dictionary<string, T>();

            if (_standard.Any())
            {
                foreach (var item in _standard)
                {
                    result[item.Key] = item.Value; 
                }
            }

            if (_map.Any())
            {
                foreach (var item in _map)
                {
                    result[item.Key] = item.Value; 
                }
            }

            return result.ToArray();
        }

        public bool ContainsKey([Required] string key)
        {
            return _map.ContainsKey(key) || _standard.ContainsKey(key);
        }

        public bool TryGetValue(string key, out T value)
        {
            bool result = false;
            value = default(T);

            if (_map.ContainsKey(key))
            {
                result = true;
                value = _map[key];
            } else if (_standard.ContainsKey(key))
            {
                result = true;
                value = _standard[key];
            }

            return result;
        }
    }
}
