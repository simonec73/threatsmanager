using System.Configuration;

namespace ThreatsManager.Engine.Config
{
    public class PropertyConfigurationCollection : ConfigurationElementCollection
    {
        public PropertyConfigurationCollection()
        {
            PropertyConfigElement details = (PropertyConfigElement)CreateNewElement();
        }

        public new PropertyConfigElement this[string name]
        {
            get
            {
                if (IndexOf(name) < 0) return null;
                return (PropertyConfigElement)BaseGet(name);
            }
        }

        public PropertyConfigElement this[int index]
        {
            get { return (PropertyConfigElement)BaseGet(index); }
        }

        public int IndexOf(string name)
        {
            name = name.ToLower();

            for (int idx = 0; idx < Count; idx++)
            {
                if (this[idx].Name.ToLower() == name)
                    return idx;
            }
            return -1;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new PropertyConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PropertyConfigElement)element).Name;
        }

        protected override string ElementName
        {
            get { return "property"; }
        }

        public void Add(PropertyConfigElement element)
        {
            BaseAdd(element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(PropertyConfigElement element)
        {
            if (BaseIndexOf(element) >= 0)
                BaseRemove(IndexOf(element.Name));
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }
    }
}