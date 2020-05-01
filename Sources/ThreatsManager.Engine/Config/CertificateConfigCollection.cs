using System;
using System.Configuration;

namespace ThreatsManager.Engine.Config
{
    public class CertificateConfigCollection : ConfigurationElementCollection
    {
        public CertificateConfigCollection()
        {
            CertificateConfig details = (CertificateConfig)CreateNewElement();
        }

        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMap;

        protected override ConfigurationElement CreateNewElement()
        {
            return new CertificateConfig();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((CertificateConfig)element).Thumbprint;
        }

        public CertificateConfig this[int index]
        {
            get => (CertificateConfig)BaseGet(index);
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new CertificateConfig this[string name] => (CertificateConfig)BaseGet(name);

        public int IndexOf(CertificateConfig element)
        {
            return BaseIndexOf(element);
        }

        public void Add(CertificateConfig element)
        {
            BaseAdd(element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(CertificateConfig element)
        {
            if (BaseIndexOf(element) >= 0)
                BaseRemove(element.Thumbprint);
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

        protected override string ElementName => "certificate";
    }
}