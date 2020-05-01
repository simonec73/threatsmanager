using System;
using System.Configuration;

namespace ThreatsManager.Engine.Config
{
    public class FolderConfigCollection : ConfigurationElementCollection
    {
        public FolderConfigCollection()
        {
            FolderConfig details = (FolderConfig)CreateNewElement();
        }

        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMap;

        protected override ConfigurationElement CreateNewElement()
        {
            return new FolderConfig();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((FolderConfig)element).Name;
        }

        public FolderConfig this[int index]
        {
            get => (FolderConfig)BaseGet(index);
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new FolderConfig this[string name] => (FolderConfig)BaseGet(name);

        public int IndexOf(FolderConfig element)
        {
            return BaseIndexOf(element);
        }

        public void Add(FolderConfig element)
        {
            BaseAdd(element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(FolderConfig element)
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

        protected override string ElementName => "folder";
    }
}