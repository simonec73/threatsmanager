using System;
using System.Configuration;

namespace ThreatsManager.Engine.Config
{
    public class ExtensionConfigCollection : ConfigurationElementCollection
    {
        public ExtensionConfigCollection()
        {
            ExtensionConfig details = (ExtensionConfig)CreateNewElement();
        }

        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMap;

        protected override ConfigurationElement CreateNewElement()
        {
            return new ExtensionConfig();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((ExtensionConfig)element).Id;
        }

        public ExtensionConfig this[int index]
        {
            get => (ExtensionConfig)BaseGet(index);
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public ExtensionConfig this[string name] => (ExtensionConfig)BaseGet(name);

        public int IndexOf(ExtensionConfig element)
        {
            return BaseIndexOf(element);
        }

        public void Add(ExtensionConfig element)
        {
            BaseAdd(element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(ExtensionConfig element)
        {
            if (BaseIndexOf(element) >= 0)
                BaseRemove(element.Id);
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

        protected override string ElementName => "extension";
    }
}