using System.IO;
using System.Xml;
using System.Xml.Serialization;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Extensions.Panels.ThreatSources
{
    public static class ThreatSourceManager
    {
        public static Capec.Attack_Pattern_Catalog GetCapecCatalog([Required] string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Capec.Attack_Pattern_Catalog));

            // load document
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;
            doc.Load(filePath);

            // remove all comments
            XmlNodeList l = doc.SelectNodes("//comment()");
            foreach (XmlNode node in l) node.ParentNode.RemoveChild(node);

            // remove all trailing spaces
            var n = doc.SelectNodes("//@type[contains(., ' ')]");
            foreach (XmlNode node in n) node.Value = node.Value.Trim();

            // remove all trailing spaces
            XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
            mgr.AddNamespace("capec", "http://capec.mitre.org/capec-2");
            var n2 = doc.SelectNodes("//descendant::capec:Submission/@Submission_Source[contains(., ' ')]", mgr);
            foreach (XmlNode node in n2) node.Value = node.Value.Trim();

            // store to memory stream and rewind
            MemoryStream ms = new MemoryStream();
            doc.Save(ms);
            ms.Seek(0, SeekOrigin.Begin);

            // deserialize using clean xml
#pragma warning disable SCS0028 // Unsafe deserialization possible from {1} argument passed to '{0}'
            return serializer.Deserialize(XmlReader.Create(ms)) as Capec.Attack_Pattern_Catalog;
#pragma warning restore SCS0028 // Unsafe deserialization possible from {1} argument passed to '{0}'
        }

#if CWE
        public static Cwe.Weakness_Catalog GetCweCatalog([Required] string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Cwe.Weakness_Catalog));

            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;
            doc.Load(filePath);

            // remove all comments
            XmlNodeList l = doc.SelectNodes("//comment()");
            foreach (XmlNode node in l) node.ParentNode.RemoveChild(node);

            // remove all trailing spaces
            var n = doc.SelectNodes("//@type[contains(., ' ')]");
            foreach (XmlNode node in n) node.Value = node.Value.Trim();

            // remove all new-lines
            var t = doc.SelectNodes("/descendant::Consequence_Technical_Impact");
            foreach (XmlNode node in t)
            {
                if (node.InnerText.Contains("\n"))
                {
                    var parts = node.InnerText.Split('\n');
                    node.InnerText = string.Concat(parts[0].Trim(), " ", parts[1].Trim());
                }
            }

            // remove all new-lines
            var t2 = doc.SelectNodes("/descendant::Method_Name");
            foreach (XmlNode node in t2)
            {
                if (node.InnerText.Contains("\n"))
                {
                    var parts = node.InnerText.Split('\n');
                    node.InnerText = string.Concat(parts[0].Trim(), " ", parts[1].Trim());
                }
            }

            // store to memory stream and rewind
            MemoryStream ms = new MemoryStream();
            doc.Save(ms);
            ms.Seek(0, SeekOrigin.Begin);

            // deserialize using clean xml
            return serializer.Deserialize(XmlReader.Create(ms)) as Cwe.Weakness_Catalog;
        }
#endif
    }
}
