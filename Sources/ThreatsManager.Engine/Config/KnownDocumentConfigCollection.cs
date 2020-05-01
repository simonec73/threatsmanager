using System;
using System.Configuration;

namespace ThreatsManager.Engine.Config
{
    public class KnownDocumentConfigCollection : ConfigurationElementCollection
    {
        public KnownDocumentConfigCollection()
        {
            KnownDocumentConfig details = (KnownDocumentConfig)CreateNewElement();
        }

        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMap;

        protected override ConfigurationElement CreateNewElement()
        {
            return new KnownDocumentConfig();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((KnownDocumentConfig)element).Path;
        }

        public KnownDocumentConfig this[int index]
        {
            get => (KnownDocumentConfig)BaseGet(index);
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new KnownDocumentConfig this[string name] => (KnownDocumentConfig)BaseGet(name);

        public int IndexOf(KnownDocumentConfig element)
        {
            return BaseIndexOf(element);
        }

        public void Add(KnownDocumentConfig element)
        {
            BaseAdd(element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(KnownDocumentConfig element)
        {
            if (BaseIndexOf(element) >= 0)
                BaseRemove(element.Path);
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

        protected override string ElementName => "doc";
    }
}