using System;
using System.Configuration;

namespace ThreatsManager.Engine.Config
{
    public class PrefixConfigCollection : ConfigurationElementCollection
    {
        public PrefixConfigCollection()
        {
            PrefixConfig details = (PrefixConfig)CreateNewElement();
        }

        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMap;

        protected override ConfigurationElement CreateNewElement()
        {
            return new PrefixConfig();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((PrefixConfig)element).Name;
        }

        public PrefixConfig this[int index]
        {
            get => (PrefixConfig)BaseGet(index);
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new PrefixConfig this[string name] => (PrefixConfig)BaseGet(name);

        public int IndexOf(PrefixConfig element)
        {
            return BaseIndexOf(element);
        }

        public void Add(PrefixConfig element)
        {
            BaseAdd(element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(PrefixConfig element)
        {
            if (BaseIndexOf(element) >= 0)
                BaseRemove(element.Name);
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

        protected override string ElementName => "prefix";
    }
}